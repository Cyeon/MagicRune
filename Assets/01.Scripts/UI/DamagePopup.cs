using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEditor.EditorTools;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();

    }

    public void Setup(float damageAmount, Vector3 pos)
    {
        transform.position = pos;
        _textMesh.SetText(damageAmount.ToString());

        _textMesh.color = Color.red;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOJump(new Vector3(pos.x + Random.Range(-1f, 1f), pos.y, pos.x), 0.8f, 1, 1f));
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
