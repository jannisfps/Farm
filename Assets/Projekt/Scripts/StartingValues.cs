using UnityEngine;

[CreateAssetMenu(fileName = "StartingValues", menuName = "Scriptable Objects/StartingValues")]
public class StartingValues : ScriptableObject
{   
    [Header("Initial GameSpeed")]
    public float startGameSpeed = 4f;

    [Header("Hitting values")]
    public float correctThreshold = 0.25f;
    public float hitY = -2.5f;

    [Header("Start PlayerStats")]
    public int health = 3;
}
