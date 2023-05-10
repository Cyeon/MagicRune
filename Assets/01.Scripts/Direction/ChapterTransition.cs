using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System;

public class ChapterTransition : MonoBehaviour
{
    private TextMeshProUGUI _chapterNumberText = null;
    private TextMeshProUGUI _chapterNameText = null;

    private CinemachineStoryboard _storyboard = null;

    private float _wipeSpeed = 0.5f;
    private float _fadeSpeed = 0.5f;

    private void Awake()
    {
        _storyboard = FindObjectOfType<CinemachineStoryboard>();
        _chapterNumberText = transform.Find("ChapterNumberText").GetComponent<TextMeshProUGUI>();
        _chapterNameText = transform.Find("ChapterNameText").GetComponent<TextMeshProUGUI>();
    }

    public void Transition()
    {
        _chapterNumberText.SetText("Chapter " + Managers.Map.Chapter.ToString());
        _chapterNameText.SetText(Managers.Map.CurrentChapter.chapterName);
        StartCoroutine(Wipe());
    }

    private IEnumerator Wipe()
    {
        _storyboard.m_SplitView = 1;
        while (_storyboard.m_SplitView >= 0)
        {
            _storyboard.m_SplitView -= _wipeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        FadeText(1f);
        yield return new WaitForSeconds(1f + _fadeSpeed);
        FadeText(0f);
        yield return new WaitForSeconds(_fadeSpeed);

        while (_storyboard.m_SplitView >= -1)
        {
            _storyboard.m_SplitView -= _wipeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void FadeText(float fade)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_chapterNameText.DOFade(fade, _fadeSpeed));
        seq.Join(_chapterNumberText.DOFade(fade, _fadeSpeed));
    }
}