using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* todo:
 * add infinite level generation - kind of done
 * add score count and display
 * add mobile controls
 * make pause at start/restart
 * some sort of main menu
 * make character not to be stuck at platform edges
 * make goo character
 * slow fall ability?
 * jump strength?
 * double jump?
*/

public class SimplePlatformController : MonoBehaviour {
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    // Use this for initialization
    private void Start()
    {
        jump = false;
    }

    void Awake () {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        //check death
        if (transform.position.y < -40f)
        {
            SceneManager.LoadScene("Main");
        }

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

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
        h = Input.GetAxis("Horizontal");
        //h = 1.0f;
#elif UNITY_IOS || UNITY_ANDROID
        h = 1.0f;
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
}
