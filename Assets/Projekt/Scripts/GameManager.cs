using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Diagnostics;
using System.Collections;

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
    public List<Fruit> fruits = new List<Fruit>();


    public StartingValues sv;
    public float GameSpeed {get; private set;}
    public Player player;

    [SerializeField] Transform normalField;
    [SerializeField] Transform goodField;
    [SerializeField] Transform perfectField;
    [SerializeField] Transform missedField;

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
        GameSpeed += sv.gameSpeedIncrease * Time.deltaTime;
    }

    IEnumerator StartRound()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    }

    public HitType CalculateHit(Fruit fruit)
    {   
        // SCORE CALCULATION
        return HitType.Okey;

        //------------------------------------------------------------------------------------
        // So früher der Spieler die Fruit einsammel, nachdem sie im radius ist desto höher der score
        //------------------------------------------------------------------------------------
        
    }
}
