using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFruitData", menuName = "Game/Fruit Data")]
public class FruitData : ScriptableObject
{
    [Header("Visuals & Info")]
    public string fruitName = "Fruit";
    public Sprite fruitSprite;

    [Header("Input Settings")]
    public List<KeyCode> requiredKeys = new List<KeyCode>();

    [Header("Score Values per HitType")]
    public float perfectScore = 100f;
    public float okeyScore = 50f;
    public float normalScore = 25f;
}