using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _goldText;

    private Image _weigetImage;
    private Sprite _noneWeigetSprite;

    public void UpdateHealthText()
    {
        _hpText.text = string.Format("{0} / {1}", Managers.GetPlayer().HP.ToString(), Managers.GetPlayer().MaxHP.ToString());
    }

    public void UpdateGoldText()
    {
        _goldText.text = Managers.Gold.Gold.ToString();
    }

    public void WeigetInit(Sprite sprite)
    {
        if(_weigetImage == null) 
        {
            _weigetImage = transform.Find("WeightButton").GetComponent<Image>();
            _noneWeigetSprite = _weigetImage.sprite;
        }

        if(sprite == null) _weigetImage.sprite = _noneWeigetSprite;
        else
        {
            _weigetImage.sprite = sprite;
        }
    }
}
