using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppSystem.Linq;
using AlternatePaths.Displays.Projectiles;
using System.Linq;
using Il2Cpp;
using PathsPlusPlus;
using AlternatePaths;

namespace Spactory;

public class TitaniumSpikes : UpgradePlusPlus<SpactoryAltPath>
{
    public override int Cost => 720;
    public override int Tier => 1;
    public override string Icon => "Tier1 Spactory Icon";
    public override string Portrait => "Tier1 Spactory";

    public override string DisplayName => "Titanium Spikes";
    public override string Description => "Spikes last longer and can pop more Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.pierce += 3;

            weaponModel.projectile.GetBehavior<AgeModel>().Lifespan *= 1.25f;
        }
    }
}

public class EnhancedProduction : UpgradePlusPlus<SpactoryAltPath>
{
    public override int Cost => 840;
    public override int Tier => 2;
    public override string Icon => "Tier2 Spactory Icon";
    public override string Portrait => "Tier2 Spactory";

    public override string DisplayName => "Enhanced Production";
    public override string Description => "Produces 2 spike piles at a time.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.rate *= 1.1f;
        }

        var spikes = towerModel.GetAttackModel().weapons[0].Duplicate();
        spikes.rate = towerModel.GetAttackModel().weapons[0].rate;

        towerModel.GetAttackModel().AddWeapon(spikes);
    }
}

public class ShieldGenerator : UpgradePlusPlus<SpactoryAltPath>
{
    public override int Cost => 3200;
    public override int Tier => 3;
    public override string Icon => "Tier3 Spactory Icon";
    public override string Portrait => "Tier3 Spactory";

    public override string DisplayName => "Shield Generator";
    public override string Description => "Spike Factory now produces force fields that block and pop Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.RemoveBehavior<SetSpriteFromPierceModel>();
            weaponModel.rate /= 1.1f;

            weaponModel.projectile.pierce *= 2;

            weaponModel.projectile.AddBehavior(new WindModel("WindModel_", 0, 10, 50, false, null, 0, null, 1));

            weaponModel.projectile.ApplyDisplay<ForceFields>();
        }
    }
}

public class StrongerShield : UpgradePlusPlus<SpactoryAltPath>
{
    public override int Cost => 11000;
    public override int Tier => 4;
    public override string Icon => "Tier4 Spactory Icon";
    public override string Portrait => "Tier4 Spactory";

    public override string DisplayName => "Stronger Force Fields";
    public override string Description => "Knockback is stronger and force fields deal more damage.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.GetBehavior<WindModel>().chance = 75;
            weaponModel.projectile.GetBehavior<WindModel>().distanceMax = 30;

            weaponModel.projectile.pierce *= 1.5f;
            weaponModel.projectile.GetBehavior<AgeModel>().Lifespan *= 1.5f;

            weaponModel.projectile.ApplyDisplay<ForceFieldsAdvanced>();
        }

        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.damage += 1;
        }
    }
}

public class Lockdown : UpgradePlusPlus<SpactoryAltPath>
{
    public override int Cost => 62000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Spactory Icon";
    public override string Portrait => "Tier5 Spactory";

    public override string Description => "Bloons leaking? Why not lockdown the exit to prevent their escape.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.GetBehavior<WindModel>().affectMoab = true;
            weaponModel.projectile.GetBehavior<WindModel>().distanceMax = 45;

            weaponModel.projectile.ApplyDisplay<ForceFieldsLockdown>();

            weaponModel.projectile.pierce *= 2f;
            weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        }

        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.damage += 3;
        }

        //var lockdown = Game.instance.model.GetTower(TowerType.SuperMonkey, 0, 0, 5).GetAbility().Duplicate();
        //towerModel.AddBehavior(lockdown);
    }
}