using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    //Renderer myRenderer;
    private PlatformFall pf;
    private SpawnCoins sc;

    float sizeX;

    //called before start
    void Awake()
    {
        pf = GetComponent<PlatformFall>();
        sc = GetComponent<SpawnCoins>();

        //find length of longest collider
        sizeX = -1f;
        List<BoxCollider2D> colliders2d = new List<BoxCollider2D>();
        GetComponents(colliders2d);
        foreach (BoxCollider2D c2d in colliders2d)
            if (c2d.size.x > sizeX)
                sizeX = c2d.size.x;
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

    public float SizeX()
    {
        return sizeX;
    }
    /*
    public bool IsVisible()
    {
        return myRenderer.isVisible;
    }
    */

}
