using System.Collections;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    [Header("Einstellungen")]
    public GameObject prefabToSpawn;
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

            if (prefabToSpawn != null)
            {
                GameObject spawnedObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
                Destroy(spawnedObject, destroyDelay);
            }
        }
    }

    public Vector2 GetRandomPositionInRadius()
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        return (Vector2)transform.position + randomPoint;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}