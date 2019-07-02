using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple camera following object
public class FollowCamera : MonoBehaviour {
    public Transform objectToFollow;
    public float smoothTime = 0.2f;
    public Vector2 offset;// = new Vector2(0f, 0f);
    //for zoom in/out
    public float minVerticalSize = 7;
    public float maxVerticalSize = 17;
    public float sizeAdjustTime = 1f;

    private float followY;
    //private Vector3 followPosition;
    private float followVerticalSize;
    private Vector3 prevPos;
    //private SimplePlatformController spc; //to get hero movement. Could calculate it locally from transform, but oh well. 
    private Camera cam; //camera which has script attached
    
    // Use this for initialization
    void Start () {
        followY = 0f;// objectToFollow.position.y;
        //spc = objectToFollow.GetComponent<SimplePlatformController>();
        cam = GetComponent<Camera>();
        followVerticalSize = 0f;
        prevPos = objectToFollow.position;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x = objectToFollow.position.x + offset.x;
        pos.y = Mathf.SmoothDamp(pos.y, objectToFollow.position.y + offset.y, ref followY, 0.2f);
        transform.position = pos;

        //zoom out on jump
        float camSizeTarget = IsTargetMovingUp() ? maxVerticalSize : minVerticalSize;
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, camSizeTarget, ref followVerticalSize, sizeAdjustTime);

        prevPos = objectToFollow.position;
    }

    private bool IsTargetMovingUp()
    {
        return prevPos.y < objectToFollow.position.y;
    }
}
