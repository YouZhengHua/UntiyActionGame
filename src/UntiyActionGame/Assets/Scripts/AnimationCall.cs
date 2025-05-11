using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class AnimationCall : MonoBehaviour
{
    private Animator _animator = new Animator();
    [SerializeField] private UnityEvent executeEvent = new UnityEvent();
    [SerializeField] private string animationName = string.Empty;
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    public void Execute()
    {
        if (!string.IsNullOrEmpty(animationName) && !_animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            return;
        #if UNITY_EDITOR
        Debug.Log("Animation Call", this);
        #endif
        executeEvent?.Invoke();
    }
}
