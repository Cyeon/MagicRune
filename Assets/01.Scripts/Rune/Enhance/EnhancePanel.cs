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

    void Start()
    {
        Managers.UI.Bind<Button>("EnhanceBtn", this.gameObject);
        Managers.UI.Bind<Button>("ExitBtn", this.gameObject);

        Managers.UI.Get<Button>("EnhanceBtn").onClick.AddListener(() => Debug.Log("Enhance Click"));
        Managers.UI.Get<Button>("ExitBtn").onClick.AddListener(() => Debug.Log("Exit Click"));
    }

    [ButtonMethod]
    public void CreateRune()
    {
        Clear();

        BaseRune[] notEnhanceRuneArray = Managers.Deck.Deck.Where(x => x.IsEnhanced == false).ToArray();
        for(int i = 0; i < notEnhanceRuneArray.Length; i++)
        {
            ExplainPanel panel = Managers.Resource.Instantiate("UI/Explain_Panel", _runeArea).GetComponent<ExplainPanel>();
            panel.SetUI(notEnhanceRuneArray[i], isReward: false);
            panel.GetComponent<RectTransform>().localScale = Vector3.one;
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
