using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class Fruit : MonoBehaviour
{   
    public FruitData fruitData;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public float timeALife {get; private set;}

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
        timeALife += Time.deltaTime * 1000;
    }


    public void SetFruitData(FruitData newData)
    {
        fruitData = newData;
        if (spriteRenderer != null && fruitData != null && fruitData.fruitSprite != null)
        {
            spriteRenderer.sprite = fruitData.fruitSprite;
        }
    }
}