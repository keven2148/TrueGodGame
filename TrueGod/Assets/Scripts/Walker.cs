using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : TrueGodGameObject {

	private static float PRE_ATK = 10;

	private static float PRE_DEF = 100;

	private static float PRE_FQC = 1;

	private static float PRE_RAG = 2;

	private static float PRE_SPD = 2;

	public Walker(GameObject obj) {
		OBJ = obj;
		ATK = PRE_ATK;
		DEF = PRE_DEF;
		FQC = PRE_FQC;
		RAG = PRE_RAG;
		SPD = PRE_SPD;
		timer = 0;
		Enemy = true;
		SCR = 100;
	}
}
