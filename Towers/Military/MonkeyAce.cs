using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;

namespace MonkeyAce;

public class SplodeyDarts : UpgradePlusPlus<PlaneAltPath>
{
    public override int Cost => 630;
    public override int Tier => 1;
    public override string Icon => "Tier1 Ace Icon";
    public override string Portrait => "Tier1 Ace";

    public override string DisplayName => "Splodey Darts";
    public override string Description => "Darts cause a small explosion when they hit a Bloon. Sharper Darts allow the explosion to pop more Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var bomb = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
        var effect = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();

        bomb.name = "SplodeyDart";

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
        {
            bomb.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SharperDarts))
        {
            bomb.projectile.pierce *= 2;
        }

        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(bomb);
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(effect);
        towerModel.GetAttackModel().weapons[0].projectile.pierce = 1;
    }
}


public class CrackShot : UpgradePlusPlus<PlaneAltPath>
{
    public override int Cost => 740;
    public override int Tier => 2;
    public override string Icon => "Tier2 Ace Icon";
    public override string Portrait => "Tier2 Ace";

    public override string DisplayName => "Crack Shot Darts";
    public override string Description => "Darts explode into even more darts.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var dart = Game.instance.model.GetTower(TowerType.DartMonkey).GetWeapon().projectile.Duplicate();

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
        {
            dart.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SharperDarts))
        {
            dart.pierce += 4;
        }


        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel
                    ("Crackshot", dart, new ArcEmissionModel("ArcEmissionModel_", 3, 0, 25, null, true, false), true, false, false));
    }
}

public class GatlingGun : UpgradePlusPlus<PlaneAltPath>
{
    public override int Cost => 2780;
    public override int Tier => 3;
    public override string Icon => "Tier3 Ace Icon";
    public override string Portrait => "Tier3 Ace";

    public override string DisplayName => "Mounted Gatling Gun";
    public override string Description => "Gains a front facing gatling gun that rapidly fires darts.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var gatling = Game.instance.model.GetTower(TowerType.HeliPilot, 4).GetAttackModel().weapons[2].Duplicate();
        gatling.projectile.GetDamageModel().damage += 1;
        gatling.name = "GatlingGun";

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
        {
            gatling.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        towerModel.GetAttackModel().AddWeapon(gatling);
    }
}

public class DogFighter : UpgradePlusPlus<PlaneAltPath>
{
    public override int Cost => 22200;
    public override int Tier => 4;
    public override string Icon => "Tier4 Ace Icon";
    public override string Portrait => "Tier4 Ace";

    public override string DisplayName => "Dog Fighter";
    public override string Description => "Plane flies a lot faster and gatling gun fires a small spread of darts that deal more damage.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetBehavior<AirUnitModel>().GetBehavior<PathMovementModel>().speed *= 2.5f;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;


        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>())
        {
            if (behavior.name.Contains("SplodeyDart"))
            {
                behavior.projectile.GetDamageModel().damage += 2;
            }
            else if (behavior.name.Contains("Crackshot"))
            {
                behavior.emission = new ArcEmissionModel("ArcEmissionModel_", 4, 0, 35, null, true, false);
            }
        }
        

        foreach (var weapon in towerModel.GetAttackModel().weapons) 
        {
            if (weapon.name.Contains("GatlingGun"))
            {
                weapon.projectile.GetDamageModel().damage += 2;
                weapon.emission = new RandomArcEmissionModel("emission", 4, 0, 0, 45, 0, null);
            }
        }
    }
}

public class SidewinderAce : UpgradePlusPlus<PlaneAltPath>
{
    public override int Cost => 51000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Ace Icon";
    public override string Portrait => "Tier5 Ace";

    public override string DisplayName => "Sidewinder Ace";
    public override string Description => "Now has two wingmonkey planes to help devastate the Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 3;
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 3, 0, false, false) { name = "MoabModifier_" });


        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>())
        {
            if (behavior.name.Contains("SplodeyDart"))
            {
                behavior.projectile.GetDamageModel().damage += 8;
                behavior.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }
            else if (behavior.name.Contains("Crackshot"))
            {
                behavior.projectile.GetDamageModel().damage += 2;
                behavior.projectile.pierce *= 2;
                behavior.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }
        }


        foreach (var weapon in towerModel.GetAttackModel().weapons)
        {
            if (weapon.name.Contains("GatlingGun"))
            {
                weapon.projectile.GetDamageModel().damage += 3;
                weapon.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 4, 0, false, false) { name = "MoabModifier_" });
                weapon.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }
        }


        var carrier = Game.instance.model.GetTower(TowerType.MonkeyBuccaneer, 5).GetAttackModel(1).Duplicate();
        var plane = carrier.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower;
        var gatling = Game.instance.model.GetTower(TowerType.HeliPilot, 4).GetAttackModel().weapons[2].Duplicate();

        plane.GetBehavior<TowerExpireOnParentUpgradedModel>().parentTowerUpgradeTier = 5;
        plane.GetAttackModel(1).weapons[0].rate = towerModel.GetAttackModel().weapons[0].rate;
        plane.GetAttackModel(1).weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        plane.GetAttackModel(0).weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        plane.GetAttackModel(0).weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 3, 2, false, false) { name = "MoabModifier_" });

        plane.GetDescendant<FighterMovementModel>().maxSpeed *= 2f;
        plane.RemoveBehavior(plane.GetAttackModel(2));

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SpyPlane))
        {
            plane.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }


        gatling.name = "GatlingGun";
        gatling.projectile.GetBehavior<TravelStraitModel>().Lifespan /= 2;
        gatling.projectile.pierce = 4;
        gatling.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        gatling.projectile.GetDamageModel().damage = 6;
        gatling.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 4, 0, false, false) { name = "MoabModifier_" });

        plane.GetAttackModel(1).AddWeapon(gatling);


        carrier.weapons[0].GetBehavior<SubTowerFilterModel>().maxNumberOfSubTowers = 2;

        towerModel.AddBehavior(carrier);
    }
}