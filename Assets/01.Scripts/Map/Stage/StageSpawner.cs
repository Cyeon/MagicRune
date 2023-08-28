using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StageSpawner
{
    public Stage SpawnStage(StageType type)
    {
        switch(type)
        {
            case StageType.Attack:
                AttackStage atkStage = Managers.Resource.Instantiate("Stage/" + typeof(AttackStage).Name).GetComponent<AttackStage>();
                atkStage.Init();
                return atkStage;

            case StageType.Boss:
                BossStage bossStage = Managers.Resource.Instantiate("Stage/" + typeof(BossStage).Name).GetComponent<BossStage>();
                bossStage.Init();
                return bossStage;

            case StageType.Adventure:
                AdventureStage adventureStage = Managers.Resource.Instantiate("Stage/" + typeof(AdventureStage).Name).GetComponent<AdventureStage>();
                adventureStage.Init();
                return adventureStage;

            case StageType.Rest:
                RestStage restStage = Managers.Resource.Instantiate("Stage/" + typeof(RestStage).Name).GetComponent<RestStage>();
                restStage.Init();
                return restStage;

            case StageType.Shop:
                ShopStage shopStage = Managers.Resource.Instantiate("Stage/" + typeof(ShopStage).Name).GetComponent<ShopStage>();
                shopStage.Init();
                return shopStage;

            case StageType.Tutorial:
                TutorialStage tutorialStage = Managers.Resource.Instantiate("Stage/" + typeof(TutorialStage).Name).GetComponent<TutorialStage>();
                tutorialStage.Init();
                return tutorialStage;

            case StageType.Elite:
                EliteStage eliteStage = Managers.Resource.Instantiate("Stage/" + typeof(EliteStage).Name).GetComponent<EliteStage>();
                eliteStage.Init();
                return eliteStage;
        }

        return null;
    }
}
