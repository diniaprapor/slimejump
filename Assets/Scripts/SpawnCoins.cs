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

        Spawn();
	}
	
    void Spawn()
    {
        for(int i=0; i<coinSpawns.Length; i++)
        {
            bool spawnAnything = Random.Range(0f, 1f) < GameController.collectableSpawnProbability;
            bool spawnGem = Random.Range(0f, 1f) < GameController.gemSpawnProbability; 
            if (spawnAnything)
            {
                GameObject obj = spawnGem ? gemPrefab : coinPrefab;
                coins[i] = (GameObject)Instantiate(obj, coinSpawns[i].transform.position, Quaternion.identity);
                //needed for proper destruction
                coins[i].transform.SetParent(transform, true);
            }
        }
    }

    void ClearCoins()
    {
        foreach (GameObject coin in coins)
        {
            Destroy(coin);
        }
    }

    public void Reset()
    {
        ClearCoins();
        Spawn();
    }
}
