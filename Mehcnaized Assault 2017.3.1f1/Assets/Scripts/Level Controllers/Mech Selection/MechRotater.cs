using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechRotater : MonoBehaviour {

    public MainMenuController MMC;
    public float rotation;

    void Start() {
        rotation = 32f;
    }


    void Update() {

        if (Input.GetKey("d") && MMC.canRotate == true) {
            On_D_down();
        }

        if (Input.GetKey("a") && MMC.canRotate == true) {
            On_A_down();
        }
    }

    private void On_D_down() {
        this.transform.Rotate(new Vector3(0, -rotation, 0) * Time.deltaTime * 5.0f);
    }

    private void On_A_down() {
        this.transform.Rotate(new Vector3(0, rotation, 0) * Time.deltaTime * 5.0f);
    }
}
