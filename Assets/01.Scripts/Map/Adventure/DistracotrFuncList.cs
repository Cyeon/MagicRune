using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IncreaseMode
{
    Amount,
    Percent,
    Unknown
}
public class DistracotrFuncList : MonoBehaviour
{
    private MapUI _mapSceneUI;
    public MapUI MapSceneUI
    {
        get
        {
            if (_mapSceneUI == null)
            {
                _mapSceneUI = Managers.Canvas.GetCanvas("MapUI").GetComponent<MapUI>();
            }
            return _mapSceneUI;
        }
    }

    private IncreaseMode increaseMode = IncreaseMode.Unknown;

    private float _index;

    /// <summary>
    /// Healing, IncreaseMaxHp 사용 전 Amount로 더해줄지 percent로 계산해서 더해줄지 결정 해주기 
    /// </summary>
    /// <param name="num">0->Amount, 1->Percent</param>
    public void SetMode(int num) // Inspector에서 사용하기 위해 enum이 아닌 int 형
    {
        Debug.Log($"Set IncreaseMode {(IncreaseMode)num}");
        increaseMode = (IncreaseMode)num;
    }

    public static void NextStage()
    {
        Managers.Map.NextStage();
        Managers.Canvas.GetCanvas("Adventure").enabled = false;
        Managers.Canvas.GetCanvas("MapUI").enabled = true;
        //MapSceneUI.adventureUI.gameObject.SetActive(false);
    }

    public void AddHp(int amount)
    {
        if (increaseMode == IncreaseMode.Amount)
            Managers.GetPlayer().AddHP(amount);
        else if (increaseMode == IncreaseMode.Percent)
            Managers.GetPlayer().AddHPPercent(amount);
    }

    public void AddMaxHp(int amount)
    {
        Managers.GetPlayer().AddMaxHp(amount);
    }

    public void AddGold(int amount)
    {
        Managers.Gold.AddGold(amount);
    }

    public void IndexSetting(float index)
    {
        _index = index;
    }

    public void BattleEnemy(Enemy enemy)
    {
        Managers.Enemy.AddEnemy(enemy);
        Managers.Scene.LoadScene(Define.Scene.DialScene);
    }

    public void RandomRuneCopy()
    {
        BaseRune baseRune = Managers.Deck.GetRandomRune();
        Managers.Deck.AddRune(baseRune);

        EventManager<BaseRune>.TriggerEvent(Define.SELECT_RUNE_EVENT, baseRune);
    }

    public void DeleteRune()
    {
        EventManager<RuneSelectMode>.TriggerEvent(Define.RUNE_EVENT_SETTING, RuneSelectMode.Delete);
    }
}