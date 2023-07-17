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
using System;
using Il2CppAssets.Scripts.Data.Quests;
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
        var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.
            GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();


        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
        {
            behavior.projectile.AddBehavior(fire);
            behavior.projectile.collisionPasses = new int[] { 0, -1 };


            if (behavior.projectile.HasBehavior<CreateProjectileOnExhaustFractionModel>() == true)
            {
                behavior.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(fire);
                behavior.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.collisionPasses = new int[] { 0, -1 };
            }
        }


        foreach (var mainBomb in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
        {
            mainBomb.projectile.AddBehavior(fire);
            mainBomb.projectile.collisionPasses = new int[] { 0, -1 };


            foreach (var clusterBomb in mainBomb.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
            {
                clusterBomb.projectile.AddBehavior(fire);
                clusterBomb.projectile.collisionPasses = new int[] { 0, -1 };


                foreach (var secondCluster in clusterBomb.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
                {
                    secondCluster.projectile.AddBehavior(fire);
                    secondCluster.projectile.collisionPasses = new int[] { 0, -1 };
                }
            }
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
        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
        {
            behavior.projectile.hasDamageModifiers = true;

            behavior.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
            behavior.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 1, 3, false, false) { name = "FortifiedModifier_" });


            if (behavior.projectile.HasBehavior<CreateProjectileOnExhaustFractionModel>() == true)
            {
                behavior.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.hasDamageModifiers = true;

                behavior.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
                behavior.projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 1, 3, false, false) { name = "FortifiedModifier_" });
            }
        }


        foreach (var mainBomb in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
        {
            mainBomb.projectile.hasDamageModifiers = true;

            mainBomb.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
            mainBomb.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 1, 3, false, false) { name = "FortifiedModifier_" });


            foreach (var clusterBomb in mainBomb.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
            {
                clusterBomb.projectile.hasDamageModifiers = true;

                clusterBomb.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
                clusterBomb.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 1, 3, false, false) { name = "FortifiedModifier_" });


                foreach (var secondCluster in clusterBomb.projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
                {
                    secondCluster.projectile.hasDamageModifiers = true;

                    secondCluster.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
                    secondCluster.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 1, 3, false, false) { name = "FortifiedModifier_" });
                }
            }
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

        var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.
            GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();


        var charge = techTerror.weapons[0].projectile;
        charge.GetBehavior<AgeModel>().Lifespan = 0.1f;
        charge.radius = 8;
        charge.scale = 8;
        charge.pierce = 10;
        charge.GetDamageModel().damage = 1;
        charge.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        charge.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        charge.AddBehavior(fire);

        var chargeBehavior = new CreateProjectileOnContactModel("", charge, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 0, null, false, false), true, false, false) { name = "PlasmaBlast_" };


        var lightningVisual = charge.Duplicate();
        lightningVisual.RemoveBehavior<DamageModel>();
        lightningVisual.RemoveBehavior<ProjectileFilterModel>();
        lightningVisual.RemoveBehavior<DistributeToChildrenBloonModifierModel>();
        lightningVisual.GetBehavior<AgeModel>().Lifespan = 0.75f;
        lightningVisual.ApplyDisplay<LightningDisplay>();

        var lightningVisualBehavior = new CreateProjectileOnContactModel("", lightningVisual, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 0, null, false, false), true, false, false) { name = "PlasmaVisual_" };


        var druid = Game.instance.model.GetTower(TowerType.Druid, 2);
        var lightningBolt = druid.GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").Duplicate();

        var lightning = lightningBolt.projectile;
        lightning.pierce = 10;
        lightning.GetBehavior<LightningModel>().splitRange = towerModel.range * 2f;
        lightning.GetBehavior<LightningModel>().splits = 1;
        lightning.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
        lightning.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        lightning.AddBehavior(fire);

        var lightningBehavior = new CreateProjectileOnContactModel("", lightning, new ArcEmissionModel("ArcEmissionModel_", 1, 0, 0, null, false, false), true, false, false) { name = "Lightning_" };


        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            if (towerModel.appliedUpgrades.Contains(UpgradeType.ClusterBombs)) { }
            else
            {
                weaponModel.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }

            weaponModel.projectile.AddBehavior(chargeBehavior);
            weaponModel.projectile.AddBehavior(lightningVisualBehavior);
            weaponModel.projectile.AddBehavior(lightningBehavior);
        }


        towerModel.GetAttackModel().weapons[0].projectile.RemoveBehavior<CreateEffectOnContactModel>();
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
        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
        {
            if (behavior.name == "PlasmaBlast_")
            {
                behavior.projectile.radius += 22;
                behavior.projectile.scale = 30;
                behavior.projectile.pierce += 25;
                behavior.projectile.GetDamageModel().damage += 9;
                behavior.projectile.hasDamageModifiers = true;
                behavior.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 20, false, false) { name = "MoabModifier_" });
            }
            else if (behavior.name == "PlasmaVisual_")
            {
                behavior.projectile.ApplyDisplay<PlasmaDisplay>();
            }
            else if (behavior.name == "Lightning_")
            {
                behavior.projectile.pierce += 15;
                behavior.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                behavior.projectile.GetBehavior<LightningModel>().splitRange *= 2;
            }
            else
            {
                if (behavior.projectile.HasBehavior<DamageModel>())
                {
                    behavior.projectile.GetDamageModel().damage += 9;
                    behavior.projectile.hasDamageModifiers = true;
                    behavior.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 20, false, false) { name = "MoabModifier_" });
                }
            }
        }

        foreach (var mainBomb in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
        {
            if (mainBomb.projectile.HasBehavior<DamageModel>())
            {
                mainBomb.projectile.GetDamageModel().damage += 9;
                mainBomb.projectile.hasDamageModifiers = true;
                mainBomb.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 20, false, false) { name = "MoabModifier_" });
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
        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>().ToArray())
        {
            if (behavior.name == "PlasmaBlast_")
            {
                behavior.projectile.radius += 45;
                behavior.projectile.scale = 75;
                behavior.projectile.pierce += 55;
                behavior.projectile.GetDamageModel().damage += 25;
                behavior.projectile.GetBehavior<DamageModifierForTagModel>().damageAddative *= 3;
            }
            else if (behavior.name == "PlasmaVisual_")
            {
                behavior.projectile.ApplyDisplay<NovaDisplay>();
            }
            else if (behavior.name == "Lightning_")
            {
                behavior.emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 0, null, false, false);
                behavior.projectile.pierce += 20;
                behavior.projectile.GetDamageModel().damage += 2;
                behavior.projectile.GetBehavior<LightningModel>().splitRange *= 2;
            }
            else
            {
                if (behavior.projectile.HasBehavior<DamageModel>())
                {
                    behavior.projectile.GetDamageModel().damage += 25;

                    foreach (var damageMod in behavior.projectile.GetBehaviors<DamageModifierForTagModel>().ToArray())
                    {
                        damageMod.damageAddative *= 3;
                    }

                    if (towerModel.appliedUpgrades.Contains(UpgradeType.HeavyBombs))
                    {
                        behavior.projectile.GetDamageModel().damage += 9;
                    }
                }
            }
        }

        foreach (var mainBomb in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>().ToArray())
        {
            if (mainBomb.projectile.HasBehavior<DamageModel>())
            {
                mainBomb.projectile.GetDamageModel().damage += 25;

                foreach (var damageMod in mainBomb.projectile.GetBehaviors<DamageModifierForTagModel>().ToArray())
                {
                    damageMod.damageAddative *= 3;
                }

                if (towerModel.appliedUpgrades.Contains(UpgradeType.HeavyBombs))
                {
                    mainBomb.projectile.GetDamageModel().damage += 9;
                }
            }
        }
    }
}