using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Right_Pistol : MonoBehaviour {

	public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 0.45f;
	private float nextFire = 0.45f;
	private float myTime = 0.0f;
	public GameObject pistolBullet;
	public int Ammo = 20;
	public Text AmmoCount;
	public bool Reloading;

	[Header("Muzzle Effects")]
	public ParticleSystem MuzzleFlash;
	public AudioSource audioSource;

	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		AmmoCount = GameObject.FindGameObjectWithTag("RightWeaponAmmo").GetComponent<Text>();
	}

	void Update () {
		myTime = myTime + Time.deltaTime;
		AmmoCount.text = Ammo.ToString ();
		Debug.DrawRay (ShotSpawn.transform.position, -ShotSpawn.transform.forward, Color.red);
		if (Input.GetMouseButton (1) && myTime > nextFire && Ammo > 0 && Reloading != true) {
			nextFire = myTime + fireDelta;
			//MuzzleFlash.Play ();
			Shoot ();
			Ammo--;
			nextFire = nextFire - myTime;
			myTime = 0.0f;
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			StartCoroutine (Player.Right_Reload ());
			StartCoroutine(Reload ());
		}
	}

	private void Shoot(){
		Debug.Log ("Shooting");
		GameObject pistolBullet_I = (GameObject)Instantiate (pistolBullet,ShotSpawn.transform.position, Quaternion.identity);
		pistolBullet_I.GetComponent<Rigidbody> ().AddForce (-transform.forward * 1000f, ForceMode.VelocityChange);
		Destroy (pistolBullet_I, 0.4f);
		//Vector3 forward = ShotSpawn.transform.TransformDirection (ShotSpawn.transform.forward);
		/*RaycastHit shotHit;
		if(Physics.Raycast(ShotSpawn.transform.position, -ShotSpawn.transform.forward,out shotHit,250)){
			//Debug.Log ("Hit soemthing at: " + shotHit.distance);
			Debug.Log ("Hit object: " + shotHit.transform.gameObject);
			if (shotHit.collider.tag == "Enemy") {
				GameObject Enemy = shotHit.collider.gameObject;
				Enemy.GetComponent<Enemy> ().Hit (1);
				Destroy (pistolBullet_I, 0.4f);
			}
		}*/

	}

	private IEnumerator Reload(){
		Reloading = true;
		yield return new WaitForSeconds(0.98f);
		Ammo = 20;
		Reloading = false;
	}
}
