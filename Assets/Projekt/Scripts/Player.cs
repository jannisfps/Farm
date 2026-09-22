using System.Runtime;
using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{   
    public TMP_Text healthText;
    public TMP_Text scoreText;
    public float score = 0;


    private bool isHitting;
    private SpriteRenderer sprite;
    private int currentField;
    
    public int health {get; private set;}

    void Start()
    {   
        sprite = GetComponent<SpriteRenderer>();

        health = GameManager.instance.sv.health;
        GameManager.instance.player = this;

        isHitting = false;
        currentField = GameManager.instance.sv.fieldstartingNumber;
    }

    void Update()
    {   
        int moveDirection = 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow))    Move(-moveDirection);
        if (Input.GetKeyDown(KeyCode.RightArrow))   Move(moveDirection);

        //isHitting = true;
        //StartCoroutine(Hitting());

        healthText.text = "HEALTH: " + health;
        scoreText.text = "SCORE: " + score;
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
