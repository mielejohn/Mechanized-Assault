using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rayDir = GetComponent<Rigidbody>().velocity;
        Debug.DrawRay (transform.position, rayDir, Color.red);
		RaycastHit shotHit;
		if(Physics.Raycast(transform.position, rayDir, out shotHit,100f)){
			//Debug.Log ("Hit soemthing at: " + shotHit.distance);
			Debug.Log ("Hit object: " + shotHit.transform.gameObject);
			if (shotHit.collider.tag == "Enemy") {
				//GameObject Enemy = shotHit.collider.gameObject;
				shotHit.collider.GetComponent<Enemy> ().Hit (20);
				Destroy (this.gameObject);
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		/*if (other.tag == "Enemy") {
			Destroy (this.gameObject);
		}*/
	}
}
