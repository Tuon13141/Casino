using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject objectPrefab;     
    public Vector3 startPoint;          
    public Vector3 endPoint;         
    public Vector3 between;

    public Transform parent;
    public void SpawnObjects()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("Object Prefab is not assigned!");
            return;
        }

        int columns = Mathf.FloorToInt((endPoint.x - startPoint.x) / between.x) + 1;
        int rows = Mathf.FloorToInt((startPoint.z - endPoint.z) / Mathf.Abs(between.z)) + 1;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                // Tính vị trí từng đối tượng
                Vector3 spawnPosition = new Vector3(
                    startPoint.x + column * between.x,
                    startPoint.y,
                    startPoint.z - row * Mathf.Abs(between.z)
                );

                // Chỉ spawn nếu vị trí không vượt qua endPoint
                if (spawnPosition.x <= endPoint.x && spawnPosition.z >= endPoint.z)
                {
                    GameObject go = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
                    go.transform.parent = parent;
                }
            }
        }

        Debug.Log("Spawned objects from " + startPoint + " to " + endPoint);
    }
}
