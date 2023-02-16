using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;

public class MagicContent : MonoBehaviour
{
    [SerializeField]
    private MagicCircle _magicCircle;
    [SerializeField]
    private float _startAxis = 90;
    [SerializeField]
    private float _distance = 300;
    [SerializeField]
    private Vector2 _offset;
    [SerializeField]
    private float _rotateSpeed = 10f;

    private float _baseRotateSpeed;
    private float _baseDistance;

    private float _axis = 90;

    private Dictionary<RuneType, List<GameObject>> _effectDict = new Dictionary<RuneType, List<GameObject>>();
    private Dictionary<bool, int> _attackEffectDict = new Dictionary<bool, int>();

    #region Bezier Parameta
    [SerializeField]
    private BezierMissile _attackEffect;
    [SerializeField]
    private Transform _attackEffectParent;
    [SerializeField]
    private RectTransform _startPos;
    [SerializeField]
    private RectTransform _enemyPos;
    [SerializeField]
    private RectTransform _playerPos;

    [SerializeField, Range(0f, 1f)]
    private float _bezierRate = 0.5f;
    #endregion

    private void Start()
    {
        _axis = _startAxis;
        _baseDistance = _distance;
        _baseRotateSpeed = _rotateSpeed;

    }

    private void Update()
    {
        if (_effectDict.ContainsKey(RuneType.Assist) == true)
        {
            _axis = (_axis + Time.deltaTime) % 360f;

            float angle = 2 * Mathf.PI / _effectDict[RuneType.Assist].Count;

            for (int i = 0; i < _effectDict[RuneType.Assist].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (_axis * _rotateSpeed * Mathf.Deg2Rad)) * _distance;
                float width = Mathf.Cos(angle * i + (_axis * _rotateSpeed * Mathf.Deg2Rad)) * _distance;
                _effectDict[RuneType.Assist][i].transform.localPosition = new Vector3(width + _offset.x, height + _offset.y, -20);
            }
        }
    }

    public void AddEffect(CardSO card, bool isMain)
    {
        if (card.RuneEffect == null) return;

        GameObject g = Instantiate(card.RuneEffect, this.transform);
        g.transform.SetParent(this.transform);
        g.transform.position = Vector3.zero;

        if (isMain == true)
        {
            Vector3 scale = g.transform.localScale;
            scale *= 2f;
            g.transform.localScale = scale;
            if (_effectDict.ContainsKey(RuneType.Main) == true)
            {
                _effectDict[RuneType.Main].Add(g);
            }
            else
            {
                _effectDict.Add(RuneType.Main, new List<GameObject> { g });
            }
        }
        else
        {
            if (_effectDict.ContainsKey(RuneType.Assist) == true)
            {
                _effectDict[RuneType.Assist].Add(g);
            }
            else
            {
                _effectDict.Add(RuneType.Assist, new List<GameObject> { g });
            }
        }

        Sort();

        if (isMain)
        {
            foreach (var c in card.MainRune.EffectDescription)
            {
                if (_attackEffectDict.ContainsKey(c.IsEnemy))
                {
                    _attackEffectDict[c.IsEnemy] += 1;
                }
                else
                {
                    _attackEffectDict.Add(c.IsEnemy, 1);
                }
            }
        }
        else
        {
            foreach (var c in card.AssistRune.EffectDescription)
            {
                if (_attackEffectDict.ContainsKey(c.IsEnemy))
                {
                    _attackEffectDict[c.IsEnemy] += 1;
                }
                else
                {
                    _attackEffectDict.Add(c.IsEnemy, 1);
                }
            }
        }
    }

    public void Clear()
    {
        if (_effectDict.ContainsKey(RuneType.Main) == true)
        {
            foreach (var e in _effectDict[RuneType.Main])
            {
                Destroy(e.gameObject);
            }
        }

        if (_effectDict.ContainsKey(RuneType.Assist) == true)
        {
            foreach (var e in _effectDict[RuneType.Assist])
            {
                Destroy(e.gameObject);
            }
        }

        _effectDict.Clear();
    }

    public void SetActive(bool value)
    {
        foreach(var e in _effectDict)
        {
            foreach(var e2 in e.Value)
            {
                e2.SetActive(value);
            }
        }
    }

    private void Sort()
    {
        if (_effectDict.ContainsKey(RuneType.Main) == true)
        {
            _effectDict[RuneType.Main][0].transform.localPosition = new Vector3(_offset.x, _offset.y, -50);
        }

        if (_effectDict.ContainsKey(RuneType.Assist) == true)
        {
            float angle = 2 * Mathf.PI / _effectDict[RuneType.Assist].Count;

            for (int i = 0; i < _effectDict[RuneType.Assist].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * _distance;
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * _distance;
                _effectDict[RuneType.Assist][i].transform.localPosition = new Vector3(width + _offset.x, height + _offset.y, -20);
            }
        }
    }

    public void AttackAnimation()
    {
        if (_effectDict.ContainsKey(RuneType.Main) == false) return;

        Sequence seq = DOTween.Sequence();
        if(_effectDict.ContainsKey(RuneType.Assist) == false)
        {
            seq.AppendInterval(0.8f);
            seq.AppendCallback(() =>
            {
                Clear();
                _distance = _baseDistance;
                _rotateSpeed = _baseRotateSpeed;
                StartCoroutine(CreateAttackEffect(_startPos, _enemyPos, _playerPos));
            });
        }
        else
        {
            seq.Append(DOTween.To(() => _distance, x => _distance = x, _baseDistance * 1.3f, 0.2f));
            seq.Join(DOTween.To(() => _rotateSpeed, x => _rotateSpeed = x, _baseRotateSpeed * 1.01f, 0.7f));
            seq.Join(transform.DOLocalMoveY(-800, 0.7f).SetRelative());
            //seq.AppendInterval(0.2f);
            seq.Append(DOTween.To(() => _distance, x => _distance = x, 0, 0.2f));

            seq.AppendCallback(() => Clear());
            seq.Append(transform.DOLocalMoveY(500, 0f).SetRelative());
            seq.AppendCallback(() => StartCoroutine(CreateAttackEffect(_startPos, _enemyPos, _playerPos)));

            //seq.AppendCallback(() =>
            //{
            //    _effectDict[RuneType.Main]l;
            //});
            seq.AppendCallback(() =>
            {
                _distance = _baseDistance;
                _rotateSpeed = _baseRotateSpeed;
                transform.DOLocalMoveY(300, 0f).SetRelative();
            });
        }
    }

    private IEnumerator CreateAttackEffect(RectTransform startPos, RectTransform enemyPos, RectTransform playerPos)
    {
        if (_magicCircle.EffectDict.ContainsKey(EffectType.Status))
        {
            foreach (var d in _magicCircle.EffectDict[EffectType.Status])
            {
                Unit target = d.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.Init(startPos, _enemyPos, 3, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Status, target, d));

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

        if (_magicCircle.EffectDict.ContainsKey(EffectType.Defence))
        {
            foreach (var d in _magicCircle.EffectDict[EffectType.Defence])
            {
                Unit target = d.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.Init(startPos, _playerPos, 3, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Defence, target, d));

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

        if (_magicCircle.EffectDict.ContainsKey(EffectType.Attack))
        {
            foreach (var d in _magicCircle.EffectDict[EffectType.Attack])
            {
                Unit target = d.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.Init(startPos, _enemyPos, 3, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Attack, target, d));

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

        if (_magicCircle.EffectDict.ContainsKey(EffectType.Draw))
        {
            foreach (var d in _magicCircle.EffectDict[EffectType.Draw])
            {
                Unit target = d.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.Init(startPos, _playerPos, 3, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Draw, target, d));

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

        if (_magicCircle.EffectDict.ContainsKey(EffectType.Destroy))
        {
            //foreach (var d in _magicCircle.EffectDict[EffectType.Destroy])
            //{
            //    Unit target = d.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;

            //    BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
            //    go.Init(startPos, _enemyPos, 3, 1300, 300, () => _magicCircle.AttackEffectFunction(EffectType.Destroy, target, d));

            //    yield return new WaitForSeconds(0.1f);
            //}

            for (int i = 0; i < _magicCircle.EffectDict[EffectType.Destroy].Count; i++)
            {
                Unit target = _magicCircle.EffectDict[EffectType.Destroy][i].IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                //if(i == _magicCircle.EffectDict[EffectType.Destroy].Count - 1)
                //{
                //    go.Init(startPos, _enemyPos, 3, 1300, 300, () =>
                //    {
                //        _magicCircle.AttackEffectFunction(EffectType.Destroy, target, _magicCircle.EffectDict[EffectType.Destroy][i]);
                //        _magicCircle.RuneDict.Clear();
                //        _magicCircle.EffectDict.Clear();
                //    });
                //}
                //else
                //{
                //    go.Init(startPos, _enemyPos, 3, 1300, 300, () => _magicCircle.AttackEffectFunction(EffectType.Destroy, target, _magicCircle.EffectDict[EffectType.Destroy][i]));
                //}
                go.Init(startPos, _enemyPos, 3, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Destroy, target, _magicCircle.EffectDict[EffectType.Destroy][i]));
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(2f);
        foreach (var item in _magicCircle.RuneDict)
        {
            foreach (Card card in item.Value)
            {
                if (card.IsFront == false)
                {
                    card.IsFront = true;
                }
                _magicCircle.CardCollector._restCards.Add(card);
            }
        }
        _magicCircle.CardCollector.UIUpdate();

        foreach (var rList in _magicCircle.RuneDict)
        {
            foreach (var r in rList.Value)
            {
                if (r.Rune == null)
                {
                    Destroy(r.gameObject);
                }
                else
                {
                    r.gameObject.SetActive(false);
                    r.transform.SetParent(_magicCircle.CardCollector.transform);
                    r.SetIsEquip(false);
                }
            }
        }

        _magicCircle.RuneDict.Clear();
        _magicCircle.EffectDict.Clear();
        _magicCircle.CardCollector.UpdateCardOutline();
    }
}
