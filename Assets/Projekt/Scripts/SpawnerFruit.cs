using UnityEngine;

public class SpawnerFruit : MonoBehaviour
{

    public GameObject fruit;
    float timer = 0f;
    public float spawnInterval = 1f;
    public float maxXOffset = 1f;


    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;
        Debug.Log("Timer");

        if (timer > spawnInterval)
        {
            SpawnTarget();
            timer = 0f;
        }

    }

    void SpawnTarget()
    {
        float xOffset = Random.Range(-maxXOffset, maxXOffset);
        Vector3 spawnOffset = new Vector3(0f, xOffset, 0f);

        Instantiate(fruit, transform.position + spawnOffset, transform.rotation);
    }
}
