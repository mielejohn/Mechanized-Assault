using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Right_Minigun : MonoBehaviour {

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

    public GameObject bulletPoolParent;
    public List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField]
    private int poolCount;

    public List<GameObject> bulletPool_2 = new List<GameObject>();
    [SerializeField]
    private int poolCount_2;

    void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		AmmoCount = GameObject.FindGameObjectWithTag("RightWeaponAmmo").GetComponent<Text>();
		Anim = GetComponent<Animator>();

        bulletPoolParent = GameObject.FindGameObjectWithTag("RightBulletParent");

        for (int i = 0; i < bulletPool.Count; i++) {
            GameObject LMG_Bullet_1 = Instantiate(minigunBullet);
            bulletPool[i] = LMG_Bullet_1;
            bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].SetActive(false);
        }

        for (int i = 0; i < bulletPool_2.Count; i++) {
            GameObject LMG_Bullet_2 = Instantiate(minigunBullet);
            bulletPool_2[i] = LMG_Bullet_2;
            bulletPool_2[i].transform.parent = bulletPoolParent.transform;
            bulletPool_2[i].SetActive(false);
        }
    }

	void Update () {
		myTime = myTime + Time.deltaTime;
		AmmoCount.text = Ammo.ToString ();
		Debug.DrawRay (ShotSpawn_1.transform.position, -ShotSpawn_1.transform.right, Color.red);

		if (Input.GetMouseButtonDown (1) && Player.canMove == true) {
			Anim.SetBool("Spin Down", false);
			Anim.SetBool ("Firing", true);
		}

		if (Input.GetMouseButton (1) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
			nextFire = myTime + fireDelta;
			MuzzleFlash_1.Play ();
			Shoot_1 ();
			MuzzleFlash_2.Play ();
			Shoot_2 ();
			Ammo--;
			nextFire = nextFire - myTime;
			myTime = 0.0f;
		}

		if (Input.GetMouseButtonUp (1) && Player.canMove == true) {
			Anim.SetBool ("Firing", false);
			Anim.SetBool("Spin Down", true);
		}

		if (Input.GetKeyDown (KeyCode.R) && Player.canMove == true) {
			StartCoroutine (Player.Right_Reload ());
			StartCoroutine(Reload ());
		}

		if (Input.GetKeyDown (KeyCode.L) && Player.canMove == true) {
			StartCoroutine( PistolSwap());
		}
	}

	private void Shoot_1(){ 
        #region Object Pool


        bulletPool[poolCount].transform.position = ShotSpawn_1.transform.position;
        bulletPool[poolCount].SetActive(true);
        StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitTillInActive(0.7f));
        bulletPool[poolCount].transform.rotation = ShotSpawn_1.transform.rotation;
        bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(-transform.right * 2500f, ForceMode.VelocityChange);

        if (poolCount >= bulletPool.Count - 1) {
            Debug.Log("pool count reset");
            poolCount = 0;
        } else {
            Debug.Log("pool count add 1");
            poolCount++;
        }
        #endregion
    }

    private void Shoot_2(){
        #region Object Pool

        bulletPool_2[poolCount].transform.position = ShotSpawn_2.transform.position;
        bulletPool_2[poolCount].SetActive(true);
        StartCoroutine(bulletPool_2[poolCount].GetComponent<Bullet>().WaitTillInActive(0.7f));
        bulletPool_2[poolCount].transform.rotation = ShotSpawn_2.transform.rotation;
        bulletPool_2[poolCount].GetComponent<Rigidbody>().AddForce(-transform.right * 2500f, ForceMode.VelocityChange);

        if (poolCount_2 >= bulletPool_2.Count - 1) {
            Debug.Log("pool count reset");
            poolCount_2 = 0;
        } else {
            Debug.Log("pool count add 1");
            poolCount_2++;
        }
        #endregion
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
		this.GetComponent<Rigidbody>().AddForce(new Vector3(30, -0.5f, 0),ForceMode.VelocityChange);
		yield return new WaitForSeconds(0.2f);
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<MeshCollider>().enabled = true;
		Destroy(this.gameObject, 10.0f);
		//Destroy(topObject, 10.0f);
	}
}
