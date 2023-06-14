using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEvent : MonoBehaviour
{
    protected Unit _unit;
    protected Status _status;

    public virtual void Invoke()
    {
        _unit = transform.parent.parent.GetComponent<Unit>();
        _status = GetComponent<Status>();
    }
}
