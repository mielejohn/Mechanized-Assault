using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float Damage;
    public GameObject Player;
    private float DistanceToPlayer = 0f;
    [Header("Weapon Range")]
    public float shortRange;
    public float mediumRange;
    public float longRange;
    public float outOfRange;

    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        DistanceToPlayer = Vector3.Distance(this.transform.position, Player.transform.position);
        Vector3 rayDir = GetComponent<Rigidbody>().velocity;
        Debug.DrawRay(transform.position, rayDir, Color.red);
        RaycastHit shotHit;
        if (Physics.Raycast(transform.position, rayDir, out shotHit, 20f) && DistanceToPlayer < outOfRange)
        {
            if (shotHit.collider.tag == "Enemy")
            {
                #region Bullet Dropoff

                if(DistanceToPlayer < shortRange) {
                    CloseHit(Damage, shotHit);
                    Debug.Log("Destroyed close range");
                    this.GetComponent<TrailRenderer>().enabled = false;
                    this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    this.gameObject.transform.position = new Vector3(0, 0, 0);
                    this.gameObject.SetActive(false);
                } else if(DistanceToPlayer >= shortRange && DistanceToPlayer < mediumRange) {
                    MediumHit(Damage, shotHit);
                    Debug.Log("Destroyed medium range");
                    this.GetComponent<TrailRenderer>().enabled = false;
                    this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    this.gameObject.transform.position = new Vector3(0, 0, 0);
                    this.gameObject.SetActive(false);
                } else if (DistanceToPlayer >= mediumRange && DistanceToPlayer < longRange) {
                    LongRangeHit(Damage, shotHit);
                    Debug.Log("Destroyed long range");
                    this.GetComponent<TrailRenderer>().enabled = false;
                    this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    this.gameObject.transform.position = new Vector3(0, 0, 0);
                    this.gameObject.SetActive(false);
                }
                #endregion
            }
        }
    }

    public void CloseHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage * 1.5f);
    }

    public void MediumHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage);
    }

    public void LongRangeHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage -1.5f);
    }

    public IEnumerator WaitDestroy(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<TrailRenderer>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.gameObject.transform.position = new Vector3(0, 0, 0);
        this.gameObject.SetActive(false);
    }
}