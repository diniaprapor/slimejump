using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour {
    public Transform[] coinSpawns;
    public GameObject coinPrefab;
    public GameObject gemPrefab;
    private GameObject[] coins;

	void Awake () {
        coins = new GameObject[coinSpawns.Length];

        //Spawn();
	}
	
    public void SpawnCollectables(float collectableSpawnProbability, float gemSpawnProbability)
    {
        for(int i=0; i<coinSpawns.Length; i++)
        {
            bool spawnAnything = Random.Range(0f, 1f) < collectableSpawnProbability;
            if (spawnAnything)
            {
                bool spawnGem = Random.Range(0f, 1f) < gemSpawnProbability;
                GameObject obj = spawnGem ? gemPrefab : coinPrefab;
                coins[i] = (GameObject)Instantiate(obj, coinSpawns[i].transform.position, Quaternion.identity);
                //needed for proper destruction
                coins[i].transform.SetParent(transform, true);
            }
        }
    }

    public void ClearCollectables()
    {
        foreach (GameObject coin in coins)
        {
            Destroy(coin);
        }
    }
    /*
    public void Reset()
    {
        ClearCoins();
        Spawn();
    }
    */
}
