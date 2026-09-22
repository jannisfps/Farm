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
        currentField = GameManager.instance.sv.fieldstartingNumber;

    }

    void Update()
    {   
        if(GameManager.instance.fruits.Count == 0) return;
        
        fruit = GameManager.instance.fruits[0];

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
        StartCoroutine(Hitting());
        
        if (_pressedThisFrame.Count == fruit.fruitData.requiredKeys.Count)
        {
            score += AddScore(GameManager.instance.CalculateHit(fruit));

            fruit.CollectFruit();
            
        
            //fruit.MissFruit();
        }
            

        healthText.text = "HEALTH: " + health;
        scoreText.text = "SCORE: " + score;
    }

    public float AddScore(HitType type)
    {
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

    public void Move(int direction)
    {   
        int max = GameManager.instance.sv.fieldstartingNumber;
        currentField = Mathf.Clamp(
            currentField += 1 * direction, max, -max);
        transform.position = new Vector3(currentField, transform.position.y, 0);
    }

    IEnumerator Hitting()
    {
        UpdateHitColor();
        yield return new WaitForSeconds(GameManager.instance.sv.hitCooldown);
        isHitting = false;
        UpdateHitColor();
    }

    public void UpdateHitColor()
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
