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
 * add 2 types of collectibles
 * make pause at start/restart
 * remove initial platform prefab, make everything generated
 * make variable size platforms
 * some sort of main menu
 * make character not to be stuck at platform edges
 * make goo character
 * slow fall ability?
 * jump strength?
 * double jump?
 * add some basic music and sound
 * + start from higher position
 * add basic effects on coin collect
 * close app on back button
 * autorotate view
*/

public class SimplePlatformController : MonoBehaviour {
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;
    public Text scoreText;
    public int scoreIncrement = 1;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private int score;
    private bool autorun = false;

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
    }

	// Update is called once per frame
	void Update () {
        //check death
        if (transform.position.y < -40f)
        {
            SceneManager.LoadScene("Main");
        }

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        //start running on first platform collision
        //because otherwise character starts moving in the air and nearly misses first platform
        if (grounded) autorun = true;

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
    }

    private void FixedUpdate()
    {
        float h = 0f;
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        //h = Input.GetAxis("Horizontal");
        if(autorun) h = 1.0f;
#elif UNITY_IOS || UNITY_ANDROID
        if(autorun) h = 1.0f;
#endif
        anim.SetFloat("Speed", Mathf.Abs(h));

        if(h * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

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

    public void AddCoinScore()
    {
        score += scoreIncrement;
        SetScoreText();
        //Debug.Log("score: " + score);
    }
}
