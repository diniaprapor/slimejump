﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

/* todo:
 * + add infinite level generation
 * + add score count and display
 * + add mobile controls
 * + not go forward until first touch
 * + resolution independent UI text
 * + make platforms less overlaping
 * + add 2 types of collectibles
 * + remove initial platform prefab, make everything generated
 * + start from higher position
 * + autorotate view
 * + double score bug
 * + make character not to be stuck at platform edges
 * + serializedobject target has been destroyed
 * + close app on back button
 * + fix jump length
 * + increase game speed with time
 * + make lower platforms visible on jump
 * + make platforms not to go too low
 * + increase platform pool
 * + fix coin positions on resized platforms
 * + make variable size platforms
 * + resolution independent score text
 * + make pause at start/restart
 * - some sort of main menu
 * + fix platform end stuck bug
 * + texture for platforms
 * + background gradient
 * + clouds + cloud colors + positioning + shapes + parallax
 * + make platforms less bright
 * + make platforms right texture size
 * + destroy not taken coins and gems
 * + refactor scripts, so that platform controller isnt attached to character
 * + refactor SpawnManager so that GameController sets its vars
 * + refactor SpawnCoins so that GameController sets its vars
 * + refactor CharacterController so that GameController sets its vars
 * + fully replace old char with new and clean up resources
 * + screenshot
 * + hold for jump strength
 * + new char stuck at front edge
 * + make straight infinite run mode for debugging
 * + fix new char anims
 * + coin and gem animation (up-down wobble)
 * + switch to dragonbones animation
 * + load char anims dynamically
 * + create char at game start instead of keeping it in scene
 * + cleanup unity animations
 * + pause button
 * - screenshot button
 * Separate main menu and gameplay
 * In-game pause menu
 * Gameover menu
 * make possible to switch characters skins (char switch menu)
 * currency system
 * fix platform texture sizes/scale
 * show previous record distance
 * !make camera resolution independent
 * settings menu
 * tasks menu
 * shop
 * achievement system
 * add some basic music and sound
 * add basic effects on coin collect
 * localize text
 * online leaderboard
 * limited restored over time lives and watch ad / in-app to skip that
 * ui icons to spritesheet
 * AssetBundle support
 * Player name
 * Play login support
 * slow fall ability?
 * double jump?
 * dash or jump + dash?
*/

//global vars and top level game logic
public class GameplayController : MonoBehaviour
{
    public float collectableSpawnProbability = 0.5f;
    public float gemSpawnProbability = 0.2f;

    public GameObject heroPrefab;//characterGO;
    public float deathLimit = -40f;
    public float gameSpeed = 1.0f;

    public Text gameOverText;
    public GameObject InGameUI;

    //private CharacterController characterController;
    private GameObject heroInstance;
    private delegate void UpdateDelegate();
    private UpdateDelegate currentUpdate;

    private Score score;
    private BgMusic bgm;

    private static bool firstRun = true;
    private Vector3 initialCharScale;

    private GUIStyle debugUIStyle = new GUIStyle();

    //called before start
    void Awake()
    {
        Assert.IsNotNull(heroPrefab, "Character GameObject not set!");
    }

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Score>();
        Assert.IsNotNull(score, "Score component not found!");

        bgm = GetComponent<BgMusic>();

        SetupGameoverState();
    }

    // Update is called once per frame
    void Update()
    {
        currentUpdate();

        //exit app on back button
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P)){
            string folder = "Screenshots\\";
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            var nameScreenshot = folder + "Screenshot_" + System.DateTime.Now.ToString("dd-mm-yyyy-hh-mm-ss") + ".png";
            ScreenCapture.CaptureScreenshot(nameScreenshot);
        }
#endif
    }

    void GameUpdate()
    {
        //check death
        if (heroInstance.transform.position.y < deathLimit)
        {
            SceneManager.LoadScene("Main");
        }
    }

    void PauseUpdate()
    {
        bool anykey = false;
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        anykey = Input.anyKeyDown;
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0){
            Touch myTouch = Input.touches[0];
            if (myTouch.phase == TouchPhase.Began)
                anykey = true;
        }
#endif
        if (anykey)
        {
            //spawn hero
            //SetCharacterVisible(true);
            InstantiateHero();
            gameOverText.gameObject.SetActive(false);
            InGameUI.SetActive(true);
            PauseOverlaySetActive(false);
            currentUpdate = GameUpdate;
            Time.timeScale = gameSpeed;
            score.Reset();

            bgm.PlayAudio("Boss Theme");
        }
    }

    void SetupGameoverState()
    {
        currentUpdate = PauseUpdate;
        Time.timeScale = 0;

        gameOverText.gameObject.SetActive(true);
        Text restartText = gameOverText.transform.Find("TapToRestart").gameObject.GetComponent<Text>();
        Text scoreValText = gameOverText.transform.Find("GOScoreVal").gameObject.GetComponent<Text>();
        Text hiScoreValText = gameOverText.transform.Find("GOHiScoreVal").gameObject.GetComponent<Text>();
        if (firstRun)
        {
            firstRun = false;
            gameOverText.text = "Go!";
            restartText.text = "Tap to start";
        }
        else
        {
            gameOverText.text = "Game Over";
            restartText.text = "Tap to restart";
        }
        scoreValText.text = score.GetScore().ToString();
        hiScoreValText.text = score.GetHiScore().ToString();

        InGameUI.SetActive(false);
        //SetCharacterVisible(false); 
        RemoveHero(); //destroy hero instance if created

        bgm.PlayAudio("Dungeon Theme");
    }

    private void SetCharacterVisible(bool visible)
    {
        if (visible)
        {
            heroInstance.transform.localScale = initialCharScale;
        }
        else
        {
            heroInstance.transform.localScale = Vector3.zero;
        }
    }

    private void InstantiateHero()
    {
        Transform hp = transform.Find("HeroSpawn");
        heroInstance = Instantiate<GameObject>(heroPrefab, hp.position, hp.rotation);
        initialCharScale = heroInstance.transform.localScale;

        //set score adding callback
        PlatformCharacterController pcc = heroInstance.GetComponent<PlatformCharacterController>();
        pcc.addScore = score.AddScore;

        SetupPlatformSpawnManager();

        //init camera follow
        SetCameraFollow(heroInstance.transform);
    }

    private void RemoveHero()
    {
        if(heroInstance != null)
        {
            SetCameraFollow(null);

            Destroy(heroInstance);
            heroInstance = null;
        }
    }

    private void SetCameraFollow(Transform t)
    {
        GameObject cam = GameObject.Find("Main Camera");
        if (cam)
        {
            FollowCamera fc = cam.GetComponent<FollowCamera>();
            if (t != null)
                fc.StartFollowing(t);
            else
                fc.StopFollowing();
        }
        else
        {
            Debug.Log("main camera not found");
        }
    }

    private void SetupPlatformSpawnManager()
    {
        PlatformSpawnManager sm = GetComponent<PlatformSpawnManager>();
        Assert.IsNotNull(sm, "Spawn Manager component not found!");

        Transform op = transform.Find("OriginPosition");
        sm.ResetAndStartSpawning(heroInstance.transform, op.position, deathLimit);
    }

    void OnGUI()
    {
        //debug stuff
        if (Debug.isDebugBuild)
        {
            debugUIStyle.fontSize = 30;
            /*
            Animator charAnimator = characterGO.GetComponent<Animator>();
            AnimatorClipInfo[] animatorClipInfo = charAnimator.GetCurrentAnimatorClipInfo(0);
            AnimationClip currentClip = animatorClipInfo.Length > 0 ? animatorClipInfo[0].clip : null;
            if (currentClip != null)
                GUI.Label(new Rect(10, 10, 300, 100), "CurrentAnimation: " + currentClip.name, debugUIStyle);
            */
        }
    }

    private void PauseOverlaySetActive(bool active)
    {
        GameObject pauseOverlay = InGameUI.transform.Find("PauseOverlay").gameObject;
        pauseOverlay.SetActive(active);
        GameObject pauseBtn = InGameUI.transform.Find("PauseButton").gameObject;
        pauseBtn.SetActive(!active);
    }

    public void PauseClickStart()
    {
        if(currentUpdate == GameUpdate && Time.timeScale > 0)
        {
            PauseOverlaySetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void PauseClickResume()
    {
        if (currentUpdate == GameUpdate && Time.timeScale < Mathf.Epsilon)
        {
            PauseOverlaySetActive(false);
            Time.timeScale = gameSpeed;
        }
    }

    public void PauseClickExit()
    {
        PauseClickResume();
    }
}