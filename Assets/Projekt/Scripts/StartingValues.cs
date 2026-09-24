using Unity.Multiplayer.Center.Common;
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

    
    [Header("Fruit Rotten Change in % (0- 100)")]
    public float rottenChance = 0;

    [Header("Switch Fruit KeyBinds")]
    public KeyCode fruitUp = KeyCode.UpArrow;
    public KeyCode fruitDown = KeyCode.DownArrow;

    
    [Header("HitFeedback Images")]
    public Sprite missedSprite;
    public Sprite normalSprite;
    public Sprite okeySprite;
    public Sprite perfectSprite;
}
