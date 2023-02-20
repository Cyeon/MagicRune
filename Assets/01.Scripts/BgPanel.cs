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
                    var ped = new PointerEventData(null);
                    ped.position = t.position;
                    List<RaycastResult> results = new List<RaycastResult>();
                    UIManager.Instance.GR.Raycast(ped, results);
                    
                    if(results.Count > 0)
                    {
                        if (results[0].gameObject.CompareTag("Card"))
                        {

                        }
                        else
                        {
                            UIManager.Instance.CardDescDown();
                            UIManager.Instance.StatusDescPopup(null, Vector3.one, false);
                        }
                    }
                }
            }
        }
    }
}
