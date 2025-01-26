using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;


public class EnemyFlying : Enemy, IEnemy
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerTransformChannel _transformChannel;

    private AIDestinationSetter _aiDestination;
    private AIPath _aiComponent;

    private void Awake()
    {
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

    public override void GetDamage(Bubble bubble)
    {
        GetHit(bubble);
        
    }

    private void GetHit(Bubble bubble) 
    {
        Vector3 bubblePosition = bubble.transform.position;
        
        switch (bubble.GetBubbleType())
        {   
            case BubbleType.electric:
            _aiComponent.maxSpeed = 0.1f;
            DOTween.To(()=> _aiComponent.maxSpeed, x => _aiComponent.maxSpeed = x, 0.5f, 1f).OnComplete(()=> _aiComponent.maxSpeed = moveSpeed);
            health -= 2;
            
            break;

            case BubbleType.fire:
            health -= 4;

            break;
            
        }

    }
    
}
