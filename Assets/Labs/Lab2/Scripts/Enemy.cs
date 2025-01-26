using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public float moveSpeed = 2f;
    public float raycastDistance = 0.6f;

    [SerializeField] protected int health;

    private int moveDirection = 1; // 1 derecha, -1 izquierda
    [SerializeField] protected Rigidbody2D rb;

    private void Awake() 
    {
        
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

    public virtual void GetDamage(Bubble bubble)
    {
        
    }

    

    
}

