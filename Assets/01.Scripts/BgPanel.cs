using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BgPanel : MonoBehaviour
{
    [SerializeField]
    private MagicCircle _magicCircle;

    public void Update()
    {
        if (_magicCircle != null)
        {
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    _magicCircle.CardCollector.AllCardDescription(false);
                }
            }
        }
    }
}
