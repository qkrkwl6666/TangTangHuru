using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileObjectPositioner : MonoBehaviour
{
    public List<Tilemap> decoObjects = new();
    private int decoCount = 35;

    private int mapSize = 250;
    private int gridSize = 30;
    private Vector2 initialPosition = new Vector2(15, 15);
    private List<Vector2> positionList = new List<Vector2>();

    void Start()
    {
        var objs = GetComponentsInChildren<Tilemap>();
        int maxRange = (mapSize / gridSize) + 1;

        for (int i = 0; i < decoCount; i++)
        {
            var rndX = Random.Range(-maxRange, maxRange);
            var rndY = Random.Range(-maxRange, maxRange);
            Vector2 newPosition = new Vector2(gridSize * rndX, gridSize * rndY);
            newPosition += initialPosition;
            positionList.Add(newPosition);

            var stageObj = Instantiate(objs[Random.Range(0, objs.Length)], transform);
            decoObjects.Add(stageObj);
        }

        for (int i = 0; i < decoObjects.Count; i++)
        {
            decoObjects[i].transform.position = positionList[i];
        }
    }
}
