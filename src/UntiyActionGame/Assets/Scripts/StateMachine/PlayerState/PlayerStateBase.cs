using Player;

namespace StateMachine
{
    public class PlayerStateBase : BaseState
    {
        protected IPlayerManager _playerManager;
        public PlayerStateBase(IPlayerManager player, IStateMachine stateMachine) : base(stateMachine)
        {
            _playerManager = player;
        }
    }
}