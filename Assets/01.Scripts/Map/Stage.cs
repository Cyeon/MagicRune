using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage
{
    public StageType type;
    public Sprite icon
    {
        get => icon;
        set
        {
            icon = value;
            _image.sprite = value;
        }
    }
    public Color color
    {
        get => color;
        set
        {
            color = value;
            _image.color = value;
        }
    }

    private Image _image;

    public void Init(StageType type, Image img)
    {
        this.type = type;
        _image = img;

        switch (type)
        {
            case StageType.Attack:
                icon = MapManager.Instance.ui.stageAtkIcon; break;
            case StageType.Event:
                icon = MapManager.Instance.ui.stageEventIcon; break;
            case StageType.Boss:
                icon = MapManager.Instance.ui.stageBossIcon; break;
        }

        color = Color.gray;
    }
}

public enum StageType
{
    Attack,
    Event,
    Boss
}
