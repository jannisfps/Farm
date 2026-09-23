using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class Fruit : MonoBehaviour
{   
    public FruitData fruitData;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public bool isRotten = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        if (fruitData != null && fruitData.fruitSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = fruitData.fruitSprite;
        }
        
        GameManager.instance.fruits.Add(this);
    }

    void Update()
    {
        rb.linearVelocityY = -GameManager.instance.GameSpeed;
        
        if (
            transform.position.y < GameManager.instance.missedField.transform.position.y - (GameManager.instance.missedField.transform.localScale.y / 2))
                GameManager.instance.player.MissFruit();
    }


    public void SetFruitData(FruitData newData)
    {
        fruitData = newData;
        if (spriteRenderer != null && fruitData != null && fruitData.fruitSprite != null)
        {
            spriteRenderer.sprite = fruitData.fruitSprite;
        }

        // Rott the fuit
        float random = Random.value;
        if (random <= GameManager.instance.sv.rottenChance / 100) isRotten = true;
        
        if (spriteRenderer != null && fruitData != null && fruitData.fruitSprite != null)
        {
            if (isRotten) spriteRenderer.sprite = fruitData.rottenSprite;
        }
    }
}