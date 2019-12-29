using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class CountdownState : AState
{
    public GameObject uiObj;
    public int steps = 3;
    public float duration = 2;

    private float counter;
    private Text counterText;
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
        counter = duration;
        counterText = uiObj.transform.Find("Countdown").gameObject.GetComponent<Text>();
        Assert.IsNotNull(counterText, "counterText not found!");
    }

    public override void Exit(AState to)
    {
        uiObj.SetActive(false);
    }

    // Update is called once per frame
    public override void Tick()
    {
        counter -= Time.deltaTime;
        //setup count display
        float stepFloat = (counter / duration) * steps;
        int step = Mathf.FloorToInt(stepFloat) + 1;
        float stepProgress = stepFloat - (step - 1);

        counterText.text = step.ToString();
        float textScale = 0.5f + stepProgress * 0.5f;
        uiObj.transform.localScale = new Vector3(textScale, textScale);

        if (counter <= 0.0)
        {
            manager.PopState();
        }
    }

    public override string GetName()
    {
        return "Countdown";
    }
}
