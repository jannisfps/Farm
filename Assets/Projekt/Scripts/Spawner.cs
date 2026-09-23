using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    [Header("Einstellungen")]
    public GameObject fruitPrefab;
    public int countPerSpawn = 1;
    public float radius = 2f;
    public float spawnInterval = 3f;

    [Header("Früchte-Freischaltung")]
    public FruitData startingFruit;
    public List<FruitData> upcomingFruits = new List<FruitData>();
    public float unlockInterval = 15f;

    [Header("Optionen")]
    public bool spawnOnStart = true;

    private List<FruitData> activeFruits = new List<FruitData>();
    private int nextFruitIndex = 0;

    private void Start()
    {
        if (startingFruit != null)
        {
            activeFruits.Add(startingFruit);
        }

        if (spawnOnStart)
        {
            StartCoroutine(SpawnRoutine());
            StartCoroutine(UnlockRoutine());
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

    private IEnumerator UnlockRoutine()
    {
        while (nextFruitIndex < upcomingFruits.Count)
        {
            yield return new WaitForSeconds(unlockInterval);

            FruitData nextFruit = upcomingFruits[nextFruitIndex];
            activeFruits.Add(nextFruit);

            nextFruitIndex++;
        }
    }

    public void SpawnAll()
    {
        if (activeFruits.Count == 0 || fruitPrefab == null) return;

        for (int i = 0; i < countPerSpawn; i++)
        {
            Vector2 randomPosition = GetRandomPositionInRadius();

            GameObject spawnedObject = Instantiate(fruitPrefab, randomPosition, Quaternion.identity);

            FruitData randomData = activeFruits[Random.Range(0, activeFruits.Count)];

            Fruit fruitComponent = spawnedObject.GetComponent<Fruit>();
            if (fruitComponent != null)
            {
                fruitComponent.SetFruitData(randomData);
            }
        }
    }

    public Vector2 GetRandomPositionInRadius()
    {
        float randomPoint = Random.Range(-radius, radius);
        return new Vector2(randomPoint, transform.position.y);
    }
}