using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (GameManager.instance.CheckHit() == true)
            {
                Debug.Log("Player: catched fruit");
            } else Debug.Log("Player: Didnt catch");
        }
    }
}
