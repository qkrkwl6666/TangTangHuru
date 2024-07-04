using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject SpawnPrefab;

    public void SpawnEnemy()
    {
        for (int i = 0; i < 10; i++)
        {
            var rndX = Random.Range(-30, 30);
            var rndY = Random.Range(-30, 30);
            Instantiate(SpawnPrefab, new Vector2(rndX, rndY), Quaternion.identity);
        }
    }


}
