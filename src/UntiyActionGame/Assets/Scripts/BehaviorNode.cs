/// <summary>
/// 行為節點(抽象類別)
/// </summary>
public abstract class BehaviorNode
{
    /// <summary>
    /// 節點狀態
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 執行成功
        /// </summary>
        Success,
        /// <summary>
        /// 執行失敗
        /// </summary>
        Failure,
        /// <summary>
        /// 執行中
        /// </summary>
        Running
    }

    /// <summary>
    /// 執行節點
    /// </summary>
    /// <returns></returns>
    public abstract Status Execute();
}