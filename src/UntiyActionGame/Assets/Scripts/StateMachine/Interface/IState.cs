namespace StateMachine
{
    public interface IState
    {
        /// <summary>
        /// 進入狀態
        /// </summary>
        public void Enter();

        /// <summary>
        /// 更新狀態
        /// </summary>
        public void Update();
        
        /// <summary>
        /// 物理更新狀態
        /// </summary>
        public void FixedUpdate();

        /// <summary>
        /// 離開狀態
        /// </summary>
        public void Exit();
    }
}