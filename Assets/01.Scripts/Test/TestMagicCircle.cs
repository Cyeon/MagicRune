using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestMagicCircle : MonoBehaviour, IPointerClickHandler
{
    private Dictionary<RuneType, List<TestCard>> _runeDict;

    private const int _mainRuneCnt = 1;

    [SerializeField]
    private float _assistRuneDistance = 3f;

    [SerializeField]
    private float _cardAreaDistance = 5f;
    public float CardAreaDistance => _cardAreaDistance;


    [SerializeField]
    private TestCard _runeTemplate;
    [SerializeField]
    private TestCard _garbageRuneTemplate;
    [SerializeField]
    private GameObject _bgPanel;
    [SerializeField]
    private Text _nameText;

    public Enemy enemy;
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity;

    private bool _isBig = false;

    public bool IsBig
    {
        get => _isBig;
        set
        {
            if (_isBig == value) return;

            _isBig = value;

            if (_isBig)
            {
                // 크기 기우기
                transform.DOComplete();
                this.transform.DOScale(Vector3.one, 0.2f);
                //_bgPanel.GetComponent<Image>().DOFade(0.7f, 0.2f);
                _bgPanel.GetComponent<CanvasGroup>().DOFade(0.7f, 0.2f);
                this.transform.DOLocalMoveY(400, 0.2f).SetRelative();
                _bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                // 꼭 여기만 클릭해야되는건 아님
                // 마법진 클릭 시 커짐

                // 카드 선택 시 커짐

                // 카드 놓으면 작아짐

                // 작게 만들기
                transform.DOComplete();
                this.transform.DOScale(new Vector3(0.3f, 0.3f, 1), 0.2f);
                //_bgPanel.GetComponent<Image>().DOFade(0, 0.2f);
                _bgPanel.GetComponent<CanvasGroup>().DOFade(0, 0.2f);
                this.transform.DOLocalMoveY(-400, 0.2f).SetRelative();
                _bgPanel.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void Awake()
    {
        _runeDict = new Dictionary<RuneType, List<TestCard>>();
    }

    private void Update()
    {
        if (IsBig == true)
        {
            Swipe1();
        }
    }

    public void SortCard()
    {
        if (_runeDict.ContainsKey(RuneType.Assist))
        {
            float angle = -2 * Mathf.PI / _runeDict[RuneType.Assist].Count;

            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                //_runeDict[RuneType.Assist][i].GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(0, 0, -1 * angle * i + 90);
                //_runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(_assistRuneDistance, 0, 0);
                _runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * _assistRuneDistance;
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * _assistRuneDistance;
                _runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);
            }
        }

        if (_runeDict.ContainsKey(RuneType.Main))
        {
            _runeDict[RuneType.Main][0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            _runeDict[RuneType.Main][0].GetComponentInParent<RectTransform>().transform.rotation = Quaternion.identity;
        }
    }

    public bool AddCard(TestCard card)
    {
        //if (_isBig == false) return false;

        // 미리 보여준 보조 룬 근체에서 손가락을 때면 그 보조룬에 장착시키기
        if (_runeDict.ContainsKey(RuneType.Main) == false || (_runeDict[RuneType.Main].Count == 0))
        {
            if (!DummyCost.Instance.CanUseMainRune(card.Rune.MainRune.Cost))
            {
                Debug.Log("메인 룬을 사용하기 위한 마나가 부족합니다.");
                return false;
            }
            if (_runeDict.ContainsKey(RuneType.Main))
            {
                if (_runeDict[RuneType.Main].Count >= _mainRuneCnt)
                {
                    Debug.Log("占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙 占쌍쏙옙占싹댐옙.");
                    return false;
                }

                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    GameObject g = Instantiate(_garbageRuneTemplate.gameObject, this.transform);
                    card.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
                    card.transform.SetParent(this.transform);
                    g.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                    g.GetComponent<RectTransform>().DOAnchorPos(GetComponent<RectTransform>().anchoredPosition, 0.3f).OnComplete(() =>
                    {
                        Destroy(g);

                        GameObject go = Instantiate(_runeTemplate.gameObject, this.transform);
                        TestCard rune = go.GetComponent<TestCard>();
                        rune.SetRune(card.Rune);
                        rune.SetIsEquip(true);
                        rune.SetCoolTime(card.Rune.MainRune.DelayTurn);
                        _runeDict[RuneType.Main].Add(rune);

                        for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                        {
                            GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                            TestCard grune = ggo.GetComponent<TestCard>();
                            grune.SetRune(null);
                            grune.SetIsEquip(true);
                            //grune.CardAnimation();
                            if (_runeDict.ContainsKey(RuneType.Assist))
                            {
                                _runeDict[RuneType.Assist].Add(grune);
                            }
                            else
                            {
                                _runeDict.Add(RuneType.Assist, new List<TestCard> { grune });
                            }
                        }

                        SortCard();
                        AssistRuneAnimanation();
                    });
                });
            }
            else
            {
                Debug.Log(card);
                Sequence seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    Sequence seq2 = DOTween.Sequence();
                    GameObject g = Instantiate(_garbageRuneTemplate.gameObject, this.transform);
                    seq2.AppendCallback(() =>
                    {
                        card.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
                        card.transform.SetParent(this.transform);
                        g.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                        g.GetComponent<RectTransform>().DOLocalMove(Vector2.zero, 0.3f);
                    });
                    seq2.AppendCallback(() => Destroy(g));
                });
                seq.AppendInterval(0.3f);
                seq.AppendCallback(() =>
                {
                    GameObject go = Instantiate(_runeTemplate.gameObject, this.transform);
                    TestCard rune = go.GetComponent<TestCard>();
                    rune.SetRune(card.Rune);
                    rune.SetIsEquip(true);
                    rune.SetCoolTime(card.Rune.MainRune.DelayTurn);
                    _runeDict.Add(RuneType.Main, new List<TestCard>() { rune });

                    for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                    {
                        GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                        TestCard grune = ggo.GetComponent<TestCard>();
                        grune.SetRune(null);
                        grune.SetIsEquip(true);
                        //grune.CardAnimation();
                        if (_runeDict.ContainsKey(RuneType.Assist))
                        {
                            _runeDict[RuneType.Assist].Add(grune);
                        }
                        else
                        {
                            _runeDict.Add(RuneType.Assist, new List<TestCard> { grune });
                        }
                    }
                    SortCard();
                    AssistRuneAnimanation();
                });

            }
        }
        else
        {
            if (!DummyCost.Instance.CanUseSubRune(card.Rune.AssistRune.Cost))
            {
                Debug.Log("보조 룬을 사용하기 위한 마나가 부족합니다.");
                return false;
            }

            int changeIndex = -1;
            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                if (_runeDict[RuneType.Assist][i].Rune == null)
                {
                    changeIndex = i;
                    break;
                }
            }

            if (changeIndex == -1) return false;

            // 애니메이션 뒤로 미루기
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                GameObject g = Instantiate(_garbageRuneTemplate.gameObject, this.transform);
                card.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
                card.transform.SetParent(this.transform);
                g.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition;
                g.GetComponent<RectTransform>().DOAnchorPos(_runeDict[RuneType.Assist][changeIndex].GetComponent<RectTransform>().anchoredPosition, 0.3f).OnComplete(() =>
                {
                    Destroy(g);
                    _runeDict[RuneType.Assist][changeIndex].SetRune(card.Rune);
                    _runeDict[RuneType.Assist][changeIndex].SetCoolTime(card.Rune.AssistRune.DelayTurn);
                    UpdateMagicName();
                    //Sequence seq2 = DOTween.Sequence();
                    //seq2.AppendInterval(0.2f);
                    //seq2.AppendCallback(() => { IsBig = false; });
                });
            });
            SortCard();
        }

        return true;
    }

    private void UpdateMagicName()
    {
        if (_runeDict.ContainsKey(RuneType.Main))
        {
            if (_runeDict[RuneType.Main].Count > 0 && _runeDict[RuneType.Main][0].Rune != null)
            {
                string name = "";
                for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
                {
                    if (_runeDict[RuneType.Assist][i].Rune != null)
                    {
                        name += _runeDict[RuneType.Assist][i].Rune.AssistRune.Name + " ";

                        if (i == 2)
                        {
                            name += "\n";
                        }
                    }
                }
                if (name == "")
                {
                    name = _runeDict[RuneType.Main][0].Rune.MainRune.Name;
                }
                else
                {
                    name = name.Substring(0, name.Length - 1);
                    name += $"의 {_runeDict[RuneType.Main][0].Rune.MainRune.Name}";
                }
                _nameText.text = name;
            }
        }
    }

    public void AssistRuneAnimanation()
    {
        Sequence seq = DOTween.Sequence();
        foreach (var r in _runeDict[RuneType.Assist])
        {
            seq.Join(r.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.3f).From());
        }
        seq.AppendCallback(() => { UpdateMagicName(); });
        //seq.AppendInterval(0.2f);
        //seq.AppendCallback(() => { IsBig = false; });
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                touchEndedPos = touch.position;
                touchDif = (touchEndedPos - touchBeganPos);

                //스와이프. 터치의 x이동거리나 y이동거리가 민감도보다 크면
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        Debug.Log("up");
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        Debug.Log("down");
                    }
                    else if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        Debug.Log("right");
                        Debug.Log("Attack");
                        if (touchBeganPos.y <= this.GetComponent<RectTransform>().anchoredPosition.y + this.GetComponent<RectTransform>().sizeDelta.y / 2
                            && touchBeganPos.y >= this.GetComponent<RectTransform>().anchoredPosition.y - this.GetComponent<RectTransform>().sizeDelta.y / 2)
                        {
                            Damage();
                        }
                    }
                    else if (touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        Debug.Log("Left");
                    }
                }
                //터치.
                else
                {
                    Debug.Log("touch");
                }
            }
        }
    }


    public void Damage()
    {
        // 그냥 로직만 적어놓는다는 느낌, 다른 곳에다가 옮길 예정

        // 마법진 회전 효과
        // 사이에 추가로 다른 효과도 있겠지
        // 그 후에 공격

        if (_runeDict.ContainsKey(RuneType.Main) == false || _runeDict[RuneType.Main].Count == 0 || _runeDict[RuneType.Main][0].Rune == null)
        {
            // 공격 안되는 이펙트
            // 뭐.. 카메라 흔들림이라던지
            Debug.Log("메인 룬 없음");
            return;
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DORotate(new Vector3(0, 0, 360 * 5), 0.7f, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));

        //int damage = 0;
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() =>
        {
            if (_runeDict.ContainsKey(RuneType.Assist))
            {
                for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
                {
                    //damage += _runeDict[RuneType.Assist][i]._runeSO.assistRuneValue;
                    //damage += _runeDict[RuneType.Assist][i].Rune.AssistRune.EffectPair
                    TestCard card = _runeDict[RuneType.Assist][i];
                    if (card.Rune != null)
                    {
                        card.UseAssistEffect();
                    }
                }

                for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
                {
                    Destroy(_runeDict[RuneType.Assist][i].gameObject);
                }
                //damage += _runeDict[RuneType.Main][i]._runeSO.mainRuneValue;
            }

            if (_runeDict.ContainsKey(RuneType.Main))
            {
                for (int i = 0; i < _runeDict[RuneType.Main].Count; i++)
                {
                    //damage += _runeDict[RuneType.Main][i]._runeSO.mainRuneValue;
                    TestCard card = _runeDict[RuneType.Main][i];
                    if (card.Rune != null)
                    {
                        card.UseMainEffect();
                    }
                }

                for (int i = 0; i < _runeDict[RuneType.Main].Count; i++)
                {
                    Destroy(_runeDict[RuneType.Main][i].gameObject);
                }
            }
        });
        seq.AppendCallback(() =>
        {
            Debug.Log("Attack Complate");
            _runeDict.Clear();

            IsBig = false;
        });

        //enemy.Damage(damage);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsBig == false)
        {
            IsBig = true;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _cardAreaDistance);
        Gizmos.color = Color.white;
    }
#endif
}
