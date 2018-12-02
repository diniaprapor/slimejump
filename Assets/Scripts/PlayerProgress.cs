using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour {
    int highestScore = 0;
    void Awake()
    {
        //needs to load before Start()
        LoadData();
        Debug.Log("player progress start");
    }

    // Use this for initialization
    void Start () {
    }
	/*
	// Update is called once per frame
	void Update () {
		
	}
    */
    void LoadData()
    {
        highestScore = PlayerPrefs.GetInt("highestScore", 0);
        Debug.Log("Player progress load data: " + highestScore);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("highestScore", highestScore);
        Debug.Log("Player progress save data: " + highestScore);
    }

    public int GetHiScore()
    {
        Debug.Log("Player progress get hi score: " + highestScore);
        return highestScore;
    }

    public void SetHiScore(int newScore)
    {
        //Debug.Log("set hi score: " + newScore);
        //Debug.Log("current score: " + highestScore);
        if (newScore > highestScore)
        {
            highestScore = newScore;
            SaveData();
        }
    }
}
