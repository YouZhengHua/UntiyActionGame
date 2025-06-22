using System;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public struct PlayerAnimationName
    {
        public const string Move = "Move";
        public const string Jump = "Jump";
        public const string Attack = "Attack";
        public const string AttackCombo = "AttackCombo";
        public const string InAir = "InAir";
    }
    public class PlayerManager : MonoBehaviour, IPlayerManager
    {
        public Vector2 MovementInput { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public Animator CharacterAnimator { get; private set; }

        [field:SerializeField] public float MoveSpeed { get; private set; }
        [SerializeField] private InputActionReference moveInput;
        [field:SerializeField] public float JumpForce { get; private set; }
        [SerializeField] private InputActionReference jumpInput;
        [SerializeField] private InputActionReference attackInput;

        private Sensor _groundSensor;
        public bool IsGrounded => _groundSensor == null || _groundSensor.HaveTarget;
        private bool _canJump = true;

        private bool _isDead = false;
        private bool _inAttack = false;


        private BaseStateMachine _stateMachine;

        /// <summary>
        /// 待機
        /// </summary>
        public IState Idle { get; private set; }

        /// <summary>
        /// 移動
        /// </summary>
        public IState Move { get; private set; }

        /// <summary>
        /// 跳躍
        /// </summary>
        public IState Jump { get; private set; }

        /// <summary>
        /// 攻擊
        /// </summary>
        public IState Attack { get; private set; }

        /// <summary>
        /// 死亡
        /// </summary>
        public IState Dead { get; private set; }

        /// <summary>
        /// 墜落
        /// </summary>
        public IState fail { get; private set; }

        private void Awake()
        {
            Rigidbody = this.GetComponent<Rigidbody2D>();
            CharacterAnimator = this.GetComponentInChildren<Animator>();
            _groundSensor = this.transform.Find("GroundSensor").GetComponent<Sensor>();

            _stateMachine = new BaseStateMachine();
            Idle = new PlayerStateIdle(this, _stateMachine, jumpInput, attackInput);
            Move = new PlayerStateMove(this, _stateMachine, jumpInput, attackInput);
            Jump = new PlayerStateJump(this, _stateMachine);
            Attack = new PlayerStateAttack(this, _stateMachine, attackInput);
            Dead = new PlayerStateDead(this, _stateMachine);
            fail = new PlayerStateFail(this, _stateMachine);

            _stateMachine.SetDefaultState(Idle);
        }

        private void Start()
        {
            moveInput.action.Enable();
            moveInput.action.performed += OnMove;
            moveInput.action.canceled += OnMove;
        }

        private void Update()
        {
            _stateMachine.UpdateState();
        }

        private void OnDestroy()
        {
            moveInput.action.Disable();
            moveInput.action.performed -= OnMove;
            moveInput.action.canceled -= OnMove;
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdateState();
        }

        /// <summary>
        /// 當玩家移動時執行。
        /// </summary>
        /// <param name="context"></param>
        private void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            MovementInput = new Vector2(MovementInput.x, 0f);
        }

        public void SetMoveSpeed()
        {
            this.Rigidbody.AddForce(this.MovementInput * this.MoveSpeed, ForceMode2D.Impulse);

            // 設定速度上限
            if (Mathf.Abs(this.Rigidbody.linearVelocity.x) > this.MoveSpeed)
            {
                this.Rigidbody.linearVelocity = new Vector2((this.Rigidbody.linearVelocity.x > 0f ? 1f : -1f) * this.MoveSpeed,
                    this.Rigidbody.linearVelocity.y);
            }
            
            this.CharacterAnimator.SetFloat(PlayerAnimationName.Move, Mathf.Abs(this.MovementInput.x));
            if (this.MovementInput.x > 0f)
            {
                this.transform.localEulerAngles = (new Vector3(0f, 0f, 0f));
            }
            else if (this.MovementInput.x < 0f)
            {
                this.transform.localEulerAngles = (new Vector3(0f, 180f, 0f));
            }
        }

        public Action JumpEnter;
        /// <summary>
        /// 當玩家跳躍時執行。
        /// </summary>
        /// <param name="context"></param>
        private void OnJump(InputAction.CallbackContext context)
        {
            if(JumpEnter != null)
                JumpEnter();
            // 無法跳躍、死亡、無接觸地板時不執行。
            if (!_canJump || _inAttack || _isDead || !_groundSensor.HaveTarget || !context.action.WasPressedThisFrame())
                return;

            _canJump = false;
            CharacterAnimator.SetTrigger(PlayerAnimationName.Jump);
        }

        public void OnJumpEnd()
        {
        }

        public void OnDead()
        {
        }

        public void OnAttackEnd()
        {
            _stateMachine.SwitchState(Idle);
        }
    }
}