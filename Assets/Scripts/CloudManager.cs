using UnityEngine;
using System.Collections;

public class CloudManager : MonoBehaviour
{
    //Set this variable to your Cloud Prefab through the Inspector
    public GameObject [] cloudPrefabs;

    //Set this variable to how often you want the Cloud Manager to make clouds in seconds.
    //For Example, I have this set to 2
    public float delay;

    //Set these variables to the lowest and highest y values you want clouds to spawn at.
    public float minY;
    public float maxY;

    //If you ever need the clouds to stop spawning, set this variable to false, by doing: CloudManagerScript.spawnClouds = false;
    public static bool spawnClouds = true;

    // Use this for initialization
    void Start()
    {
        //mainCam = GameObject.FindWithTag("MainCamera");

        //override cloud spawn range
        foreach(GameObject cloudGO in cloudPrefabs)
        {
            CloudScript cs = cloudGO.GetComponent<CloudScript>();
            cs.minY = minY;
            cs.maxY = maxY;
        }
        //Begin SpawnClouds Coroutine
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        //This will always run
        while (true)
        {
            //Only spawn clouds if the boolean spawnClouds is true
            //while (spawnClouds)
            if(spawnClouds)
            {
                //Instantiate Cloud Prefab and then wait for specified delay, and then repeat
                GameObject prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
                //GameObject cloud = 
                Instantiate(prefab);
                //cloud.transform.parent = mainCam.transform;
            }
            yield return new WaitForSeconds(delay);
        }
    }
}