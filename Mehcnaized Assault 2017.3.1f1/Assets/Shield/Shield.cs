using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	public PlayerController player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "EnemyBullet") {
			if (player.Shields > 0) {
				player.Shields -= 3;
			}
		}
	}
}
