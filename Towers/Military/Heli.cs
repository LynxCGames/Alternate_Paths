using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Linq;
using AlternatePaths.Displays.Projectiles;
using System.Linq;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using PathsPlusPlus;
using AlternatePaths;

namespace Heli;

public class HighVelocity : UpgradePlusPlus<HeliAltPath>
{
    public override int Cost => 640;
    public override int Tier => 1;
    public override string Icon => "Tier1 Heli Icon";
    public override string Portrait => "Tier1 Heli";

    public override string DisplayName => "High Velocity Darts";
    public override string Description => "More powerful darts allow them to pop more Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.pierce += 2;
        }
    }
}

public class LaserGuns : UpgradePlusPlus<HeliAltPath>
{
    public override int Cost => 780;
    public override int Tier => 2;
    public override string Icon => "Tier2 Heli Icon";
    public override string Portrait => "Tier2 Heli";

    public override string DisplayName => "Laser Guns";
    public override string Description => "Darts are replaced with lasers that deal more damage and can pop more Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
        towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-300").GetAttackModel().weapons[0].projectile.display;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.pierce += 2;
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.QuadDarts))
        {
            towerModel.GetAttackModel().weapons[1].projectile.GetDamageModel().damage += 1;
            towerModel.GetAttackModel().weapons[1].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-300").GetAttackModel().weapons[0].projectile.display;
        }
    }
}

public class Railgun : UpgradePlusPlus<HeliAltPath>
{
    public override int Cost => 2400;
    public override int Tier => 3;
    public override string Icon => "Tier3 Heli Icon";
    public override string Portrait => "Tier3 Heli";

    public override string DisplayName => "Railgun";
    public override string Description => "Periodically fires a powerful railgun that can hit lots of Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var railgun = Game.instance.model.GetTower(TowerType.HeliPilot, 4).GetAttackModel().weapons[2].Duplicate();

        railgun.projectile.pierce = 20;
        railgun.projectile.GetDamageModel().damage = 5;
        railgun.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        railgun.rate = towerModel.GetAttackModel().weapons[0].rate * 3f;
        railgun.projectile.ApplyDisplay<RailgunProj>();

        railgun.name = "_Railgun_Main";

        towerModel.GetAttackModel().AddWeapon(railgun);

        if (towerModel.appliedUpgrades.Contains(UpgradeType.IFR))
        {
            railgun.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        railgun.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
    }
}

public class HighEnergyBeam : UpgradePlusPlus<HeliAltPath>
{
    public override int Cost => 14800;
    public override int Tier => 4;
    public override string Icon => "Tier4 Heli Icon";
    public override string Portrait => "Tier4 Heli";

    public override string DisplayName => "High Energy Beams";
    public override string Description => "Railgun beams cause an explosion upon hitting a Bloon that can stun surrounding Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var bomb = Game.instance.model.GetTower(TowerType.BombShooter, 4).GetAttackModel().weapons[0].projectile.Duplicate();
        var stun = bomb.GetBehavior<CreateProjectileOnContactModel>().projectile;

        stun.pierce = 8;
        stun.GetDamageModel().damage = 1;
        stun.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        stun.radius = 15;

        stun.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        var blast = bomb.GetBehavior<CreateEffectOnContactModel>().effectModel;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            if (weaponModel.name == "_Railgun_Main")
            {
                weaponModel.projectile.GetDamageModel().damage += 1;
                weaponModel.rate = towerModel.GetAttackModel().weapons[0].rate * 2f;
                weaponModel.projectile.pierce += 5;

                weaponModel.projectile.AddBehavior(new CreateProjectileOnContactModel
                    ("CreateProjectileOnContactModel_", stun, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 1, null, true, false), false, false, false));
                weaponModel.projectile.AddBehavior(new CreateEffectOnContactModel("CreateEffectOnContactModel_", blast));
            }
        }
    }
}

public class GigaDrill : UpgradePlusPlus<HeliAltPath>
{
    public override int Cost => 58000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Heli Icon";
    public override string Portrait => "Tier5 Heli";

    public override string DisplayName => "Giga Drill Beam";
    public override string Description => "A beam of concentrated energy capable of annihilating nearly anything.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var railgunv2 = towerModel.GetAttackModel().weapons[0];

        railgunv2.projectile.GetDamageModel().damage = 16;
        railgunv2.projectile.pierce = 35;
        railgunv2.projectile.ApplyDisplay<RailgunDrill>();
        railgunv2.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        railgunv2.rate /= 4;

        var bigBomb = Game.instance.model.GetTower(TowerType.BombShooter, 5).GetAttackModel().weapons[0].projectile.Duplicate();
        var bigStun = bigBomb.GetBehavior<CreateProjectileOnContactModel>().projectile;

        bigStun.pierce = 15;
        bigStun.GetDamageModel().damage = 8;
        bigStun.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        bigStun.radius = 25;

        bigStun.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        var bigBlast = bigBomb.GetBehavior<CreateEffectOnContactModel>().effectModel;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            if (weaponModel.name == "_Railgun_Main")
            {
                weaponModel.projectile.GetDamageModel().damage = 45;
                weaponModel.rate = towerModel.GetAttackModel().weapons[0].rate * 4f;
                weaponModel.projectile.pierce += 50;

                weaponModel.projectile.ApplyDisplay<DrillBeamProj>();

                weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile = bigStun;
                weaponModel.projectile.GetBehavior<CreateEffectOnContactModel>().effectModel = bigBlast;

                weaponModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            }
        }
    }
}