using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapDial : Dial<MapRuneUI, MapRuneUI>
{
    public void Clear()
    {
        if (_dialElementList.Count == 0) return;

        for(int i = _elementDict[3].Count - 1; i >= 0; i--)
        {
            if (_elementDict[3][i] != null)
                Managers.Resource.Destroy(_elementDict[3][i].gameObject);
        }

        _dialElementList[0].SelectElement = null;
        _dialElementList[0].ElementList.Clear();
        _elementDict.Clear();
    }

    public void MapStageSpawn()
    {
        if (_dialElementList.Count == 0) return;

        for(int i = 0; i < Managers.Map.CurrentPeriodStageList.Count; i++)
        {
            StageType type = Managers.Map.CurrentPeriodStageList[i];
            MapRuneUI rune = Managers.Resource.Instantiate("Stage/" + type.ToString() + "Stage", _dialElementList[0].transform).GetComponent<MapRuneUI>();
            rune.transform.localScale = Vector3.one * 0.1f;

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
        _isAttack = false;

        _elementDict[3].Remove(_dialElementList[0].SelectElement);
        Managers.Resource.Destroy(_dialElementList[0].SelectElement.gameObject);
        Managers.Map.MapScene.mapDial.gameObject.SetActive(false);
    }
}
