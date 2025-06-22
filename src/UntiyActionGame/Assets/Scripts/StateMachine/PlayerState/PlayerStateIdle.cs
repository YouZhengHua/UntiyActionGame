using Player;
using UnityEngine.InputSystem;
using UnityEngine;

namespace StateMachine
{
    public class PlayerStateIdle : PlayerStateGround
    {
        public PlayerStateIdle(IPlayerManager player, IStateMachine stateMachine, InputActionReference jump, InputActionReference attack) : base(player, stateMachine, jump, attack)
        {

        }

        public override void Enter()
        {
            base.Enter();
            
            _playerManager.Rigidbody.linearVelocity = new Vector2(0f, _playerManager.Rigidbody.linearVelocity.y);
            _playerManager.CharacterAnimator.SetFloat(PlayerAnimationName.Move, 0f);
        }

        public override void Update()
        {
            base.Update();
            if (_playerManager.MovementInput.magnitude > 0f)
            {
                _stateMachine.SwitchState(_playerManager.Move);
            }
        }
    }
}