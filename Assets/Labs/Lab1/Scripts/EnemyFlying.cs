using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;


public class EnemyFlying : Enemy, IEnemy
{
    

    private void Awake()
    {
        _currentHealth = health;
        _aiDestination = GetComponent<AIDestinationSetter>();
        _aiComponent = GetComponent<AIPath>();

    }

    private void Start()
    {
        _playerTransform = _transformChannel.GetPlayerTransform;
        _aiDestination.target = _playerTransform;
        _aiComponent.maxSpeed = moveSpeed;
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
