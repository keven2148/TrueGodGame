using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : TrueGodGameObject {

	private static float PRE_ATK = 10;

	private static float PRE_DEF = 100;

	private static float PRE_FQC = 5;

	private static float PRE_RAG = 10;

	private static float PRE_SPD = 0;

	public Barrier(GameObject obj) {
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

	static float COST = 500f;

	public static float Get_COST() {
		return Barrier.COST;
	}

}

