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
                #region Bullet Dropoff

                if(DistanceToPlayer < shortRange) {
                    CloseHit(Damage, shotHit);
                    Debug.Log("Destroyed close range");
                    this.gameObject.SetActive(false);
                } else if(DistanceToPlayer >= shortRange && DistanceToPlayer < mediumRange) {
                    MediumHit(Damage, shotHit);
                    Debug.Log("Destroyed medium range");
                    this.gameObject.SetActive(false);
                } else if (DistanceToPlayer >= mediumRange && DistanceToPlayer < longRange) {
                    LongRangeHit(Damage, shotHit);
                    Debug.Log("Destroyed long range");
                    this.gameObject.SetActive(false);
                } else if (DistanceToPlayer >= outOfRange) {
                    Debug.Log("Destroyed out of range");
                    this.gameObject.SetActive(false);
                }
                #endregion
            } else if(shotHit.collider.tag == "Level Asset" || shotHit.collider.tag == "Ground") {
                this.gameObject.SetActive(false);
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

    public IEnumerator WaitTillInActive(float WaitTime) {
        yield return new WaitForSeconds(WaitTime);
        Debug.Log("Destroyed Wait till");
        this.gameObject.SetActive(false);
    }
}