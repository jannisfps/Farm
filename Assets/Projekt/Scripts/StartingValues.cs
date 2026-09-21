using UnityEngine;

[CreateAssetMenu(fileName = "StartingValues", menuName = "Scriptable Objects/StartingValues")]
public class StartingValues : ScriptableObject
{   
    [Header("Initial GameSpeed")]
    public float startGameSpeed = 4f;

    [Header("Hitting values")]
    public float correctThreshold = 0.375f;
    public float hitY = -2.5f;
    [Header("Thresholds in % (zusammen 100 max. -> Rest dazwischen wird als 'Good' gewertet)")]
    public float perfectThreshold = 25;
    public float normalThreshold = 33; 

    [Header("Start PlayerStats")]
    public int health = 3;
    public float hitCooldown = 0.05f;
}
