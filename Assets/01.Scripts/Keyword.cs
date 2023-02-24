using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum keywordEnum
{
    extinction,
    fire
}

[System.Serializable]
public class KeywordInfo
{
    public keywordEnum keyword;
    public string keywordName;
    [TextArea(5, 5)]
    public string information;
}

public class Keyword : MonoBehaviour
{
    public GameObject keywordPanel;

    public List<KeywordInfo> keywordList = new List<KeywordInfo>();

    public GameObject KeywordInit(keywordEnum keyword)
    {
        GameObject obj = Instantiate(keywordPanel);
        KeywordInfo info = keywordList.Where(e => e.keyword == keyword).FirstOrDefault();

        obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = info.keywordName;
        obj.transform.Find("Information").GetComponent<TextMeshProUGUI>().text = info.information;
        return obj;
    }
}
