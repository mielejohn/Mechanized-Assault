using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponStatus{Reloading, ReadytoFire};

public class MissleLauncher : MonoBehaviour {
	public PlayerController PC;
	public GameObject missleLauncher_Y;
	public GameObject missleLauncher_X;
	public GameObject missleSpawn;
	public GameObject missleObject;
	public int Ammo = 12;
	public GameObject ammoTextObject;
	public Text AmmoText;
	public GameObject ReloadImage;
	public WeaponStatus WS = WeaponStatus.ReadytoFire;
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
		if (AmmoText.gameObject.activeSelf == true) {
			AmmoText.text = Ammo.ToString ();
		}
        if (PC.targetedLeftEnemy != null || PC.targetedRightEnemy != null) {
			Tracking ();
		} else {
			missleLauncher_Y.transform.localEulerAngles = new Vector3(Mathf.Lerp(missleLauncher_Y.transform.rotation.x,0.0f,0.1f), Mathf.Lerp(missleLauncher_Y.transform.rotation.y,0.0f,0.1f), Mathf.Lerp(missleLauncher_Y.transform.rotation.z,0.0f,0.1f));
		}

		if (Input.GetKeyDown (KeyCode.F) && Ammo > 0) {
			//Debug.Log ("Hitting F");
			Shoot ();
		}

		if (Ammo <= 0 && WS != WeaponStatus.Reloading) {
			StartCoroutine(Reload());
		}
	}

	void Tracking(){
		/*Vector3 targetDirection = PC.targetedEnemy.transform.position - missleLauncher_Y.transform.position;
		float angle = Mathf.Atan2 (-targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg;
		Vector3 q = Quaternion.AngleAxis (angle, -Vector3.forward).eulerAngles;
		missleLauncher_Y.transform.rotation = Quaternion.Slerp(missleLauncher_Y.transform.rotation, Quaternion.Euler(new Vector3(0f, q.y, 0f)) ,Time.deltaTime * 2);
		missleLauncher_X.transform.rotation = Quaternion.Slerp(missleLauncher_X.transform.rotation, Quaternion.Euler(new Vector3(q.z + 124f, 0f, 0f)) ,Time.deltaTime * 2);

		/*Vector3 target = PC.Enemy.transform.position - PC.gameObject.transform.position;
		target.x = 0.0f;
		target.z = 0.0f;
		missleLauncher_Y.transform.LookAt(target);*/

		//missleLauncher_Y.transform.LookAt(PC.targetedEnemy.transform);
		/*Vector3 Rotation_targetDir = PC.Enemy.transform.position - missleLauncherRotationPoint.transform.position;
		float Rotation_step = 5 * Time.deltaTime;
		Vector3 Rotation_newDir = Vector3.RotateTowards(transform.forward, Rotation_targetDir, Swivel_step, 0.0f);
		missleLauncherRotationPoint.transform.rotation = Quaternion.LookRotation(new Vector3(Rotation_targetDir.x, 0.0f, 0.0f));*/
	}

	void Shoot ()
	{
		Ammo--;
		//Debug.Log("Shooting");
        if (PC.targetedLeftEnemy != null || PC.targetedRightEnemy != null) {
			GameObject Missle_I = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
			Destroy (Missle_I, 10.0f);
		} else {
			GameObject Missle_II = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
			Missle_II.GetComponent<Rigidbody> ().AddForce (missleSpawn.transform.forward * 100f, ForceMode.VelocityChange);
			Destroy (Missle_II, 10.0f);
		}
	}

	private IEnumerator Reload(){
		Debug.Log("Reloading");
		WS = WeaponStatus.Reloading;
		AmmoText.gameObject.SetActive(false);
		ReloadImage.SetActive(true);
		yield return new WaitForSeconds(2.0f);
		AmmoText.gameObject.SetActive(true);
		ReloadImage.SetActive(false);
		Ammo = 12;
		WS = WeaponStatus.ReadytoFire;
	}
}
