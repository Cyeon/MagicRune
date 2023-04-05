using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [Header("Portal Prefab")]
    [SerializeField] private Portal _attackPortal;
    [SerializeField] private List<Portal> _eventPortals = new List<Portal>();
    private List<Portal> _spawnPortals = new List<Portal>();
    private Portal _selectPortal;
    public Portal SelectedPortal => _selectPortal;

    [Header("Portal Position")]
    [SerializeField] private Vector2 _onePortalPosition;
    [SerializeField] private Vector2[] _twoPortalPositions = new Vector2[2];
    [SerializeField] private Vector2[] _threePortalPositions = new Vector2[3];

    public void SpawnPortal(StageType type)
    {
        switch(type)
        {
            case StageType.Attack:
                AttackPortal atkPortal = _attackPortal as AttackPortal;
                int count = Mathf.Clamp(atkPortal.GetAttackEnemyCount(), 1, 3);

                for(int i = 0; i < count; i++)
                {
                    Enemy enemy = atkPortal.GetAttackEnemy();
                    atkPortal = PoolManager.Instance.Pop(enemy.gameObject, transform).GetComponent<AttackPortal>();

                    switch(count)
                    {
                        case 1:
                            atkPortal.Init(_onePortalPosition, enemy);
                            break;

                        case 2:
                            atkPortal.Init(_twoPortalPositions[i], enemy);
                            break;

                        case 3:
                            atkPortal.Init(_threePortalPositions[i], enemy);
                            break;
                    }
                    _spawnPortals.Add(atkPortal);
                }
                break;

            case StageType.Event:
                Portal portal = PoolManager.Instance.Pop(GetEventPortal().gameObject, transform).GetComponent<Portal>();
                portal.Init(_onePortalPosition);
                _spawnPortals.Add(portal);
                break;
        }
    }

    public void ResetPortal()
    {
        _spawnPortals.ForEach(x => PoolManager.Instance.Push(x.GetComponent<Poolable>()));
        _spawnPortals.Clear();
    }

    public Portal GetEventPortal()
    {
        return _eventPortals.GetRandom();
    }
}
