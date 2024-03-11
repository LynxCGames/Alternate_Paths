using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using AlternatePaths.Displays.Projectiles;

namespace GlueGunner;

public class PressureGluer : UpgradePlusPlus<GlueGunnerAltPath>
{
    public override int Cost => 230;
    public override int Tier => 1;
    public override string Icon => "Tier1 Glue Icon";
    public override string Portrait => "Tier1 Glue";

    public override string DisplayName => "Pressure Gluer";
    public override string Description => "Can now detect Camo Bloons and glue is capable of stripping camo properties off of Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var decamo = Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<RemoveBloonModifiersModel>().Duplicate();

        towerModel.GetAttackModel().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(decamo);

        if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueSplatter))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(decamo);
        }
    }
}

public class GlueMine : UpgradePlusPlus<GlueGunnerAltPath>
{
    public override int Cost => 680;
    public override int Tier => 2;
    public override string Icon => "Tier2 Glue Icon";
    public override string Portrait => "Tier2 Glue";

    public override string DisplayName => "Glue Mines";
    public override string Description => "Periodically places glue mines on the track that will explode when Bloons get near it. Glue mines use the same glue as the Glue Gunner. (Set to normal targeting to start placing mines)";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var trap = Game.instance.model.GetTower(TowerType.SpikeFactory).GetAttackModel().Duplicate();
        var bomb = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();

        trap.weapons[0].projectile.pierce = 1;
        trap.weapons[0].projectile.GetDamageModel().damage = 0;
        trap.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        trap.weapons[0].projectile.RemoveBehavior<SetSpriteFromPierceModel>();
        trap.weapons[0].projectile.GetBehavior<AgeModel>().Lifespan /= 3;

        bomb.GetDamageModel().damage = 0;
        bomb.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        bomb.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
        bomb.collisionPasses = new int[] { -1, 0, 1 };


        if (towerModel.appliedUpgrades.Contains(UpgradeType.TheBloonSolver))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-500").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapGreen>();

            bomb.AddBehavior(slowModel);
            bomb.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-500").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.BloonLiquefier))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-400").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapGreen>();

            bomb.AddBehavior(slowModel);
            bomb.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-400").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.BloonDissolver))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-300").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapGreen>();

            bomb.AddBehavior(slowModel);
            bomb.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-300").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.CorrosiveGlue))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapYellow>();

            bomb.AddBehavior(slowModel);
            bomb.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueSoak))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-100").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapYellow>();

            bomb.AddBehavior(slowModel);
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.SuperGlue))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-005").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapPink>();

            bomb.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-005").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
            bomb.AddBehavior(slowModel);
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.RelentlessGlue))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-004").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapPink>();

            bomb.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-004").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
            bomb.AddBehavior(slowModel);
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.MOABGlue))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-003").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapPink>();

            bomb.AddBehavior(slowModel);
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.StrongerGlue))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-002").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapYellow>();

            bomb.AddBehavior(slowModel);
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.StickierGlue))
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-001").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", glue.layers, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapYellow>();

            bomb.AddBehavior(slowModel);
        }
        else
        {
            var glue = Game.instance.model.GetTowerFromId("GlueGunner-100").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            var slowModel = new SlowModel("SlowModel_", glue.multiplier, glue.lifespan, "TrapSlow", 3, glue.overlayType, glue.isUnique, glue.dontRefreshDuration, glue.effectModel, true, false, false);
            trap.weapons[0].projectile.ApplyDisplay<GlueTrapYellow>();

            bomb.AddBehavior(slowModel);
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.BiggerGlobs))
        {
            bomb.pierce += 2;
            bomb.radius += 4;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueSplatter))
        {
            bomb.pierce += 3;
            bomb.radius *= 2;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueHose))
        {
            trap.weapons[0].rate /= 3f;
        }


        trap.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("aaa", bomb, new ArcEmissionModel("TrapBomb_", 1, 0, 0, null, true, false), false, false, true));

        towerModel.AddBehavior(trap);
    }
}

public class AcidGun : UpgradePlusPlus<GlueGunnerAltPath>
{
    public override int Cost => 2100;
    public override int Tier => 3;
    public override string Icon => "Tier3 Glue Icon";
    public override string Portrait => "Tier3 Glue";

    public override string DisplayName => "Acid Gunner";
    public override string Description => "Now fires acid that deals contact damage to Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var acid = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].Duplicate();

        acid.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        acid.rate = Game.instance.model.GetTower(TowerType.GlueGunner).GetAttackModel().weapons[0].rate / 1.4f;
        acid.projectile.display = Game.instance.model.GetTower(TowerType.GlueGunner, 3).GetAttackModel().weapons[0].projectile.display;
        acid.projectile.GetDamageModel().damage = 2;
        acid.projectile.pierce = 3;
        acid.projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<RemoveBloonModifiersModel>().Duplicate());
        acid.projectile.collisionPasses = new int[] { -1, 0, 1 };

        var overlay = Game.instance.model.GetTowerFromId("GlueGunner-300").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().overlayType;
        acid.projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-100").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());


        if (towerModel.appliedUpgrades.Contains(UpgradeType.BiggerGlobs))
        {
            acid.projectile.pierce += 1;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueSplatter))
        {
            acid.projectile.pierce += 3;
        }


        var slow = Game.instance.model.GetTowerFromId("GlueGunner-100").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
        var slowModel = new SlowModel("SlowModel_", slow.multiplier, slow.lifespan, "AcidSlow", 3, overlay, slow.isUnique, slow.dontRefreshDuration, slow.effectModel, true, false, false);


        if (towerModel.appliedUpgrades.Contains(UpgradeType.GlueSoak))
        {
            var newSlow = Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            slowModel.multiplier = newSlow.multiplier;
            slowModel.lifespan = newSlow.lifespan;
            slowModel.layers = newSlow.layers;
            slow.effectModel = newSlow.effectModel;

            if (towerModel.appliedUpgrades.Contains(UpgradeType.CorrosiveGlue))
            {
                var corrosive = Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();
                var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.
                    GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate();

                fire.overlayType = overlay;
                fire.GetBehavior<DamageOverTimeModel>().name = "AcidModel_";
                fire.GetBehavior<DamageOverTimeModel>().damage = corrosive.GetBehavior<DamageOverTimeModel>().damage;
                fire.GetBehavior<DamageOverTimeModel>().interval = corrosive.GetBehavior<DamageOverTimeModel>().interval / 1.25f;
                fire.lifespan = corrosive.lifespan;

                acid.projectile.AddBehavior(fire);
            }
        }
        
        if (towerModel.appliedUpgrades.Contains(UpgradeType.StrongerGlue))
        {
            var newSlow = Game.instance.model.GetTowerFromId("GlueGunner-002").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
            slowModel.multiplier = newSlow.multiplier;
            slowModel.lifespan = newSlow.lifespan;
            slowModel.layers = newSlow.layers;
            slow.effectModel = newSlow.effectModel;
        }


        acid.projectile.AddBehavior(slowModel);
        towerModel.GetAttackModel().weapons[0] = acid;

        towerModel.GetAttackModel().GetBehavior<AttackFilterModel>().filters = Game.instance.model.GetTowerFromId("DartMonkey-003").GetAttackModel().GetBehavior<AttackFilterModel>().filters;
        towerModel.GetAttackModel().weapons[0].GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);


        var glue = Game.instance.model.GetTowerFromId("GlueGunner-300").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate();
        towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<SlowModel>().overlayType = glue.overlayType;
        towerModel.GetAttackModel(1).weapons[0].projectile.ApplyDisplay<GlueTrapGreen>();

        towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 2;
    }
}

public class DeadlyVenom : UpgradePlusPlus<GlueGunnerAltPath>
{
    public override int Cost => 4400;
    public override int Tier => 4;
    public override string Icon => "Tier4 Glue Icon";
    public override string Portrait => "Tier4 Glue";

    public override string DisplayName => "Deadly Venom";
    public override string Description => "Acid is even stronger and is fired much quicker.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 2;
        towerModel.GetAttackModel().weapons[0].rate /= 2;
        towerModel.GetAttackModel().weapons[0].emission = new RandomArcEmissionModel("VenomGunner_", 3, 0, 0, 35, 0, null);

        towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 2;
    }
}

public class KingCobra : UpgradePlusPlus<GlueGunnerAltPath>
{
    public override int Cost => 109000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Glue Icon";
    public override string Portrait => "Tier5 Glue";

    public override string DisplayName => "King Cobra";
    public override string Description => "Acid so potent that it will leave nothing behind.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].emission = new RandomArcEmissionModel("VenomGunner_", 8, 0, 0, 80, 0, null);

        towerModel.GetAttackModel().weapons[0].rate /= 2.5f;

        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage *= 5;
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 3, 0, false, false) { name = "MoabModifier_" });
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 2, 0, false, false) { name = "FortifiedModifier_" });

        if (towerModel.appliedUpgrades.Contains(UpgradeType.CorrosiveGlue) || towerModel.appliedUpgrades.Contains(UpgradeType.GlueSplatter) || towerModel.appliedUpgrades.Contains(UpgradeType.StrongerGlue))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage *= 3;
        }

        towerModel.GetAttackModel(1).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage *= 8;
    }
}