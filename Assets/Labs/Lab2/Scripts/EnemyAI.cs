using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRadius = 5f;
    public float raycastDistance = 0.6f;

    private Transform player;
    private Rigidbody2D rb;
    private int moveDirection = 1;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Calcular distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            ChasePlayer();
        }
        else
        {
            PatrolMovement();
        }
    }

    void ChasePlayer()
    {
        isChasing = true;
        // Determinar dirección hacia el jugador
        moveDirection = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
    }

    void PatrolMovement()
    {
        isChasing = false;
        // Lógica de patrullaje original
        RaycastHit2D hitGround = Physics2D.Raycast(
            transform.position,
            Vector2.right * moveDirection,
            raycastDistance,
            LayerMask.GetMask("Ground")
        );

        if (hitGround.collider != null)
        {
            moveDirection *= -1;
        }

        rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar radio de detección en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
