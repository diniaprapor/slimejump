using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : AState
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    public override void Tick()
    {
        throw new System.NotImplementedException();
    }

    public override void Enter(AState from)
    {
        throw new System.NotImplementedException();
    }

    public override void Exit(AState from)
    {
        throw new System.NotImplementedException();
    }

    public override string GetName()
    {
        return "Menu";
    }
}
