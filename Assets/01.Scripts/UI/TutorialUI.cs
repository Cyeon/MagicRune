using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private Canvas _tutorialCanvas;
    private Image _tutorialImage;
    private Image _attackTutorialImage;

    private int _index = 1;

    private void Start()
    {
        _tutorialCanvas = GetComponent<Canvas>();
        _tutorialImage = GetComponentInChildren<Image>();

        _tutorialCanvas.enabled = false;

        if(Managers.Scene.CurrentScene is DialScene)
        {
            Managers.UI.Bind<Image>("AttackTutorialImage", Managers.Canvas.GetCanvas("Popup").gameObject);
            _attackTutorialImage = Managers.UI.Get<Image>("AttackTutorialImage");
            _attackTutorialImage.enabled = false;
        }
    }

    public void Tutorial(string imageName)
    {
        _tutorialCanvas.enabled = true;
        _tutorialImage.sprite = Resources.Load<Sprite>("Tutorial/" + imageName);

        if(Managers.Scene.CurrentScene is MapScene)
            Define.MapScene.mapDial.DialElementList[0].IsDialLock = true;
        else if(Managers.Scene.CurrentScene is DialScene)
        {
            Managers.Canvas.GetCanvas("Main").enabled = false;
            Managers.Canvas.GetCanvas("Popup").enabled = false;
            Define.DialScene.Dial.DialElementList.ForEach(x => x.IsDialLock = true);
        }
    }

    public void TutorialEnd()
    {
        _tutorialCanvas.enabled = false;

        if (Managers.Scene.CurrentScene is MapScene)
            Define.MapScene.mapDial.DialElementList[0].IsDialLock = false;
        else if (Managers.Scene.CurrentScene is DialScene)
        {
            Managers.Canvas.GetCanvas("Main").enabled = true;
            Managers.Canvas.GetCanvas("Popup").enabled = true;
            Define.DialScene.Dial.DialElementList.ForEach(x => x.IsDialLock = false);
            BattleManager.Instance.TurnChange();

            _attackTutorialImage.enabled = true;
            Define.DialScene.Dial.OnDialAttack += AttackTutorialImageDown;
        }
    }

    public void NextTutorial()
    {
        Tutorial("AttackRule" +  ++_index);
        if(_tutorialImage.sprite == null)
        {
            TutorialEnd();
        }
    }

    public void AttackTutorialImageDown()
    {
        _attackTutorialImage.enabled = false;
        Define.DialScene.Dial.OnDialAttack -= AttackTutorialImageDown;
    }
}
