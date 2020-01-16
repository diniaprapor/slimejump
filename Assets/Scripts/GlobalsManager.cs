using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GlobalsManager : MonoBehaviour
{
    // not elegant approach, but not sure how to do it better, and it works
    private static Score _score;
    private static LocalizationManager _loca;
    private static PlayerProgress _playerProgress;

    public static Score GetScore()
    {
        if(_score == null)
        {
            _score = GameObject.FindObjectOfType<Score>();
            Assert.IsNotNull(_score, "Score component not found!");
        }
        return _score;
    }

    public static LocalizationManager GetLocalizationManager()
    {
        if (_loca == null)
        {
            _loca = GameObject.FindObjectOfType<LocalizationManager>();
            Assert.IsNotNull(_loca, "LocalizationManager component not found!");
        }
        return _loca;
    }

    public static PlayerProgress GetPlayerProgress()
    {
        if (_playerProgress == null)
        {
            _playerProgress = GameObject.FindObjectOfType<PlayerProgress>();
            Assert.IsNotNull(_playerProgress, "PlayerProgress component not found!");
        }
        return _playerProgress;
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
