using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
 * destroy not taken coins and gems
 * achievement system
 * slow fall ability?
 * double jump?
 * jump strength = hold duration
 * add some basic music and sound
 * add basic effects on coin collect
 * coin and gem animation (up-down wobble)
 * localize text
 * settings menu
 * make goo character
 * make possible to switch characters
 * limited restored over time lives and watch ad / in-app to skip that
 * jump strength?
 * hi score saving
*/

public class SimplePlatformController : MonoBehaviour {
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public float deathLimit = -40f;
    public float moveForce = 365f;
    public float initSpeed = 5f;
    public float maxSpeed = 5f;
    public float timeToMaxSpeed = 60; //seconds
    public float jumpForce = 1000f;
    public Transform groundCheck;
    public Text scoreText;
    public Text gameOverText;
    public Text debugSpeed;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private bool autorun = false;
    private float currentSpeed;

    private static bool firstRun = true;
    private static int score = 0;

    private delegate void UpdateDelegate();
    private UpdateDelegate currentUpdate;

    //called before start
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    private void Start()
    {
        jump = false;
        SetScoreText();
        autorun = false;
        currentSpeed = initSpeed;
#if UNITY_EDITOR
        InvokeRepeating("PrintVelocityX", 1.0f, 0.1f);
#endif
        SetupPauseState();
    }

	// Update is called once per frame
	void Update () {
        currentUpdate();
    }

    void GameUpdate()
    {
        //check death
        if (transform.position.y < deathLimit)
        {
            SceneManager.LoadScene("Main");
        }

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        //start running on first platform collision
        //because otherwise character starts moving in the air and nearly misses first platform
        if (grounded) autorun = true;

        currentSpeed = Mathf.Lerp(initSpeed, maxSpeed, Mathf.Clamp(Time.timeSinceLevelLoad / timeToMaxSpeed, 0f, 1f));
        //debugSpeed.text = currentSpeed.ToString();
        //debugSpeed.text = rb2d.velocity.x.ToString("0f");

        bool jumpInput = false;
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        jumpInput = Input.GetButtonDown("Jump");
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0){
            Touch myTouch = Input.touches[0];
            if (myTouch.phase == TouchPhase.Began)
                jumpInput = true;
        }
#endif

        if (jumpInput && grounded)
        {
            jump = true;
        }

        //exit app on back button
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //transform.position = new Vector3(0.0f, 10.5f, 0.0f);
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
            gameOverText.gameObject.SetActive(false);
            currentUpdate = GameUpdate;
            Time.timeScale = 1;
            score = 0; //setting score here, so that it can be observed after death
            SetScoreText();
        }
    }

    void SetupPauseState()
    {
        currentUpdate = PauseUpdate;
        Time.timeScale = 0;
        gameOverText.gameObject.SetActive(true);
        Text restartText = gameOverText.transform.Find("TapToRestart").gameObject.GetComponent<Text>();
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
    }

    private void FixedUpdate()
    {
        float h = 0f;
        //h = Input.GetAxis("Horizontal");
        if(autorun && grounded) h = 1.0f; //accelerate only when on ground

        anim.SetFloat("Speed", Mathf.Abs(h));

        if(h * rb2d.velocity.x < currentSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        //kinda rough solution, maybe use drag instead
        if (Mathf.Abs(rb2d.velocity.x) > currentSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * currentSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
            //Debug.Log("jump");
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void SetScoreText()
    {
        scoreText.text = score.ToString();
    }

    public bool IsMovingUp()
    {
        return rb2d.velocity.y > 0.01;
    }

    void PrintVelocityX()
    {
        debugSpeed.text = rb2d.velocity.x.ToString("0.0");
    }

    public void AddCoinScore(int value)
    {
        score += value;
        SetScoreText();
        //Debug.Log("score: " + score);
    }
}
