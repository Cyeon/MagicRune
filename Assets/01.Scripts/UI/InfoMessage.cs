using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoMessage : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();

    }

    public void Setup(string message, Vector3 pos)
    {
        transform.position = pos;
        _textMesh.SetText(message);

        Sequence seq = DOTween.Sequence();
        seq.Append(_textMesh.DOFade(1, 0.4f));
        seq.Append(transform.DOMoveY(transform.position.y + 2f, 1f).SetEase(Ease.InQuart));
        seq.Join(_textMesh.DOFade(0, 1f).SetEase(Ease.InQuart));
        seq.AppendCallback(() =>
        {
            Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
