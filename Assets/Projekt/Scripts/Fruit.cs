using JetBrains.Rider.Unity.Editor;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class Fruit : MonoBehaviour
{   
    [SerializeField] private List<KeyCode> requiredKeys = new List<KeyCode>();

    [Header("Score Values per HitType")]
    [SerializeField] private float perfectScore = 100f;
    [SerializeField] private float okeyScore = 50f;
    [SerializeField] private float normalScore = 25f;
    
    private HashSet<KeyCode> _pressedThisFrame = new HashSet<KeyCode>();
    private bool _collected;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {   
        rb.linearVelocityY = -GameManager.instance.GameSpeed;

        if (transform.position.y <= -10f) //vllt andere höhe später
        {
            Debug.Log("Hit: Missed (zu weit unten)");

            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {   
        if (_collected) return;

        //----------------------------------------------------
        // Track which required keys are currently held
        //----------------------------------------------------
        _pressedThisFrame.Clear();
        foreach (var key in requiredKeys)
            if (Input.GetKey(key))
                _pressedThisFrame.Add(key);


        //----------------------------------------------------
        // Check if ALL required keys were pressed this frame
        //----------------------------------------------------
        if (requiredKeys.Count > 0 && _pressedThisFrame.Count == requiredKeys.Count)
        {   
            CircleCollider2D cc = other.GetComponent<CircleCollider2D>();
            float center = other.gameObject.transform.position.y + cc.transform.TransformPoint(cc.offset).y;

            float radius = cc.radius;
            TryToCollect(center, radius);
        }
    }

    private void TryToCollect(float center, float radius)
    {   
        HitType hit = GameManager.instance.CalculateHit(transform, center, radius);
            
        _collected = true;
         Debug.Log("FRUIT: catched fruit : " + hit.ToString());

        // Punkte basierend auf HitType bestimmen
        float addedScore = 0f;
        switch (hit)
        {
            case HitType.Perfect:
                addedScore = perfectScore;
                break;
            case HitType.Okey:
                addedScore = okeyScore;
                break;
            case HitType.Normal:
                addedScore = normalScore;
                break;
        }

        // Score beim Player aufaddieren
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            GameManager.instance.player.score += addedScore;
        }

        // Frucht zerstören, damit sie nicht mehrfach getroffenen werden kann
        Destroy(gameObject);
    } 
}