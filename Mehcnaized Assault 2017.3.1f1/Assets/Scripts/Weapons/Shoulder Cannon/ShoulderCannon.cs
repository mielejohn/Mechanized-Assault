using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum CannonStatus{Deployed, Retracted};

public class ShoulderCannon : MonoBehaviour {

    [Header("Game Manager")]
    public GameManager GM;

	[Header("Player")]
	public PlayerController Player;

	[Header("Bullet Items")]
	public GameObject shotSpawn;
	public GameObject cannonShot;

	[Header("Ammo and Reloading")]
	public int Ammo = 5;
    private int ammoReference;
    private int extraAmmo = 20;
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

    public GameObject cannonPoolParent;
    public List<GameObject> cannonPool = new List<GameObject>();
    [SerializeField]
    private int cannonCount;

    public bool Dropped = false;
    public GameObject jetesinedParticles;

    // Use this for initialization
    void Awake () {
		AmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        ReloadImage = GameObject.FindGameObjectWithTag("LeftShoulderReloadImage");
		ReloadImage.SetActive(false);
        ammoReference = Ammo;
        cannonPoolParent = GameObject.FindGameObjectWithTag("ShoulderBulletParent");

        for (int i = 0; i < cannonPool.Count; i++) {
            GameObject S_Cannon = Instantiate(cannonShot);
            cannonPool[i] = S_Cannon;
            cannonPool[i].transform.parent = cannonPoolParent.transform;
            cannonPool[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
		Debug.DrawRay (shotSpawn.transform.position, shotSpawn.transform.up, Color.red);
		if (AmmoText.gameObject.activeSelf == true) {
            AmmoText.text = "0" + Ammo.ToString() + " / " + extraAmmo;
        }

        if (Input.GetKeyDown(KeyCode.E) && currentStatus == CannonStatus.Retracted && Player.canMove == true) {
            StartCoroutine(WeaponUp());
        } else if (Input.GetKeyDown(KeyCode.E) && currentStatus == CannonStatus.Deployed && Player.canMove == true) {
            currentStatus = CannonStatus.Retracted;
            Anim.SetBool("Bring Up", false);
            Anim.SetBool("Put Away", true);

        }

        if (Input.GetKeyDown (KeyCode.F) && Ammo > 0 && canFire == true && Player.canMove == true) {
			MuzzleFlash.Play ();
			Shoot ();
		}

        if (GM.prevState.Buttons.Y == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.Y == XInputDotNetPure.ButtonState.Pressed && currentStatus == CannonStatus.Retracted && Player.canMove == true) {
            StartCoroutine(WeaponUp());
        } else if (GM.prevState.Buttons.Y == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.Y == XInputDotNetPure.ButtonState.Pressed && currentStatus == CannonStatus.Deployed && Player.canMove == true) {
            currentStatus = CannonStatus.Retracted;
            Anim.SetBool("Bring Up", false);
            Anim.SetBool("Put Away", true);

        }

        if (GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && Ammo > 0 && canFire == true && Player.canMove == true) {
            MuzzleFlash.Play();
            Shoot();
        }
        //GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed

        if (Ammo <= ammoReference / 4 && Player.shoulderWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 1);
        } else if (Ammo > ammoReference / 4 && Player.shoulderWeaponLowAmmoNotice.activeSelf == true) {
            Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);
        }

        if (Ammo <= 0 && extraAmmo != 0) {
			StartCoroutine (Reload ());
		}

        if (Ammo <= 0 && extraAmmo == 0) {
            StartCoroutine(BreakOff());
        }   
    }

	private void Shoot(){
		audioSource.Play();
		Debug.Log ("Shooting");
		StartCoroutine(FireAnimation());
        
		Ammo--;

        #region Object Pool

        Debug.Log("About to fire Left");

        cannonPool[cannonCount].transform.position = shotSpawn.transform.position;
        cannonPool[cannonCount].SetActive(true);
        StartCoroutine(cannonPool[cannonCount].GetComponent<CannonShot>().WaitTillInActive(1.0f));
        cannonPool[cannonCount].transform.rotation = shotSpawn.transform.rotation;
        cannonPool[cannonCount].GetComponent<Rigidbody>().AddForce(shotSpawn.transform.up * 2000f, ForceMode.VelocityChange);

        Debug.Log("Just fired Left");

        if (cannonCount >= cannonPool.Count - 1) {
            Debug.Log("pool count reset");
            cannonCount = 0;
        } else {
            Debug.Log("pool count add 1");
            cannonCount++;
        }
        #endregion
        //Destroy (cannonShot_I, 3.0f);
    }

    private IEnumerator Reload(){
		Debug.Log("Reloading");
		AmmoText.gameObject.SetActive(false);
        Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);
        ReloadImage.SetActive(true);
		yield return new WaitForSeconds(2.0f);
		AmmoText.gameObject.SetActive(true);
		ReloadImage.SetActive(false);
        Ammo = ammoReference;
        extraAmmo -= ammoReference;
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
		yield return new WaitForSeconds(4.10f);
		Anim.SetBool("Fired",false);
		canFire = true;
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
