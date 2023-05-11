using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System;
using System.Diagnostics.CodeAnalysis;

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

        _chapterNumberText.gameObject.SetActive(false);
        _chapterNameText.gameObject.SetActive(false);
    }

    public void Transition()
    {
        _chapterNumberText.gameObject.SetActive(true);
        _chapterNameText.gameObject.SetActive(true);

        _chapterNumberText.SetText("Chapter " + Managers.Map.Chapter.ToString());
        _chapterNameText.SetText(Managers.Map.CurrentChapter.chapterName);

        StartCoroutine(Wipe());
    }

    private IEnumerator Wipe()
    {
        _storyboard.m_SplitView = 1;
        while (_storyboard.m_SplitView > 0)
        {
            _storyboard.m_SplitView -= _wipeSpeed * Time.deltaTime;
            if (_storyboard.m_SplitView < 0) _storyboard.m_SplitView = 0;
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

        _chapterNumberText.gameObject.SetActive(false);
        _chapterNameText.gameObject.SetActive(false);
        Managers.Map.SpawnPortal();
    }

    private void FadeText(float fade)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_chapterNameText.DOFade(fade, _fadeSpeed));
        seq.Join(_chapterNumberText.DOFade(fade, _fadeSpeed));
    }
}