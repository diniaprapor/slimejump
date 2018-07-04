﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //public int maxPlatforms = 20;
    public GameObject platformPrefab;
    public int poolSize = 5;
    public float horizontalMin = 6.5f;
    public float horizontalMax = 14f;
    public float verticalMin = -6f;
    public float verticalMax = 6f;

    private Vector2 originPosition;

    private GameObject[] platforms;
    private int currentPlatform = 0;


    // Use this for initialization
    void Start()
    {
        originPosition = transform.position;
        //Initialize the columns collection.
        platforms = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            platforms[i] = (GameObject)Instantiate(platformPrefab, originPosition, Quaternion.identity);
        }

        SpawnAll();
    }

    void SpawnOne()
    {
        Vector2 randomPosition = originPosition + new Vector2(Random.Range(horizontalMin, horizontalMax), Random.Range(verticalMin, verticalMax));
        platforms[currentPlatform].transform.position = randomPosition;
        //also reset platform and its coins

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
        //reset kinematics, rotation and coins
    }
}
