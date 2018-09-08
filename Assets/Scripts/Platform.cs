using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    //Renderer myRenderer;
    PlatformFall pf;
    SpawnCoins sc;

    // Use this for initialization
    void Start () {
        //myRenderer = GetComponent<Renderer>();
        pf = GetComponent<PlatformFall>();
        sc = GetComponent<SpawnCoins>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //reset to initial state
    public void Reset()
    {
        transform.rotation = Quaternion.identity;
        pf.Reset();
        sc.Reset();
    }
    /*
    public bool IsVisible()
    {
        return myRenderer.isVisible;
    }
    */
    void SetSize(int size)
    {
        //1 size unit, ~1/3 original platform size, fits one coin
        //update coin positioning automatically
    }
}
