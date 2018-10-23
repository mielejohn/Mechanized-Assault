using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour{
    public bool isTrainingDummy;
    public bool Targeted;
    public GameObject UICanvas;
    public bool Scanned;

    public float Health;
    public float baseHealth;
    public Slider HealthBar;
    public bool isAlive;

    public TutorialController TC;
    void Start () {
        Health = baseHealth;
    }

    void Update () {
        if (Scanned == true) {
            UICanvas.SetActive(true);
        } else {
            UICanvas.SetActive(false);
        }

        if(isTrainingDummy == true && Health <= 0) {
            TC.shotDownDrones++;
            this.gameObject.SetActive(false);
        }
    }

    public void Hit(float Damage) {
        //Debug.Log("Enemy damage = " + Damage);
        Health -= Damage;
        HealthBar.value = Health;
    }
}
