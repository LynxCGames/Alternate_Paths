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
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using PathsPlusPlus;
using AlternatePaths;

namespace TackShooter;

public class AdvancedSensors : UpgradePlusPlus<TackShooterAltPath>
{
    public override int Cost => 120;
    public override int Tier => 1;
    public override string Icon => "Tier1 Tack Icon";
    public override string Portrait => "Tier1 Tack";

    public override string DisplayName => "Advanced Sensors";
    public override string Description => "Tack Shooter can detect and pop Camo Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }
    }
}

public class Fletchette : UpgradePlusPlus<TackShooterAltPath>
{
    public override int Cost => 320;
    public override int Tier => 2;
    public override string Icon => "Tier2 Tack Icon";
    public override string Portrait => "Tier2 Tack";

    public override string DisplayName => "Fletchette Tacks";
    public override string Description => "Tacks now crack into 3 more tacks upon hitting a Bloon. Ring of Fire damage is increased.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var tack = Game.instance.model.GetTower(TowerType.TackShooter, 0).GetAttackModel().weapons[0].projectile.Duplicate();

        tack.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        if (towerModel.appliedUpgrades.Contains(UpgradeType.RingOfFire))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 2;
        }
        else
        {
            foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                weaponModel.projectile.AddBehavior(new CreateProjectileOnContactModel
                    ("CreateProjectileOnContactModel_", tack, new ArcEmissionModel("ArcEmissionModel_", 3, 0, 25, null, true, false), true, false, false));
            }
        }
    }
}

public class Firecrackers : UpgradePlusPlus<TackShooterAltPath>
{
    public override int Cost => 750;
    public override int Tier => 3;
    public override string Icon => "Tier3 Tack Icon";
    public override string Portrait => "Tier3 Tack";

    public override string DisplayName => "Firecracker Shooter";
    public override string Description => "Tack Shooter fires explosive firecrackers that pop into fragments.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var firecracker = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.Duplicate();

        towerModel.GetAttackModel().weapons[0].projectile = firecracker;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;

        var fragment = Game.instance.model.GetTower(TowerType.TackShooter).GetAttackModel().weapons[0].projectile.Duplicate();
        fragment.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        firecracker.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        firecracker.AddBehavior(new CreateProjectileOnContactModel("Fragment", fragment, new ArcEmissionModel("FragmentEmmision_", 6, 0, 360, null, true, false), true, false, false));

        firecracker.AddBehavior(new CreateProjectileOnExpireModel("Fragment", fragment, new ArcEmissionModel("FragmentEmmision_", 6, 0, 360, null, true, false), false));

        var boom = firecracker.GetBehavior<CreateProjectileOnContactModel>().projectile;
        boom.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        boom.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        boom.GetDamageModel().damage = 2;
    }
}

public class Fireworks : UpgradePlusPlus<TackShooterAltPath>
{
    public override int Cost => 4600;
    public override int Tier => 4;
    public override string Icon => "Tier4 Tack Icon";
    public override string Portrait => "Tier4 Tack";

    public override string DisplayName => "Fireworks";
    public override string Description => "Firecrackers are replaced with fireworks that branch into even more explosions.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var firework = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.Duplicate();

        towerModel.GetAttackModel().weapons[0].projectile = firework;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;

        var cluster = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.Duplicate();
        cluster.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        firework.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        cluster.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;

        var clusterBoom = cluster.GetBehavior<CreateProjectileOnContactModel>().projectile;
        clusterBoom.GetDamageModel().damage = 2;
        clusterBoom.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        clusterBoom.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        firework.AddBehavior(new CreateProjectileOnContactModel("Fragment", cluster, new ArcEmissionModel("FragmentEmmision_", 8, 0, 360, null, true, false), true, false, false));

        firework.AddBehavior(new CreateProjectileOnExpireModel("Fragment", cluster, new ArcEmissionModel("FragmentEmmision_", 8, 0, 360, null, true, false), false));

        var boom = firework.GetBehavior<CreateProjectileOnContactModel>().projectile;
        boom.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        boom.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        boom.GetDamageModel().damage = 3;

        cluster.AddBehavior(new CreateProjectileOnExpireModel("Blast", boom, new ArcEmissionModel("BlastEmmision_", 1, 0, 0, null, true, false), false));
    }
}

public class ExplosionKing : UpgradePlusPlus<TackShooterAltPath>
{
    public override int Cost => 48000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Tack Icon";
    public override string Portrait => "Tier5 Tack";

    public override string DisplayName => "Explosion King";
    public override string Description => "Explosions that never give up.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
var superBomb = Game.instance.model.GetTower(TowerType.BombShooter, 3).GetAttackModel().weapons[0].projectile.Duplicate();

        towerModel.GetAttackModel().weapons[0].projectile = superBomb;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;
        superBomb.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        var superBoom = superBomb.GetBehavior<CreateProjectileOnContactModel>().projectile;
        superBoom.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        superBoom.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        superBoom.GetDamageModel().damage = 6;

        var superCluster = Game.instance.model.GetTower(TowerType.BombShooter, 1).GetAttackModel().weapons[0].projectile.Duplicate();
        superCluster.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        superCluster.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;

        var superClusterBoom = superCluster.GetBehavior<CreateProjectileOnContactModel>().projectile;
        superClusterBoom.GetDamageModel().damage = 4;
        superClusterBoom.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        superClusterBoom.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        superBomb.AddBehavior(new CreateProjectileOnContactModel("Fragment", superCluster, new ArcEmissionModel("FragmentEmmision_", 8, 0, 360, null, true, false), true, false, false));
        superBomb.AddBehavior(new CreateProjectileOnExpireModel("Fragment", superCluster, new ArcEmissionModel("FragmentEmmision_", 8, 0, 360, null, true, false), false));
    
        var smallCluster = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.Duplicate();
        smallCluster.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        smallCluster.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;

        var smallClusterBoom = smallCluster.GetBehavior<CreateProjectileOnContactModel>().projectile;
        smallClusterBoom.GetDamageModel().damage = 2;
        smallClusterBoom.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        smallClusterBoom.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        superCluster.AddBehavior(new CreateProjectileOnContactModel("Fragment", smallCluster, new ArcEmissionModel("FragmentEmmision_", 8, 0, 360, null, true, false), true, false, false));
        superCluster.AddBehavior(new CreateProjectileOnExpireModel("Fragment", smallCluster, new ArcEmissionModel("FragmentEmmision_", 8, 0, 360, null, true, false), false));
        superCluster.AddBehavior(new CreateProjectileOnExpireModel("Blast", superClusterBoom, new ArcEmissionModel("BlastEmmision_", 1, 0, 0, null, true, false), false));

        smallCluster.AddBehavior(new CreateProjectileOnExpireModel("Blast", smallClusterBoom, new ArcEmissionModel("BlastEmmision_", 1, 0, 0, null, true, false), false));
    }
}