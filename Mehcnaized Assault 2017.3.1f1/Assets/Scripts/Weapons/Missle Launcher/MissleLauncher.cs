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
    private float fireDelta = 0.25f;
    private float nextFire = 0.25f;
    private float myTime = 0.0f;
    public GameObject ammoTextObject;
	public Text AmmoText;
	public GameObject ReloadImage;
	public WeaponStatus WS = WeaponStatus.ReadytoFire;

    public GameObject misslePoolParent;
    public List<GameObject> misslePool = new List<GameObject>();
    [SerializeField]
    private int missleCount;

    void Awake () {
		AmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
		PC = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		ReloadImage = GameObject.FindGameObjectWithTag("LeftShoulderReloadImage");
		ReloadImage.SetActive(false);

        misslePoolParent = GameObject.FindGameObjectWithTag("ShoulderBulletParent");

        for (int i = 0; i < misslePool.Count; i++) {
            GameObject S_Missle = Instantiate(missleObject);
            misslePool[i] = S_Missle;
            misslePool[i].transform.parent = misslePoolParent.transform;
            misslePool[i].SetActive(false);
        }
        Debug.Log("bulletPool count is " + misslePool.Count);
    }
	
	// Update is called once per frame
	void Update ()
	{
        myTime = myTime + Time.deltaTime;
        if (AmmoText.gameObject.activeSelf == true) {
			AmmoText.text = Ammo.ToString ();
		}
        if (PC.targetedLeftEnemy != null || PC.targetedRightEnemy != null) {
			Tracking ();
		} else {
			missleLauncher_Y.transform.localEulerAngles = new Vector3(0f,0f,0f);
		}

		if (Input.GetKey (KeyCode.F) && Ammo > 0 && myTime > nextFire && PC.canMove == true && PC.canMove == true) {
            //Debug.Log ("Hitting F");
            nextFire = myTime + fireDelta;
            Shoot ();
            nextFire = nextFire - myTime;
            myTime = 0.0f;
        }

		if (Ammo <= 0 && WS != WeaponStatus.Reloading) {
			StartCoroutine(reload());
		}
	}

	void Tracking(){
        /*Vector3 targetDirection = PC.targetedLeftEnemy.transform.position - missleLauncher_Y.transform.position;
		float angle = Mathf.Atan2 (-targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg;
		Vector3 q = Quaternion.AngleAxis (angle, -Vector3.forward).eulerAngles;
        missleLauncher_Y.transform.rotation = Quaternion.Euler(new Vector3(0f, q.y, 0f));*/

        //Debug.Log("Tracking Updated");
        //missleLauncher_X.transform.rotation = Quaternion.Euler(new Vector3(q.x, 0f, 0f));
	}

	void Shoot ()
	{
        #region Object Pool
        Ammo--;
		//Debug.Log("Shooting");
        if (PC.targetedLeftEnemy != null || PC.targetedRightEnemy != null) {
			//GameObject Missle_I = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
            misslePool[missleCount].transform.position = missleSpawn.transform.position;
            misslePool[missleCount].GetComponent<Missle>().target = PC.targetedLeftEnemy.transform;
            misslePool[missleCount].GetComponent<Missle>().lockedOn = true;
            misslePool[missleCount].SetActive(true);

            //Destroy (Missle_I, 10.0f);
        } else {
			//GameObject Missle_II = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
            misslePool[missleCount].transform.position = missleSpawn.transform.position;
            misslePool[missleCount].transform.rotation = missleSpawn.transform.rotation;
            misslePool[missleCount].GetComponent<Rigidbody> ().AddForce (missleSpawn.transform.forward * 100f, ForceMode.VelocityChange);
            misslePool[missleCount].SetActive(true);
            StartCoroutine(misslePool[missleCount].GetComponent<Missle>().DestroyMissle(10f, misslePool[missleCount].transform.position));
            //Destroy (Missle_II, 10.0f);
        }


        if (missleCount >= misslePool.Count - 1) {
            Debug.Log("pool count reset");
            missleCount = 0;
        } else {
            Debug.Log("pool count add 1");
            missleCount++;
        }
        #endregion
    }

    private IEnumerator reload() {
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