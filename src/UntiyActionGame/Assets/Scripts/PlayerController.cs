using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _movementInput;
    private Rigidbody2D _rb;
    private Animator _characterAnimator;
    private SpriteRenderer _characterSpriteRenderer;
    
    private static readonly string _animatorMoveName = "Move";

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private InputActionReference jumpInput;

    private readonly string _animatorInAirName = "InAir";
    private readonly string _animatorJumpName = "Jump";
    private Sensor _groundSensor;
    private bool _canJump = true;
    
    private bool _isDead = false;

    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _characterAnimator = this.GetComponentInChildren<Animator>();
        _characterSpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        _groundSensor = this.transform.Find("GroundSensor").GetComponent<Sensor>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput.action.Enable();
        moveInput.action.performed += OnMove;
        moveInput.action.canceled += OnMove;
        
        jumpInput.action.Enable();
        jumpInput.action.performed += OnJump;
    }

    private void OnDestroy()
    {
        moveInput.action.Disable();
        moveInput.action.performed -= OnMove;
        moveInput.action.canceled -= OnMove;
        
        jumpInput.action.Disable();
        jumpInput.action.performed -= OnJump;
    }

    private void FixedUpdate()
    {
        _characterAnimator.SetBool(_animatorInAirName, !_groundSensor.HaveTarget);
        
        if (_movementInput.magnitude > 0f)
        {
            _rb.AddForce(_movementInput * moveSpeed, ForceMode2D.Impulse);
            
            // 設定速度上限
            if (Mathf.Abs(_rb.linearVelocity.x) > moveSpeed)
            {
                _rb.linearVelocity = new Vector2((_rb.linearVelocity.x > 0f ? 1f : -1f) * moveSpeed, _rb.linearVelocity.y);
            }
        }
        else
        {
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }

    }

    /// <summary>
    /// 當玩家移動時執行。
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        // 死亡時不執行
        if (_isDead)
            return;
        
        _movementInput = context.ReadValue<Vector2>();
        _movementInput = new Vector2(_movementInput.x, 0f);
        _characterAnimator.SetFloat(_animatorMoveName, Mathf.Abs(_movementInput.x));
        if (_movementInput.x > 0f)
        {
            _characterSpriteRenderer.flipX = false;
        }
        else if (_movementInput.x < 0f)
        {
            _characterSpriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// 當玩家跳躍時執行。
    /// </summary>
    /// <param name="context"></param>
    private void OnJump(InputAction.CallbackContext context)
    {
        // 無法跳躍、死亡、無接觸地板時不執行。
        if (!_canJump || _isDead || !_groundSensor.HaveTarget)
            return;
        
        _canJump = false;
        _characterAnimator.SetTrigger(_animatorJumpName);
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void OnJumpEnd()
    {
        _canJump = true;
    }

    public void OnDead()
    {
        _isDead = true;
    }
}
  