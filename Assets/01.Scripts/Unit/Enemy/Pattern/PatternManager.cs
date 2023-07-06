using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    private Pattern _currentPattern;
    public Pattern CurrentPattern => _currentPattern;

    private Pattern _beforePattern;
    public Pattern BeforePattern => _beforePattern;

    public List<Pattern> patternList = new List<Pattern>();
    private Dictionary<string, Pattern[]> patternTreeDic = new Dictionary<string, Pattern[]>();
    private int _index = -1;

    private string _treeName;
    private bool _treeChange = false;

    [Header("UI")]
    [SerializeField] private SpriteRenderer _patternSprite;
    [SerializeField] private TextMeshPro _patternText;
    [SerializeField] private int _patternEffectCount = 8;

    private bool _isEffecting = false;
    public bool IsEffecting => _isEffecting;
    public bool isPatternActioning = false;

    public void Init()
    {
        foreach (var pattern in transform.GetComponentsInChildren<Transform>())
        {
            if (pattern.name.Contains("Tree"))
            {
                Pattern[] list = pattern.GetComponentsInChildren<Pattern>();
                patternTreeDic.Add(pattern.name, list);
            }
            else
            {
                Pattern cPattern = pattern.GetComponent<Pattern>();
                if (cPattern != null)
                {
                    if (cPattern.isIncluding) patternList.Add(cPattern);
                }
            }
        }
    }

    /// <summary>
    /// 패턴 트리 변경하는 함수 (인자를 아무것도 안 넘기면 기본트리로 돌아옴)
    /// </summary>
    /// <param name="treeName"></param>
    public void ChangeTree(string treeName = "")
    {
        if (treeName == "")
        {
            _treeChange = false;
            return;
        }

        if (treeName == _treeName) return;

        if (patternTreeDic.ContainsKey(treeName))
        {
            _treeChange = true;
            _treeName = treeName;

            _index = 0;
            ChangePattern(patternTreeDic[_treeName][_index]);
        }
    }

    public void ChangePattern(Pattern pattern)
    {
        _beforePattern = _currentPattern;
        _currentPattern = pattern;
        UpdatePatternUI();
    }

    public void NextPattern()
    {
        _index++;

        if (_treeChange)
        {
            if (_index == patternTreeDic[_treeName].Length)
            {
                _index = 0;
            }
            ChangePattern(patternTreeDic[_treeName][_index]);
            return;
        }

        if (_index == patternList.Count)
        {
            _index = 0;
        }

        ChangePattern(patternList[_index]);
    }

    public void TurnAction()
    {
        if (BattleManager.Instance.Enemy.isTurnSkip) return;
        if (BattleManager.Instance.Enemy.IsDie) return;

        isPatternActioning = !(BattleManager.Instance.Enemy.PatternManager.CurrentPattern.turnPatternAction.Count == 0);
        _isEffecting = true;
        _currentPattern.TurnAction();
        StartCoroutine(PatternEffectCoroutine());
    }

    public void StartAction()
    {
        _currentPattern.StartAction();
    }

    public void EndAction()
    {
        _currentPattern.EndAction();
    }

    public void UpdatePatternUI()
    {
        _patternSprite.sprite = _currentPattern.icon;
        _patternSprite.transform.localScale = _currentPattern.iconSize;
        _patternText.text = _currentPattern.desc;
    }

    public Pattern GetNextPattern()
    {
        Pattern p = (_index + 1 == patternList.Count) ? patternList[0] : patternList[_index + 1];
        return p;
    }

    private IEnumerator PatternEffectCoroutine()
    {
        Transform trm = _patternSprite.transform.parent;

        for(int i = 0; i < _patternEffectCount; i++)
        {
            SpriteRenderer sprite = Managers.Resource.Instantiate("UI/PatternIcon", trm).GetComponent<SpriteRenderer>();
            sprite.sprite = _currentPattern.icon;
            sprite.DOFade(1, 0);

            sprite.transform.localPosition = Vector2.zero;

            Sequence seq = DOTween.Sequence();
            seq.Append(sprite.transform.DOScale(_currentPattern.iconSize * 1.5f, 0.3f));
            seq.Join(sprite.DOFade(0, 0.3f));
            seq.AppendCallback(()=>Managers.Resource.Destroy(sprite.gameObject));

            if(_patternEffectCount - 1 == i)
            {
                _patternSprite.sprite = null;
                _patternText.SetText("");
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.4f);
        _isEffecting = false;
        PatternEnd();
    }

    public void PatternEnd()
    {
        if (_isEffecting) return;
        if (isPatternActioning) return;
        if (BattleManager.Instance.Enemy.IsDie) return;

        BattleManager.Instance.TurnChange();

    }
}
