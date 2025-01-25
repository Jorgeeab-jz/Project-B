using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Bubble : MonoBehaviour
{
    [SerializeField] protected float _bubbleForce = 5f;
    
    private Rigidbody2D _rb;


    

    public void LaunchBubble(Vector2 direction)
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.velocity = direction.normalized * _bubbleForce;
    }

    public void PopBubble()
    {
        GameObject.Destroy(gameObject);
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
    lava

}
