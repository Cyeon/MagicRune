using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    private bool _isUsing = false;
    private GameObject _panels = null;
    private GameObject _warningPanel = null;

    private CanvasGroup _optionPanelCanvasGroup;

    private void Start()
    {
        _panels = transform.Find("Panels").gameObject;
        _optionPanelCanvasGroup = _panels.transform.GetComponentInChildren<CanvasGroup>();

        Managers.UI.Bind<Slider>("Master_Slider", this.gameObject);
        Managers.UI.Bind<Slider>("BGM_Slider", this.gameObject);
        Managers.UI.Bind<Slider>("Effect_Slider", this.gameObject);
        Managers.UI.Bind<Button>("GiveUp_Button", this.gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("GiveUP_Text", this.gameObject);
        Managers.UI.Bind<Button>("OptionBGPanel_Image", this.gameObject);
        Managers.UI.Bind<Image>("WarningPanel", this.gameObject);
        Managers.UI.Bind<Button>("GiveUpAccept_Button", this.gameObject);
        Managers.UI.Bind<Button>("GiveUpCancel_Button", this.gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("WarningPopupText", this.gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("GiveUpAccept_Text", this.gameObject);

        _warningPanel = Managers.UI.Get<Image>("WarningPanel").gameObject;
        _warningPanel.SetActive(false);
        _panels.SetActive(false);

        Managers.UI.Get<Slider>("Master_Slider").onValueChanged.AddListener((volume) => Managers.Sound.SetVolume(volume, SoundType.Master));
        Managers.UI.Get<Slider>("BGM_Slider").onValueChanged.AddListener((volume) => Managers.Sound.SetVolume(volume, SoundType.Bgm));
        Managers.UI.Get<Slider>("Effect_Slider").onValueChanged.AddListener((volume) => Managers.Sound.SetVolume(volume, SoundType.Effect));

        Managers.UI.Get<Button>("GiveUp_Button").onClick.AddListener(() => PopupWarning(true));
        Managers.UI.Get<Button>("OptionBGPanel_Image").onClick.AddListener(() => ActiveUI());
        Managers.UI.Get<Button>("GiveUpAccept_Button").onClick.AddListener(() => Managers.GameQuit());
        Managers.UI.Get<Button>("GiveUpCancel_Button").onClick.AddListener(() => PopupWarning(false));

        Managers.UI.Get<Slider>("Master_Slider").value = Managers.Sound.GetVolume(SoundType.Master);
        Managers.UI.Get<Slider>("BGM_Slider").value = Managers.Sound.GetVolume(SoundType.Bgm);
        Managers.UI.Get<Slider>("Effect_Slider").value = Managers.Sound.GetVolume(SoundType.Effect);
    }

    private void Update()
    {
        if (_isUsing && Input.GetKeyDown(KeyCode.Escape))
        {
            ActiveUI();
        }
    }

    public void ActiveUI()
    {
        _isUsing = !_isUsing;

        if (_isUsing)
        {
            _panels.SetActive(_isUsing);
            Define.MapScene?.mapDial.DialLock();
            Define.DialScene?.Dial.DialLock();

            Managers.UI.UIPopup(_optionPanelCanvasGroup.transform, _optionPanelCanvasGroup);
        }
        else
        {
            Managers.UI.UIPopup(_optionPanelCanvasGroup.transform, _optionPanelCanvasGroup, false, () =>
            {
                Define.MapScene?.mapDial.DialUnlock();
                Define.DialScene?.Dial.DialUnlock();
                _panels.SetActive(_isUsing);
            });
        }
    }

    public void ActiveUI(bool isActive)
    {
        if (isActive)
        {
            Define.MapScene?.mapDial.DialLock();
            Define.DialScene?.Dial.DialLock();

            if (Managers.Scene.CurrentScene is LobbyScene)
            {
                Managers.UI.Get<TextMeshProUGUI>("GiveUP_Text").SetText("게임 종료");
            }
            else
            {
                Managers.UI.Get<TextMeshProUGUI>("GiveUP_Text").SetText("게임 포기");
            }
        }
        else
        {
            Define.MapScene?.mapDial.DialUnlock();
            Define.DialScene?.Dial.DialUnlock();
        }

        _isUsing = isActive;
        _panels.SetActive(isActive);
    }

    private void PopupWarning(bool isAcive)
    {
        if(isAcive == true)
        {
            if(Managers.Scene.CurrentScene is LobbyScene)
            {
                
                Managers.UI.Get<TextMeshProUGUI>("WarningPopupText").SetText("정말로 게임을 종료하시겠습니가?");
                Managers.UI.Get<TextMeshProUGUI>("GiveUpAccept_Text").SetText("게임 종료");
            }
            else
            {
                
                Managers.UI.Get<TextMeshProUGUI>("WarningPopupText").SetText("정말로 게임을 포기하고 로비로 돌아가시겠습니까?");
                Managers.UI.Get<TextMeshProUGUI>("GiveUpAccept_Text").SetText("게임 포기");
            }
        }

        _warningPanel.SetActive(isAcive);
    }

    private void GameGiveUp()
    {
        Managers.Gold.ResetGoldAmount();
        Managers.Map.ResetChapter();
        Managers.Deck.Init();
        Managers.Enemy.ResetEnemy();
        Managers.Scene.LoadScene(Define.Scene.LobbyScene);
    }
}