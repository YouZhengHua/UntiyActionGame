using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform? target = null;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("FlowTarget target is null.", this);
        }

    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            this.transform.position = target.position;
        }
    }
}
