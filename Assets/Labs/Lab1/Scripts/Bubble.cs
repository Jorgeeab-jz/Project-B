using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Bubble : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _bubbleRigidBody;
    [SerializeField] private PlayerStats _playerStats;

    [Space(10)]
    [Header("Input")]
    [SerializeField] private InputActionReference _playerInput;


    private void Start() {
        
    }

    private void OnEnable() {
        _playerInput.action.Enable();
        _playerInput.action.performed += Move;
        _playerInput.action.canceled += Move;
    }

    private void OnDisable() {
        _playerInput.action.performed -= Move;
        _playerInput.action.canceled -= Move;
        _playerInput.action.Disable();
    }





    // Methods
    private void Move(InputAction.CallbackContext context) 
    {
        Vector2 input = context.ReadValue<Vector2>();

        _bubbleRigidBody.AddForce(input * _playerStats.NormalSpeed);

        Debug.Log($"[Bubble]: Got input: {input}");
    }
}
