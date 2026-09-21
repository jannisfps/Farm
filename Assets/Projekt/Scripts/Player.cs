using System.Runtime;
using UnityEngine;
using TMPro;﻿
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{   
    public TMP_Text healthText;
    public TMP_Text scoreText;
    public int health {get; private set;}
    public float score = 0;

    private bool isHitting;
    private SpriteRenderer sprite;
    
    void Start()
    {   
        sprite = GetComponent<SpriteRenderer>();

        health = GameManager.instance.sv.health;
        GameManager.instance.player = this;

        isHitting = false;
    }

    void Update()
    {   
        
        //isHitting = true;
        //StartCoroutine(Hitting());

        /*if (Input.GetKeyDown(KeyCode.F))
        {   
            HitType hit = GameManager.instance.CheckHit();
            if (hit != HitType.Missed)
            {
                Debug.Log("Player: catched fruit : " + hit.ToString());
                // TODO: score
            }
            else Debug.Log("Player: Didnt catch");
            
            
        } */

        healthText.text = "HEALTH: " + health;
        
        scoreText.text = "HEALTH: " + score;
    }

    public void GainHealth(int amount)
    {
        health += amount;

        if (health <= 0)
        {
            // TODO: GameOver
        }
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
}
