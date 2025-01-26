using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerTransformChannel _transformChannel;

    private AIDestinationSetter _aiDestination;

    private void Awake()
    {
        _aiDestination = GetComponent<AIDestinationSetter>();
    }

    private void Start()
    {
        _playerTransform = _transformChannel.GetPlayerTransform;
        _aiDestination.target = _playerTransform;
    }

    public void Update()
    {
        if (_playerTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
