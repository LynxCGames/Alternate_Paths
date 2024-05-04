using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

namespace Druid;

public class CrippleThorn : UpgradePlusPlus<DruidAltPath>
{
    public override int Cost => 200;
    public override int Tier => 1;
    public override string Icon => "Tier1 Druid Icon";
    public override string Portrait => "Tier1 Druid";

    public override string DisplayName => "Cripple Thorns";
    public override string Description => "Thorns now slow Bloons hit by 30%.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new SlowModel("SlowModel_", 0.7f, 3f, "ThornSlow", 9999999, "", true, false, null, true, false, false));
        towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
    }
}

public class SeaHeart : UpgradePlusPlus<DruidAltPath>
{
    public override int Cost => 720;
    public override int Tier => 2;
    public override string Icon => "Tier2 Druid Icon";
    public override string Portrait => "Tier2 Druid";

    public override string DisplayName => "Heart of the Sea";
    public override string Description => "Periodically throws out a barracuda that travels slow but deals high damage and can pop tons of Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var wave = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().Duplicate();
        wave.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.BeastHandler, 2).GetBehavior<BeastHandlerLeashModel>().towerModel.GetAttackModel().weapons[0].projectile.display;
        wave.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3;
        wave.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed /= 2.5f;
        wave.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        wave.weapons[0].projectile.GetDamageModel().damage = 3;
        wave.weapons[0].projectile.pierce = 99;
        wave.weapons[0].rate = 3;

        towerModel.AddBehavior(wave);
    }
}

public class DruidDeep : UpgradePlusPlus<DruidAltPath>
{
    public override int Cost => 2250;
    public override int Tier => 3;
    public override string Icon => "Tier3 Druid Icon";
    public override string Portrait => "Tier3 Druid";

    public override string DisplayName => "Druid of the Deep";
    public override string Description => "Commands a loyal shark companion that is capable of shredding Bloons with ease.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var shark = Game.instance.model.GetTower(TowerType.BeastHandler, 3).GetBehavior<BeastHandlerLeashModel>().towerModel.GetAttackModel().Duplicate();
        shark.name = "CompanionShark_";
        towerModel.AddBehavior(shark);
    }
}

public class Tsunami : UpgradePlusPlus<DruidAltPath>
{
    public override int Cost => 5600;
    public override int Tier => 4;
    public override string Icon => "Tier4 Druid Icon";
    public override string Portrait => "Tier4 Druid";

    public override string Description => "Throws out 7 barracudas at a time that deal more damage, travel a bit faster, and are fired more often. Thorns now slow Bloons by 60%.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel(1).weapons[0].emission = new ArcEmissionModel("Tsunami_", 7, 0, 60, null, false, false);
        towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage += 2;
        towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 1.5f;
        towerModel.GetAttackModel(1).weapons[0].rate /= 1.5f;

        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<SlowModel>().multiplier = 0.4f;
    }
}

public class WaveMaster : UpgradePlusPlus<DruidAltPath>
{
    public override int Cost => 72000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Druid Icon";
    public override string Portrait => "Tier5 Druid";

    public override string DisplayName => "Master of the Waves";
    public override string Description => "Now commands an orca companion that rips through MOABs and throws out 10 barracudas at a time that deal even more damage.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var behaiovr in towerModel.GetAttackModels())
        {
            if (behaiovr.name.Contains("CompanionShark_"))
            {
                towerModel.RemoveBehavior(behaiovr);
            }
        }


        towerModel.GetAttackModel(1).weapons[0].emission = new ArcEmissionModel("Tsunami_", 10, 0, 90, null, false, false);
        towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage += 3;
        towerModel.GetAttackModel(1).weapons[0].rate /= 1.4f;


        var orca = Game.instance.model.GetTower(TowerType.BeastHandler, 4).GetBehavior<BeastHandlerLeashModel>().towerModel.GetAttackModel().Duplicate();
        orca.name = "CompanionOrca_";
        towerModel.AddBehavior(orca);
    }
}