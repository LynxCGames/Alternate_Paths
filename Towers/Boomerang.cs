using AlternatePaths;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Linq;
using PathsPlusPlus;
using AlternatePaths.Displays.Projectiles;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;

namespace Boomerang;

public class CamoTraining : UpgradePlusPlus<BoomerangAltPath>
{
    public override int Cost => 200;
    public override int Tier => 1;
    public override string Icon => "Tier1 Boomerang Icon";
    public override string Portrait => "Tier1 Boomerang";

    public override string DisplayName => "Camo Training";
    public override string Description => "Can now hit and deal additional damage to Camo Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.hasDamageModifiers = true;
            weaponModel.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Camo", 1, 1, false, false) { name = "CamoModifier_" });
        }
    }
}

public class Chakrams : UpgradePlusPlus<BoomerangAltPath>
{
    public override int Cost => 400;
    public override int Tier => 2;
    public override string Icon => "Tier2 Boomerang Icon";
    public override string Portrait => "Tier2 Boomerang";

    public override string Description => "Boomerangs are replaced with Chakrams that fly really fast and cause a bleed effect on Bloons struck.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var bleed = Game.instance.model.GetTowerFromId("Sauda 9").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
        bleed.name = "BleedModel";
        bleed.GetBehavior<DamageOverTimeModel>().interval = .5f;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.collisionPasses = new[] { -1, 0, 1 };
            weaponModel.projectile.AddBehavior(bleed);
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.Glaives))
        {
            towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<GlaiveChakram>();
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.KylieBoomerang))
        {
            if (towerModel.appliedUpgrades.Contains(UpgradeType.MOABDomination))
            {
                towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.BoomerangMonkey, 2, 0, 5).GetAttackModel().weapons[0].projectile.display;
            }
            else
            {
                towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.BoomerangMonkey, 2, 0, 3).GetAttackModel().weapons[0].projectile.display;
            }
        }
        else
        {
            towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<Chakram>();
        }


        if (towerModel.appliedUpgrades.Contains(UpgradeType.GlaiveRicochet))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 2f;
        }
        else
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<FollowPathModel>().Speed *= 2f;
        }
    }
}

public class SeekerBlades : UpgradePlusPlus<BoomerangAltPath>
{
    public override int Cost => 720;
    public override int Tier => 3;
    public override string Icon => "Tier3 Boomerang Icon";
    public override string Portrait => "Tier3 Boomerang";

    public override string DisplayName => "Seeker Blades";
    public override string Description => "Chakrams now seek out Bloons with lightning efficiency.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.RemoveBehavior<FollowPathModel>();

        var travel = Game.instance.model.GetTower(TowerType.DartMonkey).GetWeapon().projectile.GetBehavior<TravelStraitModel>().Duplicate();
        var seeking = Game.instance.model.GetTower(TowerType.NinjaMonkey, 0, 0, 1).GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();

        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;


        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.AddBehavior(travel);
            weaponModel.projectile.AddBehavior(seeking);
            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 4;
            weaponModel.projectile.pierce += 4;
            weaponModel.projectile.AddBehavior(new UseAttackRotationModel("aaa"));
        }
    }
}

public class PlasmaChakrams : UpgradePlusPlus<BoomerangAltPath>
{
    public override int Cost => 5200;
    public override int Tier => 4;
    public override string Icon => "Tier4 Boomerang Icon";
    public override string Portrait => "Tier4 Boomerang";

    public override string DisplayName => "Plasma Chakrams";
    public override string Description => "Plasma enhanced Chakrams deal massive amounts of damage and the bleed effect is much stronger.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.GetDamageModel().damage += 4;
            weaponModel.projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().interval *= .5f;
            weaponModel.projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 5;
            weaponModel.projectile.GetBehavior<AddBehaviorToBloonModel>().lifespan *= 2;
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.Glaives))
        {
            towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<PlasmaGlaiveChakram>();
        }
        else
        {
            towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<PlasmaChakram>();
        }
    }
}

public class BladeGod : UpgradePlusPlus<BoomerangAltPath>
{
    public override int Cost => 56000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Boomerang Icon";
    public override string Portrait => "Tier5 Boomerang";

    public override string DisplayName => "Blade God";
    public override string Description => "Throws 3 blade rings at a time that deal tremendous damage and cause struck Bloons to take more damage from all sources.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var superBleed = towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
        superBleed.name = "MoabBleedModel";
        superBleed.GetBehavior<DamageOverTimeModel>().damage = 32;
        superBleed.filter = new FilterMoabModel("MoabsOnly", false);

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.rate /= 1.75f;
            weaponModel.emission = new ArcEmissionModel("aaa", 3, 0, 25, null, false, false);
            weaponModel.projectile.GetDamageModel().damage *= 3;
            weaponModel.projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 2;
            weaponModel.projectile.AddBehavior(superBleed);
            weaponModel.projectile.AddBehavior(new AddBonusDamagePerHitToBloonModel("aaa", "bleed_Bonus_Damage", 8f, 3, 15, true, false, false, "bleed"));
        }

        towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<BladeGodProj>();
    }
}