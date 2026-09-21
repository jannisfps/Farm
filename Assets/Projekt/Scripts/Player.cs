using System.Runtime;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{   
    public TMP_Text healthText;
    public TMP_Text scoreText;
    public int health {get; private set;}
    public float score = 0;
    
    void Start()
    {
        health = GameManager.instance.sv.health;
        GameManager.instance.player = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {   
            HitType hit = GameManager.instance.CheckHit();
            if (hit != HitType.Missed)
            {
                // TODO: Hit Cooldown 
                Debug.Log("Player: catched fruit : " + hit.ToString());
                // TODO: score
            }
            else
            {
                Debug.Log("Player: Didnt catch");
            }
        } 

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
}
