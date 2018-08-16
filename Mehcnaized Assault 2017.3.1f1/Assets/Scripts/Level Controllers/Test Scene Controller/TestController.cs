using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Sirenix.OdinInspector;

public class TestController : MonoBehaviour {

    public PlayableDirector scenePlayableDirector;
    public PlayerController Player;
    public GameObject playerCamera;
    public Canvas playerCanvas;
    public float sceneTimelineLength;

    [Header("Controls")]
    public bool controlsOnScreen;
    public GameObject keyboardControls;

    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Player.canMove = true;
        playerCanvas = GameObject.FindGameObjectWithTag("Player Canvas").GetComponent<Canvas>();
        playerCamera = GameObject.FindGameObjectWithTag("Player Camera");
        //StartCoroutine(LevelOpeningCutscene(sceneTimelineLength));
    }

	void Update () {
        if (controlsOnScreen == false && Input.GetKeyDown(KeyCode.Escape)) {
            keyboardControls.SetActive(true);
            controlsOnScreen = true;
        } else if (controlsOnScreen == true && Input.GetKeyDown(KeyCode.Escape)) {
            keyboardControls.SetActive(false);
            controlsOnScreen = false;
        }
    }

    private IEnumerator LevelOpeningCutscene(float WaitTime) {
        playerCamera.SetActive(false);
        playerCanvas.renderMode = RenderMode.WorldSpace;         
        yield return new WaitForSeconds(WaitTime);
        playerCamera.SetActive(true);
        Player.canMove = true;
        playerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}
