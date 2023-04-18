using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class DeckManager
{
    private List<BaseRune> _defaultRune = new List<BaseRune>(20); // �ʱ� �⺻ ���� ��
    public List<BaseRune> DefaultRune => _defaultRune;

    public const int FIRST_DIAL_DECK_MAX_COUNT = 3; // ù��° ���̾� �� �ִ� ����

    private List<BaseRune> _firstDialDeck = new List<BaseRune>(); // ������ �����ص� ���̾� ������ 1��° �� ��.
    public List<BaseRune> FirstDialDeck => _firstDialDeck;

    private List<BaseRune> _deck = new List<BaseRune>(); // �����ϰ� �ִ� ��� ��
    public List<BaseRune> Deck => _deck;

    public void Init()
    {
        if (_deck.Count == 0) // ���� ������� ��� �����ص� �ʱ� ���� �־��� 
        {
            //if(_defaultRune.Count <= 0)
            //{
            //    //_defaultRune = new List<BaseRune>(Managers.Resource.Load<AllRuneListSO>("SO/DefaultRuneListSO").BaseRuneList);
            //}

            //if (_defaultRune.Count >= 0)
            //{
            //    for (int i = 0; i < _defaultRune.Count; i++)
            //    {
            //        AddRune(_defaultRune[i]);
            //    }
            //}

            AddRune(new Fire());
            AddRune(new FirePunch());
            AddRune(new Ice());
            AddRune(new MagicBullet());
            AddRune(new MagicShield());
            AddRune(new SnowBall());
            AddRune(new SnowBall());
            AddRune(new GroundShield());
            AddRune(new ShieldAttack());
            AddRune(new RailGun(), 3);
            AddRune(new Charge(), 3);

            for(int i = 0; i < _deck.Count; i++)
            {
                _deck[i].Init();
            }
        }
    }

    /// <summary> Deck�� �� �߰� </summary>
    public void AddRune(BaseRune rune, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            _deck.Add(rune);
        }
    }

    /// <summary> Deck���� �� ����� </summary>
    public void RemoveRune(BaseRune rune)
    {
        _deck.Remove(rune);
    }
 
    /// <summary> FirstDialDeck�� �� �߰� </summary>
    public void SetFirstDeck(BaseRune rune)
    {
        _firstDialDeck.Add(rune);
    }

    /// <summary> FistDialDeck���� �� ����� </summary>
    public void RemoveFirstDeck(BaseRune rune)
    {
        _firstDialDeck.Remove(rune);
    }

    public void RuneSwap(int fIndex, int sIndex)
    {
        _deck.SwapInPlace(fIndex, sIndex);
    }

    public void UsingDeckSort()
    {
        List<BaseRune> usingRune = new List<BaseRune>();
        List<BaseRune> notUsingRune = new List<BaseRune>();

        usingRune = _deck.Where(x => x.IsCoolTime == false).ToList();
        notUsingRune = _deck.Where(x => x.IsCoolTime == true).ToList();

        _deck.Clear();
        for(int i = 0; i < usingRune.Count; i++)
        {
            _deck.Add(usingRune[i]);
        }
        for (int i = 0; i < notUsingRune.Count; i++)
        {
            _deck.Add(notUsingRune[i]);
        }
    }

    public int GetUsingRuneCount()
    {
        int count = 0;
        for(int i = 0; i < _deck.Count; i++)
        {
            if (_deck[i].IsCoolTime == false)
            {
                count++;
            }
        }

        return count;
    }
}