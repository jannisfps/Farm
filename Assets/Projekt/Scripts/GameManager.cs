using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum FruitState
{
    Fruit, DecayedFruit
}
public enum HitType
{
    Normal, Okey, Perfect
}


[DefaultExecutionOrder(-99999)]
public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    public string currentName = "Unknown";

    public StartingValues sv;
    public float GameSpeed {get; private set;}
    public Player player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        GameSpeed = sv.startGameSpeed;
    }

    void Update()
    {
        GameSpeed += sv.gameSpeedIncrease *Time.deltaTime;
    }

    public HitType CalculateHit(Transform point, float center, float threshold)
    {   
        float minY = center - threshold;
        float maxY = center + threshold;
        
        float distance = (point.position.y - minY) / (maxY - minY); // always in [0, 1]

        if (distance <= sv.normalThreshold / 100)       return HitType.Normal;   
        if (distance >= 1 - sv.perfectThreshold / 100)  return HitType.Perfect;

        return HitType.Okey;

        //------------------------------------------------------------------------------------
        // So früher der Spieler die Fruit einsammel, nachdem sie im radius ist desto höher der score
        //------------------------------------------------------------------------------------
        
    }
}
