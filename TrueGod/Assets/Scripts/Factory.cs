using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

	public List<Walker> walkers;
	public List<Flyer> flyers;
	public List<Soldier> soldiers;
	public List<Barrier> barriers;
	public List<Outpost> outposts;

	public Tower tower;

	List<Vector3> dens_pos;
	List<Vector3> bars_pos;
	List<Vector3> wats_pos;
	List<Quaternion> bars_rot;

	public Factory() {
		tower = new Tower(GameObject.Find ("Tower"));
		walkers = new List<Walker>();
		flyers = new List<Flyer>();
		soldiers = new List<Soldier> ();
		outposts = new List<Outpost> ();
		barriers = new List<Barrier> ();
		GameObject[] wats_obj = GameObject.FindGameObjectsWithTag ("Wat");
		wats_pos = new List<Vector3> (wats_obj.Length);
		for (int i = 0; i < wats_obj.Length; i++) {
			wats_pos.Add(wats_obj [i].transform.position);
		}
		GameObject[] dens_obj = GameObject.FindGameObjectsWithTag ("Den");
		dens_pos = new List<Vector3> (dens_obj.Length);
		for (int i = 0; i < dens_obj.Length; i++) {
			dens_pos.Add(dens_obj [i].transform.position);
		}
		GameObject[] bars_obj = GameObject.FindGameObjectsWithTag ("Bar");
		bars_pos = new List<Vector3> (bars_obj.Length);
		for (int i = 0; i < bars_obj.Length; i++) {
			bars_pos.Add(bars_obj [i].transform.position);
		}
		bars_rot = new List<Quaternion> (bars_obj.Length);
		for (int i = 0; i < bars_obj.Length; i++) {
			bars_rot.Add(bars_obj [i].transform.rotation);
		}
	}

	public Walker Generate_Walker() {
		int rand = Random.Range (0, dens_pos.Count);
		GameObject prf = Resources.Load ("Prefabs/Walker") as GameObject;
		GameObject new_walker_obj = Instantiate(prf) as GameObject;
		new_walker_obj.transform.position = dens_pos[rand];
		Walker new_walker = new Walker (new_walker_obj);
		walkers.Add (new_walker);
		return new_walker;
	}

	public Flyer Generate_Flyer() {
		int rand = Random.Range (0, dens_pos.Count);
		GameObject prf = Resources.Load ("Prefabs/Flyer") as GameObject;
		GameObject new_flyer_obj = Instantiate(prf) as GameObject;
		new_flyer_obj.transform.position = dens_pos[rand];
		Flyer new_flyer = new Flyer (new_flyer_obj);
		flyers.Add (new_flyer);
		return new_flyer;
	}

	public Soldier Generate_Soldier() {
		if (soldiers.Count >= 20) {
			return null;
		}
		GameObject prf = Resources.Load ("Prefabs/Soldier") as GameObject;
		GameObject new_soldier_obj = Instantiate(prf) as GameObject;	
		new_soldier_obj.transform.position = tower.Get_OBJ().transform.position;
		Soldier new_soldier = new Soldier (new_soldier_obj);
		soldiers.Add (new_soldier);
		return new_soldier;
	}

	public Barrier Generate_Barrier() {
		if (barriers.Count >= bars_pos.Count) {
			return null;
		}
		GameObject prf = Resources.Load ("Prefabs/Barrier") as GameObject;
		GameObject new_barrier_obj = Instantiate(prf) as GameObject;
		int index = 0;
		for (int i = 0; i < bars_pos.Count; i++) {
			index = 0;
			for (; index < barriers.Count; index++) {
				if (barriers[index].Get_OBJ().transform.position == bars_pos[i]) {
					break;
				}
			}
			if (index == barriers.Count) {
				index = i;
				break;
			}
		}
		new_barrier_obj.transform.position = bars_pos [index];
		new_barrier_obj.transform.rotation = bars_rot [index];
		Barrier new_barrier = new Barrier (new_barrier_obj);
		barriers.Add (new_barrier);
		return new_barrier;
	}

	public Outpost Generate_Outpost() {
		if (outposts.Count >= wats_pos.Count) {
			return null;
		}
		GameObject prf = Resources.Load ("Prefabs/Outpost") as GameObject;
		GameObject new_outpost_obj = Instantiate(prf) as GameObject;
		int index = 0;
		for (int i = 0; i < wats_pos.Count; i++) {
			index = 0;
			for (; index < outposts.Count; index++) {
				if (outposts[index].Get_OBJ().transform.position == wats_pos[i]) {
					break;
				}
			}
			if (index == outposts.Count) {
				index = i;
				break;
			}
		}
		new_outpost_obj.transform.position = wats_pos[index];
		Outpost new_outpost = new Outpost (new_outpost_obj);
		outposts.Add (new_outpost);
		return new_outpost;
	}

}
