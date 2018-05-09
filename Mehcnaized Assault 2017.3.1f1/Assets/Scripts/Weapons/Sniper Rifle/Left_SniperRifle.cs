using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Left_SniperRifle : MonoBehaviour {

	public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 1.5f;
	private float nextFire = 1.5f;
	private float myTime = 0.0f;
	public GameObject sniperRifleBullet;
	public int Ammo = 10;
	public Text AmmoCount;
	public bool Reloading;
	public bool dropped = false;

	[Header("Muzzle Effects")]
	public ParticleSystem MuzzleFlash;
	public AudioSource audioSource;

	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		AmmoCount = GameObject.FindGameObjectWithTag("LeftWeaponAmmo").GetComponent<Text>();
	}

	void Update () {
		myTime = myTime + Time.deltaTime;
		AmmoCount.text = Ammo.ToString ();
		Debug.DrawRay (ShotSpawn.transform.position, -ShotSpawn.transform.right, Color.red);
		if (Input.GetMouseButtonDown (0) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true) {
			nextFire = myTime + fireDelta;
			MuzzleFlash.Play();
			Shoot ();
			Ammo--;
			nextFire = nextFire - myTime;
			myTime = 0.0f;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			StartCoroutine (Player.Left_Reload ());
			StartCoroutine(Reload ());
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			StartCoroutine( PistolSwap());
		}
	}

	private void Shoot(){
		Debug.Log ("Shooting");
		GameObject sniperRilfebullet_I = (GameObject)Instantiate (sniperRifleBullet,ShotSpawn.transform.position, Quaternion.identity);
		sniperRilfebullet_I.GetComponent<Rigidbody> ().AddForce (-transform.right * 5000f, ForceMode.VelocityChange);
		Destroy (sniperRilfebullet_I, 1.5f);
		//Vector3 forward = ShotSpawn.transform.TransformDirection (ShotSpawn.transform.forward);
		RaycastHit shotHit;
		if(Physics.Raycast(ShotSpawn.transform.position, -ShotSpawn.transform.right,out shotHit,1400f)){
			//Debug.Log ("Hit soemthing at: " + shotHit.distance);
			Debug.Log ("Hit object: " + shotHit.transform.gameObject);
			if (shotHit.collider.tag == "Enemy") {
				GameObject Enemy = shotHit.collider.gameObject;
				Enemy.GetComponent<Enemy> ().Hit (10);
				Destroy (sniperRilfebullet_I, 1.5f);
			}
		}

	}

	private IEnumerator Reload(){
		Reloading = true;
		yield return new WaitForSeconds(0.98f);
		Ammo = 10;
		Reloading = false;
	}

	public IEnumerator PistolSwap(){
		dropped = true;
		yield return new WaitForSeconds(1.25f);
		this.gameObject.transform.parent = null;
		this.GetComponent<Rigidbody>().useGravity=true;
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<Rigidbody>().AddForce(new Vector3(-30, -0.5f, 0),ForceMode.VelocityChange);
		yield return new WaitForSeconds(0.2f);
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<MeshCollider>().enabled = true;
		Destroy(this.gameObject, 10.0f);
		//Destroy(topObject, 10.0f);
	}
}
