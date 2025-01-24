using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Transform _bubbleCenterTransform;

    private void FixedUpdate() {
        if (_bubbleCenterTransform != null) 
        {
            transform.position = _bubbleCenterTransform.position;
        }
    }
}
