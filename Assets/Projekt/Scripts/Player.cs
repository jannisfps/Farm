using System.Runtime;
using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{   
    public TMP_Text healthText;
    public TMP_Text scoreText;
    public float score = 0;


    private bool isHitting;
    private SpriteRenderer sprite;
    private int currentField;
    private Fruit fruit;
    private HashSet<KeyCode> _pressedThisFrame = new HashSet<KeyCode>();
    
    public int health {get; private set;}

    void Awake()
    {   
        sprite = GetComponent<SpriteRenderer>();

        health = GameManager.instance.sv.health;
        GameManager.instance.player = this;

        isHitting = false;
    }

    void Update()
    {   
        if(GameManager.instance.fruits.Count == 0) return;
        
        if(fruit == null) GetFruit();
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
            HitType hit = GameManager.instance.CalculateHit(fruit);
            
            score += AddScore(fruit, hit);
            CollectFruit();
            
        
            //MissFruit();
        }
            

        //healthText.text = "HEALTH: " + health;
        scoreText.text = "SCORE: " + score;
    }

    public void CollectFruit()
    {               
        //Debug.Log($"FRUIT ({fruit.fruitData.fruitName}): catched fruit : ");
        
        
        GameManager.instance.fruits.Remove(fruit);
        Destroy(fruit.gameObject);
    } 

    public void MissFruit()
    {
        //-------------------------------
        // Miss penalty for missClick
        //-------------------------------

        //isHitting = true;
        //StartCoroutine(Hitting(GameManager.instance.sv.penaltyCooldown));
        
        //-------------------------------
        
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
        health += amount;

        if (health <= 0)
        {
            GameOver();
        }
    }

    private void GetFruit() 
    {
        if (GameManager.instance.fruits == null || 
            GameManager.instance.fruits.Count == 0 || 
            GameManager.instance.fruits[0] == null) 
        {
            fruit = null;
            return;
        }

        fruit = GameManager.instance.fruits[0];

        
        GameObject visualHighlight = new GameObject("FruitHighlight");
        
        visualHighlight.transform.position = fruit.transform.position;
        visualHighlight.transform.rotation = fruit.transform.rotation;
        
        visualHighlight.transform.SetParent(fruit.transform);
        visualHighlight.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);


        SpriteRenderer sr = visualHighlight.AddComponent<SpriteRenderer>();
        

        if (fruit.TryGetComponent<SpriteRenderer>(out SpriteRenderer targetSR))
        {
            sr.sprite = targetSR.sprite;
        }
        

        sr.color = Color.green;
        sr.sortingOrder = 98;
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
        // TODO
        // + Other behavior
        HighScore.instance.AddNewHighScore(GameManager.instance.currentName, (int)score); 
    }
}
