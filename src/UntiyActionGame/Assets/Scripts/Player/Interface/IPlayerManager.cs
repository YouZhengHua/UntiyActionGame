using StateMachine;
using UnityEngine;

namespace Player
{
    public interface IPlayerManager
    {
        public Vector2 MovementInput { get; }
        public Rigidbody2D Rigidbody { get; }
        public float MoveSpeed { get; }
        public float JumpForce { get; }
        public Animator CharacterAnimator { get; }
        /// <summary>
        /// 待機
        /// </summary>
        public IState Idle { get; }
        /// <summary>
        /// 移動
        /// </summary>
        public IState Move { get; }
        /// <summary>
        /// 跳躍
        /// </summary>
        public IState Jump { get; }
        /// <summary>
        /// 攻擊
        /// </summary>
        public IState Attack { get; }
        /// <summary>
        /// 死亡
        /// </summary>
        public IState Dead { get; }
        /// <summary>
        /// 墜落
        /// </summary>
        public IState fail { get; }
        
        public bool IsGrounded { get; }
        
        public void SetLocalEulerAngles(Vector3 localEulerAngles);
    }
}