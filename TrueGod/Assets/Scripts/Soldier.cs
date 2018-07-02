using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : TrueGodGameObject {

	private static float PRE_ATK = 20;

	private static float PRE_DEF = 100;

	private static float PRE_FQC = 2;

	private static float PRE_RAG = 3;

	private static float PRE_SPD = 5;

	public Soldier(GameObject obj) {
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

	static float COST = 200f;

	public static float Get_COST() {
		return Soldier.COST;
	}
}
