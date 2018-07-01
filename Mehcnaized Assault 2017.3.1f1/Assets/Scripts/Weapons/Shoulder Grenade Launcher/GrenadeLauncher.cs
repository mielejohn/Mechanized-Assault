using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum GrenadeLauncherStatus { Deployed, Retracted };

public class GrenadeLauncher : MonoBehaviour {

    [Header("Player")]
    public PlayerController PC;

    [Header("Bullet Items")]
    public GameObject shotSpawn;
    public GameObject Grenade;

    [Header("Ammo and Reloading")]
    public int Ammo = 8;
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
    void Start () {
        AmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ReloadImage = GameObject.FindGameObjectWithTag("LeftShoulderReloadImage");
        ReloadImage.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        Debug.DrawRay(shotSpawn.transform.position, shotSpawn.transform.forward, Color.red);
        if (AmmoText.gameObject.activeSelf == true)
        {
            AmmoText.text = Ammo.ToString();
        }

        if (Input.GetKeyDown(KeyCode.F) && Ammo > 0 && canFire == true && PC.canMove == true)
        {
            //Debug.Log ("Hitting F");
           // MuzzleFlash.Play();
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.M) && currentStatus == CannonStatus.Retracted && PC.canMove == true)
        {
            StartCoroutine(WeaponUp());
            //Anim.SetBool("Put Away",false);
            //Anim.SetBool("Bring Up",true);
            //CS = CannonStatus.Deployed;
        }
        else if (Input.GetKeyDown(KeyCode.M) && currentStatus == CannonStatus.Deployed && PC.canMove == true)
        {
            currentStatus = CannonStatus.Retracted;
            Anim.SetBool("Bring Up", false);
            Anim.SetBool("Put Away", true);

        }
    }

    private void Shoot()
    {
        //audioSource.Play();
        Debug.Log("Shooting");
        StartCoroutine(FireAnimation());
        GameObject grenadeShot_I = (GameObject)Instantiate(Grenade, shotSpawn.transform.position, shotSpawn.transform.rotation);
        grenadeShot_I.GetComponent<Rigidbody>().AddForce(shotSpawn.transform.forward * 200f, ForceMode.VelocityChange);
        StartCoroutine(grenadeShot_I.GetComponent<GrenadeShell>().GrenadeTimer());
        Ammo--;

        //Anim.SetBool("Fired",false);
        //Destroy(grenadeShot_I, 3.0f);
        //MuzzleFlash.Stop();
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reloading");
        AmmoText.gameObject.SetActive(false);
        ReloadImage.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        AmmoText.gameObject.SetActive(true);
        ReloadImage.SetActive(false);
        Ammo = 5;
    }

    private IEnumerator WeaponUp()
    {
        Anim.SetBool("Put Away", false);
        Anim.SetBool("Bring Up", true);
        yield return new WaitForSeconds(2f);
        currentStatus = CannonStatus.Deployed;
        canFire = true;
    }

    private IEnumerator FireAnimation()
    {
        Debug.Log("Firing");
        canFire = false;
        Anim.SetBool("Fired", true);
        //Anim.SetBool("Fired",false);
        yield return new WaitForSeconds(0.5f);
        Anim.SetBool("Fired", false);
        yield return new WaitForSeconds(0.7f);
        canFire = true;
    }
}
