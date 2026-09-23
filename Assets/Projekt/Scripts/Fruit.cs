using UnityEngine;
using System.Collections.Generic;

public class Fruit : MonoBehaviour
{   
    public FruitData fruitData;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public bool isRotten = false;
    private bool _hasMissed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (fruitData != null && fruitData.fruitSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = fruitData.fruitSprite;
        }
        
        if (GameManager.instance != null)
        {
            GameManager.instance.fruits.Add(this);
        }
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        rb.linearVelocityY = -GameManager.instance.GameSpeed;
        
        
        if (!_hasMissed && GameManager.instance.missedField != null)
        {
            float missedThreshold = GameManager.instance.missedField.transform.position.y - (GameManager.instance.missedField.transform.localScale.y / 2);
            
            if (transform.position.y < missedThreshold)
            {
                _hasMissed = true;
                if (GameManager.instance.player != null)
                {
                    GameManager.instance.player.MissFruit();
                }
            }
        }
    }

    public void SetFruitData(FruitData newData)
    {
        fruitData = newData;
        if (spriteRenderer != null && fruitData != null && fruitData.fruitSprite != null)
        {
            spriteRenderer.sprite = fruitData.fruitSprite;
        }

        
        if (GameManager.instance != null && GameManager.instance.sv != null)
        {
            float random = Random.value;
            if (random <= GameManager.instance.sv.rottenChance / 100f) 
            {
                isRotten = true;
            }
            
            if (spriteRenderer != null && fruitData != null && isRotten && fruitData.rottenSprite != null)
            {
                spriteRenderer.sprite = fruitData.rottenSprite;
            }
        }
    }
}