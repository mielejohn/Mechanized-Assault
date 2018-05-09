using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour {
	public PlayerController pc;
	public Transform target;
	public GameObject SpawnPoint;

	public float speed = 80f;

	private Rigidbody rb;
	// Use this for initialization
	void Awake ()
	{
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
        if (pc.targetedLeftEnemy != null || pc.targetedRightEnemy != null) {
			target = pc.targetedRightEnemy.transform;
		}
		SpawnPoint = GameObject.FindGameObjectWithTag("Missle_Launcher").GetComponent<MissleLauncher>().missleSpawn;
		rb = GetComponent<Rigidbody>();
	}

	void Start () {
		//pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		//target = pc.Enemy.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//target = pc.Enemy.transform;
        if (pc.targetedLeftEnemy != null || pc.targetedRightEnemy != null) {
			float step = speed * Time.deltaTime;
			transform.LookAt (target);
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
        } else if (pc.targetedLeftEnemy != null || pc.targetedRightEnemy != null) {
			//rb.velocity = SpawnPoint.transform.forward * speed;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Enemy") {
			other.GetComponent<Enemy>().Hit(10);
			Destroy (this.gameObject);
		}
	}
}
