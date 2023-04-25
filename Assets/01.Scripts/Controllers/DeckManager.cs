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
        // ���߿� Json �����ϸ� ���⼭ �ҷ����� ���� �ϰ���...

        if (_deck.Count == 0) // ���� ������� ��� �����ص� �ʱ� ���� �־��� 
        {
            // ����Ӽ�
            AddRune(new RailGun(), 3);
            AddRune(new Charge(), 3);
            AddRune(new LightingRod());
            AddRune(new Release());

            // �� �Ӽ�
            AddRune(new Fire());
            AddRune(new FirePunch());
            AddRune(new FireRegeneration());
            AddRune(new FireBreath());

            // �� �Ӽ�
            AddRune(new GroundShield());
            AddRune(new ShieldAttack());
            AddRune(new Attack());
            AddRune(new ThreeAttack());

            // ���� �Ӽ�
            AddRune(new Ice());
            AddRune(new SnowBall(), 2);
            AddRune(new IceShield(), 2);
            AddRune(new IceSmash(), 2);

            // ���Ӽ�
            AddRune(new MagicBullet());
            AddRune(new MagicShield());

            //SetDefaultDeck(Managers.Resource.Load<DeckSO>("SO/Deck/TestDeck").RuneList);

            RuneInit();
        }
    }

    public void SetDefaultDeck(List<BaseRuneSO> runeList)
    {
        for(int i = 0; i < runeList.Count; i++)
        {
            AddRune(Managers.Rune.GetRune(runeList[i]));
        }

        RuneInit();
    }

    private void RuneInit()
    {
        for (int i = 0; i < _deck.Count; i++)
        {
            _deck[i].Init();
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
        _deck.SwapInPlace(fIndex, sIndex); //
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

    public BaseRune GetRandomRune(List<BaseRune> ignoreRuneList = null)
    {
        List<BaseRune> newRuneList = new List<BaseRune>(Deck);

        if (ignoreRuneList != null)
        {
            for (int i = 0; i < ignoreRuneList.Count; i++)
            {
                newRuneList.Remove(ignoreRuneList[i]);
            }
        }
        int idx = Random.Range(0, newRuneList.Count);
        return newRuneList[idx];
    }
}