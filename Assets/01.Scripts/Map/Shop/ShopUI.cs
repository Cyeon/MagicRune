using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    private Transform _storeShelf;
    private KeywardRunePanel _keywardRunePanel;
    private ShopItemPanelUI _selectItem;
    private TextMeshProUGUI _reloadGoldText;
    private TextMeshProUGUI _deckRemoveGoldText;

    [SerializeField] private GameObject _buyCheck;

    private ShopItemPanelUI _beforeSelectItem;

    [SerializeField] private int _reloadGold;
    [SerializeField] private int _deckRemoveGold;

    private void Start()
    {
        _storeShelf = transform.Find("StoreShelf");
        _keywardRunePanel = transform.Find("KeywardHorizontal").GetComponent<KeywardRunePanel>();

        _reloadGoldText = transform.Find("ReloadButton/Gold/Text").GetComponent<TextMeshProUGUI>();
        _deckRemoveGoldText = transform.Find("DeckRemoveButton/Gold/Text").GetComponent<TextMeshProUGUI>();

        _reloadGoldText.SetText(_reloadGold.ToString());
        _deckRemoveGoldText.SetText(_deckRemoveGold.ToString());

        Managers.Canvas.GetCanvas("Shop").enabled = false;
    }

    public void Exit()
    {
        Managers.Map.NextStage();
        Managers.Canvas.GetCanvas("Shop").enabled = false;
        Managers.Canvas.GetCanvas("MapUI").enabled = true;
    }

    public void ShopItemReset()
    {
        for (int i = _storeShelf.transform.childCount - 1; i >= 0; --i)
        {
            Managers.Resource.Destroy(_storeShelf.transform.GetChild(i).gameObject);
        }
    }

    public void RuneItemProduct(Item item)
    {
        ShopItemPanelUI ui = Managers.Resource.Instantiate("NewItemPanel", _storeShelf).GetComponent<ShopItemPanelUI>();
        ui.Init(item, SelectItem);
    }

    // 선택
    private void SelectItem(ShopItemPanelUI shopItem)
    {
        if (_buyCheck.activeSelf == true) return;

        _beforeSelectItem = _selectItem;
        _beforeSelectItem?.SetActiveSelectPanel(false);

        _selectItem = shopItem;
        _selectItem.SetActiveSelectPanel(true);

        _keywardRunePanel.SetUI(_selectItem.item.Rune.BaseRuneSO, false);
        _keywardRunePanel.SetRune(_selectItem.item.Rune);
        _keywardRunePanel.KeywardSetting();
    }

    public void BuyCheck()
    {
        //_shopItemPanel = shopItem;

        if (_selectItem == null) return;

        if (GoldLessMessage(_selectItem.item.Gold)) return;

        _buyCheck.SetActive(true);
        _buyCheck.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(_buyCheck.transform.DOScale(1.2f, 0.2f));
        seq.Append(_buyCheck.transform.DOScale(1f, 0.1f));
    }

    public void Buy()
    {

        Sequence seq = DOTween.Sequence();
        seq.Append(_buyCheck.transform.DOScale(1.2f, 0.1f));
        seq.Append(_buyCheck.transform.DOScale(0, 0.2f));
        seq.AppendCallback(() => _buyCheck.SetActive(false));

        Managers.Gold.AddGold(-_selectItem.item.Gold);
        Managers.Sound.PlaySound("SFX/Buy", SoundType.Effect);
        _selectItem.item.Execute();

        _selectItem.SoldOut();

        _selectItem = null;
        _keywardRunePanel.SetUI(null);

        _storeShelf.transform.GetComponentsInChildren<ShopItemPanelUI>().ForEach(x => x.GoldTextColorUpdate());
    }

    public void Reload()
    {
        if (GoldLessMessage(_reloadGold)) return;

        ShopItemReset();
        (Managers.Map.currentStage as ShopStage).ShopItemInit();
        Managers.Gold.AddGold(-1 * _reloadGold);
    }

    public void DeckRemove()
    {
        if (GoldLessMessage(_deckRemoveGold)) return;

        Managers.Gold.AddGold(-1 * _deckRemoveGold);
        EventManager<RuneSelectMode>.TriggerEvent(Define.RUNE_EVENT_SETTING, RuneSelectMode.Delete);
        Managers.UI.Get<Button>("NextStageButton_Button").onClick.AddListener(() =>
        {
            Managers.Canvas.GetCanvas("Adventure").enabled = false;
            Managers.UI.Get<Button>("NextStageButton_Button").onClick.RemoveListener(() =>Managers.Canvas.GetCanvas("Adventure").enabled = false);
        });
    }

    private bool GoldLessMessage(int gold)
    {
        if(Managers.Gold.Gold < gold)
        {
            InfoMessage message = Managers.Resource.Instantiate("InfoMessage", transform).GetComponent<InfoMessage>();
            message.Setup("돈이 부족합니다.", Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            return true;
        }

        return false;
    }
}