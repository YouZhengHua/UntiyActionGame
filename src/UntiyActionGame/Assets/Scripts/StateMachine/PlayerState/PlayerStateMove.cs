using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public class PlayerStateMove : PlayerStateGround
    {
        public PlayerStateMove(IPlayerManager player, IStateMachine stateMachine, InputActionReference jump, InputActionReference attack) : base(player, stateMachine, jump, attack)
        {
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            _playerManager.Rigidbody.AddForce(_playerManager.MovementInput * _playerManager.MoveSpeed, ForceMode2D.Impulse);

            // 設定速度上限
            if (Mathf.Abs(_playerManager.Rigidbody.linearVelocity.x) > _playerManager.MoveSpeed)
            {
                _playerManager.Rigidbody.linearVelocity = new Vector2((_playerManager.Rigidbody.linearVelocity.x > 0f ? 1f : -1f) * _playerManager.MoveSpeed,
                    _playerManager.Rigidbody.linearVelocity.y);
            }
            
            _playerManager.CharacterAnimator.SetFloat(PlayerAnimationName.Move, Mathf.Abs(_playerManager.MovementInput.x));
            if (_playerManager.MovementInput.x > 0f)
            {
                _playerManager.SetLocalEulerAngles(new Vector3(0f, 0f, 0f));
            }
            else if (_playerManager.MovementInput.x < 0f)
            {
                _playerManager.SetLocalEulerAngles(new Vector3(0f, 180f, 0f));
            }

            if (_playerManager.MovementInput.magnitude < 0.01f)
            {
                _stateMachine.SwitchState(_playerManager.Idle);
            }
        }
    }
}