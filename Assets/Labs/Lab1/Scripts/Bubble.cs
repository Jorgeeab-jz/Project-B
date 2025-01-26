using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;


public class Bubble : MonoBehaviour, IBubble
{
    [SerializeField] protected float _bubbleForce = 5f;
    [SerializeField] private float _minimunSize;
    [SerializeField] private float _maximunSize;
    [SerializeField] private float _blowTime;
    [SerializeField] private Sprite _gumIcon;

    [SerializeField] private BubbleType _bubbleType;

    private Rigidbody2D _rb;
    private bool _isLaunched;

    public Sprite Sprite { get { return _gumIcon; } }


    private void OnCollisionEnter2D(Collision2D other)
    {
        IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
        if (enemy == null) return;
        enemy.GetDamage(this);
        Debug.Log("Bubble collisioned");

        Pop();
    }

    public void LaunchBubble(Vector2 direction)
    {   
        if(_isLaunched) return;
        _rb = GetComponent<Rigidbody2D>();

        _rb.velocity = direction.normalized * _bubbleForce;

        transform.SetParent(null);
        _isLaunched = true;
        if (!IsReady())
        {   
            Debug.Log($"[Bubble]: minimun: {_minimunSize}, current: {Mathf.Abs(transform.localScale.x)}, max: {_maximunSize}");
            Pop();
        }

        
    }

    public void Pop()
    {   
        transform.DOKill();
        GameObject.Destroy(gameObject);
    }

    public bool IsReady()
    {
        return Mathf.Abs(transform.localScale.x) >= _minimunSize; 
    }

    public void InflateBubble() 
    {
        transform.DOScale(_minimunSize, 0.05f);

        transform.DOLocalMoveX(1f, _blowTime);
        transform.DOScale(_maximunSize, _blowTime).OnComplete(()=> transform.DOKill());

    }


    public BubbleType GetBubbleType()
    {
        return _bubbleType;
    }
}

public enum BubbleType
{
    normal,
    fire,
    water,
    earth,
    steam,
    mud,
    lava,
    electric,
    air

}
