using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    private bool _isUsing = false;
    private GameObject _panels = null;
    private GameObject _warningPanel = null;

    private RuneDial _runeDial = null;
    private MapDial _mapDial = null;

    private void Start()
    {
        _panels = transform.Find("Panels").gameObject;

        Managers.UI.Bind<Slider>("Master_Slider", this.gameObject);
        Managers.UI.Bind<Slider>("BGM_Slider", this.gameObject);
        Managers.UI.Bind<Slider>("Effect_Slider", this.gameObject);
        Managers.UI.Bind<Button>("GiveUp_Button", this.gameObject);
        Managers.UI.Bind<Button>("OptionBGPanel_Image", this.gameObject);
        Managers.UI.Bind<Image>("WarningPanel", this.gameObject);
        Managers.UI.Bind<Button>("GiveUpAccept_Button", this.gameObject);
        Managers.UI.Bind<Button>("GiveUpCancel_Button", this.gameObject);

        _warningPanel = Managers.UI.Get<Image>("WarningPanel").gameObject;
        _warningPanel.SetActive(false);
        _panels.SetActive(false);

        Managers.UI.Get<Slider>("Master_Slider").onValueChanged.AddListener((volume) => Managers.Sound.SetVolume(volume, SoundType.Master));
        Managers.UI.Get<Slider>("BGM_Slider").onValueChanged.AddListener((volume) => Managers.Sound.SetVolume(volume, SoundType.Bgm));
        Managers.UI.Get<Slider>("Effect_Slider").onValueChanged.AddListener((volume) => Managers.Sound.SetVolume(volume, SoundType.Effect));

        Managers.UI.Get<Button>("GiveUp_Button").onClick.AddListener(() => PopupWarning(true));
        Managers.UI.Get<Button>("OptionBGPanel_Image").onClick.AddListener(() => ActiveUI());
        Managers.UI.Get<Button>("GiveUpAccept_Button").onClick.AddListener(() => GameGiveUp());
        Managers.UI.Get<Button>("GiveUpCancel_Button").onClick.AddListener(() => PopupWarning(false));

        Managers.UI.Get<Slider>("Master_Slider").value = Managers.Sound.GetVolume(SoundType.Master);
        Managers.UI.Get<Slider>("BGM_Slider").value = Managers.Sound.GetVolume(SoundType.Bgm);
        Managers.UI.Get<Slider>("Effect_Slider").value = Managers.Sound.GetVolume(SoundType.Effect);

        _mapDial = FindObjectOfType<MapDial>();
        _runeDial = FindObjectOfType<RuneDial>();
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
        _panels.SetActive(_isUsing);

        if (_isUsing)
        {
            _mapDial?.DialLock();
            _runeDial?.DialLock();
        }
        else
        {
            _mapDial?.DialUnlock();
            _runeDial?.DialUnlock();
        }
    }

    public void ActiveUI(bool isActive)
    {
        if (isActive)
        {
            _mapDial?.DialLock();
            _runeDial?.DialUnlock();
        }
        else
        {
            _mapDial?.DialUnlock();
            _runeDial?.DialUnlock();
        }

        _isUsing = isActive;
        _panels.SetActive(isActive);
    }

    private void PopupWarning(bool isAcive)
    {
        _warningPanel.SetActive(isAcive);
    }

    private void GameGiveUp()
    {
        Debug.Log("게임 포기");

        Managers.Gold.ResetGoldAmount();
        Managers.Map.ResetChapter();
        Managers.Scene.LoadScene(Define.Scene.LobbyScene);
    }
}