using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;

public class DeckManager
{
    private List<BaseRune> _defaultRune = new List<BaseRune>(20); // 초기 기본 지급 룬
    public List<BaseRune> DefaultRune => _defaultRune;

    public const int FIRST_DIAL_DECK_MAX_COUNT = 3; // 첫번째 다이얼 덱 최대 개수

    private List<BaseRune> _firstDialDeck = new List<BaseRune>(); // 사전에 설정해둔 다이얼 안쪽의 1번째 줄 덱.
    public List<BaseRune> FirstDialDeck => _firstDialDeck;

    private List<BaseRune> _deck = new List<BaseRune>(); // 소지하고 있는 모든 룬
    public List<BaseRune> Deck => _deck;

    public void Init()
    {
        // 나중에 Json 저장하면 여기서 불러오기 등을 하겠지...

        if (_deck.Count == 0) // 덱이 비어있을 경우 설정해둔 초기 덱을 넣어줌 
        {
            // 전기속성
            AddRune(new RailGun(), 3);
            AddRune(new Charge(), 3);
            AddRune(new LightingRod());
            AddRune(new Release());

            // 불 속성
            AddRune(new Fire());
            AddRune(new FirePunch());
            AddRune(new FireRegeneration());
            AddRune(new FireBreath());

            // 땅 속성
            AddRune(new GroundShield());
            AddRune(new ShieldAttack());
            AddRune(new Attack());
            AddRune(new ThreeAttack());

            // 얼음 속성
            AddRune(new Ice());
            AddRune(new SnowBall(), 2);
            AddRune(new IceShield(), 2);
            AddRune(new IceSmash(), 2);

            // 무속성
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

    /// <summary> Deck에 룬 추가 </summary>
    public void AddRune(BaseRune rune, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            _deck.Add(rune);
        }
    }

    /// <summary> Deck에서 룬 지우기 </summary>
    public void RemoveRune(BaseRune rune)
    {
        _deck.Remove(rune);
    }
 
    /// <summary> FirstDialDeck에 룬 추가 </summary>
    public void SetFirstDeck(BaseRune rune)
    {
        _firstDialDeck.Add(rune);
    }

    /// <summary> FistDialDeck에서 룬 지우기 </summary>
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