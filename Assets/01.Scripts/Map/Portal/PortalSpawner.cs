using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] private Portal _attackPortal;
    [SerializeField] private List<Portal> _eventPortals = new List<Portal>();
    private Portal _selectPortal;
    public Portal SelectedPortal => _selectPortal;

    public void SpawnPortal(StageType type)
    {
        int count = 1;
        switch(type)
        {
            case StageType.Attack:
                break;
        }
    }

    public Portal GetEventPortal()
    {
        return _eventPortals.GetRandom();
    }
}
