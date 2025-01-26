using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Bubble : MonoBehaviour
{
    [SerializeField] protected float _bubbleForce = 5f;
    [SerializeField] private float _minimunSize;
    [SerializeField] private AudioClip[] audiclips;
    
    private Rigidbody2D _rb;

    
    

    public void LaunchBubble(Vector2 direction)
    {
        SoundFXManager.instance.PlaySoundFXClip(audiclips[1], transform, 1);
        _rb = GetComponent<Rigidbody2D>();

        _rb.velocity = direction.normalized * _bubbleForce;

        transform.SetParent(null);

        if(Mathf.Abs(transform.localScale.x) < _minimunSize) 
        {
            PopBubble();
        }
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
