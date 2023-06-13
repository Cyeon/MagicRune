using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        DontDestroyOnLoad(this);
    }

    public Stage SpawnStage(StageType type)
    {
        switch(type)
        {
            case StageType.Attack:
                AttackStage atkStage = Managers.Resource.Instantiate("Stage/" + typeof(AttackStage).Name, transform).GetComponent<AttackStage>();
                atkStage.Init();
                return atkStage;

            case StageType.Boss:
                BossStage bossStage = Managers.Resource.Instantiate("Stage/" + typeof(BossStage).Name, transform).GetComponent<BossStage>();
                bossStage.Init();
                return bossStage;

            case StageType.Adventure:
                AdventureStage adventureStage = Managers.Resource.Instantiate("Stage/" + typeof(AdventureStage).Name, transform).GetComponent<AdventureStage>();
                adventureStage.Init();
                return adventureStage;

            case StageType.Rest:
                RestStage restStage = Managers.Resource.Instantiate("Stage/" + typeof(RestStage).Name, transform).GetComponent<RestStage>();
                restStage.Init();
                return restStage;

            case StageType.Shop:
                ShopStage shopStage = Managers.Resource.Instantiate("Stage/" + typeof(ShopStage).Name, transform).GetComponent<ShopStage>();
                shopStage.Init();
                return shopStage;

            case StageType.Tutorial:
                TutorialStage tutorialStage = Managers.Resource.Instantiate("Stage/" + typeof(TutorialStage).Name, transform).GetComponent<TutorialStage>();
                tutorialStage.Init();
                return tutorialStage;
        }

        return null;
    }
}
