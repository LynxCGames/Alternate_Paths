using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace Alchemist;

public class CryoAcid : UpgradePlusPlus<AlchemistAltPath>
{
    public override int Cost => 300;
    public override int Tier => 1;
    public override string Icon => "Tier1 Alch Icon";
    public override string Portrait => "Tier1 Alch";

    public override string DisplayName => "Cryo Acid";
    public override string Description => "Super chilled acid causes Bloons to be frozen when hit.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.
            AddBehavior(new FreezeModel("FreezeModel_", 0, 1f, "AcidFreeze", 1, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, false));
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.collisionPasses = new int[] { 0, -1 };
    }
}

public class CamoGoggles : UpgradePlusPlus<AlchemistAltPath>
{
    public override int Cost => 200;
    public override int Tier => 2;
    public override string Icon => "Tier2 Alch Icon";
    public override string Portrait => "Tier2 Alch";

    public override string DisplayName => "Camo Goggles";
    public override string Description => "Special goggles designed to spot camo Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
    }
}

public class PotionLauncher : UpgradePlusPlus<AlchemistAltPath>
{
    public override int Cost => 1200;
    public override int Tier => 3;
    public override string Icon => "Tier3 Alch Icon";
    public override string Portrait => "Tier3 Alch";

    public override string DisplayName => "Potion Launcher";
    public override string Description => "Now wields a powerful potion launcher that fires acid bottles faster and at a longer range.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].rate /= 3;

        towerModel.range += 12;
        towerModel.GetAttackModel().range += 12;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 1;
    }
}

public class PotentAcid : UpgradePlusPlus<AlchemistAltPath>
{
    public override int Cost => 3700;
    public override int Tier => 4;
    public override string Icon => "Tier4 Alch Icon";
    public override string Portrait => "Tier4 Alch";

    public override string DisplayName => "Potent Acid";
    public override string Description => "More potent acid deals more damage over time, deals additional damage to MOAB Bloons, and causes all Bloons to receive extra damage from all sources.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 1;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 2;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().interval -= 1f;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.hasDamageModifiers = true;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.
            AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 4, false, false) { name = "MoabModifier_" });

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.
            AddBehavior(new AddBonusDamagePerHitToBloonModel("aaa", "Acid_Bonus_Damage", 4f, 1, 5, true, false, false, "bleed"));
    }
}

public class SuperAcid : UpgradePlusPlus<AlchemistAltPath>
{
    public override int Cost => 28500;
    public override int Tier => 5;
    public override string Icon => "Tier5 Alch Icon";
    public override string Portrait => "Tier5 Alch";

    public override string DisplayName => "Super Acid";
    public override string Description => "Acid is much stronger and Bloons take massive extra damage from all sources.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].rate /= 2.5f;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 7;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 14;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition += 4;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().layers = 999;
    }
}