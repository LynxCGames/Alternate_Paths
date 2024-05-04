using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using AlternatePaths.Displays.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;

namespace Buccaneer;

public class Ripper : UpgradePlusPlus<BuccaneerAltPath>
{
    public override int Cost => 300;
    public override int Tier => 1;
    public override string Icon => "Tier1 Buccaneer Icon";
    public override string Portrait => "Tier1 Buccaneer";

    public override string Description => "Darts cause a bleed effect on Bloons which makes them take damage over time.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var bleed = Game.instance.model.GetTowerFromId("Sauda 9").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();

        foreach (var behavior in towerModel.GetBehaviors<AttackModel>())
        {
            behavior.weapons[0].projectile.AddBehavior(bleed);
            behavior.weapons[0].projectile.collisionPasses = new[] { -1, 0, 1 };
        }
    }
}

public class RaiseMoral : UpgradePlusPlus<BuccaneerAltPath>
{
    public override int Cost => 440;
    public override int Tier => 2;
    public override string Icon => "Tier2 Buccaneer Icon";
    public override string Portrait => "Tier2 Buccaneer";

    public override string DisplayName => "Raise Moral";
    public override string Description => "Gives a 6% stacking attack speed buff to nearby towers.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.AddBehavior(new RateSupportModel("Moral_SpeedBuff", 0.94f, true, "rateSupportZone", false, 1, null, "", "RaiseMoralBuff"));
    }
}

public class Dreadnought : UpgradePlusPlus<BuccaneerAltPath>
{
    public override int Cost => 1100;
    public override int Tier => 3;
    public override string Icon => "Tier3 Buccaneer Icon";
    public override string Portrait => "Tier3 Buccaneer";

    public override string Description => "Darts are replaced with heated lead that deal massive damage and fly faster.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel(0).weapons[0].projectile.GetDamageModel().damage += 3;
        towerModel.GetAttackModel(0).weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 2;

        towerModel.GetAttackModel(0).weapons[0].projectile.ApplyDisplay<LeadBall>();

        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 1;
        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval -= 1f;
    }
}

public class GhostShip : UpgradePlusPlus<BuccaneerAltPath>
{
    public override int Cost => 9500;
    public override int Tier => 4;
    public override string Icon => "Tier4 Buccaneer Icon";
    public override string Portrait => "Tier4 Buccaneer";

    public override string DisplayName => "Ghost Ship";
    public override string Description => "Gains a soul aura that damages Bloons that come near and resurrects them as members of the ghost crew.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel(0).weapons[0].projectile.ApplyDisplay<GhostBall>();


        var aura = Game.instance.model.GetTower(TowerType.TackShooter, 4).GetAttackModel().Duplicate();
        aura.weapons[0].projectile.GetDamageModel().damage = 5;
        aura.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        aura.name = "SoulAura";
        towerModel.AddBehavior(aura);

        var orbit = Game.instance.model.GetTower(TowerType.BoomerangMonkey, 5).GetBehavior<OrbitModel>().Duplicate();
        orbit.projectile.ApplyDisplay<T4SouldOrb>();
        orbit.range = aura.range;
        orbit.count = 5;
        towerModel.AddBehavior(orbit);

        if (towerModel.appliedUpgrades.Contains(UpgradeType.CrowsNest))
        {
            aura.weapons[0].GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }


        var necro = Game.instance.model.GetTower(TowerType.WizardMonkey, 0, 0, 4).GetBehavior<NecromancerZoneModel>().Duplicate();
        var necroWeapon = Game.instance.model.GetTower(TowerType.WizardMonkey, 0, 0, 4).GetAttackModel(2).Duplicate();

        necro.name = "Necrozone_";
        necroWeapon.name = "NecroAttack_";

        towerModel.AddBehavior(necroWeapon);
        towerModel.AddBehavior(necro);
    }
}

public class FlyingDutchman : UpgradePlusPlus<BuccaneerAltPath>
{
    public override int Cost => 51700;
    public override int Tier => 5;
    public override string Icon => "Tier5 Buccaneer Icon";
    public override string Portrait => "Tier5 Buccaneer";

    public override string DisplayName => "Flying Dutchman";
    public override string Description => "Ghost crew and soul aura are much stronger. Fires three lead balls at once.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel(0).weapons[0].projectile.GetDamageModel().damage += 3;

        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage += 4;
        towerModel.GetAttackModel(0).weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = 0.5f;

        var ball2 = towerModel.GetAttackModel(0).weapons[0].Duplicate();
        var ball3 = towerModel.GetAttackModel(0).weapons[0].Duplicate();

        ball2.ejectY = 15;
        ball3.ejectY = -15;

        towerModel.GetAttackModel(0).AddWeapon(ball2);
        towerModel.GetAttackModel(0).AddWeapon(ball3);



        foreach (var behevior in towerModel.GetBehaviors<AttackModel>())
        {
            if (behevior.name.Contains("SoulAura"))
            {
                towerModel.RemoveBehavior(behevior);
            }

            if (behevior.name.Contains("NecroAttack_"))
            {
                towerModel.RemoveBehavior(behevior);
            }
        }

        towerModel.RemoveBehavior<NecromancerZoneModel>();

        var aura = Game.instance.model.GetTower(TowerType.TackShooter, 5, 2).GetAttackModel().Duplicate();
        aura.weapons[0].projectile.GetDamageModel().damage = 9;
        aura.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        aura.name = "SoulAura";
        towerModel.AddBehavior(aura);

        towerModel.GetBehavior<OrbitModel>().range = aura.range;
        towerModel.GetBehavior<OrbitModel>().count *= 2;


        var necro = Game.instance.model.GetTower(TowerType.WizardMonkey, 0, 0, 5).GetBehavior<NecromancerZoneModel>().Duplicate();
        var necroWeapon = Game.instance.model.GetTower(TowerType.WizardMonkey, 0, 0, 5).GetAttackModel(2).Duplicate();

        necro.name = "Necrozone_";
        necroWeapon.name = "NecroAttack_";

        towerModel.AddBehavior(necroWeapon);
        towerModel.AddBehavior(necro);
    }
}