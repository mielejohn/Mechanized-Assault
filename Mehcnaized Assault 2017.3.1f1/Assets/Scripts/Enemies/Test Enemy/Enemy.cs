using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public int Health = 100;
	public Slider HealthBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		HealthBar.value = Health;
		if (Health <= 0) {
			Destroy (this.gameObject);
		}
	}

	public void Hit(int Damage){
		Debug.Log (this.gameObject + " was hit by a ray");
		Health -= Damage;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Bullet") {
			Destroy (other.gameObject);
		}
	}
}
