using JetBrains.Rider.Unity.Editor;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Fruit : MonoBehaviour
{   
    [SerializeField] private List<KeyCode> requiredKeys = new List<KeyCode>();
    
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

        if (_collected) return;

        //----------------------------------------------------
        // Track which required keys are currently held
        //----------------------------------------------------
        _pressedThisFrame.Clear();
        foreach (var key in requiredKeys)
            if (Input.GetKeyDown(key))
                _pressedThisFrame.Add(key);


        //----------------------------------------------------
        // Check if ALL required keys were pressed this frame
        //----------------------------------------------------
        if (_pressedThisFrame.Count == requiredKeys.Count)
        {
            TryToCollect();
        }


        if (transform.position.y <= -10f) //vllt andere höhe später
        {
            Debug.Log("Hit: Missed (zu weit unten)");

            Destroy(gameObject);
        }
    }

    private void TryToCollect()
    {   
        HitType hit = GameManager.instance.CheckHit(transform);
            
        if (hit != HitType.Missed)
        {
            Debug.Log("FRUIT: catched fruit : " + hit.ToString());
            // score hier -> hitType kann hier in score converted werden
        } 
        else Debug.Log("FRUIT: missed fruit : " + hit.ToString());
    }
}