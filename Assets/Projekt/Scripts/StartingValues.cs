using UnityEngine;

[CreateAssetMenu(fileName = "StartingValues", menuName = "Scriptable Objects/StartingValues")]
public class StartingValues : ScriptableObject
{   
    [Header("Initial GameSpeed")]
    public float startGameSpeed = 4f;
    [Header("GameSpeed Increase every Sec.")]
    public float gameSpeedIncrease = 0.01f;

    [Header("Hitting values")]
    public float visualOffset = 30f;
    public float inputOffset = 30f;
    public float threshold= 135f;

    [Header("Thresholds in % (zusammen 100 max. -> Rest dazwischen wird als 'Good' gewertet)")]
    public float perfectThreshold = 25;
    public float normalThreshold = 33; 

    [Header("Start PlayerStats")]
    public int health = 3;
    public float hitCooldown = 0.05f;

    
    [Header("Field settings")]
    public int fieldstartingNumber;
}
