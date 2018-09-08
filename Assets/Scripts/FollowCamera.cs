using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple camera following object
public class FollowCamera : MonoBehaviour {
    public Transform objectToFollow;
    public float smoothTime = 0.2f;
    public Vector2 offset;// = new Vector2(0f, 0f);

    private float followY;
    private Vector3 followPosition;
    // Use this for initialization
    void Start () {
        followY = objectToFollow.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x = objectToFollow.position.x + offset.x;
        pos.y = Mathf.SmoothDamp(pos.y, objectToFollow.position.y, ref followY, 0.2f) + offset.y;
        transform.position = pos;
    }
}
