using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [Header("Portal Prefab")]
    [SerializeField] private AttackPortal _attackPortal;
    [SerializeField] private AttackPortal _bossPortal;
    [SerializeField] private List<Portal> _eventPortals = new List<Portal>();
    private List<Portal> _spawnPortals = new List<Portal>();
    private Portal _selectPortal;
    public Portal SelectedPortal => _selectPortal;

    [Header("Portal Position")]
    [SerializeField] private Vector2 _onePortalPosition;
    [SerializeField] private Vector2[] _twoPortalPositions = new Vector2[2];
    [SerializeField] private Vector2[] _threePortalPositions = new Vector2[3];

    private bool _isSelect = false;
    public bool IsSelect => _isSelect;

    private void OnEnable()
    {
        DontDestroyOnLoad(this);
    }

    public void SpawnPortal(StageType type)
    {
        if (_spawnPortals.Count != 0)
        {
            Debug.LogWarning("이미 생성된 포탈이 있습니다.");
            return;
        }

        switch(type)
        {
            case StageType.Attack:
                int count = Mathf.Clamp(Managers.Map.CurrentChapter.GetEnemyCount(), 1, 4);

                for(int i = 0; i < count; i++)
                {
                    Enemy enemy = Managers.Map.CurrentChapter.GetEnemy();
                    AttackPortal atkPortal = Managers.Resource.Instantiate("Portal/" + _attackPortal.name, transform).GetComponent<AttackPortal>();
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
                Portal portal = Managers.Resource.Instantiate("Portal/" + GetEventPortal().name, transform).GetComponent<Portal>();
                portal.Init(_onePortalPosition);
                _spawnPortals.Add(portal);
                break;

            case StageType.Boss:
                AttackPortal bossPortal = Managers.Resource.Instantiate("Portal/" + _bossPortal.name, transform).GetComponent<AttackPortal>();
                bossPortal.Init(_onePortalPosition, Managers.Map.CurrentChapter.boss);
                _spawnPortals.Add(bossPortal);
                break;
        }
    }

    public void ResetPortal()
    {
        _spawnPortals.ForEach(x => x.PortalReset());
        _spawnPortals.ForEach(x => Managers.Resource.Destroy(x.gameObject));
        _spawnPortals.Clear();

        _isSelect = false;
    }

    public Portal GetEventPortal()
    {
        return _eventPortals.GetRandom();
    }

    public void SelectPortal(Portal portal)
    {
        _selectPortal = portal;
        _isSelect = true;

        if(portal is AttackPortal)
        {
            AttackPortal atk;
            foreach (var atkPortal in  _spawnPortals)
            {
                atk = atkPortal as AttackPortal;
                atk.PortalEnemy.isEnter = false;
            }

            atk = portal as AttackPortal;
            atk.PortalEnemy.isEnter = true;
        }

        Managers.Map.selectPortalSprite = portal.GetSprite();
    }
}
