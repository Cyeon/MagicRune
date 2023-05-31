using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnhancePanel : MonoBehaviour
{
    private BaseRune _selectRune;

    [SerializeField]
    private Transform _runeArea;
    private List<ExplainPanel> _runeList = new List<ExplainPanel>();

    private ExplainPanel _beforeRune;
    private ExplainPanel _afterRune;

    private RestUI _restUI;

    void Start()
    {
        _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();

        Managers.UI.Bind<Button>("EnhanceBtn", this.gameObject);
        Managers.UI.Bind<Button>("ExitBtn", this.gameObject);

        Managers.UI.Get<Button>("EnhanceBtn").onClick.AddListener(() => Debug.Log("Enhance Click"));
        Managers.UI.Get<Button>("ExitBtn").onClick.AddListener(() =>
        {
            _selectRune = null;

            _restUI.SetActiveExplainPanel(true);
            _restUI.SetActiveEnhancePanel(false);
            _restUI.Dial.gameObject.SetActive(true);
            _restUI.NextStage();
        });
    }

    public void CreateRune()
    {
        Clear();

        BaseRune[] notEnhanceRuneArray = Managers.Deck.Deck.Where(x => x.IsEnhanced == false).ToArray();
        for(int i = 0; i < notEnhanceRuneArray.Length; i++)
        {
            int index = i;
            ExplainPanel panel = Managers.Resource.Instantiate("UI/Explain_Panel", _runeArea).GetComponent<ExplainPanel>();
            panel.SetUI(notEnhanceRuneArray[i], isReward: false);
            panel.GetComponent<RectTransform>().localScale = Vector3.one;
            _runeList.Add(panel);
            panel.SetAction(() => SetSelectRune(_runeList[index].Rune));
        }
    }

    public void Clear()
    {
        for(int i = _runeList.Count - 1; i >= 0; i++)
        {
            Managers.Resource.Destroy(_runeList[i].gameObject);
        }

        _runeList.Clear();
    }

    public void SetSelectRune(BaseRune rune)
    {
        _selectRune = rune;

        _beforeRune.SetUI(rune);
        _beforeRune.SetUI(rune, true);
    }

    public void Enhace()
    {
        _selectRune.Enhance();
        _selectRune = null;

        Clear();
    }
}
