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
    private Button _deckRemoveButton;

    [SerializeField] private GameObject _buyCheck;

    private ShopItemPanelUI _beforeSelectItem;

    public int defaultReloadGold = 0;
    public int addReloadGold = 0;
    public int defaultDeckRemoveGold = 0;

    private int _reloadGold = 0;
    private int _deckRemoveGold = 0;
    private int _deckRemoveCount = 0;

    private void Start()
    {
        _storeShelf = transform.Find("StoreShelf");
        _keywardRunePanel = transform.Find("KeywardHorizontal").GetComponent<KeywardRunePanel>();
        _deckRemoveButton = transform.Find("DeckRemoveButton").GetComponent<Button>();

        _reloadGoldText = transform.Find("ReloadButton/Gold/Text").GetComponent<TextMeshProUGUI>();
        _deckRemoveGoldText = _deckRemoveButton.transform.Find("Gold/Text").GetComponent<TextMeshProUGUI>();
    }

    public void Init()
    {
        _deckRemoveCount = 0;
        _deckRemoveButton.transform.Find("Gold/Gold_Icon").gameObject.SetActive(true);
        _deckRemoveButton.enabled = true;

        _reloadGold = defaultReloadGold;
        _deckRemoveGold = defaultDeckRemoveGold;

        _reloadGoldText.SetText(_reloadGold.ToString());
        _reloadGoldText.color = _reloadGold > Managers.Gold.Gold ? Color.red : Color.white;
        _deckRemoveGoldText.SetText(_deckRemoveGold.ToString());
        _deckRemoveGoldText.color = _deckRemoveGold > Managers.Gold.Gold ? Color.red : Color.white;
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

        _reloadGold += addReloadGold;
        _reloadGoldText.SetText(_reloadGold.ToString());
        if (_reloadGold > Managers.Gold.Gold) _reloadGoldText.color = Color.red;
    }

    public void DeckRemove()
    {
        if (GoldLessMessage(_deckRemoveGold)) return;

        Managers.Gold.AddGold(-1 * _deckRemoveGold);
        EventManager<RuneSelectMode>.TriggerEvent(Define.RUNE_EVENT_SETTING, RuneSelectMode.Delete);
        _deckRemoveCount++;
        if (_deckRemoveGold > Managers.Gold.Gold) _deckRemoveGoldText.color = Color.red;

        if (_deckRemoveCount == 2)
        {
            _deckRemoveGoldText.color = Color.red;
            _deckRemoveGoldText.SetText("모든 기회를 소진했습니다.");
            _deckRemoveButton.transform.Find("Gold/Gold_Icon").gameObject.SetActive(false);
            _deckRemoveButton.enabled = false;
        }
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