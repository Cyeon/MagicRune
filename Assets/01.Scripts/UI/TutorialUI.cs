using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class TutorialUI : MonoBehaviour
{
    private Canvas _tutorialCanvas;
    private Image _tutorialImage;
    private GameObject _attackTutorialImage;

    private TextMeshProUGUI _tutorialMessage;

    [SerializeField]
    private Sprite _defaultSprite;

    [SerializeField]
    private RectTransform _circleReverseMaskRect;
    [SerializeField]
    private RectTransform _circleReverseMaskChildrenRect;

    [SerializeField]
    private RectTransform _squareReverseMaskRect;
    [SerializeField]
    private RectTransform _squareReverseMaskChildrenRect;

    [SerializeField]
    private RectTransform _deckRect;
    [SerializeField]
    private RectTransform _attributeRect;

    [SerializeField]
    private Button _giveBtn;
    [SerializeField]
    private Button _warningBtn;
    
    private int _index = 1; 
    private string _imageName;

    [System.Serializable]
    class TutorialDiallogue
    {
        public string Key;
        [ResizableTextArea]
        public List<string> Value;
    }

    [SerializeField]
    private List<TutorialDiallogue> _tutorialDialogue;

    private void Awake()
    {
        _tutorialCanvas = GetComponent<Canvas>();
        _tutorialImage = GetComponentInChildren<Image>();

        //_tutorialCanvas.enabled = false;
    }

    private void Start()
    {
        //if(Managers.Scene.CurrentScene is DialScene)
        //{
        _attackTutorialImage = Managers.Canvas.GetCanvas("Tutorial").transform.Find("AttackTutorial").gameObject;
        _attackTutorialImage.SetActive(false);

        _tutorialMessage = Managers.Canvas.GetCanvas("Tutorial").transform.Find("TutorialMessage").GetComponent<TextMeshProUGUI>();
        _tutorialMessage.enabled = false;

        //_giveBtn?.onClick.AddListener(() =>
        //{
            
        //});
        _warningBtn?.onClick.AddListener(() =>
        {
            TutorialEnd();
            BattleManager.Instance.TutorialEnd();
        });
        //}
    }

    public void Tutorial(string imageName, int index = 0)
    {
        _imageName = imageName;

        _tutorialCanvas.enabled = true;
        //if (index > 0)
        //{
        //    imageName += index.ToString();
        //}
        //_tutorialImage.sprite = Resources.Load<Sprite>("Tutorial/" + imageName);

        int keyIndex = _tutorialDialogue.FindIndex(x => x.Key == imageName);
        if(keyIndex > -1 && _tutorialDialogue[keyIndex]?.Value.Count > index && _tutorialDialogue[keyIndex]?.Value[index] != null)
        {
            TutorialMessage(_tutorialDialogue[keyIndex].Value[index]);
        }
        switch (imageName)
        {
            case "Tutorial":
                _circleReverseMaskRect.gameObject.SetActive(true);
                _circleReverseMaskRect.sizeDelta = new Vector2(1400, 1400);
                _circleReverseMaskRect.anchoredPosition = new Vector2(0, -1280);
                _circleReverseMaskChildrenRect.anchoredPosition = new Vector2(0, 1280);
                if (_tutorialDialogue[keyIndex]?.Value.Count == index)
                {
                    _tutorialImage.sprite = null;
                }
                break;
            case "RuneCycle":
                if (_tutorialDialogue[keyIndex]?.Value.Count == index)
                {
                    _tutorialImage.sprite = null;
                }
                else
                {
                    _tutorialImage.sprite = _defaultSprite;
                }
                break;
            case "RewardRule":
                //if (_tutorialDialogue[keyIndex]?.Value.Count == index)
                //{
                //    _tutorialImage.sprite = null;
                //}
                //else
                //{
                //    _tutorialImage.sprite = _defaultSprite;
                //}

                //_tutorialImage.gameObject.SetActive(true);
                break;
            case "LineChange":

                // Line Change 연출
                //if(index == 2)
                //{
                //    Define.DialScene.Dial.DialElementList[2].IsGlow = true;
                //}
                //else if(index == 3)
                //{
                //    Define.DialScene.Dial.LineSwap(2, 1);
                //}

                if (_tutorialDialogue[keyIndex]?.Value.Count == index)
                {
                    _tutorialImage.sprite = null;
                }
                else
                {
                    _tutorialImage.sprite = _defaultSprite;
                }
                _circleReverseMaskRect.gameObject.SetActive(true);
                break;
            case "Deck_Explain":
                if(_tutorialDialogue[keyIndex]?.Value.Count - 1 == index)
                {
                    _deckRect.gameObject.SetActive(true);
                }
                if (_tutorialDialogue[keyIndex]?.Value.Count == index)
                {
                    _tutorialImage.sprite = null;
                }
                else
                {
                    _tutorialImage.sprite = _defaultSprite;
                }

                
                break;
            case "Attribute_Select":
                if(_tutorialDialogue[keyIndex]?.Value.Count - 1 == index)
                {
                    _attributeRect.gameObject.SetActive(true);
                }
                if (_tutorialDialogue[keyIndex]?.Value.Count == index)
                {
                    _tutorialImage.sprite = null;
                }
                else
                {
                    _tutorialImage.sprite = _defaultSprite;
                }
                _deckRect.gameObject.SetActive(false);
                
                break;
            case "MapDial":
                _circleReverseMaskRect.gameObject.SetActive(true);
                _circleReverseMaskRect.sizeDelta = new Vector2(1300, 1300);
                _circleReverseMaskRect.anchoredPosition = new Vector2(0, -510);
                _circleReverseMaskChildrenRect.anchoredPosition = new Vector2(0, 510);
                break;
        }

        //if (Managers.Scene.CurrentScene is MapScene)
        //    Define.MapScene.mapDial.DialElementList[0].IsDialLock = true;
        /*else */if(Managers.Scene.CurrentScene is DialScene)
        {
            Define.DialScene.Dial.DialElementList.ForEach(x => x.IsDialLock = true);
        }
    }

    public void CanvasOff()
    {
        Managers.Canvas.GetCanvas("Main").enabled = false;
        Managers.Canvas.GetCanvas("Popup").enabled = false;
    }

    public void TutorialEnd(bool isFirst = false)
    {
        //_tutorialCanvas.enabled = false;

        _index = 1;

        if (Managers.Scene.CurrentScene is MapScene)
            Define.MapScene.mapDial.DialUnlock();
        else if (Managers.Scene.CurrentScene is DialScene)
        {
            Managers.Canvas.GetCanvas("Main").enabled = true;
            Managers.Canvas.GetCanvas("Popup").enabled = true;
            Define.DialScene.Dial.DialUnlock();

            if (isFirst)
            {
                BattleManager.Instance.TurnChange();
                _attackTutorialImage.SetActive(true);
                TutorialMessage("다이얼을 위로 드래그하여\n공격을 해보세요!");

                _circleReverseMaskRect.gameObject.SetActive(false);
                Define.DialScene.Dial.OnDialAttack += AttackTutorialImageDown;
            }
        }
    }

    public void NextTutorial()
    {
        Tutorial(_imageName, ++_index);
        if(_tutorialImage.sprite == null)
        {
            switch(_imageName)
            {
                case "Tutorial":
                    TutorialEnd(true);
                    break;

                case "LineChange":
                    _index = 1;
                    Tutorial("RuneCycle", _index);
                    break;

                case "Deck_Explain":
                    _index = 1;
                    Tutorial("Attribute_Select", _index);
                    break;

                case "RuneCycle":
                    _index = 1;
                    Tutorial("Deck_Explain", _index);

                    
                    break;

                case "Attribute_Select":
                    TutorialEnd();
                    Define.DialScene?.Turn("Enemy Turn");

                    Define.SaveData.IsTutorial = false;
                    Managers.Json.SaveJson<SaveData>("SaveData", Define.SaveData);

                    _circleReverseMaskRect.gameObject.SetActive(false);
                    _tutorialImage.gameObject.SetActive(false);
                    _attributeRect.gameObject.SetActive(false);
                    TutorialMessage("자유롭게 다이얼을 조작하여\n적을 처치하세요!");
                    Invoke("TutorialMessageEnalbedFalse", 2f);
                    BattleManager.Instance.Enemy.OnDieEvent.AddListener(() =>
                    {
                        _tutorialMessage.enabled = false;
                        //BattleManager.Instance.TutorialEnd();
                    });

                    //TutorialEnd();
                    //BattleManager.Instance.TutorialEnd();
                    break;

                default:
                    TutorialEnd();
                    break;
            }
        }
    }

    public void AttackTutorialImageDown()
    {
        _attackTutorialImage.SetActive(false);
        _tutorialMessage.enabled = false;

        Define.DialScene.Dial.OnDialAttack -= AttackTutorialImageDown;

        Tutorial("LineChange", _index);
        Managers.Canvas.GetCanvas("Main").enabled = true;
    }

    public void LobbyScene()
    {
        Managers.Scene.LoadScene(Define.Scene.LobbyScene);
    }

    private void TutorialMessage(string message)
    {
        if(_tutorialMessage == null)
            _tutorialMessage = Managers.Canvas.GetCanvas("Tutorial").transform.Find("TutorialMessage").GetComponent<TextMeshProUGUI>();
        _tutorialMessage.SetText(message);
        _tutorialMessage.enabled = true;
    }

    private void TutorialMessageEnalbedFalse()
    {
        _tutorialMessage.enabled = false;
    }
}
