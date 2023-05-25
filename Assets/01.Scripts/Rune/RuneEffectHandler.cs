using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneEffectHandler : MonoBehaviour
{
    private Dictionary<int, GameObject> _effectDict = new Dictionary<int, GameObject>();

    [SerializeField, Range(0f, 360f)]
    private float _startAngle = 90f;
    [SerializeField, Min(0f)]
    private float _distance = 5f;
    [SerializeField]
    private float _rotateSpeed = 5f;
    [SerializeField]
    private bool _isLeft = true;

    private float _oneAngle = 0f;

    private void Start()
    {
        for (int i = 1; i <= 3; i++)
        {
            _effectDict.Add(i, null);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * (_isLeft ? 1 : -1f) * Time.deltaTime);
    }

    public void EditEffect(GameObject effect, int tier)
    {
        transform.DOKill();


        if (_effectDict[tier] != null)
        {
            if (_effectDict[tier] == effect) return;
            Managers.Resource.Destroy(_effectDict[tier]);
        }

        if (effect != null)
        {
            GameObject effectObject = Managers.Resource.Instantiate(effect, this.transform);
            _effectDict[tier] = effectObject;
        }
        else
        {
            _effectDict[tier] = null;
        }

        Sort(true);
    }

    public void Sort(bool isTween = false)
    {
        GameObject[] effectArray = _effectDict.Values.ToArray();
        if (effectArray.Length <= 0) return;

        effectArray = effectArray.Where(x => x != null).ToArray();
        _oneAngle = 360f / effectArray.Length;
        Debug.Log(effectArray.Length);

        for (int i = 0; i < effectArray.Length; i++)
        {
            float width = Mathf.Cos((_oneAngle * i + _startAngle) * Mathf.Deg2Rad) * _distance;
            float height = Mathf.Sin((_oneAngle * i + _startAngle) * Mathf.Deg2Rad) * _distance;
            if (isTween)
            {
                transform.DOKill();
                effectArray[i].transform.DOMove(new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0), 0.2f);
            }
            else
            {
                effectArray[i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
            }
        }
    }

    public void Attack(int tier, Action action = null)
    {
        BezierMissile b = Managers.Resource.Instantiate("BezierMissile", this.transform.parent).GetComponent<BezierMissile>();
        b.SetEffect(_effectDict[tier]);
        switch (Define.DialScene.Dial.DialElementList[3 - tier].SelectElement.Rune.BaseRuneSO.AttributeType)
        {
            case AttributeType.None:
                break;
            case AttributeType.NonAttribute:
                b.SetTrailColor(Color.gray);
                break;
            case AttributeType.Fire:
                b.SetTrailColor(Color.red);
                break;
            case AttributeType.Ice:
                b.SetTrailColor(Color.cyan);
                break;
            case AttributeType.Wind:
                b.SetTrailColor(new Color(0, 1, 0));
                break;
            case AttributeType.Ground:
                b.SetTrailColor(new Color(0.53f, 0.27f, 0));
                break;
            case AttributeType.Electric:
                b.SetTrailColor(Color.yellow);
                break;
        }

        Transform pos = null;
        switch (Define.DialScene.Dial.DialElementList[3 - tier].SelectElement.Rune.BaseRuneSO.Direction)
        {
            case EffectDirection.Enemy:
                pos = BattleManager.Instance.Enemy.transform;
                break;
            case EffectDirection.Player:
                pos = this.transform;
                break;
        }
        Managers.Sound.PlaySound(Define.DialScene.Dial.DialElementList[3 - tier].SelectElement.Rune.BaseRuneSO.RuneSound, SoundType.Effect);
        b.Init(_effectDict[tier].transform, pos, 2f, 10f, 10f, action);
        Managers.Resource.Destroy(_effectDict[tier]);
        _effectDict[tier] = null;

        BattleManager.Instance.missileCount += 1;
    }

    public void Clear()
    {
        foreach(var effect in _effectDict)
        {
            Managers.Resource.Destroy(effect.Value);
        }
    }
}
