using Player;
using UnityEngine;

namespace StateMachine
{
    public class PlayerStateJump : PlayerStateBase
    {
        private readonly float _delayTime = 0.1f;
        private float _timer = 0f;
        
        public PlayerStateJump(IPlayerManager player, IStateMachine stateMachine) : base(player, stateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            _playerManager.CharacterAnimator.SetBool(PlayerAnimationName.InAir, true);
            _playerManager.CharacterAnimator.SetTrigger(PlayerAnimationName.Jump);
            _playerManager.Rigidbody.AddForce(Vector2.up * _playerManager.JumpForce, ForceMode2D.Impulse);
            _timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            _timer += Time.deltaTime;
            if (_playerManager.IsGrounded && _timer >= _delayTime)
            {
                _stateMachine.SwitchState(_playerManager.Idle);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            _playerManager.SetMoveSpeed();
        }
    }
}