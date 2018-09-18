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
 * make pause at start/restart
 * some sort of main menu
 * make goo character
 * slow fall ability?
 * jump strength?
 * double jump?
 * add some basic music and sound
 * add basic effects on coin collect
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
    public Text debugSpeed;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private int score;
    private bool autorun = false;
    private float currentSpeed;

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
        score = 0;
        SetScoreText();
        autorun = false;
        currentSpeed = initSpeed;
#if UNITY_EDITOR
        InvokeRepeating("PrintVelocityX", 1.0f, 0.1f);
#endif
    }

	// Update is called once per frame
	void Update () {
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

    bool IsMovingUp()
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
