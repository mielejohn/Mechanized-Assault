using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

public enum ShieldStatus { Up, Down };

public class PlayerController : MonoBehaviour
{
    public GameManager GM;

    [Header("Object References")]
    public GameObject middleReference;
    public GameObject headReference;

    [Header("Camera")]
    public GameObject Camera;
    float cameraRotationLimitX;
    public GameObject centerCameraPosition;
    public GameObject topCameraPosition;
    public GameObject rightCameraPosition;
    public GameObject leftCameraPosition;
    public GameObject bottomCameraPosition;

    [Header("Rigidbody and Movement")]
    public bool canMove;
    [Required]
    public Rigidbody RB;
    [SerializeField]
    private float jumpInput;
    private float moveX;
    private float moveZ;
    private float moveY;


    [Header("Variables")]
    [SerializeField]
    private float groundSpeed = 8;
    public float jumpSpeed = 20;
    private float boostForce;
    [SerializeField]
    private float magnitudeReference;
    [SerializeField]
    private float groundForce;
    [SerializeField]
    private float jumpForce;

    [Header("TESTER VARIABLE SELECTIONS")]
    public Dropdown frameType;

    [Header("Animations")]
    public Animator Anim;
    public float animX;
    public float animZ;

    [Header("Shoulders")]
    public GameObject RightShoulder;
    public GameObject LeftShoulder;
    public Vector3 shoulderMeleeWeaponRotation;

    [Header("Upper Arms")]
    public GameObject RightArm;
    public GameObject LeftArm;
    public Vector3 upperArmMeleeWeaponRotation;

    [Header("Lower Arms")]
    public GameObject RightLowerArm;
    public GameObject LeftLowerArm;
    public Vector3 lowerArmMeleeWeaponRotation;

    [Header("Hands")]
    public GameObject RightHand;
    public GameObject LeftHand;

    [Header("Upper Body")]
    public GameObject upperBody;

    [Header("Targeted Enemy amd UI")]
    public bool Targeting;
    public GameObject PossibleEnemy;
    public GameObject targetedRightEnemy;
    public GameObject targetedLeftEnemy;
    public GameObject possibleTargetReticle;
    public GameObject targetedReticle;
    public GameObject aimingCollider;

    [Header("Boolean Conditions")]
    public bool onGround;
    public bool Flying;
    public bool isScanning;
    public bool Boosting;
    public bool speedDoubled;
    public bool regeningThrusters;

    [Header("Shield and Health")]
    public ShieldStatus ShieldStatus = ShieldStatus.Up;
    public GameObject Shield;
    public Image healthBarImage;
    public Image shieldBarImage;
    [SerializeField]
    public float Health;
    private float healthReference;
    [SerializeField]
    public float Shields;
    private float shieldsReference;
    public Text healthText;
    public Text shieldText;

    [Header("Flying and Boosting Variables")]
    //public Slider flightTestUiBar;
    public Image thrusterBarImage;
    public float thrusterAmountReference;
    public float thrusterAmount;
    public float thrusterRecharge;
    private float fallingWeight;

    [Header("Scanning Items")]
    public GameObject scanOverlay;
    public GameObject scannerText;

    [Header("Weapon Spawns")]
    public GameObject leftWeaponSpawn;
    public GameObject rightWeaponSpawn;

    [Header("Stationary Pistols")]
    public bool leftPistolSwapped;
    public GameObject leftStationaryPistol;
    public bool rightPistolSwapped;
    public GameObject rightstationaryPistol;

    [Header("Pistol Prefabs")]
    public GameObject rightPistol;
    public GameObject leftPistol;

    [Header("TESTER SELECTIONS")]
    public string leftWeapon;
    public string rightWeapon;

    [Header("Materials")]
    public Material talkingMaterial;

    [TabGroup("Shoulder Weapon Variables")]
    public GameObject shoulderWeaponLowAmmoNotice;
    public Text shoulderWeaponNotice;

    #region Left Weapon Variables
    [TabGroup("Left Weapons")]
    public int leftWeaponAmmoCount;
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
    [TabGroup("Left Weapons")]
    public bool leftMeleeWeapon = false;
    [TabGroup("Left Weapons")]
    public GameObject leftShortSword;
    [TabGroup("Left Weapons")]
    public GameObject leftWeaponLowAmmoNotice;
    #endregion

    #region Right Weapon Variables
    [TabGroup("Right Weapons")]
    public int rightWeaponAmmoCount;
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
    [TabGroup("Right Weapons")]
    public bool rightMeleeWeapon = false;
    [TabGroup("Right Weapons")]
    public GameObject rightShortSword;
    [TabGroup("Right Weapons")]
    public GameObject rightWeaponLowAmmoNotice;
    #endregion

    public Canvas playerCanvas;


    void Awake() {
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    void Start()
    {
        shoulderWeaponNotice  = shoulderWeaponLowAmmoNotice.transform.transform.GetChild(0).GetComponent<Text>();
        Camera = GameObject.FindGameObjectWithTag("Player Camera");
        aimingCollider = GameObject.FindGameObjectWithTag("Aiming Collider");
        playerCanvas = GameObject.FindGameObjectWithTag("Player Canvas").GetComponent<Canvas>();
        #region Frame stats selection
        switch (PlayerPrefs.GetInt("FrameChoice")) {
            case 1:
                Health = 1750;
                healthReference = Health;
                magnitudeReference = 65;
                groundForce = 67;
                boostForce = 20f;
                Shields = 50;
                shieldsReference = Shields;
                fallingWeight = 4f;
                thrusterAmount = 100f;
                thrusterAmountReference = thrusterAmount;
                jumpForce = 3;
                jumpSpeed = 4;
                break;

            case 2:
                Health = 2000;
                healthReference = Health;
                magnitudeReference = 68;
                groundForce = 63;
                Shields = 50;
                shieldsReference = Shields;
                fallingWeight = 5f;
                thrusterAmount = 75f;
                thrusterAmountReference = thrusterAmount;
                jumpForce = 4;
                jumpSpeed = 5;
                break;

            case 3:
                Health = 2500;
                healthReference = Health;
                magnitudeReference = 70;
                groundForce = 56;
                Shields = 50;
                shieldsReference = Shields;
                fallingWeight = 6f;
                thrusterAmount = 50f;
                thrusterAmountReference = thrusterAmount;
                jumpForce = 3;
                jumpSpeed = 5;
                break;
        }


        #endregion

        #region Weapon Select
        switch (PlayerPrefs.GetInt("RightWeaponChoice"))
        {

            case 0:
                GameObject RightSMG_I = Instantiate(rightSubMachinegun);
                RightSMG_I.transform.position = rightWeaponSpawn.transform.position;
                RightSMG_I.transform.parent = RightHand.transform;
                break;

            case 1:
                GameObject RightAssaultRifle_I = Instantiate(rightAssaultRifle);
                RightAssaultRifle_I.transform.position = rightWeaponSpawn.transform.position;
                RightAssaultRifle_I.transform.parent = RightHand.transform;
                break;

            case 2:
                GameObject RightShotgun_I = Instantiate(rightShotgun);
                RightShotgun_I.transform.position = rightWeaponSpawn.transform.position;
                RightShotgun_I.transform.parent = RightHand.transform;
                break;

            case 3:
                GameObject RightMR_I = Instantiate(rightMarksmanRifle);
                RightMR_I.transform.position = rightWeaponSpawn.transform.position;
                RightMR_I.transform.parent = RightHand.transform;
                break;

            case 4:
                GameObject RightSniperRifle_I = Instantiate(rightSniperRifle);
                RightSniperRifle_I.transform.position = rightWeaponSpawn.transform.position;
                RightSniperRifle_I.transform.parent = RightHand.transform;
                break;

            case 5:
                GameObject RightMinigun_I = Instantiate(rightMinigun);
                RightMinigun_I.transform.position = rightWeaponSpawn.transform.position;
                RightMinigun_I.transform.parent = RightHand.transform;
                break;

            case 6:
                GameObject rightShortSword_I = Instantiate(rightShortSword);
                rightShortSword_I.transform.position = rightWeaponSpawn.transform.position;
                rightShortSword_I.transform.parent = RightHand.transform;
                rightMeleeWeapon = true;
                break;
        }

        switch (PlayerPrefs.GetInt("LeftWeaponChoice"))
        {
            case 0:
                GameObject LeftSMG_I = Instantiate(leftSubMachinegun);
                LeftSMG_I.transform.position = leftWeaponSpawn.transform.position;
                LeftSMG_I.transform.parent = LeftHand.transform;
                break;

            case 1:
                GameObject LeftAssaultRifle_I = Instantiate(leftAssaultRifle);
                LeftAssaultRifle_I.transform.position = leftWeaponSpawn.transform.position;
                LeftAssaultRifle_I.transform.parent = LeftHand.transform;
                break;

            case 2:
                GameObject LeftShotgun_I = (GameObject)Instantiate(leftShotgun);
                LeftShotgun_I.transform.position = leftWeaponSpawn.transform.position;
                LeftShotgun_I.transform.parent = LeftHand.transform;
                break;

            case 3:
                GameObject LeftMR_I = Instantiate(leftMarksmanRifle);
                LeftMR_I.transform.position = leftWeaponSpawn.transform.position;
                LeftMR_I.transform.parent = LeftHand.transform;
                break;

            case 4:
                GameObject LeftSniperRifle_I = Instantiate(leftSniperRifle);
                LeftSniperRifle_I.transform.position = leftWeaponSpawn.transform.position;
                LeftSniperRifle_I.transform.parent = LeftHand.transform;
                break;

            case 5:
                GameObject LeftMinigun_I = Instantiate(leftMinigun);
                LeftMinigun_I.transform.position = leftWeaponSpawn.transform.position;
                LeftMinigun_I.transform.parent = LeftHand.transform;
                break;

            case 6:
                GameObject leftShortSword_I = Instantiate(leftShortSword);
                leftShortSword_I.transform.position = leftWeaponSpawn.transform.position;
                leftShortSword_I.transform.parent = LeftHand.transform;
                leftMeleeWeapon = true;
                break;
        }
        #endregion

        Camera = GameObject.FindGameObjectWithTag("Player Camera");
    }

    void FixedUpdate()
    {
        #region Keyboard Movement
        if (canMove == true) {
            if (!GM.prevState.IsConnected && Boosting == false) {
                moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
                moveZ = Input.GetAxis("Vertical") * Time.deltaTime;
                if (Input.GetButton("Jump") == false && Flying == false) {
                    moveY = -0.009f;
                } else {
                    moveY = Input.GetAxis("Jump") * Time.deltaTime;
                }

                animX = Input.GetAxis("Horizontal");
                animZ = Input.GetAxis("Vertical");
                Anim.SetFloat("X_Axis", animX);
                Anim.SetFloat("Z_Axis", animZ);

                if (Flying == false){

                    Movement(moveZ, moveY *0.50f, moveX);
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                } else if (thrusterAmount > 0 && Flying == true && Input.GetButton("Jump") != false) {
                    Movement(moveZ * 1.65f, moveY, moveX * 1.65f);
                    thrusterAmount = thrusterAmount - 0.50f;
                    regeningThrusters = false;
                } else if(thrusterAmount > 0 && Flying == true && Input.GetButton("Jump") == false) {
                    if(RB.velocity.y > 10f && RB.velocity.y < 30f) {
                        RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y - 0.75f ,RB.velocity.z);
                    } else if (RB.velocity.y > 29f) {
                        RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y - 2f, RB.velocity.z);
                    }
                    Movement(moveZ * 4, moveY, moveX * 4);
                    thrusterAmount = thrusterAmount - 0.20f;
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                } else if (Flying == true && thrusterAmount <= 0 || Input.GetButton("Jump") == false || Input.GetButton("Jump") == true) {
                    moveY = -0.027f;
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                    Movement(moveZ, moveY, moveX);
                }

                if (Input.GetButtonDown("Boost") && Boosting == false) {
                    Boosting = true;
                    StartCoroutine(Dash(moveX, moveY, moveZ));
                }

                this.transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X") * 5, 0f));
                cameraRotationLimitX -= Input.GetAxis("Mouse Y") * 2.5f;
                cameraRotationLimitX = Mathf.Clamp(cameraRotationLimitX, -35, 30);
                Camera.transform.localEulerAngles = new Vector3(cameraRotationLimitX, 0.0f, 0.0f);
               
            }
        }
        #endregion

        #region Controller Movement
        if (canMove == true) {
            if (GM.prevState.IsConnected) {

                moveX = GM.state.ThumbSticks.Left.X * Time.deltaTime;
                moveZ = GM.state.ThumbSticks.Left.Y * Time.deltaTime;

                if (Input.GetButton("Jump") == false && Flying == false) {
                    moveY = -0.009f;
                } else {
                    if (Input.GetButton("Jump") == true){
                        moveY = 1f * Time.deltaTime;
                    } else if (Input.GetButton("Jump") == false) {
                        moveY = 0;
                    }
                }

                animX = GM.state.ThumbSticks.Left.X;
                animZ = GM.state.ThumbSticks.Left.Y;
                Anim.SetFloat("X_Axis", animX);
                Anim.SetFloat("Z_Axis", animZ);

                if (Flying == false) {

                    Movement(moveZ, moveY * 0.50f, moveX);
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                } else if (thrusterAmount > 0 && Flying == true && Input.GetButton("Jump") == true) {
                    Movement(moveZ * 1.65f, moveY, moveX * 1.65f);
                    thrusterAmount = thrusterAmount - 0.50f;
                    regeningThrusters = false;
                } else if (thrusterAmount > 0 && Flying == true && Input.GetButton("Jump") == false) {
                    if (RB.velocity.y > 10f && RB.velocity.y < 30f) {
                        RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y - 1.75f, RB.velocity.z);
                    } else if (RB.velocity.y > 29f) {
                        RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y - 4f, RB.velocity.z);
                    }
                    Movement(moveZ * 4, moveY, moveX * 4);
                    thrusterAmount = thrusterAmount - 0.20f;
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                } else if (Flying == true && thrusterAmount <= 0 || Input.GetButton("Jump") == false || Input.GetButton("Jump") == true) {
                    moveY = -0.027f;
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                    Movement(moveZ, moveY, moveX);
                }

                if (Input.GetButtonDown("Boost") && Boosting == false) {
                    Boosting = true;
                    StartCoroutine(Dash(moveX, moveY, moveZ));
                }

                this.transform.Rotate(new Vector3(0f, GM.state.ThumbSticks.Right.X * 5, 0f));
                cameraRotationLimitX -= GM.state.ThumbSticks.Right.Y * 2.5f;
                cameraRotationLimitX = Mathf.Clamp(cameraRotationLimitX, -35, 30);
                Camera.transform.localEulerAngles = new Vector3(cameraRotationLimitX, 0.0f, 0.0f);
            }
        }
        #endregion

        #region CameraMovement
        if (canMove == true) {
            if (Input.GetAxis("Horizontal") > 0) {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, rightCameraPosition.transform.position, Time.deltaTime * 5f);
            } else if (Input.GetAxis("Horizontal") < 0) {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, leftCameraPosition.transform.position, Time.deltaTime * 5f);
            } else if (Input.GetAxis("Jump") > 0) {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, bottomCameraPosition.transform.position, Time.deltaTime * 5f);
            } else if (Input.GetAxis("Jump") == 0 && onGround == false) {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, topCameraPosition.transform.position, Time.deltaTime * 5f);
            } else {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, centerCameraPosition.transform.position, Time.deltaTime * 5f);
            }
        }
        #endregion
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
            if (hit.distance > 15f) {
                Flying = true;
            } else {
                Flying = false;
            }
        }

        if (regeningThrusters == true) {
            thrusterAmount += 0.25f;
        }
    }

    void Update()
    {
        ThrusterBar();
        if(Input.GetKeyDown(KeyCode.P)) {
            PlayerPrefs.SetInt("Auto Aim",1);
        }

        shieldBar();
        if (Health < 0)
        {
            healthText.text = "" + 0;
        }
        else
        {
            healthText.text = Health.ToString();
        }

        #region Keyboard Actions
        if (Input.GetKeyDown(KeyCode.L) && rightPistolSwapped == false && canMove == true)
        {
            StartCoroutine(RightPistolSwap());
        }
        if (Input.GetKeyDown(KeyCode.K) && leftPistolSwapped == false && canMove == true)
        {
            StartCoroutine(LeftPistolSwap());
        }

        if (Input.GetMouseButtonDown(2) && Targeting == true) {
            UnTargeted();
        }

        if (Input.GetMouseButtonDown(2) && PossibleEnemy != null && Targeting == false && isScanning == false && PlayerPrefs.GetInt("Auto Aim") == 0)
        {
            TargetEnemy();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && isScanning == true) {
            StartCoroutine(ScannerDown());
        }

        if (Input.GetKeyUp(KeyCode.Alpha1) && isScanning == false) {
            if(Targeting == true) {
                UnTargeted();
            }
            isScanning = true;
            scanOverlay.SetActive(true);
            scannerText.SetActive(true);
            talkingMaterial.color = new Color(0f,255f,255f);

        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Time.timeScale = 1.0f;
        }
        #endregion

        #region Controller Actions

        if (GM.prevState.Buttons.RightStick == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.RightStick == XInputDotNetPure.ButtonState.Pressed && Targeting == true) {
            UnTargeted();
        }

        if (GM.prevState.Buttons.RightStick == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.RightStick == XInputDotNetPure.ButtonState.Pressed && PossibleEnemy != null && Targeting == false && isScanning == false && PlayerPrefs.GetInt("Auto Aim") == 0) {
            TargetEnemy();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && isScanning == true) {
            StartCoroutine(ScannerDown());
        }

        if (Input.GetKeyUp(KeyCode.Alpha1) && isScanning == false) {
            if (Targeting == true) {
                UnTargeted();
            }
            isScanning = true;
            scanOverlay.SetActive(true);
            scannerText.SetActive(true);
            talkingMaterial.color = new Color(0f, 255f, 255f);

        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Time.timeScale = 1.0f;
        }
        #endregion
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            if (hit.distance < 11.0f && hit.collider.tag == "Ground")
            {
                onGround = true;
                Flying = false;
                Anim.SetBool("Flying", Flying);
                Anim.SetBool("OnGround", onGround);
            }
            else
            {
                onGround = false;
                Flying = true;
                Anim.SetBool("Flying", Flying);
                Anim.SetBool("OnGround", onGround);
            }
        }

        if (targetedLeftEnemy != null || targetedRightEnemy != null)
        {
            targetedReticle.SetActive(true);
        }
        else
        {
            targetedReticle.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (targetedLeftEnemy != null && targetedRightEnemy != null)
        {

        }
        else
        {
            upperBody.transform.localEulerAngles = new Vector3(cameraRotationLimitX, upperBody.transform.rotation.y, upperBody.transform.rotation.z);
        }

        if (targetedLeftEnemy != null)
        {
            LeftArm.transform.LookAt(targetedLeftEnemy.transform);
        }

        if (targetedRightEnemy != null)
        {
            RightArm.transform.LookAt(targetedRightEnemy.transform);
        }

    }

    #region Move Methods
    void Movement(float moveZ, float moveY ,float moveX)
    {
            Vector3 movement = new Vector3(moveX, moveY, moveZ);
            RB.AddRelativeForce (movement * groundForce * groundSpeed,ForceMode.Impulse);
            if(RB.velocity.magnitude > magnitudeReference && Boosting == false) { 
               RB.velocity = groundForce * (RB.velocity.normalized);
            }
    }

    private IEnumerator ThrusterRegen(float waitTime) {
        while(thrusterAmount < thrusterAmountReference){
            thrusterAmount += 0.35f;
            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion

    #region Dash Method
    IEnumerator Dash(float moveX, float moveY, float moveZ) {
        Boosting = true;
        //StartCoroutine(boostingBooleanChange(1.5f));
        Vector3 boostDir = RB.velocity;
        float boostReferenceNumber = 0;
        while(boostReferenceNumber < 0.1) { 
            RB.AddRelativeForce(boostDir * 0.8f, ForceMode.Impulse);
            boostReferenceNumber += 0.1f;
        }
        yield return new WaitForSeconds(1.0f);
        Boosting = false;
    }
    #endregion

    #region Right Reload
    public IEnumerator Right_Reload()
    {
        GameObject rightEnemyHold = null;
        if (targetedRightEnemy != null)
        {
            rightEnemyHold = targetedRightEnemy;
            targetedRightEnemy = null;
        }
        Anim.SetBool("RightArm_Reload", true);
        yield return new WaitForSeconds(0.98f);
        Anim.SetBool("RightArm_Reload", false);
        if (rightEnemyHold != null)
        {
            targetedRightEnemy = rightEnemyHold;
            rightEnemyHold = null;
        }
    }
    #endregion

    #region Left Reload
    public IEnumerator Left_Reload()
    {
        GameObject enemyHold = null;
        if (targetedLeftEnemy != null)
        {
            enemyHold = targetedLeftEnemy;
            targetedLeftEnemy = null;
        }
        Anim.SetBool("LeftArm_Reload", true);
        yield return new WaitForSeconds(0.98f);
        Anim.SetBool("LeftArm_Reload", false);
        if (enemyHold != null)
        {
            targetedLeftEnemy = enemyHold;
            enemyHold = null;
        }
    }
    #endregion

    #region Enemy Targeting
    public void TargetEnemy() {
        PossibleEnemy.GetComponent<Enemy>().Targeted = true;
        if (leftMeleeWeapon == false) { 
            targetedLeftEnemy = PossibleEnemy;
        }

        if (rightMeleeWeapon == false) {
            targetedRightEnemy = PossibleEnemy;
        }
        Targeting = true;
        PossibleEnemy = null;
    }

    public void UnTargeted() {
        if (targetedLeftEnemy == null || targetedRightEnemy == null) {

        } else {
            targetedLeftEnemy.GetComponent<Enemy>().Targeted = false;
        }
        targetedLeftEnemy = null;
        targetedRightEnemy = null;
        Targeting = false;
    }
    #endregion

    private IEnumerator ShieldDown()
    {
        ShieldStatus = ShieldStatus.Down;
        Anim.SetBool("ShieldUp", false);
        Anim.SetBool("ShieldRecharging", false);
        Anim.SetBool("ShieldDropping", true);
        yield return new WaitForSeconds(1.0f);
        Anim.SetBool("ShieldDropping", false);
        Anim.SetBool("ShieldDown", true);
        Shield.SetActive(false);

    }

    private IEnumerator ShieldUp()
    {
        ShieldStatus = ShieldStatus.Up;
        Shield.SetActive(true);
        Anim.SetBool("ShieldDown", false);
        Anim.SetBool("ShieldDropping", false);
        Anim.SetBool("ShieldRecharging", true);
        yield return new WaitForSeconds(1.0f);
        Anim.SetBool("ShieldRecharging", false);
        Anim.SetBool("ShieldUp", true);
    }

    private IEnumerator RightPistolSwap()
    {
        GameObject enemyHold = null;
        if (targetedRightEnemy != null)
        {
            Debug.Log("Removing Right Enemy");
            enemyHold = targetedRightEnemy;
            targetedRightEnemy = null;
        }
        Anim.SetBool("RightPistolSwap", true);
        yield return new WaitForSeconds(2.4f);
        rightstationaryPistol.SetActive(false);
        rightPistol.SetActive(true);
        Anim.SetBool("RightPistolSwap", false);
        yield return new WaitForSeconds(0.65f);
        if (enemyHold != null)
        {
            targetedRightEnemy = enemyHold;
            enemyHold = null;
        }
        rightPistolSwapped = true;
    }

    private IEnumerator LeftPistolSwap()
    {
        GameObject enemyHold = null;
        if (targetedLeftEnemy != null)
        {
            Debug.Log("Removing Left Enemy");
            enemyHold = targetedLeftEnemy;
            targetedLeftEnemy = null;
        }
        Anim.SetBool("LeftPistolSwap", true);
        yield return new WaitForSeconds(2.4f);
        leftStationaryPistol.SetActive(false);
        leftPistol.SetActive(true);
        Anim.SetBool("LeftPistolSwap", false);
        yield return new WaitForSeconds(0.65f);
        if (enemyHold != null)
        {
            targetedLeftEnemy = enemyHold;
            enemyHold = null;
        }
        leftPistolSwapped = true;
    }

    public void Hit(float Damage)
    {
        if (Shields > 0)
        {
            if (Shields - Damage < 0)
            {
                float carryOverDamage = Damage - Shields;
                Shields -= Damage;
                Health -= carryOverDamage;
            }
            else
            {
                Shields -= Damage;
            }

        }
        else if (Shields <= 0)
        {
            Health -= Damage;
        }
    }

    #region UI Elements
    public void HealthBar()
    {
        healthBarImage.fillAmount = HealthBarMap(Health, 0, healthReference, 0, 1);
    }

    private float HealthBarMap(float Value, float inMin, float inMax, float outMin, float outMax)
    {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void shieldBar()
    {
        shieldBarImage.fillAmount = ShieldBarMap(Shields, 0, shieldsReference, 0, 1);
    }

    private float ShieldBarMap(float Value, float inMin, float inMax, float outMin, float outMax)
    {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void ThrusterBar() {
        thrusterBarImage.fillAmount = ThrusterBarMap(thrusterAmount , 0, thrusterAmountReference, 0, 1);
    }

    private float ThrusterBarMap(float Value, float inMin, float inMax, float outMin, float outMax) {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    #endregion

    private IEnumerator ScannerDown() {
        scanOverlay.GetComponent<Animator>().SetBool("Deactivated", true);
        yield return new WaitForSeconds(1.00f);
        isScanning = false;
        scanOverlay.SetActive(false);
        scannerText.SetActive(false);
        talkingMaterial.color = new Color(0f, 0f, 255f);
    }

    public void ActivateObject(GameObject selectedObject, int activationState) {
        //0 is inactive, 1 is active
        if(activationState == 0) {
            selectedObject.SetActive(false);
            return;
        } else if (activationState == 1) {
            selectedObject.SetActive(true);
        }
    }
}