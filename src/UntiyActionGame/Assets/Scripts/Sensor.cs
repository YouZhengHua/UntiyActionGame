using System;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] private float range = 1f;
    [SerializeField] private LayerMask targetLayerMask;
    private bool _haveTarget = false;
    private Collider2D[] _colliders;
    public bool HaveTarget => _haveTarget;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        _colliders = Physics2D.OverlapCircleAll(transform.position, range, targetLayerMask);
        _haveTarget = _colliders.Length > 0;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    #endif
}


