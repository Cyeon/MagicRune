using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColorHierachy : MonoBehaviour
{
#if UNITY_EDITOR
    //얘는 하이라키 상에 있는 이 스크립트가 붙어있는 모든 오브젝트를 딕셔너리로 관리하는거
    private static Dictionary<Object, ColorHierachy> coloredObjects = new Dictionary<Object, ColorHierachy>();

    static ColorHierachy ()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleDraw;
    }

    private static void HandleDraw(int id, Rect selectionRect)
    {
        Object obj = EditorUtility.InstanceIDToObject(id); //고유 아이디를 가지고 해당 오브젝트를 불러오는 함수

        if(obj != null && coloredObjects.ContainsKey(obj))
        {
            GameObject gObj = obj as GameObject;
            ColorHierachy ch = gObj.GetComponent<ColorHierachy>();
            if(ch != null)
            {
                PaintObject(obj, selectionRect, ch);
            }else
            {
                coloredObjects.Remove(obj);
            }
        }
    }

    private static void PaintObject(Object obj, Rect selectionRect, ColorHierachy ch)
    {
        Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

        if(Selection.activeObject != obj)
        {
            EditorGUI.DrawRect(bgRect, ch.backColor);
            string name = $"{ch.prefix} {obj.name}";

            EditorGUI.LabelField(bgRect, name, new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = ch.fontColor },
                fontStyle = FontStyle.Bold
            });
        }
        //선택된 오브젝트면 기본 유니티가 그리는대로 내버려 둔다.
    }

    public string prefix;
    public Color backColor;
    public Color fontColor;

    private void Reset()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if(coloredObjects.ContainsKey(this.gameObject) == false)
        {
            coloredObjects.Add(this.gameObject, this);
        }
    }
#endif
}
