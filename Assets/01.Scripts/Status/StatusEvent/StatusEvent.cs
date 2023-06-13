using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEvent : MonoBehaviour
{
    protected Unit _unit;
    protected Status _status;

    private void Awake()
    {
        _unit = Managers.GetPlayer().GetComponent<Unit>();
        _status = GetComponent<Status>();
    }

    public abstract void Invoke();
}
