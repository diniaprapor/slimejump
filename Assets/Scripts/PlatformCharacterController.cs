using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using DragonBones;

public class PlatformCharacterController : MonoBehaviour {
    //public Text debugSpeed;

    public float moveForce = 365f;
    public float initSpeed = 5f;
    public float maxSpeed = 5f;
    public float timeToMaxSpeed = 60; //seconds
    public float jumpForce = 1000f;
    //public float jumpForceWindow = 0.2f; //time from jump start to add jump force
    public float charScale = 1f;
    public AudioClip audioJump;
    public AudioClip audioLand;
    public AudioClip audioCoin;
    public AudioClip audioGem;

    public delegate void AddScoreDelegate(Score.CollectableType ct);
    public AddScoreDelegate addScore;

    private bool grounded = false;
    //private Animator anim;
    private Rigidbody2D rb2d;
    private bool autorun = false;
    private float currentSpeed;
    private UnityEngine.Transform groundCheck;
    private bool facingRight = true;
    private bool jump = false;
    //bool jumpInput = false;
    //private float timeSinceJump = 0.0f;
    private GUIStyle debugUIStyle = new GUIStyle();

    private UnityArmatureComponent animComponent;

    private AudioSource audioSource;

    //called before start
    void Awake()
    {
        InitAnimations();

        rb2d = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        Assert.IsNotNull(groundCheck, "GroundCheck not found!");

        AddAudioSource();
    }

    // Use this for initialization
    private void Start()
    {
        jump = false;
        //jumpInput = false;
        //timeSinceJump = jumpForceWindow;
        autorun = false;
        currentSpeed = initSpeed;

        animComponent.animation.Play("Idle");
    }

	// Update is called once per frame
	void Update () {
        bool prevGrounded = grounded;
         grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if(grounded && !prevGrounded) //detect landing moment
        {
            audioSource.PlayOneShot(audioLand);
        }
        //start running on first platform collision
        //because otherwise character starts moving in the air and nearly misses first platform
        if (grounded) autorun = true;

        //gradually increase speed to some limit
        currentSpeed = Mathf.Lerp(initSpeed, maxSpeed, Mathf.Clamp(Time.timeSinceLevelLoad / timeToMaxSpeed, 0f, 1f));
        //debugSpeed.text = currentSpeed.ToString();
        //debugSpeed.text = rb2d.velocity.x.ToString("0f");

        bool jumpInput = false;
        //jumpInput = false;
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        jumpInput = Input.GetButtonDown("Jump");
        //jumpInput = Input.GetButton("Jump");
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0){
            Touch myTouch = Input.touches[0];
            if (myTouch.phase == TouchPhase.Began)// || myTouch.phase == TouchPhase.Stationary)
                jumpInput = true;
        }
#endif

        //jump start
        if (jumpInput && grounded)
        {
            jump = true;
            audioSource.PlayOneShot(audioJump);
            //timeSinceJump = 0.0f;
        }

        //transform.position = new Vector3(0.0f, 10.5f, 0.0f);
    }

    private void FixedUpdate()
    {
        float h = 0f;
        //h = Input.GetAxis("Horizontal");
        if(autorun && grounded) h = 1.0f; //accelerate only when on ground

        //anim.SetFloat("Speed", Mathf.Abs(h));
        //anim.SetBool("Grounded", grounded);

        if (h * rb2d.velocity.x < currentSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        //kinda rough solution, maybe use drag instead
        if (Mathf.Abs(rb2d.velocity.x) > currentSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * currentSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump)
        //if(timeSinceJump <= jumpForceWindow && jumpInput)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
            //Debug.Log("jump");
        }
        //timeSinceJump += Time.deltaTime;
        UpdateAnimations();
    }

    private void FadeInAnimIfNotActive(string animName, float fadeTime)
    {
        string lastAnim = animComponent.animation.lastAnimationName;
        if (lastAnim != animName)
        {
            animComponent.animation.FadeIn(animName, fadeTime);
        }
    }

    private float HorizontalVelocityAbs()
    {
        //might change to something more complex later, hence separate function
        return Mathf.Abs(rb2d.velocity.x); 
    }

    private void InitAnimations()
    {
        string charName = "Maksim";
        UnityFactory.factory.LoadDragonBonesData(charName + "/" + charName + "_ske");
        UnityFactory.factory.LoadTextureAtlasData(charName + "/" + charName + "_tex");

        animComponent = UnityFactory.factory.BuildArmatureComponent(charName);

        animComponent.transform.localPosition = transform.position;
        animComponent.transform.localScale = transform.localScale * charScale;
        animComponent.transform.parent = transform;
    }

    private void UpdateAnimations()
    {
        //could use some sort of state machine, but for now simple logic will be enough
        //string lastAnim = animComponent.animation.lastAnimationName;
        if (grounded)
        {
            string groundedAnim = HorizontalVelocityAbs() > 0.1f ? "Run" : "Idle";
            FadeInAnimIfNotActive(groundedAnim, 0.2f);
        }
        else // in air
        {
            FadeInAnimIfNotActive("JumpFloat", 0.2f);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //will need another check, if this gets called more than once for same coin
        if (other.gameObject.CompareTag("Coin"))
        {
            //Destroy(gameObject);
            other.gameObject.SetActive(false);
            addScore(Score.CollectableType.Coin);
            audioSource.PlayOneShot(audioCoin);
            //Debug.Log("add coin score");
        }
        else if(other.gameObject.CompareTag("Gem"))
        {
            //Destroy(gameObject);
            other.gameObject.SetActive(false);
            addScore(Score.CollectableType.Gem);
            audioSource.PlayOneShot(audioGem);
            //Debug.Log("add coin score");
        }
    }

    private void AddAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        //audioSource.volume = 0.0f;
    }

    void OnGUI()
    {
        //debug stuff
        if (Debug.isDebugBuild)
        {
            debugUIStyle.fontSize = 30;
            string animName = animComponent.animation.lastAnimationName;
            animName = !string.IsNullOrEmpty(animName) ? animName : "no animation";
            GUI.Label(new Rect(10, 40, 300, 100), "CurrentAnimation: " + animName, debugUIStyle);
            /*
            Animator charAnimator = characterGO.GetComponent<Animator>();
            AnimatorClipInfo[] animatorClipInfo = charAnimator.GetCurrentAnimatorClipInfo(0);
            AnimationClip currentClip = animatorClipInfo.Length > 0 ? animatorClipInfo[0].clip : null;
            if (currentClip != null)
                GUI.Label(new Rect(10, 10, 300, 100), "CurrentAnimation: " + currentClip.name, debugUIStyle);
            */
        }
    }
}
