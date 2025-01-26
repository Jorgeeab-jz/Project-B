using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float raycastDistance = 0.6f;

    private int moveDirection = 1; // 1 derecha, -1 izquierda
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Detectar bordes o paredes para cambiar dirección
        RaycastHit2D hitGround = Physics2D.Raycast(
            transform.position,
            Vector2.right * moveDirection,
            raycastDistance,
            LayerMask.GetMask("Ground")
        );

        if (hitGround.collider != null)
        {
            // Cambiar dirección al encontrar un obstáculo
            moveDirection *= -1;
        }

        // Mover enemigo
        rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Lógica de colisión con jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aquí puedes agregar lógica de daño al jugador
            Debug.Log("Colisión con jugador");
        }
    }
}

