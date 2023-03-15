using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public Transform StageList;

    public List<PortalPanel> portals = new List<PortalPanel>();
    public List<Image> stages = new List<Image>(); 

    [SerializeField] private Transform _portalParent;
    private List<Transform> _attackPortalEffects = new List<Transform>();
    private List<Transform> _bossPortalEffects = new List<Transform>();

    private TextMeshProUGUI _healthText;
    private TextMeshProUGUI _goldText;

    public Sprite stageAtkIcon;
    public Sprite stageBossIcon;
    public Sprite stageEventIcon;

    private void Awake()
    {
        StageList = transform.Find("StageSlider/StageImage");

        Transform trm = transform.Find("Portals");
        for(int i = 0; i < trm.childCount; ++i)
        {
            portals.Add(trm.GetChild(i).GetComponent<PortalPanel>());
        }

        for (int i = 0; i < StageList.childCount; ++i)
        {
            stages.Add(StageList.GetChild(i).GetComponent<Image>());
        }

        for(int i = 0; i < _portalParent.Find("AttackPortal").childCount; ++i)
        {
            _attackPortalEffects.Add(_portalParent.Find("AttackPortal").GetChild(i));
        }

        for (int i = 0; i < _portalParent.Find("BossPortal").childCount; ++i)
        {
            _bossPortalEffects.Add(_portalParent.Find("BossPortal").GetChild(i));
        }

        _goldText = transform.Find("GoldBar").GetComponentInChildren<TextMeshProUGUI>();
        _healthText = transform.Find("HealthBar").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        MapManager.Instance.ui = this;
        MapManager.Instance.NextStage();

        InfoUIReload();
    }

    public void PortalEffectUp(StageType type)
    {
        if (type == StageType.Attack)
            _attackPortalEffects.ForEach(x => x.DOScale(1.2f, 1f));
        else
            _bossPortalEffects.ForEach(x => x.DOScale(2f, 1f));
    }

    public void ResetPortal(StageType type)
    {
        if (type == StageType.Attack)
            _attackPortalEffects.ForEach(x => x.DOScale(0,0));
        else
            _bossPortalEffects.ForEach(x => x.DOScale(0,0));

        portals.ForEach(x => x.transform.DOScale(0, 0));
    }

    public void InfoUIReload()
    {
        _goldText.text = GameManager.Instance.Gold.ToString();
        _healthText.text = GameManager.Instance.player.HP.ToString();
    }
}
