using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Linq;
using AlternatePaths.Displays.Projectiles;
using BTD_Mod_Helper;
using System.Linq;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppFacepunch.Steamworks;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using PathsPlusPlus;
using AlternatePaths;

namespace Sniper;

public class FocusedScope : UpgradePlusPlus<SniperAltPath>
{
    public override int Cost => 280;
    public override int Tier => 1;
    public override string Icon => "Tier1 Sniper Icon";
    public override string Portrait => "Tier1 Sniper";

    public override string DisplayName => "Focused Scope";
    public override string Description => "Sniper Monkey focuses his scope to see past obstacles and deal additional damage.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var attackModel in towerModel.GetAttackModels())
        {
            attackModel.weapons[0].projectile.ignoreBlockers = true;
            attackModel.weapons[0].projectile.canCollisionBeBlockedByMapLos = false;
            attackModel.attackThroughWalls = true;
        }

        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
    }
}

public class HighPowered : UpgradePlusPlus<SniperAltPath>
{
    public override int Cost => 820;
    public override int Tier => 2;
    public override string Icon => "Tier2 Sniper Icon";
    public override string Portrait => "Tier2 Sniper";

    public override string DisplayName => "High Powered Rifle";
    public override string Description => "Sniper shots cause a small explosion when they hit a Bloon. Deadly Precision and higher increase the damage and radius of the explosion.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var bomb = Game.instance.model.GetTower(TowerType.BombShooter, 0).GetAttackModel().weapons[0].projectile.Duplicate();
        var blast = bomb.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
        var explosion = bomb.GetBehavior<CreateEffectOnContactModel>().Duplicate();

        blast.GetDamageModel().damage = 1;
        blast.radius = 8;
        blast.pierce = 6;
        blast.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);


        var contactModel = new CreateProjectileOnContactModel("aaa", blast, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 0, null, false, false), true, false, false)
        { name = "RifleBlast_" };


        if (towerModel.appliedUpgrades.Contains(UpgradeType.DeadlyPrecision))
        {
            blast.GetDamageModel().damage += 2;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.MaimMOAB))
        {
            blast.GetDamageModel().damage += 2;
            blast.radius += 2;
            blast.pierce += 3;

            explosion.effectModel.scale *= 1.5f;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.CrippleMOAB))
        {
            blast.GetDamageModel().damage += 3;
            blast.radius += 4;
            blast.pierce += 6;

            explosion.effectModel.scale *= 1.5f;
        }

        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(contactModel);
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(explosion);
    }
}

public class FlackGun : UpgradePlusPlus<SniperAltPath>
{
    public override int Cost => 3500;
    public override int Tier => 3;
    public override string Icon => "Tier3 Sniper Icon";
    public override string Portrait => "Tier3 Sniper";

    public override string DisplayName => "Flack Gun";
    public override string Description => "Blast radius is bigger and fires sharp fragments. Shrapnel Shot increases fragment damage and number of fragments.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (towerModel.appliedUpgrades.Contains(UpgradeType.ShrapnelShot))
        {
            towerModel.GetAttackModel().weapons[0] = Game.instance.model.GetTower(TowerType.SniperMonkey, 0, 1).GetAttackModel().weapons[0].Duplicate();
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = 3;
            towerModel.GetAttackModel().weapons[0].projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            var bomb = Game.instance.model.GetTower(TowerType.BombShooter, 0).GetAttackModel().weapons[0].projectile.Duplicate();
            var blast = bomb.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
            var explosion = bomb.GetBehavior<CreateEffectOnContactModel>().Duplicate();

            blast.GetDamageModel().damage = 1;
            blast.radius = 8;
            blast.pierce = 6;
            blast.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);


            var contactModel = new CreateProjectileOnContactModel("aaa", blast, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 0, null, false, false), true, false, false)
            { name = "RifleBlast_" };

            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(contactModel);
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(explosion);
        }

        var weapon = towerModel.GetAttackModel().weapons[0];
        weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius += 12;
        weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 2;
        weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce += 10;

        weapon.projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.scale *= 2;


        var shrapnel = Game.instance.model.GetTower(TowerType.TackShooter).GetAttackModel().weapons[0].projectile.Duplicate();
        shrapnel.pierce = 3;
        shrapnel.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
        shrapnel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        if (towerModel.appliedUpgrades.Contains(UpgradeType.FullMetalJacket))
        {
            shrapnel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.ShrapnelShot))
        {
            shrapnel.GetDamageModel().damage = 3;

            var fragmentModel = new CreateProjectileOnContactModel("aaa", shrapnel, new ArcEmissionModel("ArcEmissionModel_", 10, 0, 360, null, true, false), true, false, false)
            { name = "RifleShrapnel_" };

            weapon.projectile.AddBehavior(fragmentModel);
        }
        else
        {
            shrapnel.GetDamageModel().damage = 2;

            var fragmentModel = new CreateProjectileOnContactModel("aaa", shrapnel, new ArcEmissionModel("ArcEmissionModel_", 6, 0, 360, null, true, false), true, false, false)
            { name = "RifleShrapnel_" };

            weapon.projectile.AddBehavior(fragmentModel);
        }
    }
}

public class Bloonzooka : UpgradePlusPlus<SniperAltPath>
{
    public override int Cost => 12800;
    public override int Tier => 4;
    public override string Icon => "Tier4 Sniper Icon";
    public override string Portrait => "Tier4 Sniper";

    public override string DisplayName => "Bloonzooka";
    public override string Description => "Damage and blast radius are significantly increased.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var rocket = Game.instance.model.GetTower(TowerType.BombShooter, 0, 2).GetAttackModel().weapons[0].Duplicate();
        var homing = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();

        homing.distance = 999;
        homing.constantlyAquireNewTarget = true;

        rocket.projectile.AddBehavior(homing);
        rocket.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3;
        rocket.projectile.GetBehavior<TravelStraitModel>().Speed /= 1.45f;
        rocket.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        if (towerModel.appliedUpgrades.Contains(UpgradeType.FasterFiring))
        {
            rocket.rate /= 1.15f;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.EvenFasterFiring))
        {
            rocket.rate /= 1.2f;
        }


        var shrapnel = Game.instance.model.GetTower(TowerType.TackShooter).GetAttackModel().weapons[0].projectile.Duplicate();
        shrapnel.GetBehavior<TravelStraitModel>().Speed *= 1.5f;
        shrapnel.GetBehavior<TravelStraitModel>().Lifespan *= 1.75f;
        shrapnel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        if (towerModel.appliedUpgrades.Contains(UpgradeType.ShrapnelShot))
        {
            shrapnel.GetDamageModel().damage = 4;
            shrapnel.pierce = 6;
        }
        else
        {
            shrapnel.GetDamageModel().damage = 3;
            shrapnel.pierce = 4;
        }

        var fragmentModel = new CreateProjectileOnContactModel("aaa", shrapnel, new ArcEmissionModel("ArcEmissionModel_", 16, 0, 360, null, true, false), true, false, false)
        { name = "RifleShrapnel_" };

        rocket.projectile.AddBehavior(fragmentModel);


        var blast = rocket.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
        blast.GetDamageModel().damage = 7;
        blast.radius = 30;
        blast.pierce = 65;
        blast.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        blast.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.FullMetalJacket))
        {
            blast.GetDamageModel().damage += 1;
            shrapnel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.LargeCalibre))
        {
            blast.GetDamageModel().damage += 2;
        }


        var explosion = rocket.projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
        explosion.scale *= 3f;


        towerModel.GetAttackModel().weapons[0] = rocket;
    }
}

public class BigBang : UpgradePlusPlus<SniperAltPath>
{
    public override int Cost => 96000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Sniper Icon";
    public override string Portrait => "Tier5 Sniper";

    public override string DisplayName => "Big Bang";
    public override string Description => "World destroying power in a single rifle.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var megaRocket = Game.instance.model.GetTower(TowerType.BombShooter, 0, 2).GetAttackModel().weapons[0].Duplicate();
        var homing = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();

        homing.distance = 999;
        homing.constantlyAquireNewTarget = true;

        megaRocket.projectile.AddBehavior(homing);
        megaRocket.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3;
        megaRocket.projectile.GetBehavior<TravelStraitModel>().Speed /= 1.5f;
        megaRocket.projectile.display = Game.instance.model.GetTower(TowerType.BombShooter, 0, 4).GetAttackModel().weapons[0].projectile.display;
        megaRocket.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);


        var megaBlast = megaRocket.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
        megaBlast.GetDamageModel().damage = 225;
        megaBlast.radius = 85;
        megaBlast.pierce = 999;
        megaBlast.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        megaBlast.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.FullMetalJacket))
        {
            megaBlast.GetDamageModel().damage += 10;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.LargeCalibre))
        {
            megaBlast.GetDamageModel().damage += 15;
        }


        var explosion = megaRocket.projectile.GetBehavior<CreateEffectOnContactModel>().effectModel;
        explosion.scale *= 8f;


        var miniRocket = Game.instance.model.GetTower(TowerType.BombShooter, 0, 2).GetAttackModel().weapons[0].Duplicate();

        miniRocket.projectile.AddBehavior(homing);
        miniRocket.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3;
        miniRocket.projectile.GetBehavior<TravelStraitModel>().Speed /= 1.35f;
        miniRocket.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);


        var miniBlast = miniRocket.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
        miniBlast.GetDamageModel().damage = 8;
        miniBlast.pierce = 24;
        miniBlast.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        miniBlast.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        var fragmentModel = new CreateProjectileOnContactModel("aaa", miniRocket.projectile, new ArcEmissionModel("ArcEmissionModel_", 6, 0, 360, null, true, false), true, false, false)
        { name = "RifleShrapnel_" };

        if (towerModel.appliedUpgrades.Contains(UpgradeType.ShrapnelShot))
        {
            miniBlast.GetDamageModel().damage += 2;
            miniBlast.pierce += 4;

            fragmentModel.emission = new ArcEmissionModel("ArcEmissionModel_", 8, 0, 360, null, true, false);
        }

        megaRocket.projectile.AddBehavior(fragmentModel);


        towerModel.GetAttackModel().weapons[0] = megaRocket;
    }
}