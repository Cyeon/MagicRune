using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// Healing, IncreaseMaxHp ��� �� Amount�� �������� percent�� ����ؼ� �������� ���� ���ֱ� 
    /// </summary>
    /// <param name="num">0->Amount, 1->Percent</param>
    public void SetMode(int num) // Inspector���� ����ϱ� ���� enum�� �ƴ� int ��
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
            GameManager.Instance.player.AddHP(amount);
        else if (increaseMode == IncreaseMode.Percent)
            GameManager.Instance.player.AddHPPercent(amount);

        MapSceneUI.InfoUIReload();
    }

    public void AddMaxHp(int amount)
    {
        GameManager.Instance.player.AddMaxHp(amount);
        MapSceneUI.InfoUIReload();
    }

    public void AddGold(int amount)
    {
        GameManager.Instance.AddGold(amount);
        MapSceneUI.InfoUIReload();
    }
    public void BattleEnemy(Enemy enemy)
    {
        Managers.Map.SelectEnemy = enemy;
        Managers.Scene.LoadScene(Define.Scene.DialScene);
    }
}