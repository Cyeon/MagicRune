using DG.Tweening;
using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialElement : MonoBehaviour
{
    private Dial _dial;
    private SpriteRenderer _lineSpriteRenderer;
    private SpriteRenderer _textSpriteRenderer;
    [SerializeField]
    private Sprite _textSprite;
    [SerializeField]
    private Sprite _glowTextSprite;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    #region Drag Parameta
    private float _rotDamp = 3;
    private Vector3 _touchPos, _offset;

    [SerializeField]
    private bool _isUseRotateOffset;

    [SerializeField]
    private float _inDistance;
    [SerializeField]
    private float _outDistance;
    #endregion

    [SerializeField]
    private float _runePoolOffset = 5f;

    private int _fingerID = -1;

    private int _lineID = -1;

    private List<RuneUI> _runeList;
    private RuneUI _selectCard;
    public RuneUI SelectCard
    {
        get => _selectCard;
        set
        {
            if (value == null)
            {
                if (_selectCard != null)
                {
                    _selectCard.SetActiveOutline(OutlineType.Default);
                    _selectCard.RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
                }
                _selectCard = value;
            }
            else
            {
                if (_selectCard != null)
                {
                    _selectCard.SetActiveOutline(OutlineType.Default);
                    _selectCard.RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
                }
                _selectCard = value;
                _selectCard.SetActiveOutline(OutlineType.Cyan);
                _selectCard.RuneColor(Color.white);
            }
        }
    }

    [SerializeField, Range(0f, 90f)]
    private float _selectOffset;
    private bool _isRotate = false;

    public bool IsGlow
    {
        set
        {
            if(value == true)
            {
                _textSpriteRenderer.sprite = _glowTextSprite;
            }
            else
            {
                _textSpriteRenderer.sprite = _textSprite;
            }
        }
    }

    DialScene _dialScene = null;

    private void Awake()
    {
        _dial = GetComponentInParent<Dial>();
        _lineSpriteRenderer = transform.Find("LineVisualSprite").GetComponent<SpriteRenderer>();
        _textSpriteRenderer = transform.Find("TextVisualSprite").GetComponent<SpriteRenderer>();
        _runeList = new List<RuneUI>();
        //_spriteRenderer.alphaHitTestMinimumThreshold = 0.04f;
    }

    private void Start()
    {
        _dialScene = Managers.Scene.CurrentScene as DialScene;

        transform.rotation = Quaternion.Euler(Vector3.zero);
        IsGlow = false;
    }

    private void Update()
    {
        Swipe1();

        if (_runeList.Count > 0 && BattleManager.Instance.IsPlayerTurn())
        {
            // UI Ǯ���ϸ鼭 �� �κ� ������
            //if (transform.eulerAngles.z <= _dial.RuneAngle / 2 || transform.eulerAngles.z >= (360f - _dial.RuneAngle / 2))
            //{
                float oneDinstance = _dial.RuneAngle / _runeList.Count;
                bool inBoolean = (transform.eulerAngles.z % oneDinstance) <= _selectOffset;
                bool outBoolean = (oneDinstance - (transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
                if (inBoolean)
                {
                    int index = (int)(transform.eulerAngles.z / oneDinstance) % (_runeList.Count);
                    //index = (index + 1) % _runeList.Count; // �߰���
                    if (_runeList[index].Rune.IsCoolTime == false)
                    {
                        SelectCard = _runeList[index];
                        if (_isRotate == true)
                        {
                            if (_selectCard != null)
                            {
                                _dialScene?.CardDescPopup(_selectCard.Rune);
                            }
                        }
                    }
                }
                else if (outBoolean)
                {
                    int index = (int)(transform.eulerAngles.z / oneDinstance) % (_runeList.Count);
                    //index = (index - 1 < 0 ? _runeList.Count - 1 : index - 1) % (_runeList.Count);
                    index = (index + 1) % (_runeList.Count);

                    if (_runeList[index].Rune.IsCoolTime == false)
                    {
                        SelectCard = _runeList[index];
                        if (_isRotate == true)
                        {
                            if (_selectCard != null)
                            {
                                _dialScene?.CardDescPopup(_selectCard.Rune);
                            }
                        }
                    }
                }
                else
                {
                    SelectCard = null;
                    if (_isRotate == true)
                    {
                        Rune rune = SelectCard == null ? null : SelectCard.Rune;
                        _dialScene?.CardDescPopup(rune);
                    }
                }
            //}
        }

        if (_isRotate && BattleManager.Instance.IsPlayerTurn())
        {
            _offset = ((Vector3)Input.GetTouch(_fingerID).position - _touchPos);

            Vector3 rot = transform.eulerAngles;

            float temp = Input.GetTouch(_fingerID).position.x > Screen.width / 2 ? _offset.x - _offset.y : _offset.x + _offset.y;

            if (Mathf.Abs(_offset.x) > Mathf.Abs(_offset.y))
            {
                if (_offset.x > 0)
                    temp = Mathf.Clamp(temp, 0, _offset.x);
                else
                    temp = Mathf.Clamp(temp, _offset.x, 0);
            }
            else
            {
                if (_offset.y > 0)
                    temp = Mathf.Clamp(temp, -_offset.y, _offset.y);
                else
                    temp = Mathf.Clamp(temp, _offset.y, -_offset.y);
            }

            rot.z += -1 * temp / _rotDamp;

            transform.rotation = Quaternion.Euler(rot);
            _touchPos = Input.GetTouch(_fingerID).position;
        }


    }

    [Obsolete]
    public void MoveRune(int index, bool isLeft = true)
    {
        if(isLeft == true)
        {
            RuneUI rune = _runeList[index];

            float radianValue = 0f;
            if(index - 1 < 0)
            {
                radianValue = (_dial.RuneAngle / _runeList.Count) * (index - 1) + (_dial.StartAngle * Mathf.Deg2Rad);
            }
            else
            {
                radianValue = (_dial.RuneAngle - (_dial.RuneAngle / _runeList.Count)) + (_dial.StartAngle * Mathf.Deg2Rad);
            }

            float height = Mathf.Sin(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            float width = Mathf.Cos(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            _runeList[index].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

            Vector2 direction = new Vector2(
                _runeList[index].transform.position.x - transform.position.x,
                _runeList[index].transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            _runeList[index].transform.rotation = angleAxis;

            _runeList.RemoveAt(index);
            _runeList.Insert(0, rune);
        }
        else
        {
            RuneUI rune = _runeList[index];

            float radianValue = (_dial.RuneAngle / _runeList.Count) * (index + 1) + (_dial.StartAngle * Mathf.Deg2Rad);

            float height = Mathf.Sin(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            float width = Mathf.Cos(radianValue) * _dial.LineDistanceArray[3 - _lineID];
            _runeList[index].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

            Vector2 direction = new Vector2(
                _runeList[index].transform.position.x - transform.position.x,
                _runeList[index].transform.position.y - transform.position.y
            );

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
            _runeList[index].transform.rotation = angleAxis;

            _runeList.RemoveAt(index);
            _runeList.Add(rune);
        }
    }

    public void SetLineID(int id)
    {
        _lineID = id;
    }

    public int GetLineID()
    {
        return _lineID;
    }

    public void AddRuneList(RuneUI rune)
    {
        _runeList.Add(rune);
    }

    public void SetRuneList(List<RuneUI> list)
    {
        _runeList = new List<RuneUI>(list);
    }

    public void ResetRuneList()
    {
        _runeList.Clear();
    }

    public bool CheckCondition(Condition condition)
    {
        ConditionType conditionType = condition.ConditionType;
        switch (conditionType)
        {
            case ConditionType.StatusComparison:
                switch (condition.ComparisonType)
                {
                    case ComparisonType.MoreThan:
                        if (!(BattleManager.Instance.player.StatusManager.GetStatus(condition.StatusType)?.TypeValue >= condition.Value))
                                return false;
                        break;

                    case ComparisonType.LessThan:
                        if (!(BattleManager.Instance.player.StatusManager.GetStatus(condition.StatusType)?.TypeValue <= condition.Value))
                            return false;
                        break;

                }
                break;
        }
        return true;
    }

    public void Attack()
    {
        if (BattleManager.Instance.enemy.IsDie == false && _selectCard != null)
        {
            for (int i = 0; i < _selectCard.Rune.EffectList.Count; i++)
            {
                Pair pair = _selectCard.Rune.EffectList[i];
                Unit target = pair.IsEnemy == true ? BattleManager.Instance.enemy : BattleManager.Instance.player;
                if (CheckCondition(_selectCard.Rune.EffectList[i].Condition))
                {
                    AttackEffectFunction(pair.EffectType, target, pair)?.Invoke();
                }
            }

            _selectCard.Rune.SetCoolTime(_selectCard.Rune.GetRune().CoolTime);
            _selectCard.SetCoolTime();
            SelectCard = null;
        }
    }

    public Action AttackEffectFunction(EffectType effectType, Unit target, Pair e)
    {
        Action action = null;
        //int c = 0;
        //for (int i = 0; i < _effectDict[RuneType.Assist].Count; i++)
        //{
        //    if (_runeTempDict[RuneType.Assist][i].Rune != null && _runeTempDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.AttributeType)
        //    {
        //        c++;
        //    }
        //}
        //if (_runeTempDict[RuneType.Main][0].Rune != null && _runeTempDict[RuneType.Main][0].Rune.AssistRune.Attribute == e.AttributeType)
        //    c++;

        switch (effectType)
        {
            case EffectType.Attack:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => BattleManager.Instance.player.Attack(e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => GameManager.Instance.player.Attack(e.Effect * c);
                        action = () => BattleManager.Instance.player.Attack(e.Effect);
                        break;
                    case AttackType.Defence:
                        action = () => BattleManager.Instance.player.Attack(BattleManager.Instance.player.Shield);
                        break;
                }
                break;
            case EffectType.Defence:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => BattleManager.Instance.player.AddShield(e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => GameManager.Instance.player.Shield += e.Effect * c;
                        action = () => BattleManager.Instance.player.AddShield(e.Effect);
                        break;
                }
                break;
            case EffectType.Status:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => target.StatusManager.AddStatus(e.StatusType, (int)e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => StatusManager.Instance.AddStatus(target, e.StatusType, (int)e.Effect * c);
                        action = () => target.StatusManager.AddStatus(e.StatusType, (int)e.Effect);
                        break;
                }
                break;
            case EffectType.DestroyStatus:
                switch (e.CountType)
                {
                    case CountType.Count:
                        action = () => target.StatusManager.RemoveStatus(e.StatusType, (int)e.Effect);
                        break;
                    case CountType.All:
                        action = () => target.StatusManager.DeleteStatus(e.StatusType);
                        break;
                }
                break;
            case EffectType.Draw:
                // ������ �ϴ� �ּ�...
                //action = () => _cardCollector.CardDraw(e.Effect);
                break;
            case EffectType.Etc:
                action = null;
                break;
        }

        return action;
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;

                float distance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(touchBeganPos), (Vector2)this.transform.position);
                if (distance >= _inDistance &&
                    distance <= _outDistance)
                {
                    if (_isRotate == true) return;

                    _fingerID = touch.fingerId;
                    _isRotate = true;

                    _touchPos = touch.position;
                }
            }
            if (touch.phase == TouchPhase.Moved)
            {

            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (_isRotate == true)
                {
                    _fingerID = -1;
                    _isRotate = false;

                    if (_runeList.Count > 0 && BattleManager.Instance.IsPlayerTurn())
                    {
                        //if (transform.eulerAngles.z <= _dial.RuneAngle / 2 + transform.eulerAngles.z || transform.eulerAngles.z >= (360f - _dial.RuneAngle / 2))
                        //{
                            if (_isUseRotateOffset)
                            {
                                float oneDinstance = _dial.RuneAngle / _runeList.Count;
                                bool inBoolean = (transform.eulerAngles.z % oneDinstance) <= _selectOffset;
                                bool outBoolean = (oneDinstance - (transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
                                if (inBoolean)
                                {
                                    int index = (int)(transform.eulerAngles.z / oneDinstance) % (_runeList.Count);
                                    //index = Mathf.Clamp(index, 0, _runeList.Count - 1);
                                    //if (_magicList[index].Rune.IsCoolTime == false)
                                    //{
                                    // ������ ���� ����
                                    DOTween.To(
                                        () => transform.eulerAngles,
                                        x => transform.eulerAngles = x,
                                        new Vector3(0, 0, ((int)(transform.eulerAngles.z / oneDinstance)) * oneDinstance),
                                        0.3f
                                    ).OnComplete(() =>
                                    {
                                        if (_selectCard != null) { _dialScene?.CardDescPopup(_selectCard.Rune); }
                                    });
                                    //}
                                }
                                else if (outBoolean)
                                {
                                    int index = ((int)(transform.eulerAngles.z / oneDinstance) + 1) % (_runeList.Count);
                                    index = (index + 1) % _runeList.Count;
                                    //index = Mathf.Clamp(index + 1, 0, _runeList.Count - 1);
                                    //if (_magicList[index].Rune.IsCoolTime == false)
                                    //{
                                    DOTween.To(
                                        () => transform.eulerAngles,
                                        x => transform.eulerAngles = x,
                                        new Vector3(0, 0, ((int)(transform.eulerAngles.z / oneDinstance)) * oneDinstance + _dial.StartAngle),
                                        0.3f
                                    ).OnComplete(() =>
                                    {
                                        if (_selectCard != null) { _dialScene?.CardDescPopup(_selectCard.Rune); }
                                    });
                                    //}
                                }
                            }
                            else
                            {
                                float oneDinstance = _dial.RuneAngle / _runeList.Count;
                                int index = (int)(transform.eulerAngles.z / oneDinstance) % (_runeList.Count);

                                float distance = transform.eulerAngles.z % oneDinstance;
                                if (distance >= oneDinstance / 2f)
                                {
                                    transform.DORotate(new Vector3(0, 0, ((index + 1) % _runeList.Count * oneDinstance)/* >= 120 ?
                                        ((index + 1) % _runeList.Count * oneDinstance) + 360f - oneDinstance * _runeList.Count :
                                        (index + 1) % _runeList.Count * oneDinstance*/), 0.3f, RotateMode.Fast)
                                        .OnComplete(() =>
                                        {
                                            //SelectCard = _runeList[index];
                                            if (_selectCard != null) { _dialScene?.CardDescPopup(_selectCard.Rune); }
                                        });
                                }
                                else
                                {
                                    transform.DORotate(new Vector3(0, 0, ((index) * oneDinstance)/* >= 120 ?
                                        ((index) * oneDinstance) + 360f - oneDinstance * _runeList.Count :
                                        ((index) * oneDinstance)*/), 0.3f, RotateMode.Fast)
                                        .OnComplete(() =>
                                        {
                                            //SelectCard = _runeList[index]; // ���� ���� ���ϴ� �ְ� �ȵ���?�� ����â ������ ������?
                                            if (_selectCard != null) { _dialScene?.CardDescPopup(_selectCard.Rune); }
                                        });
                                }
                            }
                        //}
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _inDistance);
        Gizmos.DrawWireSphere(this.transform.position, _outDistance);
        Gizmos.color = Color.white;
    }
#endif
}