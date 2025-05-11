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
    
    private readonly string _animatorMoveName = "Move";

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveInput;

    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _characterAnimator = this.GetComponentInChildren<Animator>();
        _characterSpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput.action.Enable();
        moveInput.action.performed += OnMove;
        moveInput.action.canceled += OnMove;
    }

    private void FixedUpdate()
    {
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

    private void OnMove(InputAction.CallbackContext context)
    {
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
}
  