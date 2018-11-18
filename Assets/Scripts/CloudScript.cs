using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour
{
    //Set these variables to whatever you want the slowest and fastest speed for the clouds to be, through the inspector.
    //If you don't want clouds to have randomized speed, just set both of these to the same number.
    //For Example, I have these set to 2 and 5
    public float minSpeed;
    public float maxSpeed;

    //Set these variables to the lowest and highest y values you want clouds to spawn at.
    //For Example, I have these set to 1 and 4
    public float minY;
    public float maxY;

    //Set this variable to how far off screen you want the cloud to spawn, and how far off the screen you want the cloud to be for it to despawn. You probably want this value to be greater than or equal to half the width of your cloud.
    //For Example, I have this set to 4, which should be more than enough for any cloud.
    public float buffer;

    public float minScale;
    public float maxScale;

    float speed;
    float camWidth;
    GameObject mainCam;
    SpriteRenderer sr;

    void Start()
    {
        //Set camWidth. Will be used later to check whether or not cloud is off screen.
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        mainCam = GameObject.FindWithTag("MainCamera");
        Vector3 camPos = mainCam.transform.position;
        //Set Cloud Movement Speed, and Position to random values within range defined above
        speed = Random.Range(minSpeed, maxSpeed);
        transform.position = new Vector3(camPos.x + camWidth + buffer, camPos.y + Random.Range(minY, maxY), transform.position.z);
        float randomScale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);

        sr = GetComponent<SpriteRenderer>();
        sr.color = GetPastelShade();
        Debug.Log("Cloud start " + transform.position.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = mainCam.transform.position;
        //Translates the cloud to the right at the speed that is selected
        transform.Translate(speed * Time.deltaTime, 0, 0);
        //If cloud is off Screen, Destroy it.
        if (transform.position.x + buffer < camPos.x - camWidth)
        {
            Destroy(gameObject);
        }
    }
    /*
    var cssHSL = "hsl(" + 360 * Math.random() + ',' +
                 (25 + 70 * Math.random()) + '%,' +
                 (85 + 10 * Math.random()) + '%)';
     */
    Color GetPastelShade()
    {
        float hue = Random.Range(0.55f, 0.67f);
        float sat = 0.25f + Random.Range(0f, 0.3f);
        float light = 0.85f + Random.Range(0f, 0.1f);
        return Color.HSVToRGB(hue, sat, light);
    }
}