using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class MechSelectionController : MonoBehaviour {

    public Animator Anim;
    public int SelectionStage;

    [Header("Frame Selection")]
    public GameObject FrameSelectionObjects;
    public GameObject DashFrame;
    public GameObject AssaultFrame;
    public GameObject TitanFrame;

    [Header("Weapon Selection")]
    public GameObject WeaponSelectionObjects;
    public int leftWeaponNumber;
    public int rightWeaponNumber;
    #region Left Weapon Variables
    [TabGroup("Left Weapons")]
    public GameObject leftShotgun;
    [TabGroup("Left Weapons")]
    public GameObject leftAssaultRifle;
    [TabGroup("Left Weapons")]
    public GameObject leftSubMachinegun;
    [TabGroup("Left Weapons")]
    public GameObject leftSniperRifle;
    [TabGroup("Left Weapons")]
    public GameObject leftMinigun;
    [TabGroup("Left Weapons")]
    public GameObject leftMarksmanRifle;
    #endregion

    #region Right Weapon Variables
    [TabGroup("Right Weapons")]
    public GameObject rightShotgun;
    [TabGroup("Right Weapons")]
    public GameObject rightAssaultRifle;
    [TabGroup("Right Weapons")]
    public GameObject rightSubMachinegun;
    [TabGroup("Right Weapons")]
    public GameObject rightSniperRifle;
    [TabGroup("Right Weapons")]
    public GameObject rightMinigun;
    [TabGroup("Right Weapons")]
    public GameObject rightMarksmanRifle;
    #endregion
    // Use this for initialization
    void Start () {
        FrameSelectionObjects.SetActive(false);
        StartCoroutine(FrameOut());
	}
	
	// Update is called once per frame
	void Update () {
		/*(if(Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(ArmingCageTest());
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            StartCoroutine(FrameOut());
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            StartCoroutine(FrameIn());
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            StartCoroutine(FrameInWeaponsOut());
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            StartCoroutine(FrameOutWeaponsIn());
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            StartCoroutine(WeaponsIn());
        }*/
    }
    #region Frame Select
    public IEnumerator ArmingCageTest() {
        yield return new WaitForSeconds(2.0f);
        Anim.SetBool("Cage Opening", true);
        yield return new WaitForSecondsRealtime(13.0f);
        Anim.SetBool("Cage Opening", false);
        Anim.SetBool("Cage Closing", true);
        yield return new WaitForSeconds(6.5f);
    }

    public IEnumerator FrameOut() {
        Anim.SetBool("Cage Opening", true);
        yield return new WaitForSeconds(4.0f);
        FrameSelectionObjects.SetActive(true);
    }

    public IEnumerator FrameIn() {
        Anim.SetBool("Cage Closing", true);
        yield return new WaitForSeconds(2.0f);
        Anim.SetBool("Cage Closing", false);
        Anim.SetBool("Weapons Retract/Frame Out", false);
    }

    public IEnumerator FrameInWeaponsOut() {
        FrameSelectionObjects.SetActive(false);
        Anim.SetBool("Cage Opening", false);
        Anim.SetBool("Weapons Out", true);
        Anim.SetBool("Weapons Retract/Frame Out", false);
        yield return new WaitForSeconds(3.0f);
        WeaponSelectionObjects.SetActive(true);
    }

    public IEnumerator FrameOutWeaponsIn() {
        WeaponSelectionObjects.SetActive(false);
        Anim.SetBool("Weapons Retract/Frame Out", true);
        Anim.SetBool("Weapons Out", false);
        yield return new WaitForSeconds(5.0f);
        FrameSelectionObjects.SetActive(true);

    }

    public IEnumerator WeaponsIn() {
        Anim.SetBool("Weapons IN", true);
        Anim.SetBool("Weapons Out", false);
        yield return new WaitForSeconds(3.0f);
    }

    public void ButtonNext() {
        if(SelectionStage == 0 || SelectionStage < 3) { 
            SelectionStage++;
            switch(SelectionStage) {
                case 1:
                    StartCoroutine(FrameInWeaponsOut());
                    break;

                case 2:
                    StartCoroutine(WeaponsIn());
                    break;
            }
        }
    }

    public void ButtonBack() {
        if (SelectionStage > 0 || SelectionStage < 2) {
            SelectionStage--;
            switch (SelectionStage) {
                case 0:
                    StartCoroutine(FrameOutWeaponsIn());
                    break;
            }
        }
    }

    public void DashSelect() {
        DashFrame.SetActive(true);
        AssaultFrame.SetActive(false);
        TitanFrame.SetActive(false);
    }

    public void AssaultSelect() {
        DashFrame.SetActive(false);
        AssaultFrame.SetActive(true);
        TitanFrame.SetActive(false);
    }

    public void TitanSelect() {
        DashFrame.SetActive(false);
        AssaultFrame.SetActive(false);
        TitanFrame.SetActive(true);
    }
    #endregion

    #region Weapon Select
    public void LeftWeaponButtonNext() {
        if (leftWeaponNumber == 0 || leftWeaponNumber < 5) {
            leftWeaponNumber++;
            switch (leftWeaponNumber) {
                case 0:
                    leftAssaultRifle.SetActive(false);
                    leftSubMachinegun.SetActive(true);
                    break;

                case 1:
                    leftSubMachinegun.SetActive(false);
                    leftAssaultRifle.SetActive(true);
                    leftShotgun.SetActive(false);
                    break;

                case 2:
                    leftAssaultRifle.SetActive(false);
                    leftShotgun.SetActive(true);
                    leftMarksmanRifle.SetActive(false);
                    break;

                case 3:
                    leftShotgun.SetActive(false);
                    leftMarksmanRifle.SetActive(true);
                    leftSniperRifle.SetActive(false);
                    break;

                case 4:
                    leftMarksmanRifle.SetActive(false);
                    leftSniperRifle.SetActive(true);
                    leftMinigun.SetActive(false);
                    break;

                case 5:
                    leftSniperRifle.SetActive(false);
                    leftMinigun.SetActive(true);
                    break;
            }
        }
    }

    public void LeftWeaponButtonBack() {
        if(leftWeaponNumber > 0 || leftWeaponNumber == 5) {
            leftWeaponNumber--;
            switch (leftWeaponNumber) {
                case 0:
                    leftAssaultRifle.SetActive(false);
                    leftSubMachinegun.SetActive(true);
                    break;

                case 1:
                    leftSubMachinegun.SetActive(false);
                    leftAssaultRifle.SetActive(true);
                    leftShotgun.SetActive(false);
                    break;

                case 2:
                    leftAssaultRifle.SetActive(false);
                    leftShotgun.SetActive(true);
                    leftMarksmanRifle.SetActive(false);
                    break;

                case 3:
                    leftShotgun.SetActive(false);
                    leftMarksmanRifle.SetActive(true);
                    leftSniperRifle.SetActive(false);
                    break;

                case 4:
                    leftMarksmanRifle.SetActive(false);
                    leftSniperRifle.SetActive(true);
                    leftMinigun.SetActive(false);
                    break;

                case 5:
                    leftSniperRifle.SetActive(false);
                    leftMinigun.SetActive(true);
                    break;
            }
        }
    }

    public void RightWeaponButtonNext() {
        if (rightWeaponNumber == 0 || rightWeaponNumber < 5) {
            rightWeaponNumber++;
            switch (rightWeaponNumber) {
                case 0:
                    rightAssaultRifle.SetActive(false);
                    rightSubMachinegun.SetActive(true);
                    break;

                case 1:
                    rightSubMachinegun.SetActive(false);
                    rightAssaultRifle.SetActive(true);
                    rightShotgun.SetActive(false);
                    break;

                case 2:
                    rightAssaultRifle.SetActive(false);
                    rightShotgun.SetActive(true);
                    rightMarksmanRifle.SetActive(false);
                    break;

                case 3:
                    rightShotgun.SetActive(false);
                    rightMarksmanRifle.SetActive(true);
                    rightSniperRifle.SetActive(false);
                    break;

                case 4:
                    rightMarksmanRifle.SetActive(false);
                    rightSniperRifle.SetActive(true);
                    rightMinigun.SetActive(false);
                    break;

                case 5:
                    rightSniperRifle.SetActive(false);
                    rightMinigun.SetActive(true);
                    break;
            }
        }
    }

    public void RightWeaponButtonBack() {
        if (rightWeaponNumber > 0 || rightWeaponNumber == 5) {
            rightWeaponNumber--;
            switch (rightWeaponNumber) {
                case 0:
                    rightAssaultRifle.SetActive(false);
                    rightSubMachinegun.SetActive(true);
                    break;

                case 1:
                    rightSubMachinegun.SetActive(false);
                    rightAssaultRifle.SetActive(true);
                    rightShotgun.SetActive(false);
                    break;

                case 2:
                    rightAssaultRifle.SetActive(false);
                    rightShotgun.SetActive(true);
                    rightMarksmanRifle.SetActive(false);
                    break;

                case 3:
                    rightShotgun.SetActive(false);
                    rightMarksmanRifle.SetActive(true);
                    rightSniperRifle.SetActive(false);
                    break;

                case 4:
                    rightMarksmanRifle.SetActive(false);
                    rightSniperRifle.SetActive(true);
                    rightMinigun.SetActive(false);
                    break;

                case 5:
                    rightSniperRifle.SetActive(false);
                    rightMinigun.SetActive(true);
                    break;
            }
        }
    }

    #endregion
}
