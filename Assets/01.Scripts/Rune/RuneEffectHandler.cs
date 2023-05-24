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

        Sort(false);
    }

    public void Sort(bool isReset = false)
    {
        if (isReset)
        {
            GameObject[] effectArray = _effectDict.Values.ToArray();
            if (effectArray.Length <= 0) return;

            effectArray = effectArray.Where(x => x != null).ToArray();

            for (int i = 0; i < effectArray.Length; i++)
            {
                float width = Mathf.Cos(_oneAngle * i * Mathf.Deg2Rad + _startAngle) * _distance;
                float height = Mathf.Sin(_oneAngle * i * Mathf.Deg2Rad + _startAngle) * _distance;
            }
        }
        else
        {
            GameObject[] effectArray = _effectDict.Values.ToArray();
            if (effectArray.Length <= 0) return;

            effectArray = effectArray.Where(x => x != null).ToArray();

            Vector2 dir = effectArray[0].transform.position - this.transform.position;

            float startAngle = Mathf.Atan2(dir.y, dir.x);

            for (int i = 0; i < effectArray.Length; i++)
            {
                float width = Mathf.Cos(_oneAngle * i * Mathf.Deg2Rad + startAngle) * _distance;
                float height = Mathf.Sin(_oneAngle * i * Mathf.Deg2Rad + startAngle) * _distance;
            }
        }
    }
}
