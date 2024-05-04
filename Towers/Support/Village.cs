using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;

namespace Village;

public class Stim : UpgradePlusPlus<VillageAltPath>
{
    public override int Cost => 450;
    public override int Tier => 1;
    public override string Icon => "Tier1 Village Icon";
    public override string Portrait => "Tier1 Village";

    public override string Description => "Heros in radius gain xp 8% faster.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.AddBehavior(new HeroXpScaleSupportModel("XpSupportModel", true, 1.08f, null, "", ""));
    }
}

public class Energize : UpgradePlusPlus<VillageAltPath>
{
    public override int Cost => 900;
    public override int Tier => 2;
    public override string Icon => "Tier2 Village Icon";
    public override string Portrait => "Tier2 Village";
    
    public override string Description => "Reduce ability cooldowns in range by 10%.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.AddBehavior(new AbilityCooldownScaleSupportModel("CooldownSupportModel", true, 1.1f, false, false, null, "", ""));
    }
}

public class Blacksmith : UpgradePlusPlus<VillageAltPath>
{
    public override int Cost => 2000;
    public override int Tier => 3;
    public override string Icon => "Tier3 Village Icon";
    public override string Portrait => "Tier3 Village";

    public override string Description => "Gives additional pierce and damage to all towers in range.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.AddBehavior(new PierceSupportModel("PierceSupportModel", true, 2, "PierceSupportZone", null, false, "", ""));
        towerModel.AddBehavior(new DamageSupportModel("DamageSupportModel", true, 1, "DamageSupportZone", null, false, false, 0));
    }
}

public class EnergyBeacon : UpgradePlusPlus<VillageAltPath>
{
    public override int Cost => 3600;
    public override int Tier => 4;
    public override string Icon => "Tier4 Village Icon";
    public override string Portrait => "Tier4 Village";

    public override string DisplayName => "Energy Beacon";
    public override string Description => "Gains a high-powered energy beacon to zap Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var beacon = Game.instance.model.GetTower(TowerType.Druid, 2).GetAttackModel().Duplicate();
        beacon.RemoveBehavior<RotateToTargetModel>();
        beacon.RemoveWeapon(beacon.weapons[0]);

        beacon.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        beacon.weapons[0].rate /= 2;
        beacon.weapons[0].projectile.GetDamageModel().damage += 2;
        beacon.weapons[0].projectile.pierce += 2;

        towerModel.AddBehavior(beacon);
    }
}

public class OmegaBeacon : UpgradePlusPlus<VillageAltPath>
{
    public override int Cost => 19000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Village Icon";
    public override string Portrait => "Tier5 Village";

    public override string DisplayName => "Omega Beacon";
    public override string Description => "Energy beacon is much more powerful and has a chance to stun all small Bloons hit and greatly slow MOABs.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel(1).weapons[0].rate /= 1.5f;

        towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage *= 3;
        towerModel.GetAttackModel(1).weapons[0].projectile.pierce += 2;

        towerModel.GetAttackModel(1).weapons[0].projectile.AddBehavior(new FreezeModel("FreezeModel_", 0, 1f, "BeaconStun", 999, "Stun", true, new GrowBlockModel("GrowBlockModel_"), null, 0.5f, true, true));
        towerModel.GetAttackModel(1).weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
    }
}