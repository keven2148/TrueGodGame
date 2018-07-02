using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

	public Button add_soldier;
	public Button add_barrier;
	public Button add_outpost;
	public Button god_cure;
	public Button god_rage;


	Text score_text;
	Text time_text;
	Text tower_text;
	Text game_over_text;
	Text statistic_text;

	// Use this for initialization
	public View () {
		add_soldier = GameObject.Find ("Add_Soldier").GetComponent<Button> ();
		add_barrier = GameObject.Find ("Add_Barrier").GetComponent<Button> ();
		add_outpost = GameObject.Find ("Add_Outpost").GetComponent<Button> ();
		god_cure = GameObject.Find ("God_Cure").GetComponent<Button> ();
		god_rage = GameObject.Find ("God_Rage").GetComponent<Button> ();
		score_text = GameObject.Find ("Score").GetComponent<Text> ();
		time_text = GameObject.Find ("Time").GetComponent<Text> ();
		tower_text = GameObject.Find ("Tower_HP").GetComponent<Text> ();
		game_over_text = GameObject.Find ("Game_Over").GetComponent<Text> ();
		statistic_text = GameObject.Find ("Statistic").GetComponent<Text> ();
	}

	public void Set_Score_Text(float scr) {
		score_text.text = "Score: " + scr;
	}

	public void Set_Time_Text(float tm) {
		time_text.text = "Time: " + Mathf.Floor(tm) + "s";
	}

	public void Set_Tower_Text(float tw) {
		tower_text.text = "Tower HP: " + tw;
	}

	public void Set_Game_Over_Text(float scr, float tm) {
		game_over_text.text = "Game Over!\nScore: " + scr + "\nTime: " + Mathf.Floor(tm) + "s";
	}

	public bool is_Game_Over_set() {
		return game_over_text.text.Length > 0;
	}

	public void Set_Soldier_Button_Color(bool b) {
		if (b) {
			add_soldier.image.color = new Color (0, 255, 0);
		} else {
			add_soldier.image.color = new Color (255, 0, 0);
		}
	}

	public void Set_Barrier_Button_Color(bool b) {
		if (b) {
			add_barrier.image.color = new Color (0, 255, 0);
		} else {
			add_barrier.image.color = new Color (255, 0, 0);
		}
	}

	public void Set_Outpost_Button_Color(bool b) {
		if (b) {
			add_outpost.image.color = new Color (0, 255, 0);
		} else {
			add_outpost.image.color = new Color (255, 0, 0);
		}
	}

	public void Set_Cure_Button_Color(bool b) {
		if (b) {
			god_cure.image.color = new Color (0, 255, 0);
		} else {
			god_cure.image.color = new Color (255, 0, 0);
		}
	}

	public void Set_Rage_Button_Color(bool b) {
		if (b) {
			god_rage.image.color = new Color (0, 255, 0);
		} else {
			god_rage.image.color = new Color (255, 0, 0);
		}
	}

	public void Set_Statistic_Text(int s, int b, int o, int w, int f, int l) {
		statistic_text.text = "Soldiers: " + s + "\nBarriers: " + b + "\nOutposts: " + o + "\nWalkers: " + w + "\n   Flyers: " + f + "\n   Level: " + l;
	}
}
