using System.Collections.Generic;

/// <summary>
/// 序列節點
/// AND 邏輯，所有節點都要成功才代表行為成功。
/// </summary>
public class Sequence : BehaviorNode
{
    protected List<BehaviorNode> nodes = new List<BehaviorNode>();

    public Sequence(List<BehaviorNode> nodes)
    {
        this.nodes = nodes;
    }

    public override Status Execute()
    {
        foreach (var node in nodes)
        {
            switch (node.Execute())
            {
                case Status.Failure:
                    return Status.Failure;
                case Status.Running:
                    return Status.Running;
                case Status.Success:
                    continue;
            }
        }
        return Status.Success;
    }
}

/// <summary>
/// 選擇節點
/// OR 邏輯，只要有任何一個節點成功，就完成行為。
/// </summary>
public class Selector : BehaviorNode
{
    protected List<BehaviorNode> nodes = new List<BehaviorNode>();

    public Selector(List<BehaviorNode> nodes)
    {
        this.nodes = nodes;
    }

    public override Status Execute()
    {
        foreach (var node in nodes)
        {
            switch (node.Execute())
            {
                case Status.Failure:
                    continue;
                case Status.Running:
                    return Status.Running;
                case Status.Success:
                    return Status.Success;
            }
        }
        return Status.Failure;
    }
}