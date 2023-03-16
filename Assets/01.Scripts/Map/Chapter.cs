using UnityEngine;

[System.Serializable]
public class Chapter
{
    public int chapter = 0;
    public EnemySO boss;
    [Range(0, 100)]
    public float[] eventStagesChance = new float[9];
}