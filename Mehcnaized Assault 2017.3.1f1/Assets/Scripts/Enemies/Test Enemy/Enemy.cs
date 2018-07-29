using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyAIStates { Patrolling, Chasing, Attacking };
public enum CurrentState {Alive, Dead, Paused };
public class Enemy : MonoBehaviour {

    [Header("Health")]
	public float Health = 100;
    public float baseHealth;
	public Slider HealthBar;

    [Header("Player Controller")]
    public PlayerController PC;

    [Header("Targeted and UI")]
    public bool Targeted;
    public bool Scanned;
    public GameObject UICanvas;

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
    public float attackingDistance = 1.0f;
    public float patrolDistance = 20.0f;

    [Header("Current State")]
    public CurrentState Cs = CurrentState.Paused;

    [Header("NavMesh Agent")]
    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    [Header("Animations")]
    public Animator Anim;
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

        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseHealth = Health;
        playerOfInterest = GameObject.FindGameObjectWithTag("Player");
        SwitchToPatrolling();
    }

    // Update is called once per frame
    void Update () {
        //CanvasRotate();
		if (Health <= 0) {
            Dead();
		}

        if (Targeted == true && Scanned == true)
        {
            UICanvas.SetActive(true);
        }
        else
        {
            UICanvas.SetActive(false);
        }
        if(Cs == CurrentState.Alive) { 
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

        /*if(Scanned == true) {
            UICanvas.SetActive(true);
        } else if(Scanned == false) {
            UICanvas.SetActive(false);
        }*/

        //Patrolling();

        /*if (Input.GetKey(KeyCode.UpArrow) && Anim.GetFloat("Z-Movement") < 1.0f) {
            Anim.SetFloat("Z-Movement", Anim.GetFloat("Z-Movement") + 0.01f);
        } else if (Input.GetKey(KeyCode.LeftArrow) && Anim.GetFloat("X-Movement") > -1.0f) {
            Anim.SetFloat("X-Movement", Anim.GetFloat("X-Movement") - 0.01f);
        } else if (Input.GetKey(KeyCode.RightArrow) && Anim.GetFloat("X-Movement") < 1.0f) {
            Anim.SetFloat("X-Movement", Anim.GetFloat("X-Movement") + 0.01f);
        } else if (Input.GetKey(KeyCode.DownArrow) && Anim.GetFloat("Z-Movement") > -1.0f) {
            Anim.SetFloat("Z-Movement", Anim.GetFloat("Z-Movement") - 0.01f);
        }*/
    }

    public void Hit(float Damage){
		Debug.Log ("Enemy damage = " + Damage);
		Health -= Damage;
        HealthBar.value = Health;
    }

	void OnTriggerEnter(Collider other){

	}

    private void CanvasRotate() {
        GameObject playerReferenceObject = PC.middleReference;
        UICanvas.transform.LookAt(transform.position + playerReferenceObject.transform.rotation * Vector3.right, playerReferenceObject.transform.rotation * Vector3.up);
    }

    protected virtual void OnAttackingUpdate() {
        navMeshAgent.SetDestination(playerOfInterest.transform.position);

        float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);

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

        /*if (distanceToPlayer < patrolDistance) {
            SwitchToChasing(playerOfInterest);
        }*/
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

    private void Dead() {
        if (this.gameObject == PC.targetedLeftEnemy) {
            PC.targetedLeftEnemy = null;
            PC.targetedRightEnemy = null;
            PC.Targeting = false;
            Cs = CurrentState.Dead;
        }
        //Destroy(this.gameObject);
    }
}
