using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public int value = 1;

    private SimplePlatformController spc;
    private bool valueAdded;
	
    // Use this for initialization
	void Start () {
        //might be expensive operation, since coins are spawning all time
        //consider setting it externally in some way
        GameObject player = GameObject.Find("hero");
        spc = player.GetComponent<SimplePlatformController>();
        valueAdded = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            if (spc && !valueAdded)
            {
                spc.AddCoinScore(value);
                valueAdded = true;
            }
            //Debug.Log("add coin score");
        }
    }
}
