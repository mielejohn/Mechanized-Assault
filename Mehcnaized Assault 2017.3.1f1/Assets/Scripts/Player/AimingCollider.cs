using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingCollider : MonoBehaviour {

    public PlayerController PC;
    // Use this for initialization
    void Start () {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (PC.isScanning == false && other.tag == "Enemy" && PC.Targeting == false)
        {
            PC.PossibleEnemy = other.gameObject;
        }

        if (PC.isScanning == true && other.tag == "Enemy") {
            other.GetComponent<Enemy>().Scanned = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (PC.isScanning == false && other.tag == "Enemy" && PC.Targeting == false) {
            PC.PossibleEnemy = other.gameObject;
        }

        if (PC.isScanning == true && other.tag == "Enemy" && other.GetComponent<Enemy>().Scanned != true) {
            other.GetComponent<Enemy>().Scanned = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && PC.Targeting == true){
            PC.UnTargeted();
        }

        if (other.tag == "Enemy" && PC.PossibleEnemy != null && PC.Targeting != true) {
            PC.PossibleEnemy = null;
        }

        /*if (other.tag == "Enemy" && other.GetComponent<Enemy>().Scanned == true) {
            other.GetComponent<Enemy>().Scanned = false;
        }*/
    }
}
