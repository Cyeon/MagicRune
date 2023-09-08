using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    [SerializeField] private Vector3 _originalSize;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void Setup(float damageAmount, Vector3 pos, Status status = null)
    {
        transform.position = pos;
        transform.localScale = _originalSize;
        _textMesh.DOFade(1, 0);

        _textMesh.SetText(damageAmount.ToString());
        if(status != null)
        {
            _textMesh.color = status.textColor;
            _textMesh.SetText(string.Format("{0} {1}", status.debugName, damageAmount.ToString()));
        }

        if(damageAmount == 0)
        {
            _textMesh.SetText("방어");
            _textMesh.color = Color.blue;
        }

        Vector3 rectTrm = transform.GetComponent<RectTransform>().anchoredPosition3D;
        transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(rectTrm.x, rectTrm.y, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(rectTrm.y + 100, 1)).SetEase(Ease.InQuart);
        seq.Join(_textMesh.DOFade(0, 1f).SetEase(Ease.InQuart));
        seq.AppendCallback(() =>
        {
            Managers.Resource.Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
