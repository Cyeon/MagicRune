using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    private Button _button;
    private Image _icon;
    private TextMeshProUGUI _desc;

    public void Init(Reward reward)
    {
        _icon.sprite = reward.icon;
        _desc.SetText(reward.desc);

        _button.onClick.AddListener(() => reward.GiveReward());
        _button.onClick.AddListener(() => ResourceManager.Instance.Destroy(gameObject));
    }

    public void OnEnable()
    {
        _button = GetComponent<Button>();
        _icon = transform.Find("Icon").GetComponent<Image>();
        _desc = transform.Find("Desc").GetComponent<TextMeshProUGUI>();
    }
}
