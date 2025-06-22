using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public class PlayerStateAttack : PlayerStateBase
    {
        private readonly InputActionReference _attackInput;
        
        public PlayerStateAttack(IPlayerManager player, IStateMachine stateMachine, InputActionReference attackInput) : base(player, stateMachine)
        {
            _attackInput = attackInput;
        }

        public override void Enter()
        {
            base.Enter();
            
            _attackInput.action.Enable();
            _attackInput.action.started += OnAttack;
            
            _playerManager.CharacterAnimator.SetTrigger(PlayerAnimationName.Attack);
            _playerManager.Rigidbody.linearVelocity = new Vector2(0f, _playerManager.Rigidbody.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
            _attackInput.action.started -= OnAttack;
            _attackInput.action.Disable();
            
            _playerManager.CharacterAnimator.ResetTrigger(PlayerAnimationName.Attack);
            _playerManager.CharacterAnimator.ResetTrigger(PlayerAnimationName.AttackCombo);
        } 
        
        private void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.action.IsPressed())
                return;
            
            _playerManager.CharacterAnimator.SetTrigger(PlayerAnimationName.AttackCombo);
        }
    }
}