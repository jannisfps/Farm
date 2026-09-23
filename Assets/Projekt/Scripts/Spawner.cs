using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    [Header("Einstellungen")]
    public List<GameObject> prefabsToSpawn = new List<GameObject>();
    public int countPerSpawn = 1;
    public float radius = 2f;
    public float spawnInterval = 3f;
    public float destroyDelay = 10f;

    [Header("Optionen")]
    public bool spawnOnStart = true;

    private void Start()
    {
        if (spawnOnStart)
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnAll();
        }
    }

    public void SpawnAll()
    {
        for (int i = 0; i < countPerSpawn; i++)
        {
            Vector2 randomPosition = GetRandomPositionInRadius();

            if (prefabsToSpawn.Count != 0)
            {   
                int rdm = Random.Range(0, prefabsToSpawn.Count);
                GameObject spawnedObject = Instantiate(prefabsToSpawn[rdm], randomPosition, Quaternion.identity);
                Destroy(spawnedObject, destroyDelay);
            }
        }
    }

    public Vector2 GetRandomPositionInRadius()
    {
        float randomPoint = Random.Range(-radius, radius);
        return new Vector2(randomPoint, transform.position.y);
    }
}