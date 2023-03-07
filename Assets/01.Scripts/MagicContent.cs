using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;
using System;

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
    private Dictionary<RuneType, List<GameObject>> _effectDictTemp = new Dictionary<RuneType, List<GameObject>>();

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
    #endregion

    private void Start()
    {
        _axis = _startAxis;
        _baseDistance = _distance;
        _baseRotateSpeed = _rotateSpeed;

    }

    //private void Update()
    //{
    //    if (_effectDict.ContainsKey(RuneType.Assist) == true)
    //    {
    //        _axis = (_axis + Time.deltaTime) % 360f;

    //        float angle = 2 * Mathf.PI / _effectDict[RuneType.Assist].Count;

    //        for (int i = 0; i < _effectDict[RuneType.Assist].Count; i++)
    //        {
    //            float height = Mathf.Sin(angle * i + (_axis * _rotateSpeed * Mathf.Deg2Rad)) * _distance;
    //            float width = Mathf.Cos(angle * i + (_axis * _rotateSpeed * Mathf.Deg2Rad)) * _distance;
    //            _effectDict[RuneType.Assist][i].transform.localPosition = new Vector3(width + _offset.x, height + _offset.y, -20);
    //        }
    //    }
    //}

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

        //if (isMain)
        //{
        //    foreach (var c in card.MainRune.EffectDescription)
        //    {
        //        if (_attackEffectDict.ContainsKey(c.IsEnemy))
        //        {
        //            _attackEffectDict[c.IsEnemy] += 1;
        //        }
        //        else
        //        {
        //            _attackEffectDict.Add(c.IsEnemy, 1);
        //        }
        //    }
        //}
        //else
        //{
        //    foreach (var c in card.AssistRune.EffectDescription)
        //    {
        //        if (_attackEffectDict.ContainsKey(c.IsEnemy))
        //        {
        //            _attackEffectDict[c.IsEnemy] += 1;
        //        }
        //        else
        //        {
        //            _attackEffectDict.Add(c.IsEnemy, 1);
        //        }
        //    }
        //}
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
            //_effectDict[RuneType.Main][0].transform.localPosition = new Vector3(_offset.x, _offset.y, -50);
            _effectDict[RuneType.Main][0].GetComponent<RectTransform>().anchoredPosition3D
                = new Vector3(_magicCircle.RuneDict[RuneType.Main][0].GetComponent<RectTransform>().anchoredPosition.x, _magicCircle.RuneDict[RuneType.Main][0].GetComponent<RectTransform>().anchoredPosition.y, -50);
        }

        if (_effectDict.ContainsKey(RuneType.Assist) == true)
        {
            //float angle = 2 * Mathf.PI / _effectDict[RuneType.Assist].Count;

            //for (int i = 0; i < _effectDict[RuneType.Assist].Count; i++)
            //{
            //    float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * _distance;
            //    float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * _distance;
            //    _effectDict[RuneType.Assist][i].transform.localPosition = new Vector3(width + _offset.x, height + _offset.y, -20);
            //}

            for (int i = 0; i < _effectDict[RuneType.Assist].Count; i++)
            {
                _effectDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition3D
                    = new Vector3(_magicCircle.RuneDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition.x, _magicCircle.RuneDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition.y, -20);
            }
        }
    }

    public void AttackAnimation()
    {
        if (_effectDict.ContainsKey(RuneType.Main) == false) return;

        _effectDictTemp = new Dictionary<RuneType, List<GameObject>>(_effectDict);
        Sequence seq = DOTween.Sequence();
        if(_effectDict.ContainsKey(RuneType.Assist) == false)
        {
            seq.AppendInterval(0.8f);
            seq.AppendCallback(() =>
            {
                _distance = _baseDistance;
                _rotateSpeed = _baseRotateSpeed;
                Clear();
                StartCoroutine(CreateAttackEffect(_startPos, _enemyPos, _playerPos));
            });
        }
        else
        {
            //seq.Append(DOTween.To(() => _distance, x => _distance = x, _baseDistance * 1.3f, 0.2f));
            //seq.Join(DOTween.To(() => _rotateSpeed, x => _rotateSpeed = x, _baseRotateSpeed * 1.01f, 0.7f));
            //seq.Join(transform.DOLocalMoveY(-800, 0.7f).SetRelative());
            ////seq.AppendInterval(0.2f);
            //seq.Append(DOTween.To(() => _distance, x => _distance = x, 0, 0.2f));

            //seq.AppendCallback(() => Clear());
            //seq.Append(transform.DOLocalMoveY(500, 0f).SetRelative());
            //seq.AppendCallback(() => StartCoroutine(CreateAttackEffect(_startPos, _enemyPos, _playerPos)));
            //seq.AppendCallback(() =>
            //{
            //    _distance = _baseDistance;
            //    _rotateSpeed = _baseRotateSpeed;
            //    transform.DOLocalMoveY(300, 0f).SetRelative();
            //});

            _effectDict.Clear();

            seq.AppendCallback(() =>
            {
                for (int i = 0; i < _effectDictTemp[RuneType.Assist].Count; i++)
                {
                    _effectDictTemp[RuneType.Assist][i].GetComponent<RectTransform>().DOAnchorPos(_effectDictTemp[RuneType.Main][0].GetComponent<RectTransform>().anchoredPosition, 0.7f);
                }
                
            });
            seq.InsertCallback(0f, () => this.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 360 * 1.5f), 0.7f, RotateMode.WorldAxisAdd).SetEase(Ease.OutCubic));
            //seq.AppendCallback(() => );
            seq.AppendInterval(0.5f);
            seq.AppendCallback(() =>
            {
                if (_effectDictTemp.ContainsKey(RuneType.Main) == true)
                {
                    foreach (var e in _effectDictTemp[RuneType.Main])
                    {
                        e.gameObject.SetActive(false);
                    }
                }

                if (_effectDictTemp.ContainsKey(RuneType.Assist) == true)
                {
                    foreach (var e in _effectDictTemp[RuneType.Assist])
                    {
                        e.gameObject.SetActive(false);
                    }
                }
            });
            // 이거 적과 내 위치를 각각 280씩 빼줘야하나?
            seq.AppendCallback(() => StartCoroutine(CreateAttackEffect(_startPos, _enemyPos, _playerPos)));
        }
    }

    private IEnumerator CreateAttackEffect(RectTransform startPos, RectTransform enemyPos, RectTransform playerPos)
    {
        if (_magicCircle.TempEffectDict.ContainsKey(EffectType.Status))
        {
            Debug.Log("상태이살 미시알 발사");
            foreach (var d in _magicCircle.TempEffectDict[EffectType.Status])
            {
                Unit target = d.pair.IsEnemy == true ? AttackManager.Instance.enemy : AttackManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.SetTrailColor(EffectType.Status);
                go.SetEffect(d.effect);
                go.Init(startPos, _enemyPos, 2.5f, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Status, target, d.pair));

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

        if (_magicCircle.TempEffectDict.ContainsKey(EffectType.Defence))
        {
            Debug.Log("방어 미시알 발사");
            foreach (var d in _magicCircle.TempEffectDict[EffectType.Defence])
            {
                Unit target = d.pair.IsEnemy == true ? AttackManager.Instance.enemy : AttackManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.SetTrailColor(EffectType.Defence);
                go.SetEffect(d.effect);
                go.Init(startPos, _playerPos, 2.5f, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Defence, target, d.pair));

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

        if (_magicCircle.TempEffectDict.ContainsKey(EffectType.Attack))
        {
            Debug.Log("공격 미시알 발사");
            foreach (var d in _magicCircle.TempEffectDict[EffectType.Attack])
            {
                Unit target = d.pair.IsEnemy == true ? AttackManager.Instance.enemy : AttackManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.SetTrailColor(EffectType.Attack);
                go.SetEffect(d.effect);
                go.Init(startPos, _enemyPos, 2.5f, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Attack, target, d.pair));

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

        if (_magicCircle.TempEffectDict.ContainsKey(EffectType.Draw))
        {
            Debug.Log("드로우 미시알 발사");
            foreach (var d in _magicCircle.TempEffectDict[EffectType.Draw])
            {
                Unit target = d.pair.IsEnemy == true ? AttackManager.Instance.enemy : AttackManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.SetTrailColor(EffectType.Draw);
                go.SetEffect(d.effect);
                go.Init(startPos, _playerPos, 2.5f, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Draw, target, d.pair));

                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

        if (_magicCircle.TempEffectDict.ContainsKey(EffectType.Destroy))
        {
            Debug.Log("파괴 미시알 발사");
            foreach (var d in _magicCircle.TempEffectDict[EffectType.Destroy])
            {
                Unit target = d.pair.IsEnemy == true ? AttackManager.Instance.enemy : AttackManager.Instance.player;

                BezierMissile go = Instantiate(_attackEffect, _attackEffectParent);
                go.SetTrailColor(EffectType.Destroy);
                go.SetEffect(d.effect);
                go.Init(startPos, _enemyPos, 2.5f, 1300, 300, _magicCircle.AttackEffectFunction(EffectType.Destroy, target, d.pair));

                yield return new WaitForSeconds(0.1f);
            }
        }
        _magicCircle.CardCollector.UpdateCardOutline();
        this.GetComponent<RectTransform>().rotation = Quaternion.Euler(Vector3.zero);

        yield return new WaitForSeconds(.2f);
        _magicCircle.RuneTempDict.Clear();
        _magicCircle.TempEffectDict.Clear();
        if (_effectDictTemp.ContainsKey(RuneType.Main) == true)
        {
            foreach (var e in _effectDictTemp[RuneType.Main])
            {
                Destroy(e.gameObject);
            }
        }

        if (_effectDictTemp.ContainsKey(RuneType.Assist) == true)
        {
            foreach (var e in _effectDictTemp[RuneType.Assist])
            {
                Destroy(e.gameObject);
            }
        }
        _effectDictTemp.Clear();
        
        //foreach (var item in _magicCircle.RuneDict)
        //{
        //    foreach (Card card in item.Value)
        //    {
        //        if (card.IsFront == false)
        //        {
        //            card.IsFront = true;
        //        }
        //        _magicCircle.CardCollector._restCards.Add(card);
        //    }
        //}
        //_magicCircle.CardCollector.UIUpdate();

        //foreach (var rList in _magicCircle.RuneDict)
        //{
        //    foreach (var r in rList.Value)
        //    {
        //        if (r.Rune == null)
        //        {
        //            Destroy(r.gameObject);
        //        }
        //        else
        //        {
        //            r.gameObject.SetActive(false);
        //            r.transform.SetParent(_magicCircle.CardCollector.transform);
        //            r.SetIsEquip(false);
        //        }
        //    }
        //}

        //_magicCircle.RuneDict.Clear();
        //_magicCircle.EffectDict.Clear();
        //_magicCircle.CardCollector.UpdateCardOutline();
    }
}
