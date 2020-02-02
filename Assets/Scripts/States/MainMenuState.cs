using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MainMenuState : AState
{
    public GameObject menuUI;
    public string menuMusic = "Dungeon Theme";
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }
    */
    private void InitBgMusic()
    {
        GameObject cam = GameObject.Find("Main Camera");
        Assert.IsNotNull(cam, "Main Camera not found!");
        BgMusic bgm = cam.GetComponent<BgMusic>();
        Assert.IsNotNull(bgm, "Bg Music component not found!");
        bgm.PlayAudio(menuMusic);
    }

    public override void Enter(AState from)
    {
        menuUI.SetActive(true);
        CloudManager.spawnClouds = false;
        InitBgMusic();
        //Debug.Log("init bg music");
    }

    public override void Exit(AState to)
    {
        menuUI.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    public override void Tick()
    {
        //throw new System.NotImplementedException();
    }

    public override string GetName()
    {
        return "Menu";
    }

    public void PlayClick()
    {
        manager.SwitchState("Game");
        manager.PushState("Countdown");
    }

    public void SettingsClick()
    {
        manager.PushState("Settings");
    }
}
