using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnManager : MonoBehaviour
{
    public GameObject [] platformPrefabs;
    public int poolSize = 10;
    public float horizontalGapMin = 1f;
    public float horizontalGapMax = 4f;
    public float verticalMin = 1f;
    public float verticalMax = 6f;
    public float horizontalRespawnDist = 28f;

    private float lowerLimit;
    private Transform characterTransform;
    private Vector2 originPosition;

    private GameObject[] platforms;
    private int currentPlatform = 0;
    private float previousPlatformSize;
    private int previousPlatformId;

    // Use this for initialization
    void Start()
    {
        //SetLowerLimit();
        previousPlatformSize = -1.0f;
        previousPlatformId = 0;

        //originPosition = transform.position;
        //Initialize the platforms collection.
        platforms = new GameObject[poolSize];

        SpawnAll();
    }

    private bool FirstPlatformSpawn()
    {
        return previousPlatformSize <= 0;
    }

    void SpawnOne()
    {
        //not most efficient way, but i'll switch to pools if it starts lagging at this point
        if (platforms[currentPlatform] != null)
        {
            //reset not strictly necessary since we are not using pool
            //but it will be needed if switched to pool and also does platform children cleanup
            //platforms[currentPlatform].GetComponent<Platform>().Reset(); 
            Destroy(platforms[currentPlatform]);
        }
        int prefabStartIndex = previousPlatformId == 0 ? 1 : 0; //should prevent from shortest platform appearing twice in a row
        int prefabId = FirstPlatformSpawn() ? (platformPrefabs.Length - 1) : Random.Range(prefabStartIndex, platformPrefabs.Length); //always make first platform longest
        previousPlatformId = prefabId;
        platforms[currentPlatform] = (GameObject)Instantiate(platformPrefabs[prefabId], Vector2.zero, Quaternion.identity);

        float currentPlatformSize = PlatformSize(platforms[currentPlatform].transform);
        //update origin position
        if (!FirstPlatformSpawn()) //dont update origin position for first platform
        {
            bool closeToBottom = originPosition.y - verticalMax - 1f < lowerLimit; //added 1 just to be safe.
            float above = Random.value > 0.5f || closeToBottom ? 1f : -1f; //go only up if went too low
            float horizontalMin = previousPlatformSize * 0.5f + currentPlatformSize * 0.5f + horizontalGapMin;
            float horizontalMax = previousPlatformSize * 0.5f + currentPlatformSize * 0.5f + horizontalGapMax;

            originPosition = originPosition + new Vector2(Random.Range(horizontalMin, horizontalMax), Random.Range(verticalMin, verticalMax) * above);

            //Debug.Log("Origin position: " + originPosition.ToString());
        }
        previousPlatformSize = currentPlatformSize;

        platforms[currentPlatform].transform.position = originPosition;

        currentPlatform = (currentPlatform + 1) % poolSize;
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
        float hDist = characterTransform.position.x - platforms[currentPlatform].transform.position.x;
        //float vDist = character.transform.position.y - platforms[currentPlatform].transform.position.y;
        bool canReuse = hDist > horizontalRespawnDist;// || vDist > verticalMax * verticalRespawnMult;
        if (canReuse)
        {
            //Debug.Log("reuse platform " + currentPlatform);
            SpawnOne();
        }
    }

    private float PlatformSize(Transform platform)
    {
        Platform p = platform.GetComponent<Platform>();
        return p.SizeX();
    }

    public void SetLowerLimit(float ll)
    {
        lowerLimit = ll;
        //Debug.Log("Lower limit set: " + lowerLimit.ToString());
    }

    public void SetCharacterTransform(Transform ct)
    {
        characterTransform = ct;
        //Debug.Log("Character transform set");
    }

    public void SetOriginPosition(Vector2 op)
    {
        originPosition = op;
        //Debug.Log("Origin position set: " + originPosition.ToString());
    }
}
