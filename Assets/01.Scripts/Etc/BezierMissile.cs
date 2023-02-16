using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BezierMissile : MonoBehaviour
{
    private Vector2[] m_points = new Vector2[4];

    private float m_timerMax = 0;
    private float m_timerCurrent = 0;
    private float m_speed;

    private RectTransform _rect;

    private Action _endAction;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void Init(RectTransform _startTr, RectTransform _endTr, float _speed, float _newPointDistanceFromStartTr, float _newPointDistanceFromEndTr, Action action = null)
    {
        m_speed = _speed;

        // ���� ������ �ð��� �������� ��.
        m_timerMax = Random.Range(0.8f, 1.0f);

        // ���� ����.
        m_points[0] = _startTr.anchoredPosition;

        // ���� ������ �������� ���� ����Ʈ ����.
        m_points[1] = _startTr.anchoredPosition3D +
            (_newPointDistanceFromStartTr * Random.Range(-1.0f, 1.0f) * _startTr.right) + // X (��, �� ��ü)
            (_newPointDistanceFromStartTr * Random.Range(-0.15f, 1.0f) * _startTr.up);  // Y (�Ʒ��� ����, ���� ��ü)
            //(_newPointDistanceFromStartTr * Random.Range(-1.0f, -0.8f) * _startTr.forward); // Z (�� �ʸ�)

        // ���� ������ �������� ���� ����Ʈ ����.
        m_points[2] = _endTr.anchoredPosition3D +
            (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * _endTr.right) + // X (��, �� ��ü)
            (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * _endTr.up); // Y (��, �Ʒ� ��ü)
            //(_newPointDistanceFromEndTr * Random.Range(0.8f, 1.0f) * _endTr.forward); // Z (�� �ʸ�)

        // ���� ����.
        m_points[3] = _endTr.anchoredPosition;

        _rect.anchoredPosition = _startTr.anchoredPosition;

        _endAction = action;
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
        _rect.anchoredPosition = new Vector2(
            CubicBezierCurve(m_points[0].x, m_points[1].x, m_points[2].x, m_points[3].x),
            CubicBezierCurve(m_points[0].y, m_points[1].y, m_points[2].y, m_points[3].y)
            //CubicBezierCurve(m_points[0].z, m_points[1].z, m_points[2].z, m_points[3].z)
        );

        if(_rect.anchoredPosition == m_points[3])
        {
            // �����Ѱ���
            _endAction?.Invoke();

            // �����ϰ�
            Destroy(this.gameObject);
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
        float ab = Mathf.Lerp(start, startOffset, t);
        float bc = Mathf.Lerp(startOffset, endOffset, t);
        float cd = Mathf.Lerp(endOffset, end, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }
}
