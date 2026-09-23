using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

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
    public float GameSpeed { get; private set; }
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
        else 
        {
            Destroy(gameObject);
            return;
        }

        ResetGameValues();
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

        Time.timeScale = 0f; 

        
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
       
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        
        ResetGameValues();

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetGameValues()
    {
        GameSpeed = sv.startGameSpeed;
        currentState = GameState.Playing;
        fruits.Clear();
        Time.timeScale = 1f;
    }

    public HitType CalculateHit(Fruit fruit)
    {   
        if (fruit.transform.position.y < normalField.transform.position.y + (normalField.transform.localScale.y / 2) &&
            fruit.transform.position.y > normalField.transform.position.y - (normalField.transform.localScale.y / 2))
            return HitType.Normal;
        if (fruit.transform.position.y < okeyField.transform.position.y + (okeyField.transform.localScale.y / 2) &&
            fruit.transform.position.y > okeyField.transform.position.y - (okeyField.transform.localScale.y / 2))
            return HitType.Okey;
        if (fruit.transform.position.y < perfectField.transform.position.y + (perfectField.transform.localScale.y / 2) &&
            fruit.transform.position.y > perfectField.transform.position.y - (perfectField.transform.localScale.y / 2))
            return HitType.Perfect;
        
        return HitType.Missed;
    }
}