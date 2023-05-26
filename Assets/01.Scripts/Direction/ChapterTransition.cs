using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ChapterTransition : MonoBehaviour
{
    //private TextMeshProUGUI _chapterNumberText = null;
    private TextMeshProUGUI _chapterNameText = null;
    private GameObject _chapterNameTextFadeObj;

    private Image _backgroundImage;
    private Image _fadeImage;

    public void Init()
    {
        Managers.Canvas.GetCanvas("ChapterTransition").enabled = false;

        //_chapterNumberText = transform.Find("ChapterNumberText").GetComponent<TextMeshProUGUI>();
        _chapterNameText = transform.Find("ChapterNameText").GetComponent<TextMeshProUGUI>();
        _chapterNameTextFadeObj = _chapterNameText.transform.Find("Fade").gameObject;

        _backgroundImage = transform.Find("Background").GetComponent<Image>();
        _fadeImage = transform.Find("FadeImage").GetComponent<Image>();
    }

    public void Transition()
    {
        Managers.Canvas.GetCanvas("ChapterTransition").enabled = true;

        _backgroundImage.sprite = Resources.Load<Sprite>("Sprite/MapBg_" + Managers.Map.Chapter.ToString());
        _chapterNameText.SetText(Managers.Map.CurrentChapter.chapterName);

        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeImage.DOFade(0, 0.7f));
        seq.Append(_chapterNameTextFadeObj.transform.DOScaleX(1, 1f));
        seq.Append(_chapterNameTextFadeObj.transform.DOScaleX(0, 1f));
        seq.Append(_backgroundImage.DOFade(0, 0.2f));
        seq.AppendCallback(() =>
        {
            Reset();
            Managers.Canvas.GetCanvas("ChapterTransition").enabled = false;
        });
    }

    public void Reset()
    {
        _backgroundImage.DOFade(1, 0);
        _fadeImage.DOFade(1, 0);
    }
}