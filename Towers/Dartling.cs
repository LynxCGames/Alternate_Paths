using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;

namespace Dartling;

public class PunchDarts : UpgradePlusPlus<DartlingAltPath>
{
    public override int Cost => 220;
    public override int Tier => 1;
    public override string Icon => "Tier1 Dartling Icon";
    public override string Portrait => "Tier1 Dartling";

    public override string DisplayName => "Punch Darts";
    public override string Description => "Special darts capable of knocking back Bloons. M.A.D can knockback MOABs.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new WindModel("WindModel_", 0, 6, 80, false, null, 0, null, 1));

        if (towerModel.appliedUpgrades.Contains(UpgradeType.MAD))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().affectMoab = true;
        }
    }
}

public class HighGrade : UpgradePlusPlus<DartlingAltPath>
{
    public override int Cost => 940;
    public override int Tier => 2;
    public override string Icon => "Tier2 Dartling Icon";
    public override string Portrait => "Tier2 Dartling";

    public override string DisplayName => "High-Grade Darts";
    public override string Description => "Better darts that deal more damage and knockback Bloons further.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().distanceMin += 6;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().distanceMax *= 2;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().chance = 100;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.HydraRocketPods))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().projectile.GetDamageModel().damage += 1;
        }
        else
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.MAD))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustPierceModel>().projectile.GetDamageModel().damage *= 3;
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.PlasmaAccelerator))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 2;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.RayOfDoom))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 20;
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.Buckshot))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.BloonExclusionZone))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 8;
        }
    }
}

public class Volley : UpgradePlusPlus<DartlingAltPath>
{
    public override int Cost => 3500;
    public override int Tier => 3;
    public override string Icon => "Tier3 Dartling Icon";
    public override string Portrait => "Tier3 Dartling";

    public override string Description => "Fires a small spread of darts for even more damage.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].emission = new RandomArcEmissionModel("VollyGunner_", 3, 0, 0, 15, 0, null);
    }
}

public class Flamethrower : UpgradePlusPlus<DartlingAltPath>
{
    public override int Cost => 10500;
    public override int Tier => 4;
    public override string Icon => "Tier4 Dartling Icon";
    public override string Portrait => "Tier4 Dartling";

    public override string Description => "Darts are replaced with fire that deal massive damage and set Bloons on fire but has reduced range.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.
            GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();

        towerModel.GetAttackModel().weapons[0].projectile.pierce += 2;
        towerModel.GetAttackModel().weapons[0].rate /= 1.5f;
        towerModel.GetAttackModel().weapons[0].emission = new RandomArcEmissionModel("VollyGunner_", 5, 0, 0, 25, 0, null);
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 
            Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan * 1.2f;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage *= 2;
        towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.WizardMonkey, 0, 1).GetAttackModel(1).weapons[0].projectile.display;
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(fire);
        towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { -1, 0, 1 };
    }
}

public class Incinerator : UpgradePlusPlus<DartlingAltPath>
{
    public override int Cost => 270000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Dartling Icon";
    public override string Portrait => "Tier5 Dartling";

    public override string DisplayName => "The Incinerator";
    public override string Description => "Flames so hot that Bloons are roasted almost instantly.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.pierce += 4;
        towerModel.GetAttackModel().weapons[0].rate /= 2f;
        towerModel.GetAttackModel().weapons[0].emission = new RandomArcEmissionModel("VollyGunner_", 12, 0, 0, 60, 0, null);
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage *= 8;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 2, 0, false, false) { name = "MoabModifier_" });
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "BAD", 3, 0, false, false) { name = "BADModifier_" });

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().distanceMin *= 3;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>().distanceMax *= 3;
    }
}