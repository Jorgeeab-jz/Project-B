using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;
using System.Linq;


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterController : MonoBehaviour
{
    [Header("References")]

    //Player

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private InputActionReference _movementInput;
    [SerializeField] private InputActionReference _jumpInput;
    [SerializeField] private InputActionReference _castInput;
    [SerializeField] private CinemachineVirtualCamera _camera;

    //Bubble
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private Transform _CastPosition;
    [SerializeField] private Transform _aimPoint;
    private Bubble _castedBubble;


    [Space(10)]
    [Header("Values")]

    //Player
    [SerializeField] private float _lookRightCameraValue;
    [SerializeField] private float _lookLeftCameraValue;
    [SerializeField] private float _cameraChangeSpeed;

    //Bubbles
    [SerializeField] private float _maxBubbleScale;
    [SerializeField] private float _minBubbleScale;
    [SerializeField] private float _blowBubbleTime;
    private bool _isCasting = false;

    private FrameInput _frameInput;
    private Vector2 _frameVelocity;
    private bool _cachedQueryStartInColliders;

    #region Interface

    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;

    #endregion

    private float _time;

    private void Awake()
    {

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    private void Update()
    {
        _time += Time.deltaTime;

    }

    private void OnEnable()
    {
        _movementInput.action.performed += GatherMovementInput;
        _jumpInput.action.performed += GatherJumpInput;
        _castInput.action.performed += CastBubble;


        _movementInput.action.canceled += GatherMovementInput;
        _jumpInput.action.canceled += GatherJumpInput;
        _castInput.action.canceled += CastBubble;

        _movementInput.action.Enable();
        _jumpInput.action.Enable();
        _castInput.action.Enable();
    }

    private void OnDisable()
    {
        _movementInput.action.Disable();
        _jumpInput.action.Disable();
        _castInput.action.Disable();

        _movementInput.action.performed -= GatherMovementInput;
        _jumpInput.action.performed -= GatherJumpInput;
        _castInput.action.performed -= CastBubble;

        _movementInput.action.canceled -= GatherMovementInput;
        _jumpInput.action.canceled -= GatherJumpInput;
        _castInput.action.canceled -= CastBubble;
    }

    private void GatherJumpInput(InputAction.CallbackContext context)
    {
        _frameInput.JumpDown = context.ReadValueAsButton();
        _frameInput.JumpHeld = context.ReadValueAsButton();


        if (_frameInput.JumpDown)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }

        Debug.Log("Gathered Jump Input");

    }

    private void GatherMovementInput(InputAction.CallbackContext context)
    {
        

        Vector2 input = context.ReadValue<Vector2>();

        if(!_isCasting)
        {
            _animator.SetBool("IsRunning", input.x != 0);
        }

        _frameInput = new FrameInput
        {
            Move = input,

        };

        if (_stats.SnapInput)
        {
            _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
            _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
        }

        ChangeLookDirection(input.x);

    }

    private void FixedUpdate()
    {
        CheckCollisions();

        if (_isCasting) return;

        HandleJump();
        HandleDirection();
        HandleGravity();

        ApplyMovement();
    }

    #region Collisions

    private float _frameLeftGrounded = float.MinValue;
    private bool _grounded;

    private void CheckCollisions()
    {

        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling
        bool groundHit = Physics2D.CapsuleCast(_collider.bounds.center, _collider.size, _collider.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
        bool ceilingHit = Physics2D.CapsuleCast(_collider.bounds.center, _collider.size, _collider.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

        // Hit a Ceiling
        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        // Landed on the Ground
        if (!_grounded && groundHit)
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        }
        // Left the Ground
        else if (_grounded && !groundHit)
        {
            _grounded = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;


    }

    #endregion


    #region Jumping

    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

    private void HandleJump()
    {
        if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rigidBody.velocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (_grounded || CanUseCoyote) ExecuteJump();

        Debug.Log("Jump Handled");

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = _stats.JumpPower;
        Jumped?.Invoke();

        Debug.Log("Tried to exectute jump");
    }

    #endregion

    #region Horizontal

    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }
    }

    #endregion

    #region Gravity

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {   
            _animator.SetBool("IsFalling", false);
            _animator.SetBool("IsJumping", false);
            _frameVelocity.y = _stats.GroundingForce;
        }
        else
        {
            if (_frameVelocity.y > 0f) 
            {
                _animator.SetBool("IsJumping", true);
            }else if (_frameVelocity.y < 0f) 
            {
                _animator.SetBool("IsFalling", true);
            }
            
            var inAirGravity = _stats.FallAcceleration;
            if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    #endregion

    private void ApplyMovement() => _rigidBody.velocity = _frameVelocity;

    #region Camera

    private void ChangeLookDirection(float input)
    {

        if (input > 0)
        {
            transform.localScale = new Vector3(1,1,1);

        }
        else if (input < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        
        }
    }



    #endregion

    #region Bubbles

    private void CastBubble(InputAction.CallbackContext context)
    {

        if (context.ReadValueAsButton())
        {   
        
            _isCasting = true;
            _animator.SetBool("IsCasting",true);
            _rigidBody.velocity = Vector2.zero;
            
            _castedBubble = Instantiate(_bubblePrefab, _CastPosition).GetComponent<Bubble>();
            _castedBubble.transform.DOScale(_minBubbleScale, 0.05f);

            _castedBubble.transform.DOLocalMoveX(1f, _blowBubbleTime);
            _castedBubble.transform.DOScale(_maxBubbleScale, _blowBubbleTime);

        }
        else
        {   
            Vector2 direction = _CastPosition.position - _aimPoint.position; 

            float minimumBubbleSize = _maxBubbleScale - 0.4f;
            float scaleAbsolute = Math.Abs(_castedBubble.transform.localScale.x);

            _castedBubble.transform.DOKill();
            _castedBubble.LaunchBubble(direction);

            _animator.SetBool("IsCasting",false);
            _isCasting = false;
            
        }

        Debug.Log(context.ReadValueAsButton() ? "Casted Pressed" : "Casted Released");


    }

    #endregion

}

public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public Vector2 Move;
}


