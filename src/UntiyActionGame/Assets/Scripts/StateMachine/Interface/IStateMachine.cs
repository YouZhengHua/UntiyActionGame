namespace StateMachine
{
    public interface IStateMachine
    {
        public IState CurrentState { get; }
        /// <summary>
        /// 設定初始狀態
        /// </summary>
        /// <param name="state">初始狀態</param>
        public void SetDefaultState(IState state);

        /// <summary>
        /// 更新狀態
        /// </summary>
        public void UpdateState();

        /// <summary>
        /// 切換狀態
        /// </summary>
        /// <param name="newState">新狀態</param>
        public void SwitchState(IState newState);
    }
}