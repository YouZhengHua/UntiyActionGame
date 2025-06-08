using UnityEngine;

namespace StateMachine
{
    public class BaseState : IState
    {
        protected IStateMachine _stateMachine;

        public BaseState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void FixedUpdate()
        {
        }
    }
}