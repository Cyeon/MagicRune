using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicContent : MonoBehaviour
{
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

    private bool _isAdd = false;
    private float _axis = 90;

    private Dictionary<RuneType, List<GameObject>> _effectDict = new Dictionary<RuneType, List<GameObject>>();

    [SerializeField]
    private int _bezierResolution = 30;
    private Vector3[] _bezierPoints;
    private float _frameSpeed = 0;
    [SerializeField]
    private float _attackSpeed;
    [SerializeField]
    private float _attackDelay;

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

    public void AddEffect(GameObject effect, bool isMain)
    {
        if (effect == null) return;

        GameObject g = Instantiate(effect, this.transform);
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
            
        }
        else
        {
            seq.Append(DOTween.To(() => _distance, x => _distance = x, _baseDistance * 1.3f, 0.2f));
            seq.Join(DOTween.To(() => _rotateSpeed, x => _rotateSpeed = x, _baseRotateSpeed * 1.01f, 0.7f));
            //seq.AppendInterval(0.2f);
            seq.Append(DOTween.To(() => _distance, x => _distance = x, 0, 0.2f));
            seq.AppendCallback(() => Clear());
            //seq.AppendCallback(() =>
            //{
            //    _effectDict[RuneType.Main]l;`````````````````````````
            //});
            seq.AppendCallback(() =>
            {
                _distance = _baseDistance;
                _rotateSpeed = _baseRotateSpeed;
            });
        }

        
    }

    private void Draw()
    {
        //int count = 2;
        //Vector3 pA = _effectDict[RuneType.Main][0].transform.position;
        //Vector3 pC = GameManager.Instance.enemy.transform.position;
        //Vector3 pB = pA - pC;
        //pB /= 2;
        //pB.x += Random.value >= 0.5f ? -5f : 5f;

        //_bezierPoints = new Vector3[count + 1];
        //float unit = 1.0f / count;

        //int i = 0; float t = 0f;
        //for (; i < count + 1; i++, t += unit)
        //{
        //    float u = (1 - t);
        //    float t2 = t * t;
        //    float u2 = u * u;

        //    _bezierPoints[i] =
        //        pA * u2 +
        //        pB * (t * u * 2) +
        //        pC * t2
        //    ;
        //}
        //StartCoroutine(DrawCoroutine());
    }

    private IEnumerator DrawCoroutine()
    {
        yield return new WaitForSeconds(_attackDelay);

        for (int i = 0; i < _bezierPoints.Length; i++)
        {
            yield return new WaitForSeconds(_frameSpeed);
            _effectDict[RuneType.Main][0].transform.position = _bezierPoints[i];
        }
    }
}
