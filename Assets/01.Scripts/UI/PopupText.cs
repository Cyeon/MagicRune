using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    private TextMeshPro _text;

    private void Start()
    {
        TryGetComponent<TextMeshPro>(out _text);
    }

    public void SetText(string message, float delay = 1f, float duration = 0.5f)
    {
        if(_text == null)
        {
            TryGetComponent<TextMeshPro>(out _text);
        }

        if (_text != null)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                _text.SetText(message);
                _text.color = Color.white;
            });
            seq.SetDelay(delay);
            seq.Append(_text.DOFade(0f, duration).OnComplete(() =>
            {
                Managers.Resource.Destroy(this.gameObject);
            }));
        }
        else
        {
            Debug.LogWarning("Text is Null");
        }
    }
}
