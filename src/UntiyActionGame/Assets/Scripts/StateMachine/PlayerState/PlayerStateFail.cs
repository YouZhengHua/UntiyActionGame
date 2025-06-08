using Player;

namespace StateMachine
{
    public class PlayerStateFail : PlayerStateBase
    {
        public PlayerStateFail(IPlayerManager player, IStateMachine stateMachine) : base(player, stateMachine)
        {
            
        }
    }
}