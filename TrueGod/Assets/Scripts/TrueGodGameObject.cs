using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueGodGameObject {

	protected float ATK;

	protected float DEF;

	protected float FQC;

	protected float RAG;

	protected float SPD;

	protected float SCR;

	private static float PRE_ATK = 100;

	private static float PRE_DEF = 100;

	private static float PRE_FQC = 1;

	private static float PRE_RAG = 1;

	private static float PRE_SPD = 1;

	protected GameObject OBJ;

	public float timer;

	public TrueGodGameObject() {
		timer = 0;
	}


	public TrueGodGameObject(GameObject obj) {
		OBJ = obj;
		ATK = PRE_ATK;
		DEF = PRE_DEF;
		FQC = PRE_FQC;
		RAG = PRE_RAG;
		SPD = PRE_SPD;
		SCR = 0;
		timer = 0;
		Enemy = false;
	}

	public void Endure_Attack(float atk) {
		this.DEF -= atk;
	}

	public void Launch_Attack(TrueGodGameObject obj) {
		obj.DEF -= this.ATK;
	}

	public bool Is_Dead() {
		return (this.DEF <= 0);
	}

	public void Move() {

	}

	public void Shoot() {

	}

	public GameObject Get_OBJ() {
		return this.OBJ;
	}

	public float Get_ATK() {
		return this.ATK;
	}

	public float Get_DEF() {
		return this.DEF;
	}


	public float Get_FQC() {
		return this.FQC;
	}

	public float Get_RAG() {
		return this.RAG;
	}

	public float Get_SPD() {
		return this.SPD;
	}

	public float Get_SCR() {
		return this.SCR;
	}

	public bool Enemy;

	public bool Is_Enemy() {
		return Enemy;
	}

	public static void Level_Up(float degree) {
		PRE_ATK = Mathf.Floor(PRE_ATK * degree);
		PRE_DEF = Mathf.Floor(PRE_DEF * degree);
		PRE_FQC = Mathf.Floor(PRE_FQC * degree);
		PRE_RAG = Mathf.Floor(PRE_RAG * degree);
		PRE_SPD = Mathf.Floor(PRE_SPD * degree);
	}

}
