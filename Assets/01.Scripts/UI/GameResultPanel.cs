using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GameResultPanel : MonoBehaviour
{
    [SerializeField]
    private Transform _titleObj;
    [SerializeField]
    private Transform _panel;
    [SerializeField]
    private Transform _textListObj;

    private float _titleFinishPositionY;
    private List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();

    private void Start()
    {
        _titleFinishPositionY = _titleObj.localPosition.y;
        
        _panel.localScale = new Vector3(_panel.localScale.x, 0, _panel.localScale.z);
        _titleObj.localPosition = Vector3.zero;
        _titleObj.localScale = Vector3.zero;

        for (int i = 0; i < _textListObj.childCount; i++)
        {
            _textList.Add(_textListObj.GetChild(i).GetComponent<TextMeshProUGUI>());
        }

        _textList.ForEach(x => x.gameObject.SetActive(false));

        Popup();
    }

    private void Popup()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.1f);
        seq.Append(_titleObj.DOScale(Vector3.one * 1.1f, 0.5f));
        seq.Append(_titleObj.DOScale(Vector3.one, 0.3f));
        seq.Append(_titleObj.DOLocalMoveY(_titleFinishPositionY, 0.7f));
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
    }
}
