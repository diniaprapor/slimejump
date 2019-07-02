using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class Score : MonoBehaviour
{
    public enum CollectableType
    {
        Coin,
        Gem,
        None
    }

    public int[] collectableValues = new int[] { 1, 5 };

    public Text scoreText;
    private PlayerProgress playerProgress;
    private static int score = 0;

    private void SetScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void AddScore(CollectableType ct)
    {
        int intCt = (int)ct;
        Assert.IsTrue(intCt < collectableValues.Length);
        score += collectableValues[intCt];
        SetScoreText();
        playerProgress.SetHiScore(score);
        //Debug.Log("score: " + score);
    }

    public int GetHiScore()
    {
        return playerProgress.GetHiScore();
    }

    public int GetScore()
    {
        return score;
    }

    public void Reset()
    {
        score = 0;
        SetScoreText();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerProgress = GetComponent<PlayerProgress>();
        SetScoreText();
    }


    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
