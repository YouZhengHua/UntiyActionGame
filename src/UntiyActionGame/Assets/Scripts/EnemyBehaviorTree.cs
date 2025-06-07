using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : Enemy
{
    private BehaviorNode _rootNode;
    private Transform _target;
    private Vector3 _patrolPoint;
    private float _patrolRadius = 5f;
    private float _attackRange = 2f;
    private float _idleTime = 2f;
    private float _currentIdleTime = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        SetupBehaviorTree();
    }

    private void SetupBehaviorTree()
    {
        var isDead = new CheckDeadNode(this);
        var idle = new IdleNode(this);
        var patrol = new PatrolNode(this);
        var checkTarget = new CheckTargetNode(this);
        var followTarget = new FollowTargetNode(this);
        var attack = new AttackNode(this);

        _rootNode = new Selector(new List<BehaviorNode>
        {
            isDead,
            new Sequence(new List<BehaviorNode>
            {
                checkTarget,
                new Selector(new List<BehaviorNode>
                {
                    attack,
                    followTarget
                })
            }),
            new Selector(new List<BehaviorNode>
            {
                idle,
                patrol
            })
        });
    }

    private void Update()
    {
        _rootNode.Execute();
    }

    // 具体行为节点实现
    private class IdleNode : BehaviorNode
    {
        private EnemyBehavior _enemy;

        public IdleNode(EnemyBehavior enemy)
        {
            _enemy = enemy;
        }

        public override Status Execute()
        {
            if (_enemy._currentIdleTime <= 0)
            {
                _enemy._currentIdleTime = _enemy._idleTime;
                return Status.Failure;
            }

            _enemy._currentIdleTime -= Time.deltaTime;
            _enemy.Move(Vector2.zero);
            return Status.Success;
        }
    }

    private class PatrolNode : BehaviorNode
    {
        private EnemyBehavior _enemy;

        public PatrolNode(EnemyBehavior enemy)
        {
            _enemy = enemy;
        }

        public override Status Execute()
        {
            if (_enemy._patrolPoint == Vector3.zero)
            {
                _enemy._patrolPoint = _enemy.transform.position + Random.insideUnitSphere * _enemy._patrolRadius;
                _enemy._patrolPoint.y = _enemy.transform.position.y;
            }

            Vector2 direction = (_enemy._patrolPoint - _enemy.transform.position).normalized;
            _enemy.Move(direction);
            _enemy._rb.linearVelocity = direction * _enemy._speed;

            if (Vector3.Distance(_enemy.transform.position, _enemy._patrolPoint) < 0.1f)
            {
                _enemy._patrolPoint = Vector3.zero;
            }

            return Status.Success;
        }
    }

    private class CheckTargetNode : BehaviorNode
    {
        private EnemyBehavior _enemy;

        public CheckTargetNode(EnemyBehavior enemy)
        {
            _enemy = enemy;
        }

        public override Status Execute()
        {
            Collider2D target = Physics2D.OverlapCircle(_enemy.transform.position, 
                _enemy.findTargetRange, _enemy.findTargetLayer);

            if (target != null)
            {
                _enemy._target = target.transform;
                return Status.Success;
            }

            _enemy._target = null;
            return Status.Failure;
        }
    }

    private class FollowTargetNode : BehaviorNode
    {
        private EnemyBehavior _enemy;

        public FollowTargetNode(EnemyBehavior enemy)
        {
            _enemy = enemy;
        }

        public override Status Execute()
        {
            if (_enemy._target == null) return Status.Failure;

            Vector2 direction = (_enemy._target.position - _enemy.transform.position).normalized;
            _enemy.Move(direction);
            _enemy._rb.linearVelocity = direction * _enemy._speed;

            return Status.Success;
        }
    }

    private class AttackNode : BehaviorNode
    {
        private EnemyBehavior _enemy;

        public AttackNode(EnemyBehavior enemy)
        {
            _enemy = enemy;
        }

        public override Status Execute()
        {
            if (_enemy._target == null) return Status.Failure;

            if (Vector2.Distance(_enemy._target.position, _enemy.transform.position) <= _enemy._attackRange)
            {
                _enemy._rb.linearVelocity = Vector2.zero;
                _enemy._animator.SetTrigger("Attack");
                return Status.Success;
            }

            return Status.Failure;
        }
    }

    private class CheckDeadNode : BehaviorNode
    {
        private EnemyBehavior _enemy;

        public CheckDeadNode(EnemyBehavior enemy)
        {
            _enemy = enemy;
        }

        public override Status Execute()
        {
            if (_enemy._isDead)
            {
                _enemy._rb.linearVelocity = Vector2.zero;
                _enemy._animator.SetTrigger("Death");
                return Status.Success;
            }
            return Status.Failure;
        }
    }
}