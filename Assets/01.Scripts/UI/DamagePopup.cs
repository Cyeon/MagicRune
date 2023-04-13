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

        _textMesh.SetText(damageAmount.ToString());
        if(status != null)
        {
            _textMesh.color = status.textColor;
            _textMesh.SetText(string.Format("{0} {1}", status.debugName, damageAmount.ToString()));
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOJump(new Vector3(pos.x, pos.y + Random.Range(-3f, 3f), pos.x), 10f, 1, 1f)).SetEase(Ease.InQuart);
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
