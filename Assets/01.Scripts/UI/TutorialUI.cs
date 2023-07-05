using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private Canvas _tutorialCanvas;
    private Image _tutorialImage;
    private GameObject _attackTutorialImage;

    private TextMeshProUGUI _tutorialMessage;

    private int _index = 1; 
    private string _imageName;

    private void Awake()
    {
        _tutorialCanvas = GetComponent<Canvas>();
        _tutorialImage = GetComponentInChildren<Image>();

        _tutorialCanvas.enabled = false;
    }

    private void Start()
    {
        if(Managers.Scene.CurrentScene is DialScene)
        {
            _attackTutorialImage = Managers.Canvas.GetCanvas("Popup").transform.Find("AttackTutorial").gameObject;
            _attackTutorialImage.SetActive(false);

            _tutorialMessage = Managers.Canvas.GetCanvas("Popup").transform.Find("TutorialMessage").GetComponent<TextMeshProUGUI>();
            _tutorialMessage.enabled = false;

        }
    }

    public void Tutorial(string imageName, int index = 0)
    {
        _imageName = imageName;

        _tutorialCanvas.enabled = true;
        if (index > 0)
        {
            imageName += index.ToString();
        }
        _tutorialImage.sprite = Resources.Load<Sprite>("Tutorial/" + imageName);

        if(Managers.Scene.CurrentScene is MapScene)
            Define.MapScene.mapDial.DialElementList[0].IsDialLock = true;
        else if(Managers.Scene.CurrentScene is DialScene)
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
        _tutorialCanvas.enabled = false;
        _index = 1;

        if (Managers.Scene.CurrentScene is MapScene)
            Define.MapScene.mapDial.DialElementList[0].IsDialLock = false;
        else if (Managers.Scene.CurrentScene is DialScene)
        {
            Managers.Canvas.GetCanvas("Main").enabled = true;
            Managers.Canvas.GetCanvas("Popup").enabled = true;
            Define.DialScene.Dial.DialElementList.ForEach(x => x.IsDialLock = false);

            if(isFirst)
            {
                BattleManager.Instance.TurnChange();
                _attackTutorialImage.SetActive(true);
                TutorialMessage("다이얼을 위로 드래그하여\n공격을 해보세요!");
                Define.DialScene.Dial.OnDialAttack += AttackTutorialImageDown;
            }
        }
    }

    public void NextTutorial()
    {
        Tutorial(_imageName, ++_index);
        if(_tutorialImage.sprite == null)
        {
            Debug.Log(_imageName);
            switch(_imageName)
            {
                case "AttackRule":
                    TutorialEnd(true);
                    break;

                case "LineChange":
                    _index = 1;
                    Tutorial("RuneCycle", _index);
                    break;

                case "RuneCycle":
                    TutorialEnd();
                    Define.DialScene?.Turn("Enemy Turn");
                    Managers.Map.SaveData.IsTutorial = false;
                    TutorialMessage("자유롭게 다이얼을 조작하여\n적을 처치하세요!");
                    BattleManager.Instance.Enemy.OnDieEvent.AddListener(() => _tutorialMessage.enabled = false);
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
        _tutorialMessage.SetText(message);
        _tutorialMessage.enabled=true;
    }
}
