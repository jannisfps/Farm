using UnityEngine;

public class Note : MonoBehaviour
{   
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.instance.fruits.Add(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityY = -GameManager.instance.GameSpeed;

/*
        if(transform.position.y <= -10)
        {
            GameManager.instance.player.GainHealth(-1);
            Destroy(gameObject);
            GameManager.instance.fruits.Remove(gameObject.transform);
            // TODO: condition anpassen
        }
*/
    }
}
