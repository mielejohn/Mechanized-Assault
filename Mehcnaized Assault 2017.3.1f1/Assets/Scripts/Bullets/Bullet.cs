using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float Damage;
    public GameObject Player;
    float DistanceToPlayer = 0f;
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
        //Debug.Log("Bullet distance to player" + DistanceToPlayer);

        Vector3 rayDir = GetComponent<Rigidbody>().velocity;
        Debug.DrawRay(transform.position, rayDir, Color.red);
        RaycastHit shotHit;
        if (Physics.Raycast(transform.position, rayDir, out shotHit, 100f))
        {
            //Debug.Log ("Hit soemthing at: " + shotHit.distance);
            //Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy")
            {
                //GameObject Enemy = shotHit.collider.gameObject;
                #region Wokring Bullet Dropoff

                if(DistanceToPlayer < shortRange) {
                    //shotHit.collider.GetComponent<Enemy>().Hit(Damage * 1.5f);
                    CloseHit(Damage, shotHit);
                    Destroy(this.gameObject);
                } else if(DistanceToPlayer >= shortRange && DistanceToPlayer < mediumRange) {
                    //shotHit.collider.GetComponent<Enemy>().Hit(Damage);
                    MediumHit(Damage, shotHit);
                    Destroy(this.gameObject);
                } else if (DistanceToPlayer >= mediumRange && DistanceToPlayer < longRange) {
                    //shotHit.collider.GetComponent<Enemy>().Hit(Damage/0.85f);
                    LongRangeHit(Damage, shotHit);
                    Destroy(this.gameObject);
                } else if (DistanceToPlayer >= outOfRange) {
                    Destroy(this.gameObject);
                }
                #endregion
                /*shotHit.collider.GetComponent<Enemy>().Hit(Damage);
                Destroy(this.gameObject);*/
                //this.gameObject.SetActive(false);
            } else if(shotHit.collider.tag == "Level Asset" || shotHit.collider.tag == "Ground") {
                //this.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
        //Debug.Log("Bullet distance to player" + DistanceToPlayer);
    }

   /* void OnTriggerEnter(Collider other){

        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Hit(Damage);
            Destroy(this.gameObject);
        }

	}*/

    public void CloseHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage * 1.5f);
    }

    public void MediumHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage);
    }

    public void LongRangeHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage -1.5f);
    }
}
