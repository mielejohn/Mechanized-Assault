using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XInputDotNetPure;
using UnityEngine.UI;

using Sirenix.OdinInspector;

public enum ShieldStatus { Up, Down };

public class PlayerController : MonoBehaviour
{

    [Header("Object References")]
    public GameObject middleReference;
    public GameObject headReference;

    [Header("Camera")]
    [Required]
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
    public float boostForce = 10;
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
    //public BlendTree BT;
    public GameManager GM;
    public float animX;
    public float animZ;

    [Header("Arms")]
    public GameObject RightArm;
    public GameObject LeftArm;

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
    public GameObject aimgingCollider;

    [Header("Boolean Conditions")]
    public bool onGround;
    public bool Flying;
    public bool isScanning;
    public bool Boosting;

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

    [Header("Flying Variables")]
    public Slider flightTestUiBar;
    public float thrustAmount;
    public float thrustRecharge;
    private float fallingWeight;

    [Header("Scanning Items")]
    public GameObject scanOverlay;
    public GameObject scannerText;

    [Header("Weapons UI")]
    public Text leftWeaponName;
    public Text rightWeaponName;
    public Text shoulderWeaponName;

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
    void Start()
    {
        #region Frame Stats Select
        switch(frameType.value) {
            case 0:
                //65 Magnitude Reference
                //73 Ground Force
                Health = 1500;
                healthReference = Health;
                magnitudeReference = 65;
                groundForce = 63;
                Shields = 50;
                shieldsReference = Shields;
                fallingWeight = 200f;
                break;

            case 1:
                //50 Magnitude Reference
                //50-60 Ground Force
                Health = 2000;
                healthReference = Health;
                magnitudeReference = 68;
                groundForce = 50;
                Shields = 50;
                shieldsReference = Shields;
                fallingWeight = 150f;
                break;

            case 2:
                //50 Magnitude Reference
                //40 Ground Force
                Health = 2750;
                healthReference = Health;
                magnitudeReference = 45;
                groundForce = 30;
                Shields = 50;
                shieldsReference = Shields;
                fallingWeight = 100f;
                break;

        }


        #endregion

        #region Weapon Select
        switch (rightWeapon)
        {

            case "Assault Rifle":
                GameObject RightAssaultRifle_I = Instantiate(rightAssaultRifle);
                RightAssaultRifle_I.transform.position = rightWeaponSpawn.transform.position;
                RightAssaultRifle_I.transform.parent = RightHand.transform;
                //rightWeaponName.text = "HC-67";
                break;

            case "Shotgun":
                GameObject RightShotgun_I = Instantiate(rightShotgun);
                RightShotgun_I.transform.position = rightWeaponSpawn.transform.position;
                RightShotgun_I.transform.parent = RightHand.transform;
                //rightWeaponName.text = "CCS-50";
                break;

            case "Marksman Rifle":
                GameObject RightMR_I = Instantiate(rightMarksmanRifle);
                RightMR_I.transform.position = rightWeaponSpawn.transform.position;
                RightMR_I.transform.parent = RightHand.transform;
                //rightWeaponName.text = "DMR-203";
                break;

            case "Minigun":
                GameObject RightMinigun_I = Instantiate(rightMinigun);
                RightMinigun_I.transform.position = rightWeaponSpawn.transform.position;
                RightMinigun_I.transform.parent = RightHand.transform;
                //rightWeaponName.text = "LCMG-15";
                break;

            case "Sniper Rifle":
                GameObject RightSniperRifle_I = Instantiate(rightSniperRifle);
                RightSniperRifle_I.transform.position = rightWeaponSpawn.transform.position;
                RightSniperRifle_I.transform.parent = RightHand.transform;
                //rightWeaponName.text = "LRS-500";
                break;

            case "Submachine Gun":
                GameObject RightSMG_I = Instantiate(rightSubMachinegun);
                RightSMG_I.transform.position = rightWeaponSpawn.transform.position;
                RightSMG_I.transform.parent = RightHand.transform;
                //rightWeaponName.text = "SC-67";
                break;
        }

        switch (leftWeapon)
        {

            case "Assault Rifle":
                GameObject LeftAssaultRifle_I = Instantiate(leftAssaultRifle);
                LeftAssaultRifle_I.transform.position = leftWeaponSpawn.transform.position;
                LeftAssaultRifle_I.transform.parent = LeftHand.transform;
                //leftWeaponName.text = "HC-67";
                break;

            case "Shotgun":
                GameObject LeftShotgun_I = (GameObject)Instantiate(leftShotgun);
                LeftShotgun_I.transform.position = leftWeaponSpawn.transform.position;
                LeftShotgun_I.transform.parent = LeftHand.transform;
                //leftWeaponName.text = "CCS-50";
                break;

            case "Marksman Rifle":
                GameObject LeftMR_I = Instantiate(leftMarksmanRifle);
                LeftMR_I.transform.position = leftWeaponSpawn.transform.position;
                LeftMR_I.transform.parent = LeftHand.transform;
                //leftWeaponName.text = "DMR-203";
                break;

            case "Minigun":
                GameObject LeftMinigun_I = Instantiate(leftMinigun);
                LeftMinigun_I.transform.position = leftWeaponSpawn.transform.position;
                LeftMinigun_I.transform.parent = LeftHand.transform;
                //leftWeaponName.text = "LCMG-15";
                break;

            case "Sniper Rifle":
                GameObject LeftSniperRifle_I = Instantiate(leftSniperRifle);
                LeftSniperRifle_I.transform.position = leftWeaponSpawn.transform.position;
                LeftSniperRifle_I.transform.parent = LeftHand.transform;
                //leftWeaponName.text = "LRS-500";
                break;

            case "Submachine Gun":
                GameObject LeftSMG_I = Instantiate(leftSubMachinegun);
                LeftSMG_I.transform.position = leftWeaponSpawn.transform.position;
                LeftSMG_I.transform.parent = LeftHand.transform;
                //leftWeaponName.text = "SC-67";
                break;
        }
        #endregion
    }

    void FixedUpdate()
    {
        #region Keyboard Movement
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime * groundSpeed;
        moveZ = Input.GetAxis("Vertical") * Time.deltaTime * groundSpeed;
        moveY = Input.GetAxis("Jump") * Time.deltaTime * jumpSpeed;
        animX = Input.GetAxis("Horizontal");
        animZ = Input.GetAxis("Vertical");

        if (Input.GetButton("Horizontal") && canMove == true || Input.GetButton("Vertical") && canMove == true || Input.GetButton("Jump") && canMove == true)
        {
            MoveForwards(moveZ, moveX);
            //Debug.Log("Normalized Velocity: " + RB.velocity.normalized);
            //Debug.Log("Normalized Velocity X: " + RB.velocity.normalized.x);
            //Debug.Log("Normalized Velocity Z: " + RB.velocity.normalized.z);
            //MoveSideways (moveX, animX);
            //MoveFlying(moveY);
        }
        //Debug.Log("Rigidbody Velocity reference is: " + RB.velocity.magnitude);
        if (canMove == true && Input.GetButton("Jump") && thrustAmount > 0f) {
            MoveFlying(moveY);
            thrustAmount -= 0.1f;
        } else if (Input.GetButton("Jump") == false && onGround == false || thrustAmount <= 0f && onGround == false) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
                if (hit.distance > 15f) {
                    moveY = hit.distance;
                    MoveFlying(-moveY / fallingWeight);

                } else if(hit.distance <15) {
                    moveY = -0.3f;
                    MoveFlying(moveY);
                }
            }
        } 

        if(thrustAmount > 0 && onGround == false && Input.GetButton("Jump") == false) {
            moveY = -0.3f;
            MoveFlying(moveY);
        }


        Anim.SetFloat("X_Axis", animX);
        Anim.SetFloat("Z_Axis", animZ);
        #endregion

        #region CameraMovement
        if (Input.GetAxis ("Horizontal") > 0) {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, rightCameraPosition.transform.position, Time.deltaTime * 1.3f);
		} else if (Input.GetAxis ("Horizontal") < 0) {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, leftCameraPosition.transform.position, Time.deltaTime * 1.3f);
		} else if (Input.GetAxis ("Jump") > 0) {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, bottomCameraPosition.transform.position, Time.deltaTime * 1.3f);
		} else if (Input.GetAxis("Jump") == 0 && onGround == false) {
            //PlayerCamera.transform.Translate (Vector3.up * Time.deltaTime * 7);
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, topCameraPosition.transform.position, Time.deltaTime * 1.3f);
		} else {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, centerCameraPosition.transform.position, Time.deltaTime * 1.3f);
        }
        #endregion

        #region Dead Code
        /*if (Input.GetButtonDown ("Jump") == false) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, -Vector3.up, out hit)) {
				if (hit.distance < 15.0f && hit.collider.tag == "Ground") {
					RB.velocity.y = -5f;
				}
			}
		}*/

        /*if(Input.GetButtonUp ("Horizontal")){
			RB.velocity = new Vector3 (Mathf.Lerp (RB.velocity.x, 0, 0.3f),Mathf.Lerp (RB.velocity.y, 0, 0.3f),Mathf.Lerp (RB.velocity.z, 0, 0.3f));
		}

		if(Input.GetButtonUp ("Vertical")){
			RB.velocity = new Vector3 (0,0,Mathf.Lerp (RB.velocity.z, 0, 1.0f));
		}

		if(Input.GetButtonUp ("Jump")){
			RB.velocity = new Vector3 (0,Mathf.Lerp (RB.velocity.y, 0, 1.0f),0);
		}*/

        /*if (Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine (Right_Reload ());
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			StartCoroutine (Left_Reload ());
		}*/

        #region Controller Movement
        if (GM.state.ThumbSticks.Left.X > 0 || GM.state.ThumbSticks.Left.Y > 0 || GM.state.ThumbSticks.Left.X < 0 || GM.state.ThumbSticks.Left.Y < 0)    
        {
            //float moveX = GM.state.ThumbSticks.Left.X * Time.deltaTime * groundSpeed;
            //float moveZ = GM.state.ThumbSticks.Left.Y * Time.deltaTime * groundSpeed;
            animX = GM.state.ThumbSticks.Left.X;
            animZ = GM.state.ThumbSticks.Left.Y;
            //MoveForwards (moveZ, animZ);
            //MoveSideways(moveX, animX);
        }
        #endregion

        #region Camera Rotation
        if(canMove == true){
            this.transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X") * 5, 0f));
            cameraRotationLimitX -= Input.GetAxis("Mouse Y") * 2.5f;
            cameraRotationLimitX = Mathf.Clamp(cameraRotationLimitX, -35, 30);
            Camera.transform.localEulerAngles = new Vector3(cameraRotationLimitX, 0f, 0f);
        }
        #endregion
        //LeftArm.transform.localEulerAngles =  new Vector3 (cameraRotationLimitX, 90,LeftArm.transform.rotation.z);
        //RightArm.transform.localEulerAngles =  new Vector3 (cameraRotationLimitX, 90,RightArm.transform.rotation.z);

        //Spine.transform.localEulerAngles = new Vector3 (Spine.transform.rotation.x, Spine.transform.rotation.y, -cameraRotationLimitX);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //HealthBar();
        flightTestUiBar.value = thrustAmount;
        if (Health < 0)
        {
            healthText.text = "" + 0;
        }
        else
        {
            healthText.text = Health.ToString();
        }
        shieldBar();
        if (Shields < 0)
        {
            //shieldText.text = "" + 0;
        }
        else
        {
            //shieldText.text = Shields.ToString();
        }

        if (Shields <= 0)
        {
            if (ShieldStatus != ShieldStatus.Down)
            {
                StartCoroutine(ShieldDown());
            }
        }
        else
        {
            //Shield.SetActive (true);
            if (ShieldStatus != ShieldStatus.Up)
            {
                StartCoroutine(ShieldUp());
            }
        }

        if (Input.GetKeyDown(KeyCode.L) && rightPistolSwapped == false && canMove == true)
        {
            StartCoroutine(RightPistolSwap());
        }
        if (Input.GetKeyDown(KeyCode.K) && leftPistolSwapped == false && canMove == true)
        {
            StartCoroutine(LeftPistolSwap());
        }

        if (Input.GetButtonDown("Boost") && Boosting == false)
        {
            //Debug.Log("Pressed dash forward");
            StartCoroutine(Dash());
        }

        if (Input.GetMouseButtonDown(2) && Targeting == true) {
            UnTargeted();
        }

        if (Input.GetMouseButtonDown(2) && PossibleEnemy != null && Targeting == false && isScanning == false)
        {
            PossibleEnemy.GetComponent<Enemy>().Targeted = true;
            targetedLeftEnemy = PossibleEnemy;
            targetedRightEnemy = PossibleEnemy;
            Targeting = true;
            PossibleEnemy = null;
        }

        if(Input.GetKeyUp(KeyCode.Alpha1) && isScanning == false) {
            if(Targeting == true) {
                UnTargeted();
            }
            isScanning = true;
            scanOverlay.SetActive(true);
            scannerText.SetActive(true);
            talkingMaterial.color = new Color(0f,255f,255f);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && isScanning == true) {
            StartCoroutine(ScannerDown());
        }

        #region Dead Code
        //RightArm.transform.LookAt (Enemy.transform, new Vector3(0,0,1));
        /*
		Vector3 targetDir = Enemy.transform.position - RightArm.transform.position;
		float step = 10 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards (RightArm.transform.forward, targetDir, step, 0.0f);
		Debug.DrawRay (RightArm.transform.position, newDir, Color.red);
		Debug.Log ("New direction is " + newDir);
		RightArm.transform.rotation = Quaternion.LookRotation (newDir);
		*/
        //RightArm.transform.rotation = Quaternion.Slerp(RightArm.transform.rotation, Quaternion.LookRotation(Enemy.transform.position), 10*Time.deltaTime);
        /*if (Enemy != null) {
			Vector3 relativePos = Enemy.transform.position - RightArm.transform.position;
			Quaternion rotation = Quaternion.LookRotation (relativePos);
			RightArm.transform.localEulerAngles = new Vector3 (rotation.x * 40, 90f, rotation.z);
		}*/
        #endregion
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            if (hit.distance < 11.0f && hit.collider.tag == "Ground")
            {
                //Debug.Log("on the ground");
                onGround = true;
                Flying = false;
                Anim.SetBool("Flying", Flying);
                Anim.SetBool("OnGround", onGround);
            }
            else
            {
                //Debug.Log("Off the ground");
                onGround = false;
                Flying = true;
                Anim.SetBool("Flying", Flying);
                Anim.SetBool("OnGround", onGround);
            }
        }

        if (PossibleEnemy != null)
        {
            //possibleTargetReticle.SetActive(true);
        }
        else if (targetedLeftEnemy != null || targetedRightEnemy != null)
        {
            targetedReticle.SetActive(true);
        }
        else
        {
            targetedReticle.SetActive(false);
            //possibleTargetReticle.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            Time.timeScale = 0.5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Time.timeScale = 1.0f;
        }
    }

    void LateUpdate()
    {
        //RightArm.transform.LookAt (Enemy.transform.position * 2f - RightArm.transform.position, new Vector3(0,0.5f, 0));
        //targetedRightEnemy = targetedEnemy;
        //targetedLeftEnemy = targetedEnemy;
        if (targetedLeftEnemy != null && targetedRightEnemy != null)
        {
            #region Dead Code
            //Debug.Log("Tracking enemy");
            //Vector3 forward = RightArm.transform.TransformDirection(Vector3.forward) * 10;
            //Debug.DrawRay(LeftArm.transform.position, -LeftArm.transform.forward, Color.green);
            //LeftArm.transform.LookAt(targetedEnemy.transform);
            //RightArm.transform.LookAt(targetedEnemy.transform);
            //RightArm.transform.rotation = Quaternion.Slerp(RightArm.transform.rotation, Quaternion.LookRotation(Enemy.transform.position), 10*Time.deltaTime);
            /*Vector3 targetDir = Enemy.transform.position - RightArm.transform.position;
			float step = 5 * Time.deltaTime;
			//Vector3 relativePos = RightArm.transform.position - Enemy.transform.position;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
			RightArm.transform.rotation = Quaternion.LookRotation(new Vector3( newDir.x + 70, newDir.y -100f, newDir.z-160f));
			//Quaternion rotation = Quaternion.LookRotation (relativePos,Vector3.forward);
			//RightArm.transform.rotation = rotation;
			//RightArm.transform.LookAt (Enemy.transform.position * 2f - RightArm.transform.position, transform.right);*/
            #endregion
        }
        else
        {
            upperBody.transform.localEulerAngles = new Vector3(cameraRotationLimitX, upperBody.transform.rotation.y, upperBody.transform.rotation.z);
        }
        //Debug.Log(rotation);

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
    void MoveForwards(float moveZ, float moveX)
    {
        //if(Boosting != true) {
            Vector3 movement = new Vector3(moveX, 0.00f, moveZ);
            RB.AddRelativeForce (movement * groundForce,ForceMode.Impulse);
            //Debug.Log("Current velocity mag is: " + RB.velocity.magnitude);
            if(RB.velocity.magnitude > magnitudeReference) { 
               RB.velocity = groundForce * (RB.velocity.normalized);
            }
        //}
        //RB.velocity = new Vector3 (1 * groundForce * moveX,0,1 * groundForce * moveZ) ;
        //Vector3 direction = RB.GetRelativePointVelocity(Vector3.right * moveX * groundForce);
        //direction += RB.velocity;
        //RB.velocity = movement;
        //RB.AddRelativeForce(movement * 30.0f - RB.velocity);
        //transform.position += transform.forward * Time.deltaTime * moveZ * groundForce;
        //transform.position += transform.right * Time.deltaTime * moveX * groundForce;
    }

    void MoveFlying(float moveY)
    {
        //Vector3 movement = new Vector3 (moveY,0.0f,0.0f);
        RB.AddForce(transform.up * jumpForce * jumpSpeed * moveY, ForceMode.Impulse);
        if (RB.velocity.magnitude > magnitudeReference) {
            RB.velocity = jumpForce * (RB.velocity.normalized);
        }
    }
    #endregion

    #region Dash Method
    IEnumerator Dash()
    {
        Boosting = true;
        groundForce *= 2;
        yield return new WaitForSeconds(0.5f);
        groundForce /= 2;
        Boosting = false;
        if (RB.velocity.magnitude > magnitudeReference) {
            RB.velocity = groundForce * (RB.velocity.normalized);
        }
        /*groundSpeed += groundSpeed/2;
		yield return new WaitForSeconds (1.0f);
		groundSpeed = 10;*/
        /*switch (DashDirection)
        {

            case "Forward":
                Debug.Log("Dash: " + DashDirection);

                //RB.AddForce(transform.forward * groundSpeed * 10, ForceMode.Impulse);
                //RB.AddRelativeForce(transform.forward * 150.0f * boostForce);
                //groundForce *= 3;
                yield return new WaitForSeconds(0.5f);
                //groundForce /= 3;
                //RB.AddRelativeForce(-transform.forward * 70.0f * boostForce);

                //RB.AddForce(-transform.forward * groundSpeed * 20, ForceMode.Impulse);
                break;

            case "Right":
                Debug.Log("Dash: " + DashDirection);
                //RB.AddForce(transform.right * groundSpeed * 10, ForceMode.Impulse);
                break;

            case "Left":
                Debug.Log("Dash: " + DashDirection);
                //RB.AddForce(-transform.right * groundSpeed * 10, ForceMode.Impulse);
                break;

            case "Backwards":
                Debug.Log("Dash: " + DashDirection);
                //RB.AddForce(-transform.forward * groundSpeed * 10, ForceMode.Impulse);
                break;
        }*/
    }
    #endregion

    #region Right Reload
    public IEnumerator Right_Reload()
    {
        GameObject enemyHold = null;
        if (targetedRightEnemy != null)
        {
            //Debug.Log("Removing Right Enemy");
            enemyHold = targetedRightEnemy;
            targetedRightEnemy = null;
        }
        Anim.SetBool("RightArm_Reload", true);
        yield return new WaitForSeconds(0.98f);
        Anim.SetBool("RightArm_Reload", false);
        if (enemyHold != null)
        {
            targetedRightEnemy = enemyHold;
            enemyHold = null;
        }
    }
    #endregion

    #region Left Reload
    public IEnumerator Left_Reload()
    {
        GameObject enemyHold = null;
        if (targetedLeftEnemy != null)
        {
            //Debug.Log("Removing Left Enemy");
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

    void OnTriggerEnter(Collider other)
    {
        /*if (other.tag == "EnemyBullet") {
			if (Shields > 0) {
				Shields -= 3;
			} else {
				Health -= 3;
			}
		}*/
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
        Anim.SetBool("ShieldDown", false);
        Anim.SetBool("ShieldDropping", false);
        Anim.SetBool("ShieldRecharging", true);
        yield return new WaitForSeconds(1.0f);
        Anim.SetBool("ShieldRecharging", false);
        Anim.SetBool("ShieldUp", true);
        Shield.SetActive(true);
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
    #endregion

    private IEnumerator ScannerDown() {
        scanOverlay.GetComponent<Animator>().SetBool("Deactivated", true);
        yield return new WaitForSeconds(1.00f);
        isScanning = false;
        scanOverlay.SetActive(false);
        scannerText.SetActive(false);
        talkingMaterial.color = new Color(0f, 0f, 255f);
    }
    #region Tester Methods
    public void FrameChange() {
        switch (frameType.value) {
            case 0:
                //65 Magnitude Reference
                //73 Ground Force
                Health = 1500;
                healthReference = Health;
                magnitudeReference = 65;
                groundForce = 63;
                Shields = 50;
                shieldsReference = Shields;
                break;

            case 1:
                //50 Magnitude Reference
                //50-60 Ground Force
                Health = 2000;
                healthReference = Health;
                magnitudeReference = 68;
                groundForce = 50;
                Shields = 50;
                shieldsReference = Shields;
                break;

            case 2:
                //50 Magnitude Reference
                //40 Ground Force
                Health = 2750;
                healthReference = Health;
                magnitudeReference = 45;
                groundForce = 30;
                Shields = 50;
                shieldsReference = Shields;
                break;

        }
    }
    #endregion

}