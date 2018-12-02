using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour {
    public Transform[] coinSpawns;
    public GameObject coinPrefab;
    public GameObject gemPrefab;
    private GameObject[] coins;

	// Use this for initialization
	void Start () {
        coins = new GameObject[coinSpawns.Length];
        /*
        for (int i = 0; i < coinSpawns.Length; i++)
        {
            coins[i] = (GameObject)Instantiate(coinPrefab, coinSpawns[i].transform.position, Quaternion.identity);
            coins[i].SetActive(false);
        }
        */
        Spawn();
	}
	
    void Spawn()
    {
        for(int i=0; i<coinSpawns.Length; i++)
        {
            bool spawnAnything = Random.Range(0, 2) > 0;
            bool spawnGem = Random.Range(0, 5) == 0; //20%
            if (spawnAnything)
            {
                GameObject obj = spawnGem ? gemPrefab : coinPrefab;
                coins[i] = (GameObject)Instantiate(obj, coinSpawns[i].transform.position, Quaternion.identity);
                //needed for proper destruction
                coins[i].transform.SetParent(transform, true);
            }
            /*
            coins[i].transform.position = coinSpawns[i].transform.position;
            int coinFlip = Random.Range(0, 2);
            if (coinFlip > 0)
                coins[i].SetActive(true);
                */
            //coins.Add(Instantiate(coinPrefab, coinSpawns[i].position, Quaternion.identity));
        }
    }

    void ClearCoins()
    {
        foreach (GameObject coin in coins)
        {
            //coin.SetActive(false);
            Destroy(coin);
        }
    }

    public void Reset()
    {
        ClearCoins();
        Spawn();
    }
}
