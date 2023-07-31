using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class AttributeSpecialization : MonoBehaviour
{
    [SerializeField] private Image attributeImage; 

    [SerializeField] private Sprite fireAttribute;
    [SerializeField] private Sprite iceAttribute;
    [SerializeField] private Sprite groundAttribute;
    [SerializeField] private Sprite electricAttribute;

    private void Start()
    {
        ChangeImage(Managers.Rune.GetSelectAttribute());
    }

    public void ChangeImage(AttributeType attributeType)
    {
        if (attributeType == AttributeType.None) return;

        attributeImage.gameObject.SetActive(true);
        switch (attributeType)
        {
            case AttributeType.Fire:
                attributeImage.sprite = fireAttribute;
                break;
            case AttributeType.Ice:
                attributeImage.sprite = iceAttribute;
                break;
            case AttributeType.Ground:
                attributeImage.sprite = groundAttribute;
                break;
            case AttributeType.Electric:
                attributeImage.sprite = electricAttribute;
                break;
        }
    }
}
