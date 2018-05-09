using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using XInputDotNetPure;
using UnityEngine.UI;

using Sirenix.OdinInspector;

public enum ShieldStatus{Up, Down};

public class PlayerController : MonoBehaviour {

	[Header("Camera")]
	public GameObject Camera;
	float cameraRotationLimitX;

	[Header("Rigidbody")]
	public Rigidbody RB;

	[Header("Speed Variables")]
	//DASH Speed is 10
	//Assault Speed is 7
	//Titan Speed is 4

	public float groundSpeed = 8;
	public float jumpSpeed = 20;
	public float boostForce = 10;
	public float groundForce;	

	[Header("Animations")]
	public Animator Anim;
	public BlendTree BT;
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

	[Header("Targeted Enemy")]
	public GameObject targetedRightEnemy;
	public GameObject targetedLeftEnemy;

	[Header("Boolean Conditions")]
	public bool OnGround;
	public bool Flying;

	[Header("Shield and Health")]
	public ShieldStatus ShieldStatus = ShieldStatus.Up;
	public GameObject Shield;
	public Slider ShieldSlider;
	public Slider HealthSlider;
	public int Health = 30;
	public int Shields = 30;

	[Header("Weapon Spawns")]
	public GameObject leftWeaponSpawn;
	public GameObject rightWeaponSpawn;

	[Header("Stationary Pistols")]
	public GameObject leftStationaryPistol;
	public GameObject rightstationaryPistol;

	[Header("Pistol Prefabs")]
	public GameObject rightPistol;
	public GameObject leftPistol;

	[Header("TESTER SELECTIONS")]
	public string leftWeapon;
	public string rightWeapon;

    [BoxGroup("Left Weapons")]
    public GameObject leftShotgun;
    [BoxGroup("Left Weapons")]
    public GameObject leftAssaultRifle;
    [BoxGroup("Left Weapons")]
    public GameObject leftSubMachinegun;
    [BoxGroup("Left Weapons")]
    public GameObject leftSniperRifle;
    [BoxGroup("Left Weapons")]
    public GameObject leftMinigun;
    [BoxGroup("Left Weapons")]
    public GameObject leftMarksmanRifle;

    [BoxGroup("Right Weapons")]
    public GameObject rightShotgun;
    [BoxGroup("Right Weapons")]
    public GameObject rightAssaultRifle;
    [BoxGroup("Right Weapons")]
    public GameObject rightSubMachinegun;
    [BoxGroup("Right Weapons")]
    public GameObject rightSniperRifle;
    [BoxGroup("Right Weapons")]
    public GameObject rightMinigun;
    [BoxGroup("Right Weapons")]
    public GameObject rightMarksmanRifle;

	// Use this for initialization
	void Start ()
	{
		#region Weapon Select
		switch (rightWeapon) {

		case "Assault Rifle":
				GameObject RightAssaultRifle_I = Instantiate (rightAssaultRifle);
				RightAssaultRifle_I.transform.position = rightWeaponSpawn.transform.position;
				RightAssaultRifle_I.transform.parent = RightHand.transform;
			break;

		case "Shotgun":
				GameObject RightShotgun_I =  Instantiate (rightShotgun);
				RightShotgun_I.transform.position = rightWeaponSpawn.transform.position;
				RightShotgun_I.transform.parent = RightHand.transform;
			break;

		case "Marksman Rifle":
				GameObject RightMR_I =  Instantiate (rightMarksmanRifle);
				RightMR_I.transform.position = rightWeaponSpawn.transform.position;
				RightMR_I.transform.parent = RightHand.transform;
			break;

		case "Minigun":
				GameObject RightMinigun_I =  Instantiate (rightMinigun);
				RightMinigun_I.transform.position = rightWeaponSpawn.transform.position;
				RightMinigun_I.transform.parent = RightHand.transform;
			break;

		case "Sniper Rifle":
				GameObject RightSniperRifle_I =  Instantiate (rightSniperRifle);
				RightSniperRifle_I.transform.position = rightWeaponSpawn.transform.position;
				RightSniperRifle_I.transform.parent = RightHand.transform;
			break;

		case "Submachine Gun":
				GameObject RightSMG_I =  Instantiate (rightSubMachinegun);
				RightSMG_I.transform.position = rightWeaponSpawn.transform.position;
				RightSMG_I.transform.parent = RightHand.transform;
			break;
		}

		switch (leftWeapon) {

			case "Assault Rifle":
				GameObject LeftAssaultRifle_I =  Instantiate (leftAssaultRifle);
				LeftAssaultRifle_I.transform.position = leftWeaponSpawn.transform.position;
				LeftAssaultRifle_I.transform.parent = LeftHand.transform;
			break;

			case "Shotgun":
				GameObject LeftShotgun_I = (GameObject) Instantiate (leftShotgun);
				LeftShotgun_I.transform.position = leftWeaponSpawn.transform.position;
				LeftShotgun_I.transform.parent = LeftHand.transform;
			break;

		case "Marksman Rifle":
				GameObject LeftMR_I =  Instantiate (leftMarksmanRifle);
				LeftMR_I.transform.position = leftWeaponSpawn.transform.position;
				LeftMR_I.transform.parent = LeftHand.transform;
			break;

		case "Minigun":
				GameObject LeftMinigun_I =  Instantiate (leftMinigun);
				LeftMinigun_I.transform.position = leftWeaponSpawn.transform.position;
				LeftMinigun_I.transform.parent = LeftHand.transform;
			break;

		case "Sniper Rifle":
				GameObject LeftSniperRifle_I =  Instantiate (leftSniperRifle);
				LeftSniperRifle_I.transform.position = leftWeaponSpawn.transform.position;
				LeftSniperRifle_I.transform.parent = LeftHand.transform;
			break;

		case "Submachine Gun":
				GameObject LeftSMG_I =  Instantiate (leftSubMachinegun);
				LeftSMG_I.transform.position = leftWeaponSpawn.transform.position;
				LeftSMG_I.transform.parent = LeftHand.transform;
			break;
		}
		#endregion
	}

	void FixedUpdate ()
	{
		#region Keyboard Movement
		if (Input.GetButton ("Horizontal") || Input.GetButton ("Vertical") || Input.GetButton ("Jump")) {
			float moveX = Input.GetAxis ("Horizontal") * Time.deltaTime * groundSpeed;
			float moveZ = Input.GetAxis ("Vertical") * Time.deltaTime * groundSpeed;
			float moveY = Input.GetAxis ("Jump") * Time.deltaTime * jumpSpeed;

			animX = Input.GetAxis ("Horizontal");
			animZ = Input.GetAxis ("Vertical");
			MoveForwards (moveZ, moveX, animZ, animX);
			//MoveSideways (moveX, animX);
			MoveFlying (moveY);
		}
		Anim.SetFloat ("X_Axis", animX);
		Anim.SetFloat ("Z_Axis", animZ);
		#endregion

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
		if (GM.state.ThumbSticks.Left.X > 0 || GM.state.ThumbSticks.Left.Y > 0 || GM.state.ThumbSticks.Left.X < 0 || GM.state.ThumbSticks.Left.Y < 0) {
			float moveX = GM.state.ThumbSticks.Left.X * Time.deltaTime * groundSpeed;
			float moveZ = GM.state.ThumbSticks.Left.Y * Time.deltaTime * groundSpeed;
			animX = GM.state.ThumbSticks.Left.X;
			animZ = GM.state.ThumbSticks.Left.Y;
			//MoveForwards (moveZ, animZ);
			MoveSideways (moveX, animX);
		}
		#endregion

		#region Camera Rotation
		this.transform.Rotate (new Vector3 (0f, Input.GetAxis ("Mouse X") * 5, 0f));
		cameraRotationLimitX -= Input.GetAxis ("Mouse Y") * 2.5f;
		cameraRotationLimitX = Mathf.Clamp (cameraRotationLimitX, -35, 30);
		Camera.transform.localEulerAngles = new Vector3 (cameraRotationLimitX, 0f, 0f);
		#endregion
		//LeftArm.transform.localEulerAngles =  new Vector3 (cameraRotationLimitX, 90,LeftArm.transform.rotation.z);
		//RightArm.transform.localEulerAngles =  new Vector3 (cameraRotationLimitX, 90,RightArm.transform.rotation.z);

		//Spine.transform.localEulerAngles = new Vector3 (Spine.transform.rotation.x, Spine.transform.rotation.y, -cameraRotationLimitX);
	}

	// Update is called once per frame
	void Update ()
	{

		ShieldSlider.value = Shields;
		HealthSlider.value = Health;
		if (Shields <= 0) {
			if (ShieldStatus != ShieldStatus.Down) {
				StartCoroutine (ShieldDown ());
			}
		} else {
			//Shield.SetActive (true);
			if (ShieldStatus != ShieldStatus.Up) {
				StartCoroutine (ShieldUp ());
			}
		}
		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			Shields = 30;
			//StartCoroutine(ShieldUp());
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			targetedLeftEnemy = null;
			targetedRightEnemy = null;
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			StartCoroutine (RightPistolSwap ());
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			StartCoroutine (LeftPistolSwap ());
		}

		if (Input.GetButtonDown ("Jump") && Input.GetKey(KeyCode.W)) {
			Debug.Log("Pressed dash forward");
			StartCoroutine(Dash("Forward"));
		} else if (Input.GetButtonDown ("Jump") && Input.GetKey(KeyCode.A)) {
			Debug.Log("Pressed dash left");
			//StartCoroutine(Dash("Left"));
		} else if (Input.GetButtonDown ("Jump") && Input.GetKey(KeyCode.D)) {
			Debug.Log("Pressed dash right");
			//StartCoroutine(Dash("Right"));
		}else if (Input.GetButtonDown ("Jump") && Input.GetKey(KeyCode.S)) {
			Debug.Log("Pressed dash backwards");
			//StartCoroutine(Dash("Backwards"));
		}
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
		RaycastHit hit;
		if(Physics.Raycast(transform.position, -Vector3.up, out hit)){
			if (hit.distance < 15.0f && hit.collider.tag == "Ground") {
				Debug.Log ("on the ground");
				OnGround = true;
				Flying = false;	
				Anim.SetBool ("Flying", Flying);
				Anim.SetBool ("OnGround", OnGround);
			} else {
				Debug.Log ("Off the ground");
				OnGround = false;
				Flying = true;
				Anim.SetBool ("Flying", Flying);
				Anim.SetBool ("OnGround", OnGround);
			}
		}
	}

	void LateUpdate()
	{
		//RightArm.transform.LookAt (Enemy.transform.position * 2f - RightArm.transform.position, new Vector3(0,0.5f, 0));
		//targetedRightEnemy = targetedEnemy;
		//targetedLeftEnemy = targetedEnemy;
		if (targetedLeftEnemy != null && targetedRightEnemy != null)
		{
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
	void MoveForwards(float moveZ, float moveX, float animZ, float animX){
		Vector3 movement = new Vector3 (moveX * 2.0f, 0.0f, moveZ*2.0f);
		//RB.AddForce (transform.forward * groundForce * moveZ,ForceMode.VelocityChange);
		//RB.velocity = new Vector3 (1 * groundForce * moveX,0,1 * groundForce * moveZ) ;
		//Vector3 direction = RB.GetRelativePointVelocity(Vector3.right * moveX * groundForce);
		//direction += RB.velocity;
		//RB.velocity = movement;
		//RB.AddRelativeForce(movement * 30.0f - RB.velocity);
		transform.position += transform.forward * Time.deltaTime * moveZ * groundForce;
		transform.position += transform.right * Time.deltaTime * moveX * groundForce;
	}

	void MoveSideways(float moveX, float animX){
		//Vector3 movement = new Vector3 (moveX,0.0f,0.0f);
		//RB.AddForce (transform.right * groundForce * moveX ,ForceMode.VelocityChange);
		//RB.velocity = new Vector3 (1 * groundForce * moveX,0,0) ;
		Anim.SetFloat ("X_Axis", animX);
	}

	void MoveFlying(float moveY){
		//Vector3 movement = new Vector3 (moveY,0.0f,0.0f);
		RB.AddForce (transform.up * 2 * moveY ,ForceMode.Impulse);
	}
	#endregion

	#region Dash Method
	IEnumerator Dash (string DashDirection)
	{
		/*groundSpeed += groundSpeed/2;
		yield return new WaitForSeconds (1.0f);
		groundSpeed = 10;*/
		switch (DashDirection) {

		case "Forward":
			Debug.Log("Dash: " + DashDirection);
			//RB.AddForce(transform.forward * groundSpeed * 10, ForceMode.Impulse);
			RB.AddRelativeForce(transform.forward * 70.0f*boostForce);
			yield return new WaitForSeconds (1.0f);
			RB.AddRelativeForce(-transform.forward * 70.0f*boostForce);
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
		}
	}
	#endregion

	#region Right Reload
	public IEnumerator Right_Reload()
	{
		GameObject enemyHold = null;
		if (targetedRightEnemy != null)
		{
			Debug.Log("Removing Right Enemy");
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
	public IEnumerator Left_Reload(){
		GameObject enemyHold = null;
		if (targetedLeftEnemy != null)
		{
			Debug.Log("Removing Left Enemy");
			enemyHold = targetedLeftEnemy;
			targetedLeftEnemy = null;
		}
		Anim.SetBool ("LeftArm_Reload", true);
		yield return new WaitForSeconds (0.98f);
		Anim.SetBool ("LeftArm_Reload", false);
		if (enemyHold != null)
		{
			targetedLeftEnemy = enemyHold;
			enemyHold = null;
		}	
	}
	#endregion
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "EnemyBullet") {
			if (Shields > 0) {
				Shields -= 3;
			} else {
				Health -= 3;
			}
		}
	}

	private IEnumerator ShieldDown(){
		ShieldStatus = ShieldStatus.Down;
		Anim.SetBool ("ShieldUp", false);
		Anim.SetBool ("ShieldRecharging", false);
		Anim.SetBool ("ShieldDropping", true);
		yield return new WaitForSeconds (1.0f);
		Anim.SetBool ("ShieldDropping", false);
		Anim.SetBool ("ShieldDown", true);
		Shield.SetActive (false);

	}

	private IEnumerator ShieldUp(){
		ShieldStatus = ShieldStatus.Up;
		Anim.SetBool ("ShieldDown", false);
		Anim.SetBool ("ShieldDropping", false);
		Anim.SetBool ("ShieldRecharging", true);
		yield return new WaitForSeconds (1.0f);
		Anim.SetBool ("ShieldRecharging", false);
		Anim.SetBool ("ShieldUp", true);
		Shield.SetActive (true);
	}

	private IEnumerator RightPistolSwap ()
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
	}
}