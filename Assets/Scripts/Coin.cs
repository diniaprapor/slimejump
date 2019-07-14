using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public float wobbleDistance = 0.1f;
    public float wobblePeriod = 0.5f;
    private Vector2 savedPosition;
    private float wobbleTimer;
    // Use this for initialization
	void Start () {
        savedPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        float newY = savedPosition.y + Mathf.Sin(wobbleTimer * Mathf.PI * 2.0f) * wobbleDistance;
        wobbleTimer += Time.deltaTime / wobblePeriod;
        wobbleTimer -= Mathf.Floor(wobbleTimer); //keep <1
        transform.position = new Vector2(savedPosition.x, newY);
    }

}
