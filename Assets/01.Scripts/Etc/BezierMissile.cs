using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

public class BezierMissile : MonoBehaviour
{
    #region Trail Color
    [SerializeField]
    private Color _attackColor;
    [SerializeField]
    private Color _defenceColor;
    [SerializeField]
    private Color _statusColor;
    [SerializeField]
    private Color _utillColor;
    #endregion

    private Vector2[] m_points = new Vector2[4];

    private float m_timerMax = 0;
    private float m_timerCurrent = 0;
    private float m_speed;

    private Transform _transform;

    private Action _endAction;
    private Action _goldGiveAction;

    private TrailRenderer _trail;

    GameObject _effect;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _trail = GetComponent<TrailRenderer>();
    }

    public void Init(Transform _startTr, Transform _endTr, float _speed, float _newPointDistanceFromStartTr, float _newPointDistanceFromEndTr, Action action = null)
    {
        m_timerCurrent = 0;
        //if(_effect != null)
        //{
        //    ResourceManager.Instance.Destroy(_effect);
        //    _effect = null;
        //}

        m_speed = _speed;

        // ���� ������ �ð��� �������� ��.
        m_timerMax = Random.Range(0.8f, 1.0f);

        // ���� ����.
        m_points[0] = _startTr.position;

        // ���� ������ �������� ���� ����Ʈ ����.
        m_points[1] = _startTr.position +
            (_newPointDistanceFromStartTr * Random.Range(-1.0f, 1.0f) * _startTr.right) + // X (��, �� ��ü)
            (_newPointDistanceFromStartTr * Random.Range(-0.15f, 1.0f) * _startTr.up);  // Y (�Ʒ��� ����, ���� ��ü)
            //(_newPointDistanceFromStartTr * Random.Range(-1.0f, -0.8f) * _startTr.forward); // Z (�� �ʸ�)

        // ���� ������ �������� ���� ����Ʈ ����.
        m_points[2] = _endTr.position +
            (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * _endTr.right) + // X (��, �� ��ü)
            (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * _endTr.up); // Y (��, �Ʒ� ��ü)
            //(_newPointDistanceFromEndTr * Random.Range(0.8f, 1.0f) * _endTr.forward); // Z (�� �ʸ�)

        // ���� ����.
        m_points[3] = _endTr.position;

        _transform.position = _startTr.position;

        _endAction = action;
    }

    public void Init(Vector2 _startTr, Vector2 _endTr, float _speed, float _newPointDistanceFromStartTr, float _newPointDistanceFromEndTr, Action action = null)
    {
        m_timerCurrent = 0f;
        //if (_effect != null)
        //{
        //    ResourceManager.Instance.Destroy(_effect);
        //    _effect = null;
        //}

        m_speed = _speed;

        // ���� ������ �ð��� �������� ��.
        m_timerMax = Random.Range(0.8f, 1.0f);

        // ���� ����.
        m_points[0] = _startTr;

        // ���� ������ �������� ���� ����Ʈ ����.
        m_points[1] = _startTr +
            (_newPointDistanceFromStartTr * Random.Range(-1.0f, 1.0f) * new Vector2(_startTr.x, 0)) + // X (��, �� ��ü)
            (_newPointDistanceFromStartTr * Random.Range(-0.15f, 1.0f) * new Vector2(0, _startTr.y));  // Y (�Ʒ��� ����, ���� ��ü)
                                                                                        //(_newPointDistanceFromStartTr * Random.Range(-1.0f, -0.8f) * _startTr.forward); // Z (�� �ʸ�)

        // ���� ������ �������� ���� ����Ʈ ����.
        m_points[2] = _endTr +
            (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * new Vector2(_endTr.x, 0)) + // X (��, �� ��ü)
            (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * new Vector2(0, _endTr.y)); // Y (��, �Ʒ� ��ü)
                                                                                  //(_newPointDistanceFromEndTr * Random.Range(0.8f, 1.0f) * _endTr.forward); // Z (�� �ʸ�)

        // ���� ����.
        m_points[3] = _endTr;

        _transform.position = _startTr;

        _endAction = action;
    }

    public void Init(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float _speed, Action endAction = null, Action goldAction = null)
    {
        m_timerCurrent = 0f;
        m_speed = _speed;

        // ���� ������ �ð��� �������� ��.
        m_timerMax = Random.Range(0.8f, 1.0f);

        m_points[0] = p1;
        m_points[1] = p2;
        m_points[2] = p3;
        m_points[3] = p4;

        _transform.position = p1;

        _endAction = endAction;
        _goldGiveAction = goldAction;
    }

    public void SetTrailColor(EffectType type)
    {
        switch (type)
        {
            case EffectType.Attack:
                _trail.startColor = _attackColor;
                _trail.endColor = _attackColor;
                break;
            case EffectType.Defence:
                _trail.startColor = _defenceColor;
                _trail.endColor = _defenceColor;
                break;
            case EffectType.Status:
                _trail.startColor = _statusColor;
                _trail.endColor = _statusColor;
                break;
            case EffectType.DestroyStatus:
                _trail.startColor = _attackColor;
                _trail.endColor = _attackColor;
                break;
            case EffectType.Draw:
                _trail.startColor = _utillColor;
                _trail.endColor = _utillColor;
                break;
            case EffectType.Etc:
                break;
        }
    }

    public void SetTrailColor(Color color)
    {
        _trail.startColor = color;
        _trail.endColor = color;
    }

    public void SetEffect(GameObject go)
    {
        if (go == null) return;

        _effect = Managers.Resource.Instantiate($"Effects/" + go.name, this.transform);
        //_effect.transform.localScale *= 3f;
        //Vector3 pos = _effect.transform.position;
        //pos.z = 0;
        //_effect.transform.position = pos;
        _effect.transform.localPosition = Vector2.zero;
        //_effect.transform.DOScale(Vector3.one * 200f, 0.2f);
    }

    void Update()
    {
        if (m_timerCurrent > m_timerMax)
        {
            return;
        }

        // ��� �ð� ���.
        m_timerCurrent += Time.deltaTime * m_speed;

        // ������ ����� X,Y,Z ��ǥ ���.
        _transform.position = new Vector2(
            CubicBezierCurve(m_points[0].x, m_points[1].x, m_points[2].x, m_points[3].x),
            CubicBezierCurve(m_points[0].y, m_points[1].y, m_points[2].y, m_points[3].y)
            //CubicBezierCurve(m_points[0].z, m_points[1].z, m_points[2].z, m_points[3].z)
        );

        // �����Ѱ���
        if((Vector2)_transform.position == m_points[3])
        {
            Managers.Resource.Destroy(_effect);
            _effect = null;

            _goldGiveAction?.Invoke();
            // �ڵ��ϰ�
            if(BattleManager.Instance.Enemy.IsDie == false)
            {
                _endAction?.Invoke();
            }

            m_timerCurrent = 0;
            // Ǯ��
            BattleManager.Instance.MissileAttackEnd();
            Managers.Resource.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 3�� ������ �.
    /// </summary>
    /// <param name="start">���� ��ġ</param>
    /// <param name="startOffset">���� ��ġ���� �󸶳� ���� �� ���ϴ� ��ġ</param>
    /// <param name="endOffset">���� ��ġ���� �󸶳� ���� �� ���ϴ� ��ġ</param>
    /// <param name="end">���� ��ġ</param>
    /// <returns></returns>
    private float CubicBezierCurve(float start, float startOffset, float endOffset, float end)
    {
        // (0~1)�� ���� ���� ������ � ���� ���ϱ� ������, ������ ���� �ð��� ���ߴ�.
        float t = m_timerCurrent / m_timerMax; // (���� ��� �ð� / �ִ� �ð�)

        // ������.
        /*
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
        */

        // �����Ѵ�� ���ϰ� ����.
        float startLerp = Mathf.Lerp(start, startOffset, t);
        float offsetLerp = Mathf.Lerp(startOffset, endOffset, t);
        float endLerp = Mathf.Lerp(endOffset, end, t);

        float startOffsetLerp = Mathf.Lerp(startLerp, offsetLerp, t);
        float endOffsetLerp = Mathf.Lerp(offsetLerp, endLerp, t);

        return Mathf.Lerp(startOffsetLerp, endOffsetLerp, t);
    }
}
