using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MapDefine;

[System.Serializable]
public class Stage
{
    public StageType type;
    public Sprite icon;
    public Color color;

    private Image _image;
    private int _index;

    public void Init(StageType type, Image img, int idx)
    {
        this.type = type;
        _image = img;
        _index = idx;

        switch (type)
        {
            case StageType.Attack:
                ChangeResource(Color.gray, MapSceneUI.stageAtkIcon); break;
            case StageType.Event:
                ChangeResource(Color.gray, MapSceneUI.stageEventIcon); break;
            case StageType.Boss:
                ChangeResource(Color.gray, MapSceneUI.stageBossIcon); break;
            case StageType.Rest:
                ChangeResource(Color.gray, MapSceneUI.stageRestIcon); break;
        }
    }

    public void ChangeResource(Color color, Sprite sprite = null)
    {
        if (_image == null) _image = MapSceneUI.stages[_index];

        if (color != null)
        {
            _image.color = color;
            this.color = color;
        }

        if(sprite != null)
        {
            _image.sprite = sprite;
            icon = sprite;
        }
    }
}

public enum StageType
{
    Attack,
    Event,
    Boss,
    Rest
}
