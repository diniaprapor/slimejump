using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    private SimplePlatformController spc; 
	// Use this for initialization
	void Start () {
        GameObject player = GameObject.Find("hero");
        spc = player.GetComponent<SimplePlatformController>();
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
            if(spc)
                spc.AddCoinScore();
        }
    }
}
