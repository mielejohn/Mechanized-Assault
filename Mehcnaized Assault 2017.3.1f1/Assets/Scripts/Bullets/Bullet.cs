using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int Damage;

    void Update()
    {
        Debug.DrawRay(transform.position, this.gameObject.transform.forward, Color.red);
        RaycastHit shotHit;
        if (Physics.Raycast(transform.position, this.transform.forward, out shotHit, 100f))
        {
            //Debug.Log ("Hit soemthing at: " + shotHit.distance);
            Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy")
            {
                //GameObject Enemy = shotHit.collider.gameObject;
                shotHit.collider.GetComponent<Enemy>().Hit(Damage);
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
