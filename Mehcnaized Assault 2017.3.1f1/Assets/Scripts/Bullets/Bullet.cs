using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int Damage;

    void Update()
    {
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
                shotHit.collider.GetComponent<Enemy>().Hit(Damage);
                Destroy(this.gameObject);
                //this.gameObject.SetActive(false);
            } else if(shotHit.collider.tag == "Level Asset" || shotHit.collider.tag == "Ground") {
                //this.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

   /* void OnTriggerEnter(Collider other){

        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Hit(Damage);
            Destroy(this.gameObject);
        }

	}*/
}
