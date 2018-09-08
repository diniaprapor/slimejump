using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //public int maxPlatforms = 20;
    public GameObject platformPrefab;
    public int poolSize = 5;
    public float horizontalMin = 11f;
    public float horizontalMax = 14f;
    public float verticalMin = 1f;
    public float verticalMax = 6f;
    public float horizontalRespawnMult = 2f;
    public float verticalRespawnMult = 3f;

    public GameObject character;

    private Vector2 originPosition;

    private GameObject[] platforms;
    private int currentPlatform = 0;


    // Use this for initialization
    void Start()
    {
        originPosition = transform.position;
        //Initialize the platforms collection.
        platforms = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            platforms[i] = (GameObject)Instantiate(platformPrefab, originPosition, Quaternion.identity);
        }

        SpawnAll();

        //Debug.Log("Platform H " + platformPrefab.transform.localScale.y);
        //Debug.Log("Platform W " + platformPrefab.transform.localScale.x);
    }

    void SpawnOne()
    {
        float above = Random.value > 0.5f ? 1f: -1f;
        Vector2 randomPosition = originPosition + new Vector2(Random.Range(horizontalMin, horizontalMax), Random.Range(verticalMin, verticalMax) * above);
        platforms[currentPlatform].transform.position = randomPosition;
        Debug.Log("platform position: " + platforms[currentPlatform].transform.position);
        originPosition = randomPosition;
        currentPlatform++;
        if (currentPlatform >= poolSize)
            currentPlatform = 0;
    }

    void SpawnAll()
    {
        for (int i = 0; i < poolSize; i++)
        {
            SpawnOne();
        }
    }

    private void Update()
    {
        //check when there is a need for new platform and reuse oldest one
        //int nextPlatform = (currentPlatform + 1) % poolSize;
        float hDist = character.transform.position.x - platforms[currentPlatform].transform.position.x;
        //float vDist = character.transform.position.y - platforms[currentPlatform].transform.position.y;
        bool canReuse = hDist > horizontalMax * horizontalRespawnMult;// || vDist > verticalMax * verticalRespawnMult;
        if (canReuse)
        {
            Debug.Log("reuse platform " + currentPlatform);
            GameObject savedPlatform = platforms[currentPlatform];
            SpawnOne();
            //needs to be after spawn for correct use of position
            savedPlatform.GetComponent<Platform>().Reset();
        }
        //reset kinematics, rotation and coins
    }
}
