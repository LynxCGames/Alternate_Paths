using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using AlternatePaths.Displays.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace Super;

public class Scorch : UpgradePlusPlus<SuperAltPath>
{
    public override int Cost => 1800;
    public override int Tier => 1;
    public override string Icon => "Tier1 Super Icon";
    public override string Portrait => "Tier1 Super";

    public override string Description => "Super darts catch Bloons on fire.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.
            GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();

        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(fire);
        towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { -1, 0, 1 };
    }
}

public class BreakerDarts : UpgradePlusPlus<SuperAltPath>
{
    public override int Cost => 2700;
    public override int Tier => 2;
    public override string Icon => "Tier2 Super Icon";
    public override string Portrait => "Tier2 Super";

    public override string DisplayName => "Breaker Darts";
    public override string Description => "Even more super darts deal additional damage to MOAB and fortified Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.hasDamageModifiers = true;

        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 2, 0, false, false) { name = "MoabModifier_" });
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 2, 0, false, false) { name = "FortifiedModifier_" });
    }
}

public class SpectralClaws : UpgradePlusPlus<SuperAltPath>
{
    public override int Cost => 7300;
    public override int Tier => 3;
    public override string Icon => "Tier3 Super Icon";
    public override string Portrait => "Tier3 Super";

    public override string DisplayName => "Spectral Claws";
    public override string Description => "Periodically throws out two spectral orbs that break into several fast-homing shards.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();

        var orb = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].Duplicate();
        var shard = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].projectile.Duplicate();

        orb.rate /= 1.5f;
        orb.emission = new ArcEmissionModel("ArcEmissionModel_", 2, 0, 180, null, false, false);
        orb.projectile.pierce = 999;
        orb.projectile.GetDamageModel().damage = 0;
        orb.projectile.ApplyDisplay<SpectralOrb>();

        orb.projectile.GetBehavior<TravelStraitModel>().Speed /= 2;

        shard.pierce = 3;
        shard.GetDamageModel().damage = 4;
        shard.AddBehavior(new TrackTargetModel("", 999, seeking.trackNewTargets, true, seeking.maxSeekAngle, seeking.ignoreSeekAngle, seeking.turnRate * 6, seeking.overrideRotation, seeking.useLifetimeAsDistance));
        shard.GetBehavior<TravelStraitModel>().Speed *= 2;
        shard.GetBehavior<TravelStraitModel>().Lifespan *= 3;
        shard.ApplyDisplay<SpectralShard>();

        if (towerModel.appliedUpgrades.Contains(UpgradeType.PlasmaBlasts))
        {
            shard.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        }

        orb.projectile.AddBehavior(new CreateProjectileOnExpireModel("SpectralShards", shard, new ArcEmissionModel("", 3, 0, 30, null, false, false), false));

        towerModel.GetAttackModel().AddWeapon(orb);
    }
}

public class SuppressedPower : UpgradePlusPlus<SuperAltPath>
{
    public override int Cost => 90000;
    public override int Tier => 4;
    public override string Icon => "Tier4 Super Icon";
    public override string Portrait => "Tier4 Super";

    public override string DisplayName => "Suppressed Power";
    public override string Description => "Now fires beams of high-power energy.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 2;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval -= 1f;


        var shard = towerModel.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile;

        shard.GetDamageModel().damage += 1;
        shard.GetDamageModel().immuneBloonProperties = BloonProperties.None;


        towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 4, 0, 45, null, false, false);
        towerModel.GetAttackModel().weapons[0].projectile.pierce += 2;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
        towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<EnergyBeam>();
    }
}

public class ArcaneGuardian : UpgradePlusPlus<SuperAltPath>
{
    public override int Cost => 485000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Super Icon";
    public override string Portrait => "Tier5 Super";

    public override string DisplayName => "Arcane Guardian";
    public override string Description => "A being of pure energy that destroys Bloons with its energy beams.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 26;


        var shard = towerModel.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile;

        towerModel.GetAttackModel().weapons[1].projectile.GetBehavior<CreateProjectileOnExpireModel>().emission = new ArcEmissionModel("ArcEmissionModel_", 5, 0, 50, null, false, false);
        shard.GetDamageModel().damage = 100;
        shard.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 4, 0, false, false) { name = "MoabModifier_" });
        shard.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 2, 0, false, false) { name = "FortifiedModifier_" });


        towerModel.GetAttackModel().weapons[0].projectile.pierce *= 3;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = 14;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;


        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<DamageModifierForTagModel>())
        {
            if (behavior.name.Contains("MoabModifier_"))
            {
                behavior.damageMultiplier = 6;
            }
        }
    }
}