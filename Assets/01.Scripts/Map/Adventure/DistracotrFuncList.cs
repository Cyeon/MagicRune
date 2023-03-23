using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MapDefine;

public enum DistractorFunc
{
    NextStage,
    Healing
}

public enum IncreaseMode
{
    Amount,
    Percent,
    Unknown
}
public class DistracotrFuncList : MonoBehaviour
{
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

    public void NextStage()
    {
        MapManager.Instance.NextStage();
        MapSceneUI.adventureUI.gameObject.SetActive(false);
    }

    public void AddHp(int amount)
    {
        if (increaseMode == IncreaseMode.Amount)
            GameManager.Instance.player.AddHP(amount);
        else if (increaseMode == IncreaseMode.Percent)
            GameManager.Instance.player.AddHPPercent(amount);

        MapSceneUI.InfoUIReload();
        NextStage();
    }

    public void AddMaxHp(int amount)
    {
        GameManager.Instance.player.AddMaxHp(amount);
        MapSceneUI.InfoUIReload();
        NextStage();
    }

    public void AddGold(int amount)
    {
        GameManager.Instance.AddGold(amount);
        MapSceneUI.InfoUIReload();
        NextStage();
    }
    public void BattleEnemy(EnemySO enemy)
    {
        MapManager.Instance.selectEnemy = enemy;
        SceneManager.LoadScene("DialScene");
    }
}