using AlternatePaths;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using PathsPlusPlus;

namespace IceMonkey;

public class CrystalShard : UpgradePlusPlus<IceMonkeyAltPath>
{
    public override int Cost => 160;
    public override int Tier => 1;
    public override string Icon => "Tier1 Ice Icon";
    public override string Portrait => "Tier1 Ice";

    public override string DisplayName => "Crystal Shard";
    public override string Description => "Shoots ice shards that damage and slow Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var shard = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().Duplicate();
        shard.name = "ShardModel";
        shard.weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.IceMonkey, 0, 0, 5).GetAttackModel().weapons[0].projectile.display;
        shard.weapons[0].projectile.scale /= 1.25f;
        shard.weapons[0].rate /= 1.2f;
        shard.weapons[0].projectile.AddBehavior(new SlowModel("SlowModel_", .7f, 3f, "ShardSlow", 9999999, "", true, false, null, true, false, false));
        shard.weapons[0].projectile.collisionPasses = new int[] { 0, -1 };

        towerModel.GetAttackModel().RemoveBehavior<TargetCloseModel>();


        if (towerModel.appliedUpgrades.Contains(UpgradeType.ColdSnap))
        { 
            shard.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
            shard.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        else 
        { 
            shard.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead | BloonProperties.White; 
        }

        towerModel.AddBehavior(shard);


        if (towerModel.appliedUpgrades.Contains(UpgradeType.CryoCannon))
        {
            towerModel.GetAttackModel(1).RemoveWeapon(towerModel.GetAttackModel(1).weapons[0]);
            towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1;
        }
    }
}


public class Frostbite : UpgradePlusPlus<IceMonkeyAltPath>
{
    public override int Cost => 220;
    public override int Tier => 2;
    public override string Icon => "Tier2 Ice Icon";
    public override string Portrait => "Tier2 Ice";

    public override string Description => "Ice shards deal more damage and now freeze Bloons instead.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (towerModel.appliedUpgrades.Contains(UpgradeType.CryoCannon)) { towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1; }
        else
        {
            foreach (var attack in towerModel.GetAttackModels())
            {
                if (attack.name == "ShardModel")
                {
                    attack.weapons[0].projectile.GetDamageModel().damage += 1;
                    attack.weapons[0].projectile.RemoveBehavior<SlowModel>();
                    attack.weapons[0].projectile.AddBehavior(new FreezeModel("FreezeModel_", 0, 1f, "ShardFreeze", 1, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, false));
                    attack.weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                }
            }
        }
    }
}

public class IceMagic : UpgradePlusPlus<IceMonkeyAltPath>
{
    public override int Cost => 2640;
    public override int Tier => 3;
    public override string Icon => "Tier3 Ice Icon";
    public override string Portrait => "Tier3 Ice";

    public override string DisplayName => "Ice Magic";
    public override string Description => "Ice shards are replaced with magic icicles that break into smaller homing shards. Cold Snap allows the icicles to strip off camo from Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();

        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;
        seeking.turnRate *= 2;


        var shard = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].projectile.Duplicate();
        shard.display = Game.instance.model.GetTower(TowerType.IceMonkey, 0, 0, 5).GetAttackModel().weapons[0].projectile.display;
        shard.scale /= 1.25f;
        shard.pierce += 2;
        shard.AddBehavior(seeking);
        shard.GetBehavior<TravelStraitModel>().Lifespan *= 6;


        if (towerModel.appliedUpgrades.Contains(UpgradeType.ColdSnap))
        { 
            shard.GetDamageModel().immuneBloonProperties = BloonProperties.White;
            shard.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

            shard.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<RemoveBloonModifiersModel>().Duplicate());
            towerModel.GetAttackModel(1).weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<RemoveBloonModifiersModel>().Duplicate());
        }
        else 
        { 
            shard.GetDamageModel().immuneBloonProperties = BloonProperties.Lead | BloonProperties.White; 
        }


        foreach (var attack in towerModel.GetAttackModels())
        {
            if (towerModel.appliedUpgrades.Contains(UpgradeType.LargerRadius))
            {
                towerModel.range = 61;
                attack.range = 61;
            }
            else
            {
                towerModel.range = 49;
                attack.range = 49; 
            }


            if (attack.name == "ShardModel")
            {
                if (attack.weapons[0] != null)
                {
                    attack.weapons[0].rate /= 1.2f;
                    attack.weapons[0].projectile.pierce = 1;
                    attack.weapons[0].projectile.scale *= 1.25f;
                    attack.weapons[0].projectile.GetDamageModel().damage += 2;
                    attack.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", shard, new ArcEmissionModel("ArcEmissionModel_", 3, 0, 30, null, true, false), true, false, false));
                }
            }
        }
    }
}

public class FrostBreath : UpgradePlusPlus<IceMonkeyAltPath>
{
    public override int Cost => 3200;
    public override int Tier => 4;
    public override string Icon => "Tier4 Ice Icon";
    public override string Portrait => "Tier4 Ice";

    public override string DisplayName => "Frost Breath";
    public override string Description => "Spews a continuous beam of frost that deals massive damage and freezes all Bloons hit.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var breath = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].Duplicate();
        breath.name = "FrostBreath";
        breath.projectile.display = Game.instance.model.GetTower(TowerType.IceMonkey, 0, 0, 3).GetAttackModel().weapons[0].projectile.display;
        breath.rate /= 12;
        breath.projectile.GetDamageModel().damage = 2;
        breath.projectile.pierce = 3;

        breath.projectile.AddBehavior(new FreezeModel("FreezeModel_", 0, 1f, "BreathFreeze", 999999, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, false));
        breath.projectile.collisionPasses = new int[] { 0, -1 };

        if (towerModel.appliedUpgrades.Contains(UpgradeType.ColdSnap)) 
        { 
            breath.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White; 
            breath.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
        else 
        { 
            breath.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead | BloonProperties.White; 
        }

        towerModel.GetAttackModel(1).AddWeapon(breath);


        foreach (var attack in towerModel.GetAttackModels())
        {
            if (attack.name == "ShardModel")
            {
                if (attack.weapons[0] != null)
                {
                    attack.weapons[0].projectile.GetDamageModel().damage *= 2;
                    attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().emission = new ArcEmissionModel("ArcEmissionModel_", 5, 0, 50, null, true, false);
                    attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1;
                    attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce *= 2;
                }
            }
        }
    }
}

public class PolarVortex : UpgradePlusPlus<IceMonkeyAltPath>
{
    public override int Cost => 71000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Ice Icon";
    public override string Portrait => "Tier5 Ice";

    public override string DisplayName => "Polar Vortex";
    public override string Description => "A true master of the cold capable of shredding Bloons with its orbiting icicles.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel(1).weapons[1].projectile.GetDamageModel().damage += 2;
        towerModel.GetAttackModel(1).weapons[1].projectile.hasDamageModifiers = true;
        towerModel.GetAttackModel(1).weapons[1].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 3, 5, false, false) { name = "MoabModifier_" });
        towerModel.GetAttackModel(1).weapons[1].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;


        var icicleOrbit = Game.instance.model.GetTower(TowerType.BoomerangMonkey, 5).GetBehavior<OrbitModel>().Duplicate();
        icicleOrbit.projectile.display = Game.instance.model.GetTower(TowerType.IceMonkey, 0, 0, 5).GetAttackModel().weapons[0].projectile.display;
        icicleOrbit.range = 20;

        var icicleOrbit2 = icicleOrbit.Duplicate();
        icicleOrbit2.range = 45;
        icicleOrbit2.count = 5;

        towerModel.AddBehavior(icicleOrbit);
        towerModel.AddBehavior(icicleOrbit2);


        var icicleDamage = Game.instance.model.GetTower(TowerType.BoomerangMonkey, 5).GetAttackModel(1).Duplicate();
        icicleDamage.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        icicleDamage.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        icicleDamage.weapons[0].projectile.GetDamageModel().damage *= 4;
        icicleDamage.weapons[0].projectile.pierce *= 3;
        icicleDamage.range = 49;
        icicleDamage.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 2, 0, false, false) { name = "MoabModifier_" });
        icicleDamage.weapons[0].projectile.AddBehavior(new FreezeModel("FreezeModel_", 0, 1f, "ShardFreeze", 1, "Ice", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, false));
        icicleDamage.weapons[0].projectile.collisionPasses = new int[] { 0, -1 };

        towerModel.AddBehavior(icicleDamage);
    }
}