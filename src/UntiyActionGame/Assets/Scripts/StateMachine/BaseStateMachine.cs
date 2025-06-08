namespace StateMachine
{
    public class BaseStateMachine : IStateMachine
    {
        public IState CurrentState { get; protected set; }
        
        public void SetDefaultState(IState state)
        {
            CurrentState = state;
            CurrentState.Enter();
        }

        public void UpdateState()
        {
            CurrentState.Update();
        }
        
        public void FixedUpdateState()
        {
            CurrentState.FixedUpdate();
        }

        public void SwitchState(IState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}