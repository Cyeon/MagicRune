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
    private GameObject _chapterNameFade;
    private Image _fadeImage;
    private Transform _outline;

    public void Init()
    {
        Managers.Canvas.GetCanvas("ChapterTransition").enabled = false;

        //_chapterNumberText = transform.Find("ChapterNumberText").GetComponent<TextMeshProUGUI>();
        _chapterNameText = transform.Find("ChapterNameText").GetComponent<TextMeshProUGUI>();
        _chapterNameFade = _chapterNameText.transform.GetChild(0).gameObject;
        _fadeImage = transform.Find("FadeImage").GetComponent<Image>();
        _outline = transform.Find("Outline");
    }

    public void Transition()
    {
        Managers.Canvas.GetCanvas("MapDial").enabled = false;
        Define.MapScene.mapDial.gameObject.SetActive(false);
        Managers.Canvas.GetCanvas("ChapterTransition").enabled = true;

        _chapterNameText.SetText(Managers.Map.CurrentChapter.chapterName);
        if (Define.SaveData.IsTutorial)
        {
            _chapterNameText.SetText("튜토리얼");
            Managers.Map.ResetChapter();
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeImage.DOFade(0, 0.7f));
        seq.Append(_outline.DOScaleY(2, 0.5f));
        seq.Join(_chapterNameFade.transform.DOScaleY(1, 0.7f));
        seq.AppendInterval(0.3f);
        seq.Append(_outline.DOScaleY(0, 0.65f));
        seq.Join(_chapterNameFade.transform.DOScaleY(0, 0.6f));
        seq.AppendCallback(() =>
        {
            Reset();
            Managers.Canvas.GetCanvas("ChapterTransition").enabled = false;
            Managers.Canvas.GetCanvas("MapDial").enabled = true;
            Define.MapScene.mapDial.gameObject.SetActive(true);
            if (Define.SaveData.IsTutorial)
                Managers.Canvas.GetCanvas("TutorialCanvas").GetComponent<TutorialUI>().Tutorial("MapDial", 1);
        });
    }

    public void Reset()
    {
        _fadeImage.DOFade(1, 0);
        _chapterNameFade.transform.DOScaleY(0, 0);
        _outline.DOScaleY(0, 0);
    }
}