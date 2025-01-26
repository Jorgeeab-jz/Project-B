using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour, IEnemy
{
    public float moveSpeed = 2f;
    public float raycastDistance = 0.6f;

    [SerializeField] protected int health;

    [SerializeField] protected Transform _playerTransform;
    [SerializeField] protected PlayerTransformChannel _transformChannel;
    [SerializeField] protected Animator _animator;
    [SerializeField] private StarGrabChannel _resetChannel;

    protected AIDestinationSetter _aiDestination;
    protected AIPath _aiComponent;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int _receivedDamage;

    private int moveDirection = 1; // 1 derecha, -1 izquierda
    [SerializeField] protected Rigidbody2D rb;
    private CircleCollider2D collider2D;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        collider2D = GetComponent<CircleCollider2D>();
        _resetChannel.onInteraction += KillEnemy;
    }

    private void OnDisable()
    {
        _resetChannel.onInteraction -= KillEnemy;
    }

    void Update()
    {
        // Detectar bordes o paredes para cambiar direcci�n
        RaycastHit2D hitGround = Physics2D.Raycast(
            transform.position,
            Vector2.right * moveDirection,
            raycastDistance,
            LayerMask.GetMask("Ground")
        );

        if (hitGround.collider != null)
        {
            // Cambiar direcci�n al encontrar un obst�culo
            moveDirection *= -1;
        }

        // Mover enemigo
        rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // L�gica de colisi�n con jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aqu� puedes agregar l�gica de da�o al jugador
            Debug.Log("Colisi�n con jugador");
        }
    }

    private void GetHit(BubbleType bubble)
    {

        Debug.Log("[Enemy]: Getting damage");


        switch (bubble)
        {
            case BubbleType.electric:
                _aiComponent.maxSpeed = 0.1f;
                DOTween.To(() => _aiComponent.maxSpeed, x => _aiComponent.maxSpeed = x, 0.5f, 1.5f).OnComplete(() => _aiComponent.maxSpeed = moveSpeed);

                break;

            case BubbleType.fire:
                DOTween.To(() => _aiComponent.maxSpeed, x => _aiComponent.maxSpeed = x, 0.5f, 0.2f).OnComplete(() => _aiComponent.maxSpeed = moveSpeed);

                break;

            case BubbleType.air:
                DOTween.To(() => _aiComponent.maxSpeed, x => _aiComponent.maxSpeed = x, 0.5f, 0.05f).OnComplete(() => _aiComponent.maxSpeed = moveSpeed);

                break;

            case BubbleType.lava:
                DOTween.To(() => _aiComponent.maxSpeed, x => _aiComponent.maxSpeed = x, 0.5f, 0.3f).OnComplete(() => _aiComponent.maxSpeed = moveSpeed);

                break;

        }


    }

    public void GetDamageAmmount(int damage)
    {
        _currentHealth -= damage;
    }

    protected void CheckHealth()
    {
        if (_currentHealth <= 0)
        {
            KillEnemy();
        }
    }

    protected void KillEnemy()
    {   
        collider2D.enabled = false;
        StartCoroutine(KillAnimation());
    }

    IEnumerator KillAnimation()
    {

        _animator.SetInteger("Health", _currentHealth);
        yield return new WaitForSeconds(0.5f);
        GameObject.Destroy(gameObject);
    }

    IEnumerator DamageAnimation()
    {
        _animator.SetFloat("Damage", 1f);
        yield return new WaitForSeconds(0.1f);
        _animator.SetFloat("Damage", -1f);
    }

    public void GetDamage(BubbleType bubble)
    {
        StartCoroutine(DamageAnimation());
        GetHit(bubble);
        CheckHealth();
    }
}

