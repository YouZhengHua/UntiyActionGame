using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _movementInput;
    private Rigidbody2D _rb;
    private Animator _characterAnimator;
    
    private static readonly string AnimatorMoveName = "Move";

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private InputActionReference attackInput;

    private readonly string _animatorNameInAir = "InAir";
    private readonly string _animatorNameJump = "Jump";
    private readonly string _animatorNameAttack = "Attack";
    private readonly string _animatorNameAttackCombo = "AttackCombo";
    private Sensor _groundSensor;
    private bool _canJump = true;
    
    private bool _isDead = false;
    private bool _inAttack = false;

    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _characterAnimator = this.GetComponentInChildren<Animator>();
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
        
        attackInput.action.Enable();
        attackInput.action.performed += OnAttack;
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
        _characterAnimator.SetBool(_animatorNameInAir, !_groundSensor.HaveTarget);
        // 如果在地板進行攻擊，直接強迫停下來。
        if (_inAttack && _groundSensor.HaveTarget)
        {
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }
        else if (_movementInput.magnitude > 0f)
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
            _rb.linearVelocity = new Vector2(Mathf.Lerp(_rb.linearVelocity.x, 0f, 10f * Time.deltaTime), _rb.linearVelocity.y);
        }

        if (_canJump == false && _groundSensor.HaveTarget && _characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            _canJump = true;
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
        if (_inAttack)
        {
            _movementInput = Vector2.zero;
        }
        _characterAnimator.SetFloat(AnimatorMoveName, Mathf.Abs(_movementInput.x));
        if (_movementInput.x > 0f)
        {
            this.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (_movementInput.x < 0f)
        {
            this.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    /// <summary>
    /// 當玩家跳躍時執行。
    /// </summary>
    /// <param name="context"></param>
    private void OnJump(InputAction.CallbackContext context)
    {
        // 無法跳躍、死亡、無接觸地板時不執行。
        if (!_canJump || _inAttack || _isDead || !_groundSensor.HaveTarget || !context.action.WasPressedThisFrame())
            return;
        
        _canJump = false;
        _characterAnimator.SetTrigger(_animatorNameJump);
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    private void OnAttack(InputAction.CallbackContext context)
    {
        // 死亡時不執行
        if (_isDead || !context.action.WasPressedThisFrame())
            return;

        if (_inAttack == false)
        {
            _inAttack = true;
            _characterAnimator.SetTrigger(_animatorNameAttack);
        }
        else
        {
            _characterAnimator.SetTrigger(_animatorNameAttackCombo);
        }
    }

    public void OnJumpEnd()
    {
        _canJump = true;
    }

    public void OnDead()
    {
        _isDead = true;
    }
    
    public void OnAttackEnd()
    {
        _inAttack = false;
        _characterAnimator.ResetTrigger(_animatorNameAttack);
        _characterAnimator.ResetTrigger(_animatorNameAttackCombo);
    }
}
  