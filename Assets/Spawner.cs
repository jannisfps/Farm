using System.Collections;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    [Header("Einstellungen")]
    public GameObject prefabToSpawn;
    public int countPerSpawn = 1;
    public float lineWidth = 10f;
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
            Vector2 randomPosition = GetRandomPositionOnLine();

            if (prefabToSpawn != null)
            {
                GameObject spawnedObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
                Destroy(spawnedObject, destroyDelay);
            }
        }
    }

    public Vector2 GetRandomPositionOnLine()
    {
        float randomX = Random.Range(-lineWidth / 2f, lineWidth / 2f);
        return new Vector2(transform.position.x + randomX, transform.position.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 left = transform.position + Vector3.left * (lineWidth / 2f);
        Vector3 right = transform.position + Vector3.right * (lineWidth / 2f);
        Gizmos.DrawLine(left, right);
    }
}