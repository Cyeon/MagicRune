using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanel<T1, T2> : MonoBehaviour where T1 : MonoBehaviour where T2 : class
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    protected Dial<T1, T2> _dial;

    [SerializeField]
    protected float _inDistance;
    [SerializeField]
    protected float _outDistance;

    protected virtual void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _inDistance);
        Gizmos.DrawWireSphere(this.transform.position, _outDistance);
        Gizmos.color = Color.white;
    }
#endif
}
