using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public Transform StageList;

    public List<Image> stages = new List<Image>(); 

    private TextMeshProUGUI _healthText;
    private TextMeshProUGUI _goldText;

    public Sprite stageAtkIcon;
    public Sprite stageBossIcon;
    public Sprite stageEventIcon;

    private void Start()
    {
        UIManager.Instance.Bind<TextMeshProUGUI>("Main Gold Amount", CanvasManager.Instance.GetCanvas("MapUI").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("Main Health Amount", CanvasManager.Instance.GetCanvas("MapUI").gameObject);

        for (int i = 0; i < StageList.childCount; ++i)
        {
            stages.Add(StageList.GetChild(i).GetComponent<Image>());
        }

        _goldText = UIManager.Instance.Get<TextMeshProUGUI>("Main Gold Amount");
        _healthText = UIManager.Instance.Get<TextMeshProUGUI>("Main Health Amount");

        MapManager.Instance.NextStage();
    }

    public void InfoUIReload()
    {
        _goldText?.SetText(GameManager.Instance.Gold.ToString());
        _healthText?.SetText(GameManager.Instance.player.HP.ToString()+" / "+GameManager.Instance.player.MaxHealth.ToString());
    }
}
