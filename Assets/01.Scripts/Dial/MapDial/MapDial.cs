using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapDial : Dial<MapRuneUI, MapRuneUI>
{
    public void Clear()
    {
        if (_dialElementList.Count == 0) return;

        if (_elementDict != null && _elementDict.ContainsKey(3) == true)
        {
            for (int i = _elementDict[3].Count - 1; i >= 0; i--)
            {
                if (_elementDict[3][i] != null)
                    Managers.Resource.Destroy(_elementDict[3][i].gameObject);
            }
        }

        if (_dialElementList != null || _dialElementList[0] != null)
        {
            _dialElementList[0].SelectElement = null;
            if (_dialElementList[0].ElementList != null)
            {
                _dialElementList[0].ElementList.Clear();
            }
        }

        _elementDict.Clear();
    }

    public void MapStageSpawn()
    {
        if (_dialElementList.Count == 0) return;

        for(int i = 0; i < Managers.Map.CurrentPeriodStageList.Count; i++)
        {
            StageType type = Managers.Map.CurrentPeriodStageList[i];
            MapRuneUI rune = Managers.Map.StageSpawner.SpawnStage(Managers.Map.CurrentPeriodStageList[i]).GetComponent<MapRuneUI>();
            rune.transform.SetParent(_dialElementList[0].transform);


            rune.SetInfo(rune.GetComponent<Stage>().InStage);
            AddCard(rune, 3);
            _dialElementList[0].AddRuneList(rune);
        }

        RuneSort();
    }

    public override void Attack()
    {
        if (_dialElementList[0].SelectElement == null) return;

        if (_isAttack == true) return;
        _isAttack = true;

        _dialElementList[0].SelectElement.ClickAction()?.Invoke();

        _elementDict[3].Remove(_dialElementList[0].SelectElement);
        Managers.Resource.Destroy(_dialElementList[0].SelectElement.gameObject);
        _dialElementList[0].Attack();

        _isAttack = false;
        Managers.Map.MapScene.mapDial.gameObject.SetActive(false);
    }
}
