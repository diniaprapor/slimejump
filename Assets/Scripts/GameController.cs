using System.Collections;
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
 * fully replace old char with new and clean up resources
 * new char stuck at front edge
 * fix new char anims
 * hold for jump strength
 * jump strength = hold duration?
 * show previous record distance
 * !make camera resolution independent
 * make possible to switch characters skins
 * settings menu
 * achievement system
 * hi score saving
 * make platform collision a bit wider than visual part
 * slow fall ability?
 * double jump?
 * add some basic music and sound
 * add basic effects on coin collect
 * coin and gem animation (up-down wobble)
 * localize text
 * limited restored over time lives and watch ad / in-app to skip that
*/

//global vars and top level game logic
public class GameController : MonoBehaviour
{
    //can't set 'em in inspector, maybe will refactor some day
    public static float collectableSpawnProbability = 0.5f;
    public static float gemSpawnProbability = 0.2f;

    public GameObject characterGO;
    public float deathLimit = -40f;

    public Text gameOverText;

    //private CharacterController characterController;
    private delegate void UpdateDelegate();
    private UpdateDelegate currentUpdate;

    private Score score;

    private static bool firstRun = true;
    private Vector3 initialCharScale;

    //called before start
    void Awake()
    {
        Assert.IsNotNull(characterGO, "Character GameObject not set!");
        initialCharScale = characterGO.transform.localScale;
        SetupSpawnManager();
    }
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Score>();

        SetupPauseState();

        //set score adding callback
        PlatformCharacterController pcc = characterGO.GetComponent<PlatformCharacterController>();
        pcc.addScore = score.AddScore;
    }

    // Update is called once per frame
    void Update()
    {
        currentUpdate();

        //exit app on back button
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    void GameUpdate()
    {
        //check death
        if (characterGO.transform.position.y < deathLimit)
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
            SetCharacterVisible(true);
            gameOverText.gameObject.SetActive(false);
            currentUpdate = GameUpdate;
            Time.timeScale = 1;
            score.Reset();
        }
    }

    void SetupPauseState()
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
        SetCharacterVisible(false);
    }

    private void SetCharacterVisible(bool visible)
    {
        if (visible)
        {
            characterGO.transform.localScale = initialCharScale;
        }
        else
        {
            characterGO.transform.localScale = Vector3.zero;
        }
    }

    private void SetupSpawnManager()
    {
        SpawnManager sm = GetComponent<SpawnManager>();
        Assert.IsNotNull(sm, "Spawn Manager component not found!");

        sm.SetLowerLimit(deathLimit);
        sm.SetCharacterTransform(characterGO.transform);
        //sot sure if the best way, but looks better than using position of GameController object.
        Transform op = transform.Find("OriginPosition");
        sm.SetOriginPosition(op.position);
    }
}
