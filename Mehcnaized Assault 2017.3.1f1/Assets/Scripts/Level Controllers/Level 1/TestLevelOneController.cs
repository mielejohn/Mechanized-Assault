using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TestLevelOneController : MonoBehaviour {

    public Animator Anim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            StartCoroutine(EleveatorAnim());
        }
    }

    public IEnumerator EleveatorAnim() {
        yield return new WaitForSeconds(1.0f);
        Anim.SetBool("Elevator Move", true);
    }
}
