using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptables/Data/Player Stats")]
public class PlayerStats : ScriptableObject
{   
    [Header("Values")]
    [SerializeField] private int _maxSpeed;
    [SerializeField] private int _normalSpeed;
    [SerializeField] private int _bubbleHealth;

    // Getters

    public int MaxSpeed { get { return _maxSpeed; } }
    public int NormalSpeed { get { return _normalSpeed; } }
    public int BubbleHealth { get { return _bubbleHealth; } }
}
