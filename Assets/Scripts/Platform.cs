using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    //Renderer myRenderer;
    PlatformFall pf;
    SpawnCoins sc;

    //called before start
    void Awake()
    {
        pf = GetComponent<PlatformFall>();
        sc = GetComponent<SpawnCoins>();
    }
    // Use this for initialization
    void Start () {
        //myRenderer = GetComponent<Renderer>();
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

}
