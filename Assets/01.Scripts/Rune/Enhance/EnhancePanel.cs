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

    [SerializeField]
    private ExplainPanel _beforeRune;
    [SerializeField]
    private ExplainPanel _afterRune;

    private RestUI _restUI;

    private void OnEnable()
    {
        _beforeRune.SetUI(null, false, false);
        _afterRune.SetUI(null, false, false);
    }

    void Start()
    {
        _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();

        Managers.UI.Bind<Button>("EnhanceBtn", this.gameObject);
        Managers.UI.Bind<Button>("ExitBtn", this.gameObject);

        Managers.UI.Get<Button>("EnhanceBtn").onClick.AddListener(() =>
        {
            StartCoroutine(EnhanceCoroutine());
        });
        Managers.UI.Get<Button>("ExitBtn").onClick.AddListener(() =>
        {
            _selectRune = null;

            _restUI.SetActiveExplainPanel(true);
            _restUI.SetActiveEnhancePanel(false);
            _restUI.Dial.gameObject.SetActive(true);
            _restUI.NextStage();
        });
    }

    public IEnumerator EnhanceCoroutine()
    {
        // 강화 이펙트 생성
        // 이펙트 시간뒤에 강화하기
        yield return new WaitForSeconds(1f);

        _selectRune.Enhance();
        _selectRune = null;
        
        // 화면 터치하면 다음 스테이지로

        _restUI.SetActiveExplainPanel(true);
        _restUI.SetActiveEnhancePanel(false);
        _restUI.Dial.gameObject.SetActive(true);
        _restUI.NextStage();
    }

    public void CreateRune()
    {
        Debug.Log("Create Rune!");

        Clear();

        BaseRune[] notEnhanceRuneArray = Managers.Deck.Deck.Where(x => x.IsEnhanced == false && x.IsIncludeKeyword(KeywordType.CantEnhance) == false).ToArray();
        for(int i = 0; i < notEnhanceRuneArray.Length; i++)
        {
            int index = i;
            ExplainPanel panel = Managers.Resource.Instantiate("UI/Explain_Panel", _runeArea).GetComponent<ExplainPanel>();
            panel.SetUI(notEnhanceRuneArray[i], isReward: false);
            panel.GetComponent<RectTransform>().localScale = Vector3.one;
            panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            panel.SetAction(() => SetSelectRune(_runeList[index].Rune));
            _runeList.Add(panel);
        }
    }

    public void Clear()
    {
        for(int i = _runeList.Count - 1; i >= 0; i--)
        {
            Managers.Resource.Destroy(_runeList[i].gameObject);
        }

        _runeList.Clear();
    }

    public void SetSelectRune(BaseRune rune)
    {
        _selectRune = rune;

        _beforeRune.SetUI(rune, false, false);
        _afterRune.SetUI(rune, true, false);
    }

    public void Enhace()
    {
        _selectRune.Enhance();
        _selectRune = null;

        Clear();
    }
}
