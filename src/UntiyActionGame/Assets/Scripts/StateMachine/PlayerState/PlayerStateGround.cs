using Player;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public class PlayerStateGround : PlayerStateBase
    {
        protected InputActionReference _jump, _attack; 
        public PlayerStateGround(IPlayerManager player, IStateMachine stateMachine, InputActionReference jump, InputActionReference attack) : base(player, stateMachine)
        {
            _jump = jump;
            _attack = attack;
        }

        public override void Enter()
        {
            base.Enter();
            _jump.action.Enable();
            _attack.action.Enable();
            
            _jump.action.started += OnJump;
            
            _attack.action.started += OnAttack;
            
            _playerManager.CharacterAnimator.SetBool(PlayerAnimationName.InAir, false);
        }
        
        public override void Exit()
        {
            base.Exit();
            _jump.action.Disable();
            _attack.action.Disable();
            
            _jump.action.started -= OnJump;
            
            _attack.action.started -= OnAttack;
        }

        protected virtual void OnJump(InputAction.CallbackContext context)
        {
            if (!_playerManager.IsGrounded) return;
            _stateMachine.SwitchState(_playerManager.Jump);
        }

        protected virtual void OnAttack(InputAction.CallbackContext context)
        {
            if (!_playerManager.IsGrounded) return;
            _stateMachine.SwitchState(_playerManager.Attack);
        }
    }
}