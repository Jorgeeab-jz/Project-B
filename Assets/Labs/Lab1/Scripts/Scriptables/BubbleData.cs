using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleData", menuName = "Scriptables/Data/Bubble")]
public class BubbleData : ScriptableObject
{
    [SerializeField] private BubbleType _type;

    [SerializeField] private float _velocity;

    [SerializeField] private float _damage;

    [SerializeField] private GameObject _bubblePrefab;

    public BubbleType BubbleType { get { return _type; } }
    public float Velocity { get { return _velocity; } }
    public float Damage { get { return _damage; } }
    public GameObject BubblePrefab { get { return _bubblePrefab; } }

}
