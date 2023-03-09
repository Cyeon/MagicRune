using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public Transform StageList;

    public List<PortalPanel> portals = new List<PortalPanel>();
    public List<Image> stages = new List<Image>();

    [SerializeField] private Transform _portalParent;
    private List<Transform> _portalEffects = new List<Transform>();

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

        for(int i = 0; i < _portalParent.childCount; ++i)
        {
            _portalEffects.Add(_portalParent.GetChild(i));
        }
    }

    private void OnEnable()
    {
        MapManager.Instance.ui = this;
        MapManager.Instance.NextStage();
    }

    public void StageUI()
    {
        for(int i = 0; i < 9; ++i)
        {
            stages[i].sprite = MapManager.Instance.portalList[i].icon;
            stages[i].color = MapManager.Instance.portalList[i].color;
        }
    }

    public void PortalEffectUp()
    {
        _portalEffects.ForEach(x => x.DOScale(1.2f, 1f));
    }
}
