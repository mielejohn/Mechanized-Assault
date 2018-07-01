using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum CannonStatus{Deployed, Retracted};

public class ShoulderCannon : MonoBehaviour {

	[Header("Player")]
	public PlayerController PC;

	[Header("Bullet Items")]
	public GameObject shotSpawn;
	public GameObject cannonShot;

	[Header("Ammo and Reloading")]
	public int Ammo = 5;
	public GameObject ammoTextObject;
	public Text AmmoText;
	public GameObject ReloadImage;

	[Header("Aniamtions")]
	public Animator Anim;
    [EnumToggleButtons]
    public CannonStatus currentStatus;

	[Header("Main Body")]
	public GameObject CannonBody;

	[Header("Cannon fire status")]
	private bool canFire = false;

	[Header("Cannon Effects")]
	public ParticleSystem MuzzleFlash;
	public AudioSource audioSource;

	// Use this for initialization
	void Awake () {
		AmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
		PC = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		ReloadImage = GameObject.FindGameObjectWithTag("LeftShoulderReloadImage");
		ReloadImage.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.DrawRay (shotSpawn.transform.position, shotSpawn.transform.up, Color.red);
		if (AmmoText.gameObject.activeSelf == true) {
			AmmoText.text = Ammo.ToString ();
		}

		if (Input.GetKeyDown (KeyCode.F) && Ammo > 0 && canFire == true && PC.canMove == true) {
			//Debug.Log ("Hitting F");
			MuzzleFlash.Play ();
			Shoot ();
		}

		/*if (Input.GetKeyDown (KeyCode.F)) {
			MuzzleFlash.Play ();
		}*/

		if (Ammo <= 0) {
			StartCoroutine (Reload ());
		}

		if (Input.GetKeyDown (KeyCode.M) && currentStatus == CannonStatus.Retracted && PC.canMove == true) {
			StartCoroutine(WeaponUp());
			//Anim.SetBool("Put Away",false);
			//Anim.SetBool("Bring Up",true);
			//CS = CannonStatus.Deployed;
		} else if (Input.GetKeyDown (KeyCode.M) && currentStatus == CannonStatus.Deployed && PC.canMove == true) {
			currentStatus = CannonStatus.Retracted;
			Anim.SetBool("Bring Up",false);
			Anim.SetBool("Put Away",true);

		}
	}

	/*void LateUpdate ()
	{
		if (PC.Enemy != null&&CS == CannonStatus.Deployed) {
			//CannonBody.transform.LookAt(PC.Enemy.transform);
			Vector3 relativePos = CannonBody.transform.position - PC.Enemy.transform.position;
			Quaternion rotation = Quaternion.LookRotation (relativePos,Vector3.forward);
			CannonBody.transform.rotation = rotation;
			CannonBody.transform.LookAt (PC.Enemy.transform.position - CannonBody.transform.position, transform.forward);
		}
	}*/

	private void Shoot(){
		audioSource.Play();
		Debug.Log ("Shooting");
		StartCoroutine(FireAnimation());
		GameObject cannonShot_I = (GameObject)Instantiate (cannonShot,shotSpawn.transform.position, Quaternion.identity);
		cannonShot_I.GetComponent<Rigidbody> ().AddForce (shotSpawn.transform.up * 4000f, ForceMode.VelocityChange);
		Ammo--;

		//Anim.SetBool("Fired",false);
		Destroy (cannonShot_I, 3.0f);
		//MuzzleFlash.Stop();
	}

	private IEnumerator Reload(){
		Debug.Log("Reloading");
		AmmoText.gameObject.SetActive(false);
		ReloadImage.SetActive(true);
		yield return new WaitForSeconds(2.0f);
		AmmoText.gameObject.SetActive(true);
		ReloadImage.SetActive(false);
		Ammo = 5;
	}

	private IEnumerator WeaponUp(){
		Anim.SetBool("Put Away",false);
		Anim.SetBool("Bring Up",true);
		yield return new WaitForSeconds(3.00f);		
		currentStatus = CannonStatus.Deployed;
		canFire = true;
	}

	private IEnumerator FireAnimation(){
		Debug.Log("Firing");
		canFire = false;
		Anim.SetBool("Fired",true);
		//Anim.SetBool("Fired",false);
		yield return new WaitForSeconds(4.10f);
		Anim.SetBool("Fired",false);
		canFire = true;
	}
}
