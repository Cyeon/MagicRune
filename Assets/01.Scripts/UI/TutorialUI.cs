using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private Canvas _tutorialCanvas;
    private Image _tutorialImage;
    private GameObject _attackTutorialImage;

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
                Define.DialScene.Dial.OnDialAttack += AttackTutorialImageDown;
            }
        }
    }

    public void NextTutorial()
    {
        Tutorial(_imageName, ++_index);
        if(_tutorialImage.sprite == null)
        {
            if (_imageName == "AttackRule")
                TutorialEnd(true);
            else if (_imageName == "DeckRule")
            {
                TutorialEnd();
                Define.DialScene?.Turn("Enemy Turn");
                Managers.Map.isTutorial = false;
            }
            else
                TutorialEnd();
        }
    }

    public void AttackTutorialImageDown()
    {
        _attackTutorialImage.SetActive(false);
        Define.DialScene.Dial.OnDialAttack -= AttackTutorialImageDown;

        Tutorial("DeckRule", _index);
        Managers.Canvas.GetCanvas("Main").enabled = true;
    }
}
