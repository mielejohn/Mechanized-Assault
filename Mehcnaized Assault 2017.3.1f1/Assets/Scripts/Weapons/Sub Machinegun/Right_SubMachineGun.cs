using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Right_SubMachineGun : MonoBehaviour {

    public GameManager GM;
    public GameObject topObject;
	public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 0.10f;
	private float nextFire = 0.10f;
	private float myTime = 0.0f;
	public GameObject subMachinegunBullet;
	public int Ammo = 100;
    private int ammoReference;
    public Text AmmoCount;
	public bool Reloading;
	public bool dropped = false;

	[Header("Muzzle Effects")]
	public ParticleSystem MuzzleFlash;
	public AudioSource audioSource;
    public AudioClip shotSound;
    public AudioClip reloadSound;

    public GameObject bulletPoolParent;
    public List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField]
    private int poolCount;

    void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		AmmoCount = GameObject.FindGameObjectWithTag("RightWeaponAmmo").GetComponent<Text>();
        bulletPoolParent = GameObject.FindGameObjectWithTag("RightBulletParent");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        ammoReference = Ammo;
        for (int i = 0; i < bulletPool.Count; i++) {
            GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            bulletPool[i] = RSMG_Bullet;
            bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].SetActive(false);
        }
        //Debug.Log("bulletPool count is " + bulletPool.Count);
    }

	void Update () {
		myTime = myTime + Time.deltaTime;
		AmmoCount.text = Ammo.ToString ();
		Debug.DrawRay (ShotSpawn.transform.position, -ShotSpawn.transform.right, Color.red);

        if (!GM.prevState.IsConnected) {
            if (Input.GetMouseButton (1) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
			    nextFire = myTime + fireDelta;
			    MuzzleFlash.Play();
                audioSource.PlayOneShot(shotSound);
			    Shoot ();
			    Ammo--;
			    nextFire = nextFire - myTime;
			    myTime = 0.0f;
		    }
        }

        if (GM.prevState.IsConnected) {
            if (GM.prevState.Triggers.Right > 0.45f && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash.Play();
                audioSource.PlayOneShot(shotSound);
                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }
        }

        if(Ammo <= ammoReference / 4 && Player.rightWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 1);
        } else if(Ammo > ammoReference / 4 && Player.rightWeaponLowAmmoNotice.activeSelf == true) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 0);
        }

        if (Ammo <= 0 && Reloading == false) {
            StartCoroutine(Player.Right_Reload());
            audioSource.PlayOneShot(reloadSound);
            StartCoroutine(Reload());
        }

		if (Input.GetKeyDown (KeyCode.L) && Player.canMove == true) {
			StartCoroutine( PistolSwap());
		}
	}

	private void Shoot(){
		Debug.Log ("Shooting");
        /*RaycastHit shotHit;
        if (Physics.Raycast(ShotSpawn.transform.position, ShotSpawn.transform.right, out shotHit, 300f)) {

            Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy") {
                subMachinegunBullet.GetComponent<Bullet>().MediumHit(2, shotHit);
            }
        }*/
        #region Object Pool

        if (bulletPool[poolCount] == null) {
            GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            bulletPool[poolCount] = RSMG_Bullet;
            bulletPool[poolCount].transform.parent = bulletPoolParent.transform;
            bulletPool[poolCount].SetActive(false);
        }

        bulletPool[poolCount].SetActive(true);
        bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = false;
        bulletPool[poolCount].transform.position = ShotSpawn.transform.position;
        bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = true;
        bulletPool[poolCount].transform.rotation = ShotSpawn.transform.rotation;
        bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(transform.right * 200f, ForceMode.VelocityChange);
        StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitDestroy(0.7f));

        if (poolCount >= bulletPool.Count - 1) {
            Debug.Log("pool count reset");
            poolCount = 0;
        } else {
            Debug.Log("pool count add 1");
            poolCount++;
        }
        #endregion


    }

    private IEnumerator Reload(){
		Reloading = true;
		yield return new WaitForSeconds(0.98f);
		Ammo = 100;
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
	}
}
