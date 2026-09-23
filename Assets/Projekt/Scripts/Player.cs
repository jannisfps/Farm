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

    private bool isHitting;
    private SpriteRenderer sprite;
    private Fruit fruit;
    private HashSet<KeyCode> _pressedThisFrame = new HashSet<KeyCode>();
    
    public int health {get; private set;}

    private GameObject visualHighlight;
    public Material highlightMaterial;

    void Awake()
    {   
        sprite = GetComponent<SpriteRenderer>();

        health = GameManager.instance.sv.health;
        GameManager.instance.player = this;

        isHitting = false;
    }

    void Start()
    {
       
        GainHealth(0); 
    }

    void Update()
    {   
        if (GameManager.instance.currentState == GameState.GameOver) return;
        if(GameManager.instance.fruits.Count == 0) return;
        
        if(fruit == null) GetFruit(0);
        if(fruit == null) return;

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

        _pressedThisFrame.Clear();
        foreach(KeyCode key in fruit.fruitData.requiredKeys)
        {
            if (Input.GetKeyDown(key))
            {
                _pressedThisFrame.Add(key);
            }
        }

        if (_pressedThisFrame.Count == 0) return;
        
        isHitting = true;
        StartCoroutine(Hitting(GameManager.instance.sv.hitCooldown));
        
        if (_pressedThisFrame.Count == fruit.fruitData.requiredKeys.Count)
        {   
            if (fruit.isRotten)
            {
                GainHealth(-1);
            }
            else
            {
                HitType hit = GameManager.instance.CalculateHit(fruit);
                score = Mathf.Clamp(score + AddScore(fruit, hit), 0, 999999);
            }

            CollectFruit();
        }

        scoreText.text = "SCORE: " + score;
    }

    public void CollectFruit()
    {   
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
        GameManager.instance.TriggerGameOver();
    }
}