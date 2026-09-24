using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{   
    public static Spawner2D instance;

    [Header("--- 1. BASIS EINSTELLUNGEN ---")]
    [Tooltip("Das Haupt-Prefab für alle Früchte.")]
    public GameObject fruitPrefab;

    [Tooltip("Start-Pause zwischen zwei Spawns (in Sekunden).")]
    public float spawnInterval = 3f;

    [Tooltip("Breite des Spawn-Bereichs nach links und rechts (X-Achse).")]
    public float radius = 2f;

    [Tooltip("Wie viele Früchte gleichzeitig pro Spawn erzeugt werden.")]
    public int countPerSpawn = 1;


    [Header("--- 2. SPAWN-BESCHLEUNIGUNG ---")]
    [Tooltip("Alle wie vielen Sekunden wird das Spawnen schneller?")]
    public float speedIncreaseInterval = 10f;

    [Tooltip("Um wie viele Sekunden wird die Spawn-Pause jeweils verkürzt?")]
    public float spawnIntervalDecrease = 0.1f;

    [Tooltip("Sicherheitsgrenze: Schneller als diesen Wert (in Sek.) wird nicht gespawnt.")]
    public float minSpawnInterval = 0.5f;

    [Tooltip("Liste der nächsten Früchte. Werden nacheinander freigeschaltet.")]
    public List<FruitData> upcomingFruits = new List<FruitData>();
    
    [Tooltip("Liste der Spezial-Früchte.")]
    public List<FruitData> specialFruits = new List<FruitData>();

    [Tooltip("Alle wie vielen Sekunden wird die nächste Frucht aus der Liste freigeschaltet?")]
    public float unlockInterval = 15f;


    [Header("--- 4. OPTIONEN ---")]
    [Tooltip("Soll das Spawnen automatisch direkt bei Spielstart beginnen?")]
    public bool spawnOnStart = true;


    private List<FruitData> activeFruits = new List<FruitData>();
    private int nextFruitIndex = 0;

    private void Awake()
    {   
        instance = this;
        if (upcomingFruits.Count != 0)
        {
            activeFruits.Add(upcomingFruits[0]);
        }

        if (spawnOnStart)
        {
            StartCoroutine(SpawnRoutine());
            StartCoroutine(UnlockRoutine());
            StartCoroutine(SpeedUpRoutine());
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

    private IEnumerator SpeedUpRoutine()
    {
        while (spawnInterval > minSpawnInterval)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);
            spawnInterval = Mathf.Max(spawnInterval - spawnIntervalDecrease, minSpawnInterval);
        }
    }

    public void SpawnAll()
    {
        if (activeFruits.Count == 0 || fruitPrefab == null) return;

        for (int i = 0; i < countPerSpawn; i++)
        {   
            FruitData randomData = activeFruits[Random.Range(0, activeFruits.Count)];

            if (specialFruits.Count != null) {
                foreach (FruitData data in specialFruits) 
                {

                    float rdm = Random.value;
                    if (rdm <= data.SpawnChance) {
                        randomData = data;
                    }
                }
            }

            Vector2 randomPosition = GetRandomPositionInRadius();

            GameObject spawnedObject = Instantiate(fruitPrefab, randomPosition, Quaternion.identity);

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