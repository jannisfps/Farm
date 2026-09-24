using System.Runtime;
using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{   
    public GameObject[] healthHearts; 
    public TMP_Text scoreText;
    public float score = 0;
    [SerializeField] private Sprite loseSprite;

    private bool isHitting;
    private SpriteRenderer sprite;
    private Fruit fruit;
    private List<KeyCode> _pressedThisFrame = new List<KeyCode>();
    
    public int health {get; private set;}

    private GameObject visualHighlight;
    public Material highlightMaterial;
    private HashSet<KeyCode> allKeys = new HashSet<KeyCode>();

    void Awake()
    {   
        sprite = GetComponent<SpriteRenderer>();

        health = GameManager.instance.sv.health;
        GameManager.instance.player = this;

        isHitting = false;
    }

    void Start()
    {
        HighScore.instance.AddNewHighScore("System", 0);
        GainHealth(0); 

        foreach(FruitData data in Spawner2D.instance.upcomingFruits)
        {
            allKeys.Add(data.requiredKey);
        }
    }

    void Update()
    {   
        if (isHitting) return;
        if (GameManager.instance.currentState == GameState.GameOver) return;
        if(GameManager.instance.fruits.Count == 0) return;
        
        if(fruit == null) GetFruit(0);
        if(fruit == null) return;

        /*
        if (Input.GetKeyDown(GameManager.instance.sv.fruitUp) && GameManager.instance.fruits.Count > GameManager.instance.fruits.IndexOf(fruit))
        {   
            int index = GameManager.instance.fruits.IndexOf(fruit);
            GetFruit(index + 1);
        }

        if (Input.GetKeyDown(GameManager.instance.sv.fruitDown) && GameManager.instance.fruits.IndexOf(fruit) != 0)
        {   
            int index = GameManager.instance.fruits.IndexOf(fruit);
            GetFruit(index - 1);
        }

        if(fruit == null) return;
        */

        _pressedThisFrame.Clear();

        foreach(KeyCode key in allKeys)
        {
            if (Input.GetKeyDown(key))
            {
                _pressedThisFrame.Add(key);
            }
        }

        if (_pressedThisFrame.Count == 0) return;
        if (_pressedThisFrame.Count > 1 || _pressedThisFrame[0] != fruit.fruitData.requiredKey) 
        {   
            // Hit Cooldown
            isHitting = true;
            StartCoroutine(Hitting(GameManager.instance.sv.hitCooldown));
            return;
        }
        
          
        if (fruit.isRotten)
        {
            GainHealth(-1);
        }
        else
        {
            HitType hit = GameManager.instance.CalculateHit(fruit);

            if (hit == HitType.OffTiming) return;

            HitFeedback(fruit, hit);
            score = Mathf.Clamp(score + AddScore(fruit, hit), 0, 999999);
            Move(fruit); //if (hit != HitType.Missed) 
            if (hit != HitType.Missed) fruit.PlayCatchSound(); else fruit.PlayMissedSound();
        }

        CollectFruit();

        scoreText.text = "SCORE: " + score;
    }

    public void CollectFruit()
    {   
        if(fruit.fruitData.isChicken) GainHealth(1);
        GameManager.instance.fruits.Remove(fruit);
        Destroy(fruit.gameObject);
    } 

    public void MissFruit()
    {
        if (fruit != null && !fruit.isRotten)
        {
            GainHealth(-1);
        }

        GameManager.instance.fruits.Remove(fruit);
        Destroy(fruit.gameObject);
    }

    public float AddScore(Fruit fruit, HitType type)
    {
        switch (type) 
        {
            case HitType.Missed: return -GameManager.instance.sv.missFruitCost;
            case HitType.Normal: return fruit.fruitData.normalScore;
            case HitType.Okey: return fruit.fruitData.okeyScore;
            case HitType.Perfect: return fruit.fruitData.perfectScore;
        }
        return 0;
    }

    public void GainHealth(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, GameManager.instance.sv.health);

        if (health <= 0)
        {
            GameOver();
        }

        for (int i = 0; i < healthHearts.Length; i++)
        {
            if (healthHearts[i] == null) continue;

            if (i < health)
            {
                healthHearts[i].transform.GetChild(1).gameObject.SetActive(true);
            } 
            else 
            {
                healthHearts[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    private void GetFruit(int index) 
    {
        if (GameManager.instance.fruits == null || 
            GameManager.instance.fruits.Count == index || 
            GameManager.instance.fruits[index] == null) 
        {
            fruit = null;
            return;
        }

        if (visualHighlight != null) Destroy(visualHighlight);

        fruit = GameManager.instance.fruits[index];

        visualHighlight = new GameObject("FruitHighlight");
        
        visualHighlight.transform.position = fruit.transform.position;
        visualHighlight.transform.rotation = fruit.transform.rotation;
        
        visualHighlight.transform.SetParent(fruit.transform);
        visualHighlight.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

        SpriteRenderer sr = visualHighlight.AddComponent<SpriteRenderer>();

        if (fruit.TryGetComponent<SpriteRenderer>(out SpriteRenderer targetSR))
        {
            sr.sprite = targetSR.sprite;
            sr.material = highlightMaterial;
            sr.color = Color.cyan;
            sr.sortingLayerID = targetSR.sortingLayerID;
            sr.sortingOrder = targetSR.sortingOrder - 1;
        }

    }

    private IEnumerator Hitting(float hitCooldown)
    {
        UpdateHitColor();
        yield return new WaitForSeconds(hitCooldown);
        isHitting = false;
        UpdateHitColor();
    }

    private void UpdateHitColor()
    {
        if (isHitting)
            sprite.color = Color.blue;
        else
            sprite.color = Color.white;
    }

    public void GameOver()
    {
        HighScore.instance.AddNewHighScore(GameManager.instance.currentName, (int)score); 
        if (loseSprite != null) sprite.sprite = loseSprite;
        GameManager.instance.TriggerGameOver();
    }

    private void Move(Fruit fruit)
    {   
        if (fruit == null) return;
        transform.position = new Vector3(fruit.transform.position.x, transform.position.y, transform.position.z);
    }

    private void HitFeedback(Fruit fruit, HitType type) 
    {
        if (fruit == null) return;

        GameObject hitFeedbackObj = new GameObject("HitFeedback");
        hitFeedbackObj.transform.position = fruit.transform.position;
        hitFeedbackObj.transform.rotation = fruit.transform.rotation;
        
        SpriteRenderer sr = hitFeedbackObj.AddComponent<SpriteRenderer>();
        
        switch (type) 
        {
            case HitType.Normal: 
                if (GameManager.instance.sv.normalSprite != null) sr.sprite = GameManager.instance.sv.normalSprite;
                break;
            case HitType.Okey:
                if (GameManager.instance.sv.okeySprite != null) sr.sprite = GameManager.instance.sv.okeySprite;
                break;
            case HitType.Perfect:
                if (GameManager.instance.sv.perfectSprite != null) sr.sprite = GameManager.instance.sv.perfectSprite;
                break;
            default:
                if (GameManager.instance.sv.missedSprite != null) sr.sprite = GameManager.instance.sv.missedSprite;
                break;

        }

        if (fruit.TryGetComponent<SpriteRenderer>(out SpriteRenderer targetSR))
        {
            sr.sortingLayerID = targetSR.sortingLayerID;
            sr.sortingOrder = targetSR.sortingOrder - 1;
        }
        
        Destroy(hitFeedbackObj, 2);
    }
}