using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestController : MonoBehaviour {

    public GameObject dashFrame;
    public GameObject assaultFrame;
    public GameObject titanFrame;

    public PlayableDirector scenePlayableDirector;
    public PlayerController Player;
    public GameObject playerCamera;
    public Canvas playerCanvas;
    public GameObject playerCanvasOBJ;
    public float sceneTimelineLength;

    public int enemyCount;

    [Header("Controls")]
    public bool controlsOnScreen;
    public GameObject keyboardControls;

    [Header("Scene Transition")]
    public bool sceneChangeActive = false;
    public GameObject screenFader;
    public Slider loadingBar;
    public AsyncOperation async;

    private void Awake() {
        switch (PlayerPrefs.GetInt("FrameChoice")) {
            case 1:
                Instantiate(dashFrame);
                break;

            case 2:
                Instantiate(assaultFrame);
                break;

            case 3:
                Instantiate(titanFrame);
                break;
        }
    }

    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Player.canMove = false;
        playerCanvas = GameObject.FindGameObjectWithTag("Player Canvas").GetComponent<Canvas>();
        playerCanvasOBJ = GameObject.FindGameObjectWithTag("Player Canvas");
        playerCamera = GameObject.FindGameObjectWithTag("Player Camera");

        StartCoroutine(LevelOpeningCutscene(sceneTimelineLength));
    }

	void Update () {
        if (controlsOnScreen == false && Input.GetKeyDown(KeyCode.Escape)) {
            keyboardControls.SetActive(true);
            playerCanvasOBJ.SetActive(false);
            controlsOnScreen = true;
        } else if (controlsOnScreen == true && Input.GetKeyDown(KeyCode.Escape)) {
            keyboardControls.SetActive(false);
            playerCanvasOBJ.SetActive(true);
            controlsOnScreen = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            LoadScene(1);
        }

        if(enemyCount <= 0 && sceneChangeActive != true) {
            sceneChangeActive = true;
            StartCoroutine(MissionOver());
        }

        if (enemyCount <= 0 && sceneChangeActive != true) {
            sceneChangeActive = true;
            StartCoroutine(MissionOver());
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && sceneChangeActive != true) {
            sceneChangeActive = true;
            StartCoroutine(MissionOver());
        }
    }

    private IEnumerator LevelOpeningCutscene(float WaitTime) {
        playerCamera.SetActive(false);
        playerCanvas.renderMode = RenderMode.WorldSpace;         
        yield return new WaitForSeconds(WaitTime);
        playerCamera.SetActive(true);
        Player.Camera = playerCamera;
        Player.canMove = true;
        playerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    public void LoadScene(int SceneNumber) {
        Player.canMove = false;
        screenFader.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));
    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

    public IEnumerator MissionOver() {
        yield return new WaitForSeconds(2.0f);
        PlayerPrefs.SetInt("FromMission", 1);
        LoadScene(0);
    }
}
