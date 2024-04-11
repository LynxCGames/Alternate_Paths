using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using PathsPlusPlus;
using AlternatePaths;

namespace DartMonkey;

public class BloontoniumDarts : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 180;
    public override int Tier => 1;
    public override string Icon => "Tier1 Dart Icon";
    public override string Portrait => "Tier1 Dart";

    public override string DisplayName => "Bloontonium Darts";
    public override string Description => "Darts can now pop all Bloon types. Spike-o-Pult gains increased damage.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SpikeOPult))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
        }
    }
}

public class RubberSeeking : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 330;
    public override int Tier => 2;
    public override string Icon => "Tier2 Dart Icon";
    public override string Portrait => "Tier2 Dart";

    public override string DisplayName => "Rubber Seeking Darts";
    public override string Description => "Darts now seek out Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();

        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;


        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(seeking);
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 4;
        towerModel.GetAttackModel().weapons[0].projectile.pierce += 1;
    }
}

public class DartGunner : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 560;
    public override int Tier => 3;
    public override string Icon => "Tier3 Dart Icon";
    public override string Portrait => "Tier3 Dart";

    public override string DisplayName => "Dart Gunner";
    public override string Description => "Dart gun shoots faster and pops 2 layers.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.range += 5;
        towerModel.GetAttackModel().range += 5;

        towerModel.GetAttackModel().weapons[0].rate /= 1.4f;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
    }
}

public class Rifleman : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 2400;
    public override int Tier => 4;
    public override string Icon => "Tier4 Dart Icon";
    public override string Portrait => "Tier4 Dart";

    public override string DisplayName => "Rifleman";
    public override string Description => "Dart gun now fires incredibly fast and deals more damage to Ceramic Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.range += 8;
        towerModel.GetAttackModel().range += 8;

        towerModel.GetAttackModel().weapons[0].rate /= 4;
        towerModel.GetAttackModel().weapons[0].projectile.pierce += 2;

        towerModel.GetAttackModel().weapons[0].projectile.hasDamageModifiers = true;
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
    }
}

public class DartTank : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 28600;
    public override int Tier => 5;
    public override string Icon => "Tier5 Dart Icon";
    public override string Portrait => "Tier5 Dart";

    public override string DisplayName => "Plasma tank";
    public override string Description => "Machine gun attack fires deadly plasma and Plasma Tank periodically fires a Bloontonium rocket that obliterates MOAB-class Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.range += 12;
        towerModel.GetAttackModel().range += 12;

        towerModel.GetAttackModel().weapons[0].projectile.pierce *= 2;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 3;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 7;
        towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.SuperMonkey, 2).GetAttackModel().weapons[0].projectile.display;


        var missileWeapon = Game.instance.model.GetTower(TowerType.BombShooter, 1, 2).GetAttackModel().weapons[0].Duplicate();
        missileWeapon.rate = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].rate;
        missileWeapon.projectile.display = Game.instance.model.GetTower(TowerType.DartlingGunner, 0, 3).GetAttackModel().weapons[0].projectile.display;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.EnhancedEyesight))
        {
            missileWeapon.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            missileWeapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        var missile = missileWeapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
        missile.GetDamageModel().damage = 8;
        missile.hasDamageModifiers = true;
        missile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 56, false, false) { name = "MoabModifier_" });
        missile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        towerModel.GetAttackModel().AddWeapon(missileWeapon);
    }
}