using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public Transform StageList;

    private List<Image> stages = new List<Image>();
    public List<Image> Stages
    {
        get
        {
            if (stages.Count <= 0)
            {
                for (int i = 0; i < StageList.childCount; ++i)
                {
                    stages.Add(StageList.GetChild(i).GetComponent<Image>());
                }
            }

            return stages;
        }
    }

    private TextMeshProUGUI _healthText;
    private TextMeshProUGUI _goldText;

    public Sprite stageAtkIcon;
    public Sprite stageBossIcon;
    public Sprite stageEventIcon;
    
    private void Start()
    {
        Managers.UI.Bind<TextMeshProUGUI>("Main Gold Amount", Managers.Canvas.GetCanvas("MapUI").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("Main Health Amount", Managers.Canvas.GetCanvas("MapUI").gameObject);

        if(stages.Count <= 0)
        {
            for (int i = 0; i < StageList.childCount; ++i)
            {
                stages.Add(StageList.GetChild(i).GetComponent<Image>());
            }
        }

        _goldText = Managers.UI.Get<TextMeshProUGUI>("Main Gold Amount");
        _healthText = Managers.UI.Get<TextMeshProUGUI>("Main Health Amount");

        Managers.Map.NextStage();
    }

    public void InfoUIReload()
    {
        _goldText?.SetText(GameManager.Instance.Gold.ToString());
        _healthText?.SetText(GameManager.Instance.player.HP.ToString()+" / "+GameManager.Instance.player.MaxHealth.ToString());
    }
}
