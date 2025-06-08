using Player;
using UnityEngine;

namespace StateMachine
{
    public class PlayerStateJump : PlayerStateBase
    {
        public PlayerStateJump(IPlayerManager player, IStateMachine stateMachine) : base(player, stateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            _playerManager.CharacterAnimator.SetBool(PlayerAnimationName.InAir, true);
            _playerManager.CharacterAnimator.SetTrigger(PlayerAnimationName.Jump);
            _playerManager.Rigidbody.AddForce(Vector2.up * _playerManager.JumpForce, ForceMode2D.Impulse);
        }

        public override void Update()
        {
            base.Update();
            
            if (_playerManager.IsGrounded)
            {
                _stateMachine.SwitchState(_playerManager.Idle);
            }
        }
    }
}