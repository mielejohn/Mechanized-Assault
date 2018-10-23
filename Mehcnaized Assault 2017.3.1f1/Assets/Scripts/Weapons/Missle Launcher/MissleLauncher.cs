using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponStatus{Reloading, ReadytoFire};

public class MissleLauncher : MonoBehaviour {

    public GameManager GM;
    public PlayerController Player;
	public GameObject missleLauncher_Y;
	public GameObject missleLauncher_X;
	public GameObject missleSpawn;
	public GameObject missleObject;
	public int Ammo = 12;
    private int ammoReference;
    public int extraAmmo = 0;
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

    public AudioSource audioSource;
    public bool Dropped = false;
    public GameObject jetesinedParticles;

    void Awake () {
		AmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		ReloadImage = GameObject.FindGameObjectWithTag("LeftShoulderReloadImage");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
		ReloadImage.SetActive(false);
        ammoReference = Ammo;
        misslePoolParent = GameObject.FindGameObjectWithTag("ShoulderBulletParent");

        for (int i = 0; i < misslePool.Count; i++) {
            GameObject S_Missle = Instantiate(missleObject);
            misslePool[i] = S_Missle;
            misslePool[i].transform.parent = misslePoolParent.transform;
            misslePool[i].SetActive(false);
        }
        Debug.Log("bulletPool count is " + misslePool.Count);
    }
	
	void Update ()
	{
        myTime = myTime + Time.deltaTime;
        if (AmmoText.gameObject.activeSelf == true) {
			AmmoText.text = Ammo.ToString() + " / " + extraAmmo;
		}
        if (Player.targetedLeftEnemy != null || Player.targetedRightEnemy != null) {
			Tracking ();
		} else {
			missleLauncher_Y.transform.localEulerAngles = new Vector3(0f,0f,0f);
		}
        if(Dropped == false){
		    if (Input.GetKey (KeyCode.F) && Ammo > 0 && myTime > nextFire && Player.canMove == true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                Shoot ();
                audioSource.PlayOneShot(audioSource.clip);
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }


            if (GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && Ammo > 0 && myTime > nextFire && Player.canMove == true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                Shoot();
                audioSource.Play();
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }
        }

        if (Ammo <= ammoReference / 4 && Player.shoulderWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 1);
        } else if (Ammo > ammoReference / 4 && Player.shoulderWeaponLowAmmoNotice.activeSelf == true) {
            Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);
        }

        if (Ammo <= 0 && WS != WeaponStatus.Reloading && extraAmmo != 0) {
			StartCoroutine(reload());
		}

        if (Ammo <= 0 && extraAmmo == 0) {
            StartCoroutine(BreakOff());
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
        if (Player.targetedLeftEnemy != null || Player.targetedRightEnemy != null) {
			//GameObject Missle_I = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
            misslePool[missleCount].transform.position = missleSpawn.transform.position;
            misslePool[missleCount].GetComponent<Missle>().target = Player.targetedLeftEnemy.transform;
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
            //Debug.Log("pool count reset");
            missleCount = 0;
        } else {
            //Debug.Log("pool count add 1");
            missleCount++;
        }
        #endregion
    }

    private IEnumerator reload() {
        Debug.Log("Reloading");
        WS = WeaponStatus.Reloading;
        AmmoText.gameObject.SetActive(false);
        Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);
        ReloadImage.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        AmmoText.gameObject.SetActive(true);
        ReloadImage.SetActive(false);
        Ammo = ammoReference;
        extraAmmo -= ammoReference;
        WS = WeaponStatus.ReadytoFire;
    }

    private IEnumerator BreakOff() {
        Dropped = true;
        Player.shoulderWeaponNotice.text = "DETACHED";
        yield return new WaitForSeconds(0.50f);
        this.gameObject.transform.parent = null;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        jetesinedParticles.SetActive(true);
        this.GetComponent<Rigidbody>().AddForce(-this.transform.forward * 0.6f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.3f);
        this.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(10.0f);
        this.gameObject.SetActive(false);
    }
}