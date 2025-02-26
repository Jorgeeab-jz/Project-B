using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;


public class Bubble : MonoBehaviour, IBubble
{
    [SerializeField] protected float _bubbleForce = 5f;
    [SerializeField] private int _damage;
    [SerializeField] private float _minimunSize;
    [SerializeField] private AudioClip[] audiclips;
    
    private Rigidbody2D _rb;
    [SerializeField] private float _maximunSize;
    [SerializeField] private float _blowTime;
    [SerializeField] private Sprite _gumIcon;

    [SerializeField] private BubbleType _bubbleType;

    
    private bool _isLaunched;

    public Sprite Sprite { get { return _gumIcon; } }


    private void OnCollisionEnter2D(Collision2D other)
    {
        IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
        if (enemy == null) return;
        enemy.GetDamage(_bubbleType);
        enemy.GetDamageAmmount(_damage);
        Debug.Log("Bubble collisioned");

        Pop();
    }

    public void LaunchBubble(Vector2 direction)
    {
        SoundFXManager.instance.PlaySoundFXClip(audiclips[1], transform, 1);
        _rb = GetComponent<Rigidbody2D>();

        transform.DOKill();

        if (!IsReady())
        {
            Pop();
            return;
        }
        else
        {
            _rb = GetComponent<Rigidbody2D>();

            _rb.velocity = direction.normalized * _bubbleForce;

            transform.SetParent(null);
            _isLaunched = true;
        }




    }

    public void Pop()
    {
        transform.DOKill();
        SoundFXManager.instance.PlaySoundFXClip(audiclips[0], transform, 1);
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
        transform.DOScale(_maximunSize, _blowTime).OnComplete(() => transform.DOKill());

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
