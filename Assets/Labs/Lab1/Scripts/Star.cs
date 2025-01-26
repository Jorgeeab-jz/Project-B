using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private StarGrabChannel _grabChannel;

    private void OnTriggerEnter2D(Collider2D other) {
        GrabStar();
    }

    private void GrabStar() 
    {
        _grabChannel?.onInteraction.Invoke();
        GameObject.Destroy(gameObject);
    }
}
