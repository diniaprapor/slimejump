using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour {
    public float fallDelay = 1.0f;

    private Rigidbody2D rb2d;

	// Use this for initialization
	void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            Invoke("Fall", fallDelay);
    }

    void Fall()
    {
        //rb2d.isKinematic = false;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Reset()
    {
        CancelInvoke();
        rb2d.angularVelocity = 0f;
        rb2d.velocity = Vector2.zero;
        //rb2d.isKinematic = true;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
    }
}
