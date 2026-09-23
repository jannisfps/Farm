using UnityEngine;

[CreateAssetMenu(fileName = "StartingValues", menuName = "Scriptable Objects/StartingValues")]
public class StartingValues : ScriptableObject
{   
    [Header("Initial GameSpeed")]
    public float startGameSpeed = 4f;
    [Header("GameSpeed Increase every Sec.")]
    public float gameSpeedIncrease = 0.01f;

    [Header("Start PlayerStats")]
    public int health = 3;
    public float hitCooldown = 0.05f;
    public float penaltyCooldown = 0.5f;
    
    public int missFruitCost = 20;
}
