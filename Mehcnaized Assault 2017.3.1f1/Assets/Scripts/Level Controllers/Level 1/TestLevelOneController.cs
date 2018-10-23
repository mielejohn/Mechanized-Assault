using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestLevelOneController : MonoBehaviour {

    public Animator Anim;
    public GameObject Elevator;
    public PlayerController PC;

    [Header("Scene Transition")]
    public GameObject screenFader;
    public Slider loadingBar;
    public AsyncOperation async;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            PlayerPrefs.SetInt("Completed Tutorial", 1);
            LoadScene(0);
         }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            StartCoroutine(EleveatorAnim());
        }
    }

    public IEnumerator EleveatorAnim() {
        yield return new WaitForSeconds(0.7f);
        PC.canMove = false;
        PC.gameObject.transform.parent = Elevator.transform;
        yield return new WaitForSeconds(3.0f);
        Anim.SetBool("Elevator On", true);
        yield return new WaitForSeconds(6.0f);
        PC.gameObject.transform.parent = null;
        PC.canMove = true;

    }

    public void LoadScene(int SceneNumber) {
        //screenFader.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));

    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        PlayerPrefs.SetInt("FromMission", 1);
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            //loadingBar.value = async.progress;
            yield return null;
        }
    }
}
