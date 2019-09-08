using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : AState
{
    public GameObject menuUI;
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }
    */
    public override void Enter(AState from)
    {
        menuUI.SetActive(true);
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
    }
}
