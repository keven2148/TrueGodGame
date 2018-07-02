using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	public static float generate_walker_interval = 1f;
	public static float generate_flyer_interval = 3f;

	public static float generate_walker_timer;
	public static float generate_flyer_timer;

	static float CURE_COST = 2000f;
	static float RAGE_COST = 1000f;

	Factory factory;

	View view;

	public static float player_score;

	static int game_level;
	static Vector3 hit_pos;
	static bool ready2hit = false;

	void Start () {
		GameObject[] dens = GameObject.FindGameObjectsWithTag ("Den");
		Vector3[] dens_pos = new Vector3[dens.Length];
		for (int i = 0; i < dens.Length; i++) {
			dens_pos [i] = dens [i].transform.position;
		}
		factory = new Factory ();
		view = new View ();
		player_score = 0;
		generate_walker_timer = 0;
		generate_flyer_timer = 0;
		view.add_soldier.onClick.AddListener (Add_Soldier);
		view.add_barrier.onClick.AddListener (Add_Barrier);
		view.add_outpost.onClick.AddListener (Add_Outpost);
		view.god_cure.onClick.AddListener (God_Cure);
		view.god_rage.onClick.AddListener (God_Rage);
		game_level = 0;
	}

	void Add_Soldier() {
		if (player_score >= Soldier.Get_COST ()) {
			if (factory.Generate_Soldier () != null) {
				Reduce_Score (Soldier.Get_COST ());
			}
		}
	}

	void Add_Barrier() {
		if (player_score >= Barrier.Get_COST ()) {
			if (factory.Generate_Barrier () != null) {
				Reduce_Score (Barrier.Get_COST ());
			}
		}
	}

	void Add_Outpost() {
		if (player_score >= Outpost.Get_COST ()) {
			if (factory.Generate_Outpost () != null) {
				Reduce_Score (Outpost.Get_COST ());
			}
		}
	}

	void God_Cure() {
		if (player_score >= CURE_COST) {
			factory.tower.Recover ();
			Reduce_Score (CURE_COST);
		}
	}

	void God_Rage() {
		if (player_score >= RAGE_COST) {
			ready2hit = true;
			//God_Attack ();
			//Reduce_Score (RAGE_COST);
		}
	}

	void Go_Harder() {
		Walker.Level_Up (1.1f);
		Flyer.Level_Up (1.1f);
		generate_walker_interval /= 1.1f;
		generate_flyer_interval /= 1.1f;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0) && ready2hit) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				hit_pos = hit.point;
				God_Attack ();print (hit_pos);
				Reduce_Score (RAGE_COST);ready2hit = false;
			}
		}
		if (factory.tower.Is_Dead()) {
			if (!view.is_Game_Over_set()) {
				view.add_soldier.onClick.RemoveListener (Add_Soldier);
				view.add_barrier.onClick.RemoveListener (Add_Barrier);
				view.add_outpost.onClick.RemoveListener (Add_Outpost);
				view.god_cure.onClick.RemoveListener (God_Cure);
				view.god_rage.onClick.RemoveListener (God_Rage);
				Game_Over ();
			}						
			return;
		}

		view.Set_Soldier_Button_Color (player_score >= Soldier.Get_COST ());
		view.Set_Barrier_Button_Color (player_score >= Barrier.Get_COST ());
		view.Set_Outpost_Button_Color (player_score >= Outpost.Get_COST ());
		view.Set_Cure_Button_Color (player_score >= CURE_COST);
		view.Set_Rage_Button_Color (player_score >= RAGE_COST);
		view.Set_Score_Text (player_score);
		view.Set_Time_Text (Time.time);
		view.Set_Tower_Text (factory.tower.Get_DEF());
		view.Set_Statistic_Text (factory.soldiers.Count, factory.barriers.Count, factory.outposts.Count, factory.walkers.Count, factory.flyers.Count, game_level);

		int cur_level = Mathf.FloorToInt (player_score / 1000);
		if (cur_level > game_level) {
			game_level = cur_level;
			Go_Harder ();
		}
		if (Time.time - generate_walker_timer > generate_walker_interval) {
			Walker new_walker = factory.Generate_Walker();
			Set_Nav (new_walker, factory.tower);
			generate_walker_timer = Time.time;
		}
		if (Time.time - generate_flyer_timer > generate_flyer_interval) {
			Flyer new_flyer = factory.Generate_Flyer();
			Set_Nav (new_flyer, factory.tower);
			generate_flyer_timer = Time.time;
		}

		Tower_Attack_Flyers ();
		Tower_Attack_Walkers ();
		Soldiers_Attack_Walkers ();
		Barriers_Attack_Walkers ();
		Outposts_Attack_Flyers ();
		Outposts_Attack_Walkers ();
		Walkers_Attack_Tower ();
		Walkers_Attack_Soldiers ();
		Walkers_Attack_Barriers ();
		Flyers_Attack_Outposts ();
		Flyers_Attack_Tower ();
		Flyers_Attack_Barriers ();
	}

	bool Attack(Vector3 atk_pos, TrueGodGameObject defender, float atk, float range) {
		if (!defender.Is_Dead ()) {
			float distance = Vector3.Distance(atk_pos, defender.Get_OBJ().transform.position);
			if (distance <= range) {
				defender.Endure_Attack (atk);
				if (defender.Is_Dead ()) {
					print (defender.Get_OBJ().name + " Destroyed!!!");
					GameObject.Destroy (defender.Get_OBJ());
				}
				return true;
			}
		}
		return false;
	}

	bool Attack(TrueGodGameObject attacker, TrueGodGameObject defender) {
		if (Time.time - attacker.timer >= 1f / attacker.Get_FQC () && !attacker.Is_Dead () && !defender.Is_Dead ()) {
			float distance = Vector3.Distance(attacker.Get_OBJ().transform.position, defender.Get_OBJ().transform.position);
			if (distance <= attacker.Get_RAG()) {
				attacker.Launch_Attack (defender);
				Fire(attacker, defender);
				attacker.timer = Time.time;
				if (defender.Is_Dead ()) {
					print (defender.Get_OBJ().name + " Destroyed!!!");
					GameObject.Destroy (defender.Get_OBJ());
					if (defender.Is_Enemy ()) {
						Add_Score (defender.Get_SCR());
					}
				}
				return true;
			}
		}
		return false;
	}

	void Add_Score(float scr) {
		player_score += scr;
		print ("Player Score: " + player_score);
	}

	void Reduce_Score(float scr) {
		player_score -= scr;
		print ("Player Score: " + player_score);
	}

	void Fire(TrueGodGameObject attacker, TrueGodGameObject defender) {
		GameObject prf = Resources.Load ("Prefabs/Bullet") as GameObject;
		GameObject new_bullet = Instantiate(prf) as GameObject;
		new_bullet.transform.position = attacker.Get_OBJ ().transform.position;
		new_bullet.GetComponent<Bullet>().direction = defender.Get_OBJ().transform.position - attacker.Get_OBJ ().transform.position;
	}

	void God_Attack() {
		for (int i = factory.walkers.Count - 1; i >= 0; i--) {
			Attack (hit_pos, factory.walkers[i], 500f, 10f);
		}
		for (int i = factory.flyers.Count - 1; i >= 0; i--) {
			Attack (hit_pos, factory.flyers[i], 500f, 10f);
		}
	}

	void Barriers_Attack_Walkers() {
		for (int i = factory.barriers.Count - 1; i >= 0; i--) {
			int index = Get_Closest<Walker> (factory.barriers[i], factory.walkers);
			if (index != -1) {
				Attack (factory.barriers[i], factory.walkers [index]);
			}
		}
	}

	void Outposts_Attack_Flyers() {
		for (int i = factory.outposts.Count - 1; i >= 0; i--) {
			int index = Get_Closest<Flyer> (factory.outposts[i], factory.flyers);
			if (index != -1) {
				Attack (factory.outposts[i], factory.flyers [index]);
			}
		}
	}

	void Outposts_Attack_Walkers() {
		for (int i = factory.outposts.Count - 1; i >= 0; i--) {
			int index = Get_Closest<Walker> (factory.outposts[i], factory.walkers);
			if (index != -1) {
				Attack (factory.outposts[i], factory.walkers [index]);
			}
		}
	}

	void Walkers_Attack_Tower() {
		for (int i = factory.walkers.Count - 1; i >= 0; i--) {
			Attack (factory.walkers[i], factory.tower);
		}
	}

	void Walkers_Attack_Soldiers() {
		for (int i = factory.walkers.Count - 1; i >= 0; i--) {
			int index = Get_Closest (factory.walkers[i], factory.soldiers);
			if (index != -1) {
				Attack (factory.walkers[i], factory.soldiers[index]);
				if (factory.soldiers[index].Is_Dead ()) {
					factory.soldiers.RemoveAt (index);
				}
			}
		}
	}

	void Walkers_Attack_Barriers() {
		for (int i = factory.walkers.Count - 1; i >= 0; i--) {
			int index = Get_Closest (factory.walkers[i], factory.barriers);
			if (index != -1) {
				Attack (factory.walkers[i], factory.barriers[index]);
				if (factory.barriers[index].Is_Dead ()) {
					factory.barriers.RemoveAt (index);
				}
			}
		}
	}

	void Flyers_Attack_Outposts() {
		for (int i = factory.flyers.Count - 1; i >= 0; i--) {
			int index = Get_Closest (factory.flyers[i], factory.outposts);
			if (index != -1) {
				Attack (factory.flyers[i], factory.outposts[index]);
				if (factory.outposts[index].Is_Dead ()) {
					factory.outposts.RemoveAt (index);
				}
			}
		}
	}

	void Flyers_Attack_Tower() {
		for (int i = factory.flyers.Count - 1; i >= 0; i--) {
			Attack (factory.flyers[i], factory.tower);
		}
	}

	void Flyers_Attack_Barriers() {
		for (int i = factory.flyers.Count - 1; i >= 0; i--) {
			int index = Get_Closest (factory.flyers[i], factory.barriers);
			if (index != -1) {
				Attack (factory.flyers[i], factory.barriers[index]);
				if (factory.barriers[index].Is_Dead ()) {
					factory.barriers.RemoveAt (index);
				}
			}
		}
	}

	void Soldiers_Attack_Walkers() {
		for (int i = factory.soldiers.Count - 1; i >= 0; i--) {
			int index = Get_Closest<Walker> (factory.soldiers[i], factory.walkers);
			if (index != -1) {
				Set_Nav (factory.soldiers [i], factory.walkers [index]);
				Attack (factory.soldiers [i], factory.walkers [index]);
				if (factory.walkers[index].Is_Dead ()) {
					factory.walkers.RemoveAt (index);
				}
			}
		}
	}

	void Tower_Attack_Walkers() {
		int index = Get_Closest<Walker> (factory.tower, factory.walkers);
		if (index != -1) {
			Attack (factory.tower, factory.walkers[index]);
			if (factory.walkers[index].Is_Dead ()) {
				factory.walkers.RemoveAt (index);
			}
		}
	}

	void Tower_Attack_Flyers() {
		int index = Get_Closest<Flyer> (factory.tower, factory.flyers);
		if (index != -1) {
			Attack (factory.tower, factory.flyers[index]);
			if (factory.flyers[index].Is_Dead ()) {
				factory.flyers.RemoveAt (index);
			}
		}
	}

	void Set_Nav(TrueGodGameObject subj, TrueGodGameObject obj) {
		if (subj.Get_OBJ() == null || obj.Get_OBJ() == null) {
			return;
		}
		UnityEngine.AI.NavMeshAgent nav = subj.Get_OBJ().GetComponent<UnityEngine.AI.NavMeshAgent>();
		nav.stoppingDistance = 1f;
		nav.speed = subj.Get_SPD ();
		nav.SetDestination(obj.Get_OBJ().transform.position);
	}

	void Set_Nav(TrueGodGameObject subj, Vector3 pos) {
		if (subj.Get_OBJ() == null) {
			return;
		}
		UnityEngine.AI.NavMeshAgent nav = subj.Get_OBJ().GetComponent<UnityEngine.AI.NavMeshAgent>();
		nav.stoppingDistance = 1f;
		nav.speed = subj.Get_SPD ();
		nav.SetDestination(pos);
	}

	int Get_Closest<T>(TrueGodGameObject subject, List<T> objects) where T : TrueGodGameObject {
		float closest = 1000000;
		int index = -1;
		for (int i = 0; i < objects.Count; i++) {
			if (objects [i].Is_Dead () ||  subject.Is_Dead()) {
				continue;
			}
			float distance = Vector3.Distance(subject.Get_OBJ().transform.position, objects[i].Get_OBJ().transform.position);
			if (distance < closest) {
				closest = distance;
				index = i;
			}
		}
		return index;
	}

	void Game_Over() {
		view.Set_Game_Over_Text (player_score, Time.time);
	}

}
