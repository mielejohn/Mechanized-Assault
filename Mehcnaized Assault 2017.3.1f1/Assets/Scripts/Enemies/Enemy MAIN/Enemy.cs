using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour{

    public bool Targeted;
    public GameObject UICanvas;
    public bool Scanned;

    public float Health = 100;
    public float baseHealth;
    public Slider HealthBar;

    void Start () {
        baseHealth = Health;
    }

    void Update () {
        if (Targeted == true && Scanned == true) {
            UICanvas.SetActive(true);
        } else {
            UICanvas.SetActive(false);
        }
    }

    public void Hit(float Damage) {
        Debug.Log("Enemy damage = " + Damage);
        Health -= Damage;
        HealthBar.value = Health;
    }
}
