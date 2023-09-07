using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultPanel : MonoBehaviour
{
    [SerializeField]
    private Transform _titleObj;
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private Transform _panel;
    [SerializeField]
    private Transform _textListObj;
    [SerializeField]
    private Button _gameExitButton;

    private Vector3 _titleFinishPosition;
    private List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();

    [Header("Text")]
    [SerializeField]
    private string _defeatMessage;
    [SerializeField]
    private Color _defeatColor;

    [SerializeField]
    private string _winMessage;
    [SerializeField]
    private Color _winColor;

    private void Start()
    {
        _titleFinishPosition = _titleObj.GetComponent<RectTransform>().anchoredPosition;
        
        _panel.localScale = new Vector3(_panel.localScale.x, 0, _panel.localScale.z);

        _titleObj.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0);
        _titleObj.localScale = Vector3.zero;

        _titleText = _titleObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < _textListObj.childCount; i++)
        {
            _textList.Add(_textListObj.GetChild(i).GetComponent<TextMeshProUGUI>());
        }

        _textList.ForEach(x => x.gameObject.SetActive(false));

        _gameExitButton.gameObject.SetActive(false);
        _gameExitButton.onClick.AddListener(Managers.GameQuit);

        Define.SaveData.IsTimerPlay = false;
        Popup();
    }

    private void Popup()
    {
        if(Managers.GetPlayer().HP == 0)
        {
            _titleText.SetText(_defeatMessage);
            _titleText.color = _defeatColor;
        }
        else
        {
            _titleText.text = _winMessage;
            _titleText.color = _winColor;
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.1f);
        seq.Append(_titleObj.DOScale(Vector3.one * 1.1f, 0.5f));
        seq.Append(_titleObj.DOScale(Vector3.one, 0.3f));
        seq.Append(_titleObj.GetComponent<RectTransform>().DOAnchorPos(_titleFinishPosition, 0.7f));
        seq.Join(_panel.DOScaleY(1, 0.7f));
        seq.AppendInterval(0.1f);
        for(int i=0; i < _textList.Count; i++)
        {
            int index = i;
            seq.AppendCallback(() =>
            {
                _textList[index].gameObject.SetActive(true);
                switch(index)
                {
                    case 0:
                        TimeSpan playTimeSpan = TimeSpan.FromSeconds(Define.SaveData.TimerSecond);
                        string formattedPlayTime = string.Format("{0:D2}:{1:D2}:{2:D2}", playTimeSpan.Hours, playTimeSpan.Minutes, playTimeSpan.Seconds);
                        _textList[index].SetText(string.Format("플레이 시간: {0}", formattedPlayTime));
                        break;
                    case 1:
                        _textList[index].SetText(string.Format("진행도: {0}-{1} 스테이지", Managers.Map.Chapter, Managers.Map.Stage + 1));
                        break;
                    case 2:
                        _textList[index].SetText(string.Format("획득한 총 골드: {0} 골드", Define.SaveData.TotalGold));
                        break;
                    case 3:
                        _textList[index].SetText(string.Format("처치한 적: {0} 마리", Define.SaveData.KillEnemyAmount));
                        break;
                }
                _textList[index].transform.localScale = Vector3.one * 1.2f;
            });
            seq.Append(_textList[index].transform.DOScale(Vector3.one, 0.2f));
            seq.AppendInterval(0.2f);
        }
        seq.AppendCallback(()=>_gameExitButton.gameObject.SetActive(true));
    }
}
