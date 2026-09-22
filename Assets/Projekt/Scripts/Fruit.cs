using UnityEngine;
using System.Collections.Generic;

public class Fruit : MonoBehaviour
{   
    [SerializeField] private FruitData fruitData;

    private HashSet<KeyCode> _pressedThisFrame = new HashSet<KeyCode>();
    private bool _collected;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        if (fruitData != null && fruitData.fruitSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = fruitData.fruitSprite;
        }
    }

    void Update()
    {   
        rb.linearVelocityY = -GameManager.instance.GameSpeed;

        if (transform.position.y <= -10f) // vllt andere Höhe später
        {
            Debug.Log("Hit: Missed (zu weit unten)");
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {   
        if (_collected || fruitData == null) return;

        //----------------------------------------------------
        // Track which required keys are currently held
        //----------------------------------------------------
        _pressedThisFrame.Clear();
        foreach (var key in fruitData.requiredKeys)
            if (Input.GetKey(key))
                _pressedThisFrame.Add(key);

        //----------------------------------------------------
        // Check if ALL required keys were pressed this frame
        //----------------------------------------------------
        if (fruitData.requiredKeys.Count > 0 && _pressedThisFrame.Count == fruitData.requiredKeys.Count)
        {   
            CircleCollider2D cc = other.GetComponent<CircleCollider2D>();
            if (cc != null)
            {
                float center = cc.transform.TransformPoint(cc.offset).y;
                float radius = cc.radius;
                TryToCollect(center, radius);
            }
        }
    }

    private void TryToCollect(float center, float radius)
    {   
        HitType hit = GameManager.instance.CalculateHit(transform, center, radius);
            
        _collected = true;
        Debug.Log($"FRUIT ({fruitData.fruitName}): catched fruit : " + hit.ToString());

      
        float addedScore = 0f;
        switch (hit)
        {
            case HitType.Perfect:
                addedScore = fruitData.perfectScore;
                break;
            case HitType.Okey:
                addedScore = fruitData.okeyScore;
                break;
            case HitType.Normal:
                addedScore = fruitData.normalScore;
                break;
        }

        
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            GameManager.instance.player.score += addedScore;
        }

        
        Destroy(gameObject);
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