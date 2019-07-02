using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlatformCharacterController : MonoBehaviour {
    //public GameController gameController; //kinda breaks incapsulation, but not sure how to handle that other way 
    public Text debugSpeed;

    public float moveForce = 365f;
    public float initSpeed = 5f;
    public float maxSpeed = 5f;
    public float timeToMaxSpeed = 60; //seconds
    public float jumpForce = 1000f;

    public delegate void AddScoreDelegate(Score.CollectableType ct);
    public AddScoreDelegate addScore;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private bool autorun = false;
    private float currentSpeed;
    private Transform groundCheck;
    private bool facingRight = true;
    private bool jump = false;

    //called before start
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        Assert.IsNotNull(groundCheck, "GroundCheck not found!");
    }

    // Use this for initialization
    private void Start()
    {
        jump = false;
        autorun = false;
        currentSpeed = initSpeed;
#if UNITY_EDITOR
        //InvokeRepeating("PrintVelocityX", 1.0f, 0.1f);
#endif
    }

	// Update is called once per frame
	void Update () {
         grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        //start running on first platform collision
        //because otherwise character starts moving in the air and nearly misses first platform
        if (grounded) autorun = true;

        //gradually increase speed to some limit
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

        //transform.position = new Vector3(0.0f, 10.5f, 0.0f);
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

    public bool IsMovingUp()
    {
        return rb2d.velocity.y > 0.01;
    }

    void PrintVelocityX()
    {
        debugSpeed.text = rb2d.velocity.x.ToString("0.0");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //will need another check, if this gets called more than once for same coin
        if (other.gameObject.CompareTag("Coin"))
        {
            //Destroy(gameObject);
            other.gameObject.SetActive(false);
            addScore(Score.CollectableType.Coin);
            //Debug.Log("add coin score");
        }
        else if(other.gameObject.CompareTag("Gem"))
        {
            //Destroy(gameObject);
            other.gameObject.SetActive(false);
            addScore(Score.CollectableType.Gem);
            //Debug.Log("add coin score");
        }
    }
}
