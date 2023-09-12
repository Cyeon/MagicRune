using MyBox;

public class BlazingFire : BaseRune
{

    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(BlazingFire).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), IsIncludeKeyword(KeywordName.Penetration));
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.OverHeat, GetAbliltiValue(EffectType.Status, StatusName.OverHeat).RoundToInt());
    }

    public override object Clone()
    {
        BlazingFire blazingFire = new BlazingFire();
        blazingFire.Init();
        blazingFire.UnEnhance();
        return blazingFire;
    }
}