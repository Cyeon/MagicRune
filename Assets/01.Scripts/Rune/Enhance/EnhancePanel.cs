using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnhancePanel : MonoBehaviour
{
    private BaseRune _selectRune;

    [SerializeField]
    private Transform _runeArea;
    private List<BasicRunePanel> _runeList = new List<BasicRunePanel>();

    [SerializeField]
    private ExplainPanel _beforeRune;
    [SerializeField]
    private ExplainPanel _afterRune;

    [SerializeField]
    private Button _enhanceBtn;
    [SerializeField]
    private Button _exitBtn;

    private void OnEnable()
    {
        _beforeRune.SetUI(null, false, false);
        _afterRune.SetUI(null, false, false);

        _enhanceBtn.onClick.RemoveAllListeners();
        _enhanceBtn.onClick.AddListener(() =>
        {
            StartCoroutine(EnhanceCoroutine());
        });
        _exitBtn.onClick.RemoveAllListeners();
        _exitBtn.onClick.AddListener(() =>
        {
            _selectRune = null;

            //_restUI.SetActiveExplainPanel(true);
            //_restUI.SetActiveEnhancePanel(false);
            //_restUI.Dial.gameObject.SetActive(true);
            //_restUI.NextStage();

            //Managers.Map.NextStage();
            Managers.Resource.Destroy(this.gameObject);
        });
    }

    void Start()
    {
       
    }

    public IEnumerator EnhanceCoroutine()
    {
        if (_selectRune == null) yield break;

        // 강화 이펙트 생성
        // 이펙트 시간뒤에 강화하기
        yield return new WaitForSeconds(1f);

        _selectRune.Enhance();
        _selectRune = null;

        // 화면 터치하면 다음 스테이지로

        //_restUI.SetActiveExplainPanel(true);
        //_restUI.SetActiveEnhancePanel(false);
        //_restUI.Dial.gameObject.SetActive(true);

        //Managers.Map.NextStage();
        Managers.Resource.Destroy(this.gameObject);
    }

    public void CreateRune()
    {
        Clear();

        BaseRune[] notEnhanceRuneArray = Managers.Deck.Deck.Where(x => x.IsEnhanced == false && x.IsIncludeKeyword(KeywordName.CantEnhance) == false).ToArray();
        for(int i = 0; i < notEnhanceRuneArray.Length; i++)
        {
            int index = i;
            BasicRunePanel panel = Managers.Resource.Instantiate("UI/RunePanel/Basic", _runeArea).GetComponent<BasicRunePanel>();
            panel.SetUI(notEnhanceRuneArray[i], notEnhanceRuneArray[i].IsEnhanced);
            panel.GetComponent<RectTransform>().localScale = Vector3.one;
            panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            panel.ClickAction += (() => SetSelectRune(_runeList[index].Rune));
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
