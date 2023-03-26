using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DialElement : MonoBehaviour
{
    private Dial _dial;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _dragDistance = 800;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    #region Drag Parameta
    private float _rotDamp = 3;
    [SerializeField]
    private float _touchDamp = 5f;
    private Vector3 _touchPos, _offset;

    [SerializeField]
    private bool _isUseRotateOffset;

    [SerializeField]
    private float _inDistance;
    [SerializeField]
    private float _outDistance;
    #endregion

    private int _fingerID = -1;

    private List<RuneUI> _magicList;
    private RuneUI _selectCard;
    public RuneUI SelectCard
    {
        get => _selectCard;
        set
        {
            if(value == null)
            {
                if(_selectCard != null)
                {
                    _selectCard.SetActiveOutline(OutlineType.Default);
                }
                _selectCard = value;
            }
            else
            {
                if(_selectCard != null)
                {
                    _selectCard.SetActiveOutline(OutlineType.Default);
                }
                _selectCard = value;
                _selectCard.SetActiveOutline(OutlineType.Cyan);
            }
        }
    }

    [SerializeField, Range(0f, 90f)]
    private float _selectOffset;
    private bool _isRotate = false;

    private void Awake()
    {
        _dial = GetComponentInParent<Dial>();
        _spriteRenderer = transform.Find("LineVisualSprite").GetComponent<SpriteRenderer>();
        _magicList = new List<RuneUI>();
        //_spriteRenderer.alphaHitTestMinimumThreshold = 0.04f;
    }

    private void Update()
    {
        Swipe1();

        if (_magicList.Count > 0)
        {
            float oneDinstance = 360f / _magicList.Count;
            bool inBoolean = (_spriteRenderer.transform.eulerAngles.z % oneDinstance) <= _selectOffset;
            bool outBoolean = (oneDinstance - (_spriteRenderer.transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
            if (inBoolean)
            {
                int index = (int)(_spriteRenderer.transform.eulerAngles.z / oneDinstance) % (_magicList.Count);
                if (_magicList[index].Rune.IsCoolTime == false)
                {
                    SelectCard = _magicList[index];
                    if (_isRotate == true)
                    {
                        UIManager.Instance.CardDescPopup(SelectCard.Rune);
                    }
                }
            }
            else if (outBoolean)
            {
                //int index = (int)(oneDinstance - (_image.transform.eulerAngles.z / oneDinstance)) % (_cardList.Count);
                //Debug.Log(index);
                //index = (index - 1) % _cardList.Count;
                //SelectCard = _cardList[index];

                int index = (int)(_spriteRenderer.transform.eulerAngles.z / oneDinstance) % (_magicList.Count);
                index = (index + 1) % _magicList.Count;
                if (_magicList[index].Rune.IsCoolTime == false)
                {
                    SelectCard = _magicList[index];
                    if (_isRotate == true)
                    {
                        UIManager.Instance.CardDescPopup(SelectCard.Rune);
                    }
                }
            }
            else
            {
                SelectCard = null;
                if (_isRotate == true)
                {
                    Rune rune = SelectCard == null ? null : SelectCard.Rune;
                    UIManager.Instance.CardDescPopup(rune);
                }
            }
        }

        if (_isRotate)
        {
            _offset = ((Vector3)Input.GetTouch(_fingerID).position - _touchPos);

            Vector3 rot = transform.eulerAngles;

            float temp = Input.mousePosition.x > Screen.width / 2 ? _offset.x - _offset.y : _offset.x + _offset.y;

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

    public void SetCardList(List<RuneUI> list)
    {
        _magicList = new List<RuneUI>(list);
    }

    public void Attack()
    {
        for(int i = 0; i < _selectCard.Rune.EffectList.Count; i++)
        {
            Pair pair = _selectCard.Rune.EffectList[i];
            Unit target = pair.IsEnemy == true ? BattleManager.Instance.enemy : BattleManager.Instance.player;
            AttackEffectFunction(pair.EffectType, target, pair)?.Invoke();
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
                        action = () => StatusManager.Instance.AddStatus(target, e.StatusType, (int)e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => StatusManager.Instance.AddStatus(target, e.StatusType, (int)e.Effect * c);
                        action = () => StatusManager.Instance.AddStatus(target, e.StatusType, (int)e.Effect);
                        break;
                }
                break;
            case EffectType.Destroy:
                action = () => StatusManager.Instance.RemStatus(target, e.StatusType);
                break;
            case EffectType.Draw:
                // Áö±ÝÀº ÀÏ´Ü ÁÖ¼®...
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

                    if (_magicList.Count > 0)
                    {
                        if (_isUseRotateOffset)
                        {
                            float oneDinstance = 360f / _magicList.Count;
                            bool inBoolean = (_spriteRenderer.transform.eulerAngles.z % oneDinstance) <= _selectOffset;
                            bool outBoolean = (oneDinstance - (_spriteRenderer.transform.eulerAngles.z % oneDinstance)) <= _selectOffset;
                            if (inBoolean)
                            {
                                int index = (int)(_spriteRenderer.transform.eulerAngles.z / oneDinstance) % (_magicList.Count);
                                if (_magicList[index].Rune.IsCoolTime == false)
                                {
                                    // µ¹¸®°í ÀÖÀ» ¶§¸¸
                                    DOTween.To(
                                        () => transform.eulerAngles,
                                        x => transform.eulerAngles = x,
                                        new Vector3(0, 0, ((int)(transform.eulerAngles.z / oneDinstance)) * oneDinstance),
                                        0.3f
                                    ).OnComplete(() => UIManager.Instance.CardDescPopup(_selectCard.Rune));
                                }
                            }
                            else if (outBoolean)
                            {
                                int index = (int)(transform.eulerAngles.z / oneDinstance) % (_magicList.Count);
                                index = (index + 1) % _magicList.Count;
                                if (_magicList[index].Rune.IsCoolTime == false)
                                {
                                    DOTween.To(
                                        () => transform.eulerAngles,
                                        x => transform.eulerAngles = x,
                                        new Vector3(0, 0, ((int)(transform.eulerAngles.z / oneDinstance) + 1) * oneDinstance),
                                        0.3f
                                    ).OnComplete(() => UIManager.Instance.CardDescPopup(_selectCard.Rune));
                                }
                            }
                        }
                        else
                        {
                            float oneDinstance = 360f / _magicList.Count;
                            int index = (int)(transform.eulerAngles.z / oneDinstance) % (_magicList.Count);

                            float distance = transform.eulerAngles.z % oneDinstance;
                            if (distance >= oneDinstance / 2f)
                            {
                                transform.DORotate(new Vector3(0, 0, (index + 1) % _magicList.Count * oneDinstance), 0.3f, RotateMode.Fast).OnComplete(() => UIManager.Instance.CardDescPopup(_selectCard.Rune));
                            }
                            else
                            {
                                transform.DORotate(new Vector3(0, 0, index * oneDinstance), 0.3f, RotateMode.Fast).OnComplete(() => UIManager.Instance.CardDescPopup(_selectCard.Rune));
                            }
                        }
                    }
                }

                touchEndedPos = touch.position;
                touchDif = (touchEndedPos - touchBeganPos);

                //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½. ï¿½ï¿½Ä¡ï¿½ï¿½ xï¿½Ìµï¿½ï¿½Å¸ï¿½ï¿½ï¿½ yï¿½Ìµï¿½ï¿½Å¸ï¿½ï¿½ï¿½ ï¿½Î°ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Å©ï¿½ï¿½
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("up");
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("down");
                    }
                    else if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("right");
                    }
                    else if (touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("Left");
                    }
                }
                //ï¿½ï¿½Ä¡.
                else
                {
                    //Debug.Log("touch");
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
