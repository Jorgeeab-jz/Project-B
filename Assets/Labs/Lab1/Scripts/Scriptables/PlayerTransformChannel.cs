using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTransformChannel", menuName = "Scriptables/Channels/Player Transform")]
public class PlayerTransformChannel : ScriptableObject
{
    [SerializeField] private Transform _playerTransform;

    public void SetTransform(Transform transform) 
    {
        _playerTransform = transform;
    }

    public Transform GetPlayerTransform { get { return _playerTransform; } }
}
