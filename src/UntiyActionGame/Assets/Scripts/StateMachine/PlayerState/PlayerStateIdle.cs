using Player;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace StateMachine
{
    public class PlayerStateIdle : PlayerStateGround
    {
        public PlayerStateIdle(IPlayerManager player, IStateMachine stateMachine, InputActionReference jump, InputActionReference attack) : base(player, stateMachine, jump, attack)
        {
            
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