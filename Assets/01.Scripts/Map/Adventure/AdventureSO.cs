using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Adventure/Adventure SO")]
public class AdventureSO : ScriptableObject
{
    public string adventureName;
    public Sprite image;

    [Tooltip("���� ���丮")]
    [TextArea(0, 10)]
    public string message;

    [Tooltip("������")]
    public List<Distractor> distractors = new List<Distractor>();
}
