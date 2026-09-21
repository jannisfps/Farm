using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-99999)]
public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    public float gameSpeed = 4f;
    public List<Transform> fruits = new List<Transform>();

    private const float correctThreshold = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckHit()
    {   
        foreach (Transform fr in fruits)
        {
            if (gameSpeed > fr.position.y + correctThreshold)
            {
                //midiNotes.Remove (midiNote); // TODO figure out a way to remove notes after they have been passed
            }
            if (gameSpeed < fr.position.y + correctThreshold && gameSpeed > fr.position.y - correctThreshold)
            {
                return true;
            }
        }
        return false;
    }
}
