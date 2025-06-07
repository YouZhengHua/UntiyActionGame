using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float _speed = 3f;
    [SerializeField] protected int _health = 1;
    [SerializeField] protected int _damage = 1;
    
    protected bool _isDead = false;

    protected Rigidbody2D _rb;
    
    protected Animator _animator;

    [SerializeField] protected LayerMask findTargetLayer;
    [SerializeField] protected float findTargetRange = 10f;

    
    
    protected void Move(Vector2 direction)
    {
        _animator.SetFloat("Move", Mathf.Abs(direction.x));
    }
}
