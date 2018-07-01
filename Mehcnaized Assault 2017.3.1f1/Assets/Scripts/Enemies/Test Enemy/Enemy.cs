using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public int Health = 100;
    public int baseHealth;
	public Slider HealthBar;
    public PlayerController PC;
    public bool Targeted;
    public bool Scanned;
    public GameObject UICanvas;
    public Text damageIndicator;

    [Header("Patrol Points")]
    public GameObject patrolPointOne;
    public GameObject patrolPointTwo;
    public GameObject patrolPointSelected;
    // Use this for initialization
    void Start () {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseHealth = Health;
	}
	
	// Update is called once per frame
	void Update () {
        //CanvasRotate();
		if (Health <= 0) {
            Dead();
		}

        /*if (Targeted == true && Scanned == true)
        {
            UICanvas.SetActive(true);
        }
        else
        {
            UICanvas.SetActive(false);
        }*/

        if(Scanned == true) {
            UICanvas.SetActive(true);
            if(Health <=70) {
                damageIndicator.text = "Damage Indicator: Low";
            }

            if (Health <= 70) {
                damageIndicator.text = "Damage Indicator: Medium";
            }

            if (Health <= 70) {
                damageIndicator.text = "Damage Indicator: High";
            }
        } else if(Scanned == false) {
            UICanvas.SetActive(false);
        }

        //Patrolling();
	}

	public void Hit(int Damage){
		Debug.Log (this.gameObject + " was hit by a ray");
		Health -= Damage;
        //HealthBar.value = Health;
    }

	void OnTriggerEnter(Collider other){
		if (other.tag == "Bullet") {
			Destroy (other.gameObject);
		}
	}

    private void Dead()
    {
        if (this.gameObject == PC.targetedLeftEnemy)
        {
            PC.targetedLeftEnemy = null;
            PC.targetedRightEnemy = null;
            PC.Targeting = false;
        }
        Destroy(this.gameObject);
    }

    private void CanvasRotate() {
        GameObject playerReferenceObject = PC.middleReference;
        UICanvas.transform.LookAt(transform.position + playerReferenceObject.transform.rotation * Vector3.right, playerReferenceObject.transform.rotation * Vector3.up);
    }

    void Patrolling() {
        this.transform.position = Vector3.MoveTowards(transform.position, patrolPointSelected.transform.position, 0.5f);
        float distance = Vector3.Distance(transform.position, patrolPointSelected.transform.position);
        if (distance < 0.2) {
            if (patrolPointSelected == patrolPointOne) {
                patrolPointSelected = patrolPointTwo;
            } else if (patrolPointSelected == patrolPointTwo) {
                patrolPointSelected = patrolPointOne;
            }
        }

    }
}
