using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Linq;
using System.Linq;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using PathsPlusPlus;
using AlternatePaths;
using PlasmaEffects;

namespace BombShooter;

public class Incendiary : UpgradePlusPlus<BombAltPath>
{
    public override int Cost => 280;
    public override int Tier => 1;
    public override string Icon => "Tier1 Bomb Icon";
    public override string Portrait => "Tier1 Bomb";

    public override string DisplayName => "Incendiary Ordinance";
    public override string Description => "Bombs now set Bloons on fire.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(fire);
            weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, -1 };
        }
    }
}

public class CeramicBuster : UpgradePlusPlus<BombAltPath>
{
    public override int Cost => 500;
    public override int Tier => 2;
    public override string Icon => "Tier2 Bomb Icon";
    public override string Portrait => "Tier2 Bomb";

    public override string DisplayName => "Ceramic Buster";
    public override string Description => "Bombs deal significant extra damage to Ceramic and Fortified Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.hasDamageModifiers = true;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
            weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 1, 3, false, false) { name = "FortifiedModifier_" });
        }
    }
}

public class LightningCharge : UpgradePlusPlus<BombAltPath>
{
    public override int Cost => 1200;
    public override int Tier => 3;
    public override string Icon => "Tier3 Bomb Icon";
    public override string Portrait => "Tier3 Bomb";

    public override string DisplayName => "Lightning Charge";
    public override string Description => "Electrically charged bombs release a small lightning effect when they explode.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var super = Game.instance.model.GetTowerFromId(TowerType.SuperMonkey + "-050");

        var techTerror = super.GetDescendants<AttackModel>().ToArray().First(a => a.name == "AttackModel_TechTerror_").Duplicate();

        var lightning = techTerror.weapons[0].projectile;
        lightning.GetBehavior<AgeModel>().Lifespan = 0.1f;
        lightning.radius = 8;
        lightning.scale = 8;
        lightning.pierce = 10;
        lightning.GetDamageModel().damage = 1;
        lightning.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        lightning.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        var lightningBehavior = new CreateProjectileOnContactModel("", lightning, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 0, null, false, false), true, false, false)
        { name = "PlasmaBlast_" };


        var lightningVisual = lightning.Duplicate();
        lightningVisual.RemoveBehavior<DamageModel>();
        lightningVisual.RemoveBehavior<ProjectileFilterModel>();
        lightningVisual.RemoveBehavior<DistributeToChildrenBloonModifierModel>();
        lightningVisual.GetBehavior<AgeModel>().Lifespan = 0.75f;
        lightningVisual.ApplyDisplay<LightningDisplay>();

        var lightningVisualBehavior = new CreateProjectileOnContactModel("", lightningVisual, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 0, null, false, false), true, false, false)
        { name = "PlasmaVisual_" };


        var druid = Game.instance.model.GetTower(TowerType.Druid, 2);
        var lightningBolt = druid.GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").Duplicate();

        var charge = lightningBolt.projectile;
        charge.pierce = 10;
        charge.GetBehavior<LightningModel>().splitRange = towerModel.range * 1.5f;
        charge.GetBehavior<LightningModel>().splits = 1;

        charge.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
        charge.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        var chargeBehavior = new CreateProjectileOnContactModel("", charge, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 0, null, false, false), true, false, false)
        { name = "Lightning_" };

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.AddBehavior(lightningBehavior);
            weaponModel.projectile.AddBehavior(lightningVisualBehavior);
            weaponModel.projectile.AddBehavior(chargeBehavior);

            weaponModel.projectile.RemoveBehavior<CreateEffectOnContactModel>();
        }
    }
}

public class PlasmaBomb : UpgradePlusPlus<BombAltPath>
{
    public override int Cost => 4500;
    public override int Tier => 4;
    public override string Icon => "Tier4 Bomb Icon";
    public override string Portrait => "Tier4 Bomb";

    public override string DisplayName => "Plasma Bombs";
    public override string Description => "Bomb Shooter fires unstable plasma bombs that explode violently.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var behaviors in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
        {
            if (behaviors.name == "PlasmaBlast_")
            {
                behaviors.projectile.radius = 30;
                behaviors.projectile.scale = 30;
                behaviors.projectile.pierce = 35;
                behaviors.projectile.GetDamageModel().damage = 10;
                behaviors.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 20, false, false) { name = "MoabModifier_" });
            }
            else if (behaviors.name == "PlasmaVisual_")
            {
                behaviors.projectile.ApplyDisplay<PlasmaDisplay>();
            }
            else if (behaviors.name == "Lightning_")
            {
                behaviors.projectile.pierce = 25;
                behaviors.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }
            else
            {
                behaviors.projectile.GetDamageModel().damage += 9;
                behaviors.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 20, false, false) { name = "MoabModifier_" });
            }
        }
    }
}

public class SuperNova : UpgradePlusPlus<BombAltPath>
{
    public override int Cost => 48000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Bomb Icon";
    public override string Portrait => "Tier5 Bomb";

    public override string DisplayName => "Super Nova";
    public override string Description => "Highly condensed plasma bombs create devastating explosions.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var behaviors in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
        {
            if (behaviors.name == "PlasmaBlast_")
            {
                behaviors.projectile.radius = 75;
                behaviors.projectile.scale = 75;
                behaviors.projectile.pierce = 90;
                behaviors.projectile.GetDamageModel().damage = 35;
                behaviors.projectile.GetBehavior<DamageModifierForTagModel>().damageAddative *= 3;
            }
            else if (behaviors.name == "PlasmaVisual_")
            {
                behaviors.projectile.ApplyDisplay<NovaDisplay>();
            }
            else if (behaviors.name == "Lightning_")
            {
                behaviors.emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 0, null, false, false);
                behaviors.projectile.pierce = 20;
                behaviors.projectile.GetDamageModel().damage = 4;
            }
            else
            {
                behaviors.projectile.GetDamageModel().damage += 25;

                foreach (var damageMod in behaviors.projectile.GetBehaviors<DamageModifierForTagModel>().ToArray())
                {
                    damageMod.damageAddative *= 3;
                }

                if (towerModel.appliedUpgrades.Contains(UpgradeType.HeavyBombs))
                {
                    behaviors.projectile.GetDamageModel().damage += 9;
                }
            }
        }
    }
}