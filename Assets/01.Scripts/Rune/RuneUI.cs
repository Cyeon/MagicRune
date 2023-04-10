using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuneUI : MonoBehaviour
{
    public Dial Dial;
    public DialElement DialElement;

    private Rune _rune;
    public Rune Rune => _rune;

    #region UI
    private Transform _runeArea;
    private SpriteRenderer _runeImage;
    //private GameObject _outline;
    private TextMeshPro _coolTimeText;
    private SpriteRenderer _xImage;
    #endregion

    [SerializeField]
    private Material[] _outlineMaterialArray;

    private void Awake()
    {
        _runeArea = transform.Find("MagicArea");
        _runeImage = _runeArea.Find("MagicImage").GetComponent<SpriteRenderer>();
        //_outline = _magicArea.Find("Outline").gameObject;
        _coolTimeText = _runeArea.Find("CoolTimeText").GetComponent<TextMeshPro>();
        _xImage = _runeArea.Find("X Image").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _coolTimeText.gameObject.SetActive(false);
        RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
        _xImage.gameObject.SetActive(false);
    }

    //private void OnBecameInvisible()
    //{
    //    DialElement.MoveRune(0, false); // 이 함수를 부르는 것 맞는 거 같은데 일단 여기서는 아님
    //}

    public void SetActiveOutline(OutlineType type)
    {
        //_magicImage.material = _outlineMaterialArray[(int)type];
    }

    public void SetRune(Rune magic)
    {
        _rune = magic;
    }

    public void UpdateUI()
    {
        _runeImage.sprite = _rune.GetRune().RuneImage;
    }

    public void RuneColor(Color color)
    {
        _runeImage.color = color;
    }

    public void SetCoolTime()
    {
        if (_rune.GetCoolTime() > 0)
        {
            //_runeImage.color = Color.gray;
            SetActiveOutline(OutlineType.Default);
            _coolTimeText.SetText(_rune.GetCoolTime().ToString());
            _coolTimeText.gameObject.SetActive(true);
            RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
            _xImage.gameObject.SetActive(true);
        }
        else
        {
            //_runeImage.color = Color.white;
            _coolTimeText.gameObject.SetActive(false);
            _xImage.gameObject.SetActive(false);
        }
    }
}
