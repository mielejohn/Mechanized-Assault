﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rover_Enemy : MonoBehaviour {

    [Header("Health")]
	public float Health = 100;
    public float baseHealth;
	public Slider HealthBar;

    [Header("Class References")]
    public PlayerController PC;
    public Enemy enemyClass;
    public TestController TC;

    [Header("Targeted and UI")]
    //public bool Targeted;
    //public bool Scanned;
    //public GameObject UICanvas;

    [Header("States and Patrol points")]
    protected EnemyAIStates state = EnemyAIStates.Patrolling;
    public List<GameObject> patrolPoints = null;
    public GameObject patrollingInterestPoint;
    public GameObject playerOfInterest;
    //public GameObject patrolPointOne;
    //public GameObject patrolPointTwo;

    [Header("Speeds")]
    public float walkingSpeed = 3.0f;
    public float chasingSpeed = 5.0f;
    public float attackingSpeed = 1.5f;

    [Header("Distances")]
    public float attackingDistance;
    public float attackStoppingDistance;
    public float patrolDistance = 20.0f;

    [Header("Current State")]
    public EnemyAILifeStates Cs = EnemyAILifeStates.Paused;

    [Header("NavMesh Agent")]
    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    [Header("Animations")]
    public Animator Anim;
    public Rigidbody RB;

    [Header("Explosions")]
    public GameObject explosion01;
    public GameObject explosion02;
    public GameObject explosion03;
    // Use this for initialization

    void Start () {
        /*if (patrolPoints == null) {
            print("FIND POINTS...");
            patrolPoints = new List<GameObject>();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PatrolPoints")) {
                Debug.Log("Adding Enemy Patrol Point: " + go.transform.position);
                patrolPoints.Add(go);
            }
        }*/

        Health = baseHealth;
        RB = GetComponent<Rigidbody>();
        playerOfInterest = GameObject.FindGameObjectWithTag("Player");
        SwitchToPatrolling();
    }

    // Update is called once per frame
    void Update () {

		if (enemyClass.Health <= 0 && Cs != EnemyAILifeStates.Dead) {
            enemyClass.isAlive = false;
            StartCoroutine(Dead());
		}

        if(Cs == EnemyAILifeStates.Alive) { 
            switch (state) {
                case EnemyAIStates.Attacking:
                    OnAttackingUpdate();
                    break;
                case EnemyAIStates.Chasing:
                    OnChasingUpdate();
                    break;
                case EnemyAIStates.Patrolling:
                    OnPatrollingUpdate();
                    break;
            }
        }
        //Debug.Log(navMeshAgent.velocity.normalized);
        Anim.SetFloat("Z-Movement", navMeshAgent.velocity.normalized.z);
        Anim.SetFloat("X-Movement", navMeshAgent.velocity.normalized.x);
    }

	void OnTriggerEnter(Collider other){

	}

    private void CanvasRotate() {
        GameObject playerReferenceObject = PC.middleReference;
        enemyClass.UICanvas.transform.LookAt(transform.position + playerReferenceObject.transform.rotation * Vector3.right, playerReferenceObject.transform.rotation * Vector3.up);
    }

    protected virtual void OnAttackingUpdate() {
        float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);

        if (distance > attackStoppingDistance){
            Debug.Log("Distance < attackStoppingDistance");
            navMeshAgent.SetDestination(playerOfInterest.transform.position);
        } else {
            navMeshAgent.SetDestination(this.transform.position);
        }


        //Debug.Log ("Distance Attacking: " + distance);
        if (distance > attackingDistance) {
            SwitchToChasing(playerOfInterest);
        }
    }

    protected virtual void OnChasingUpdate() {
        navMeshAgent.SetDestination(playerOfInterest.transform.position);

        float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
        float distanceToPlayer = Vector3.Distance(transform.position, playerOfInterest.transform.position);


        //Debug.Log ("Distance Chasing: " + distance);
        if (distance <= attackingDistance) {
            SwitchToAttacking(playerOfInterest);
        }

        if (distanceToPlayer > patrolDistance) {
            SwitchToPatrolling();
        }
    }

    protected virtual void OnPatrollingUpdate() {
        navMeshAgent.SetDestination(patrollingInterestPoint.transform.position);

        float distance = Vector3.Distance(transform.position, patrollingInterestPoint.transform.position);
        //Debug.Log("Distance to selected patrol point is: " + distance);
        float distanceToPlayer = Vector3.Distance(transform.position, playerOfInterest.transform.position);

        //Debug.Log("Patrolling Distance: " + distance);
        if (distance <= navMeshAgent.stoppingDistance) {
            ChangePatrolPoint();
        }

        if (distanceToPlayer < patrolDistance && playerOfInterest.GetComponent<PlayerController>().canMove == true) {
            SwitchToChasing(playerOfInterest);
        }
    }


    protected void SwitchToPatrolling() {
        state = EnemyAIStates.Patrolling;
        Debug.Log("Switch to patrol");
        ChangePatrolPoint();
    }

    protected void SwitchToAttacking(GameObject target) {
        state = EnemyAIStates.Attacking;
        Debug.Log("Switch to attack");
    }

   protected void SwitchToChasing(GameObject target) {
        state = EnemyAIStates.Chasing;
        Debug.Log("Switch to chasing");
        playerOfInterest = target;
    }

    protected virtual void ChangePatrolPoint() {
        int choice = Random.Range(0, patrolPoints.Count);
        patrollingInterestPoint = patrolPoints[choice];
        Debug.Log(this.gameObject.ToString() + " is changing patrolpoint");
    }

    private IEnumerator Dead() {
        Cs = EnemyAILifeStates.Dead;
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (this.gameObject == PC.targetedLeftEnemy) {
            PC.targetedLeftEnemy = null;
            PC.targetedRightEnemy = null;
            PC.Targeting = false;
            TC.enemyCount--;
        }
        explosion01.SetActive(true);
        explosion02.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        explosion03.SetActive(true);
    }
}
