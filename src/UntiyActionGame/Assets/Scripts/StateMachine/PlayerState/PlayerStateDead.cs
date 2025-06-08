using Player;

namespace StateMachine
{
    public class PlayerStateDead : PlayerStateBase
    {
        public PlayerStateDead(IPlayerManager player, IStateMachine stateMachine) : base(player, stateMachine)
        {
            
        }
    }
}