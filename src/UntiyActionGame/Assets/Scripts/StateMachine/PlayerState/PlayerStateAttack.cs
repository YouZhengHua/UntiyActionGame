using Player;

namespace StateMachine
{
    public class PlayerStateAttack : PlayerStateBase
    {
        public PlayerStateAttack(IPlayerManager player, IStateMachine stateMachine) : base(player, stateMachine)
        {
            
        }
    }
}