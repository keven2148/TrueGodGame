using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


	public Vector3 direction;

	float timer;

	// Use this for initialization
	void Start () {
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timer > 0.1) {
			Destroy (this.gameObject);
			return;
		}
		transform.position = transform.position + direction * Time.deltaTime * 10;
	}

	void OnCollisionEnter(Collision collision) {
		Destroy (this.gameObject);
	}
}
