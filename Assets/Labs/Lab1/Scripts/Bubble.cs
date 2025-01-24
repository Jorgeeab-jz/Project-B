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

    public BubbleType GetBubbleType(int bubbleSum)
    {
        BubbleType type = BubbleType.normal;

        switch (bubbleSum)
        {
            case 1:
                type = BubbleType.fire;
                break;

            case 2:
                type = BubbleType.steam;
                break;

            case 3:
                type = BubbleType.water;
                break;

            case 4:
                type = BubbleType.mud;
                break;

            case 5:
                type = BubbleType.earth;
                break;

            case 6:
                type = BubbleType.lava;
                break;
        }

        return type;

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
