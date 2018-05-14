using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Right_AssaultRifle : MonoBehaviour {

	public GameObject topObject;
	public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 0.25f;
	private float nextFire = 0.25f;
	private float myTime = 0.0f;
	public GameObject assaultRifleBullet;
	public int Ammo = 50;
	public Text AmmoCount;
	public bool Reloading;
	public bool dropped = false;

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
		Debug.DrawRay (ShotSpawn.transform.position, -ShotSpawn.transform.right, Color.red);
		if (Input.GetMouseButton (1) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true) {
			nextFire = myTime + fireDelta;
			MuzzleFlash.Play ();
			Shoot ();
			Ammo--;
			nextFire = nextFire - myTime;
			myTime = 0.0f;
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			StartCoroutine (Player.Right_Reload ());
			StartCoroutine(Reload ());
		}

		if (Input.GetKeyDown (KeyCode.L)) {
			StartCoroutine( PistolSwap());
		}
	}

	private void Shoot(){
		Debug.Log ("Shooting");
		GameObject ARbullet_I = (GameObject)Instantiate (assaultRifleBullet,ShotSpawn.transform.position, Quaternion.identity);
		ARbullet_I.GetComponent<Rigidbody> ().AddForce (-transform.right * 2000f, ForceMode.VelocityChange);
		Destroy (ARbullet_I, 0.7f);
		//Vector3 forward = ShotSpawn.transform.TransformDirection (ShotSpawn.transform.forward);
		/*RaycastHit shotHit;
		if(Physics.Raycast(ShotSpawn.transform.position, -ShotSpawn.transform.right,out shotHit,600)){
			//Debug.Log ("Hit soemthing at: " + shotHit.distance);
			Debug.Log ("Hit object: " + shotHit.transform.gameObject);
			if (shotHit.collider.tag == "Enemy") {
				GameObject Enemy = shotHit.collider.gameObject;
				Enemy.GetComponent<Enemy> ().Hit (2);
				Destroy (ARbullet_I, 0.7f);
			}
		}*/

	}

	private IEnumerator Reload(){
		Reloading = true;
		yield return new WaitForSeconds(0.98f);
		Ammo = 50;
		Reloading = false;
	}

	public IEnumerator PistolSwap(){
		dropped = true;
		yield return new WaitForSeconds(1.25f);
		this.gameObject.transform.parent = null;
		this.GetComponent<Rigidbody>().useGravity=true;
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<Rigidbody>().AddForce(new Vector3(30, -0.5f, 0),ForceMode.VelocityChange);
		yield return new WaitForSeconds(0.2f);
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<MeshCollider>().enabled = true;
		Destroy(this.gameObject, 10.0f);
		Destroy(topObject, 10.0f);
	}
}
