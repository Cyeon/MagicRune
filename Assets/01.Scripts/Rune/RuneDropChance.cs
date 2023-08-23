using System;

[Serializable]
public struct RuneDropChance
{
    public StageType stageType;

    public int normal;
    public int rare;
    public int epic;
    public int legendary;

    public int Rare => normal + rare;
    public int Epic => Rare + epic;
}
