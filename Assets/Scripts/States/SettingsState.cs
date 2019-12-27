using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SettingsState : AState
{
    public GameObject uiObj;
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }
    */
    public override void Enter(AState from)
    {
        Assert.IsNotNull(uiObj, "uiObj not found!");
        uiObj.SetActive(true);
    }

    public override void Exit(AState to)
    {
        uiObj.SetActive(false);
    }

    // Update is called once per frame
    public override void Tick()
    {
        
    }

    public override string GetName()
    {
        return "Settings";
    }
}
