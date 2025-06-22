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
            
            _playerManager.SetMoveSpeed();

            if (_playerManager.MovementInput.magnitude < 0.01f)
            {
                _stateMachine.SwitchState(_playerManager.Idle);
            }
        }
    }
}