using System.Collections;
using UnityEngine;

public class MagneticField : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(MagneticField).Name);
        base.Init();
    }

    

    public override object Clone()
    {
        MagneticField magneticField = new MagneticField();
        magneticField.Init();
        magneticField.UnEnhance();
        return magneticField;
    }
}