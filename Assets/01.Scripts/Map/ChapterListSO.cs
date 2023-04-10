using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Map/ChapterList")]
public class ChapterListSO : ScriptableObject
{
    public List<Chapter> chapterList = new List<Chapter>();
}
