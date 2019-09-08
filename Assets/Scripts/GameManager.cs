using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    public AState[] states;

    protected AState activeState;
    protected Dictionary<string, AState> stateDict = new Dictionary<string, AState>();
    protected List<AState>stateStack = new List<AState>();

    private GUIStyle debugUIStyle = new GUIStyle();

    // Start is called before the first frame update
    protected void OnEnable()
    {
        // We build a dictionnary from state for easy switching using their name.
        stateDict.Clear();

        if (states.Length == 0)
            return;

        for (int i = 0; i < states.Length; ++i)
        {
            states[i].manager = this;
            stateDict.Add(states[i].GetName(), states[i]);
        }

        stateStack.Clear();
        PushState(states[0].GetName());
    }

    // Update is called once per frame
    void Update()
    {
        if (stateStack.Count > 0)
        {
            stateStack[stateStack.Count - 1].Tick();
        }
    }

    // State management
    public void SwitchState(string newState)
    {
        AState state = FindState(newState);
        if (state == null)
        {
            Debug.LogError("Can't find the state named " + newState);
            return;
        }

        stateStack[stateStack.Count - 1].Exit(state);
        state.Enter(stateStack[stateStack.Count - 1]);
        stateStack.RemoveAt(stateStack.Count - 1);
        stateStack.Add(state);
    }

    public AState FindState(string stateName)
    {
        AState state;
        if (!stateDict.TryGetValue(stateName, out state))
        {
            return null;
        }

        return state;
    }

    public void PopState()
    {
        if (stateStack.Count < 2)
        {
            Debug.LogError("Can't pop states, only one in stack.");
            return;
        }

        stateStack[stateStack.Count - 1].Exit(stateStack[stateStack.Count - 2]);
        stateStack[stateStack.Count - 2].Enter(stateStack[stateStack.Count - 2]);
        stateStack.RemoveAt(stateStack.Count - 1);
    }

    public void PushState(string name)
    {
        AState state;
        if (!stateDict.TryGetValue(name, out state))
        {
            Debug.LogError("Can't find the state named " + name);
            return;
        }

        if (stateStack.Count > 0)
        {
            stateStack[stateStack.Count - 1].Exit(state);
            state.Enter(stateStack[stateStack.Count - 1]);
        }
        else
        {
            state.Enter(null);
        }
        stateStack.Add(state);
    }

    void OnGUI()
    {
        //debug stuff
        if (Debug.isDebugBuild)
        {
            debugUIStyle.fontSize = 30;
            string currentState = "undefined";
            if (stateStack.Count > 0)
            {
                currentState = stateStack[stateStack.Count - 1].GetName();
            }
            GUI.Label(new Rect(10, 10, 300, 100), "Current State: " + currentState, debugUIStyle);
        }
    }
}

public abstract class AState : MonoBehaviour
{
    [HideInInspector]
    public GameManager manager;

    public abstract void Enter(AState from);
    public abstract void Exit(AState to);
    public abstract void Tick();

    public abstract string GetName();
}