using DG.Tweening;
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
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    public void EditEffect(GameObject effect, int tier)
    {
        if (_effectDict[tier] == effect) return;

        if (_effectDict[tier] != null)
        {
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
                effectArray[i].transform.DOMove(new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0), 0.2f);
            }
            else
            {
                effectArray[i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
            }
        }
    }
}
