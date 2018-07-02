using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : TrueGodGameObject {

	private static float PRE_ATK = 10;

	private static float PRE_DEF = 1000;

	private static float PRE_FQC = 5;

	private static float PRE_RAG = 20;

	private static float PRE_SPD = 0;

	public Tower(GameObject obj) {
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

	public void Recover() {
		DEF = PRE_DEF;
	}
}
