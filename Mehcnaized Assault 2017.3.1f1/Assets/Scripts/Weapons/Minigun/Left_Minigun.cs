	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Left_Minigun : MonoBehaviour {

	public PlayerController Player;
	public GameObject ShotSpawn_1;
	public GameObject ShotSpawn_2;
	private float fireDelta = 0.12f;
	private float nextFire = 0.12f;
	private float myTime = 0.0f;
	public GameObject minigunBullet;
	public int Ammo = 150;
	public Text AmmoCount;
	public bool Reloading;
	public Animator Anim;
	public bool dropped = false;

	[Header("Muzzle Effects")]
	public ParticleSystem MuzzleFlash_1;
	public ParticleSystem MuzzleFlash_2;
	public AudioSource audioSource;


	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		AmmoCount = GameObject.FindGameObjectWithTag("LeftWeaponAmmo").GetComponent<Text>();
		Anim = GetComponent<Animator>();
	}

	void Update ()
	{
		myTime = myTime + Time.deltaTime;
		AmmoCount.text = Ammo.ToString ();
		Debug.DrawRay (ShotSpawn_1.transform.position, -ShotSpawn_1.transform.right, Color.red);

		if (Input.GetMouseButtonDown (0)) {
			Anim.SetBool("Spin Down", false);
			Anim.SetBool ("Firing", true);
		}

		if (Input.GetMouseButton (0) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true) {
			//Anim.SetBool ("Firing", true);
			nextFire = myTime + fireDelta;
			MuzzleFlash_1.Play ();
			Shoot_1 ();
			MuzzleFlash_2.Play ();
			Shoot_2 ();
			Ammo--;
			nextFire = nextFire - myTime;
			myTime = 0.0f;
		}

		if (Input.GetMouseButtonUp (0)) {
			Anim.SetBool ("Firing", false);
			Anim.SetBool("Spin Down", true);
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			StartCoroutine (Player.Left_Reload ());
			StartCoroutine(Reload ());
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			StartCoroutine( PistolSwap());
		}
	}

	private void Shoot_1(){
		Debug.Log ("Shooting");
		GameObject minigunbullet_I = (GameObject)Instantiate (minigunBullet,ShotSpawn_1.transform.position, Quaternion.identity);
		minigunbullet_I.GetComponent<Rigidbody> ().AddForce (-transform.right * 2500f, ForceMode.VelocityChange);
		Destroy (minigunbullet_I, 0.75f);
		Vector3 forward = ShotSpawn_1.transform.TransformDirection (ShotSpawn_1.transform.forward);
		RaycastHit shotHit;
		if(Physics.Raycast(ShotSpawn_1.transform.position, -ShotSpawn_1.transform.right,out shotHit,700)){
			//Debug.Log ("Hit soemthing at: " + shotHit.distance);
			Debug.Log ("Hit object: " + shotHit.transform.gameObject);
			if (shotHit.collider.tag == "Enemy") {
				GameObject Enemy = shotHit.collider.gameObject;
				Enemy.GetComponent<Enemy> ().Hit (3);
				Destroy (minigunbullet_I, 0.75f);
			}
		}

	}

	private void Shoot_2(){
		Debug.Log ("Shooting");
		GameObject minigunbullet_II = (GameObject)Instantiate (minigunBullet,ShotSpawn_2.transform.position, Quaternion.identity);
		minigunbullet_II.GetComponent<Rigidbody> ().AddForce (-transform.right * 2500f, ForceMode.VelocityChange);
		Destroy (minigunbullet_II, 0.75f);
		//Vector3 forward = ShotSpawn_2.transform.TransformDirection (ShotSpawn_2.transform.forward);
		RaycastHit shotHit;
		if(Physics.Raycast(ShotSpawn_2.transform.position, -ShotSpawn_2.transform.right,out shotHit,700)){
			//Debug.Log ("Hit soemthing at: " + shotHit.distance);
			Debug.Log ("Hit object: " + shotHit.transform.gameObject);
			if (shotHit.collider.tag == "Enemy") {
				GameObject Enemy = shotHit.collider.gameObject;
				Enemy.GetComponent<Enemy> ().Hit (3);
				Destroy (minigunbullet_II, 0.75f);
			}
		}

	}

	private IEnumerator Reload(){
		Reloading = true;
		yield return new WaitForSeconds(0.98f);
		Ammo = 150;
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
