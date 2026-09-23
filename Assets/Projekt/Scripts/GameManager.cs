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
    OffTiming, Missed, Normal, Okey, Perfect
}
public enum GameState
{
    Playing, GameOver
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

    [Header("UI & GameState")]
    public GameObject gameOverCanvas;
    public GameState currentState = GameState.Playing;

    [SerializeField] Transform normalField;
    [SerializeField] Transform okeyField;
    [SerializeField] Transform perfectField;
    [SerializeField] public Transform missedField;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        GameSpeed = sv.startGameSpeed;
        Time.timeScale = 1f; // Sicherstellen, dass das Spiel bei Start nicht pausiert ist
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            GameSpeed += sv.gameSpeedIncrease * Time.deltaTime;
        }
    }

    public void TriggerGameOver()
    {
        currentState = GameState.GameOver;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Time.timeScale = 0f; // Stoppt Zeit und Physik komplett
    }

    IEnumerator StartRound()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    }

    public HitType CalculateHit(Fruit fruit)
    {   
        if (
            fruit.transform.position.y < normalField.transform.position.y + (normalField.transform.localScale.y / 2) &&
            fruit.transform.position.y > normalField.transform.position.y - (normalField.transform.localScale.y / 2))
            return HitType.Normal;
        if (
            fruit.transform.position.y < okeyField.transform.position.y + (okeyField.transform.localScale.y / 2) &&
            fruit.transform.position.y > okeyField.transform.position.y - (okeyField.transform.localScale.y / 2))
            return HitType.Okey;
        if (
            fruit.transform.position.y < perfectField.transform.position.y + (perfectField.transform.localScale.y / 2) &&
            fruit.transform.position.y > perfectField.transform.position.y - (perfectField.transform.localScale.y / 2))
            return HitType.Perfect;
        
        return HitType.Missed;
    }
}