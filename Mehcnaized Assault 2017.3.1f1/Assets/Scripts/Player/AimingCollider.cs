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
        if (other.tag == "Enemy" && PC.Targeting == false)
        {
            PC.PossibleEnemy = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && PC.PossibleEnemy != null)
        {
            PC.PossibleEnemy = null;
        }
    }
}
