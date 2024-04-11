using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
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
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
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
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", tack, new ArcEmissionModel("ArcEmissionModel_", 3, 0, 25, null, true, false), true, false, false));
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
        var effect = Game.instance.model.GetTower(TowerType.MortarMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
        var sound = Game.instance.model.GetTower(TowerType.MortarMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
        var explosion = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
        var bombEffect = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
        var fragment = Game.instance.model.GetTower(TowerType.TackShooter).GetAttackModel().weapons[0].projectile.Duplicate();


        effect.effectModel = bombEffect;

        fragment.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        explosion.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        explosion.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        explosion.GetDamageModel().damage = 2;

        towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.display;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 0.2f;
        towerModel.GetAttackModel().weapons[0].projectile.pierce = 999;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage -= 1;
        towerModel.GetAttackModel().weapons[0].projectile.RemoveBehavior<CreateProjectileOnContactModel>();
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExpireModel("Fragment", fragment, new ArcEmissionModel("FragmentEmmision_", 6, 0, 360, null, true, false), false));
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExpireModel("Explosion", explosion, new ArcEmissionModel("FragmentEmmision_", 1, 0, 0, null, true, false), false));
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(effect);
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(sound);
    }
}

public class Fireworks : UpgradePlusPlus<TackShooterAltPath>
{
    public override int Cost => 5600;
    public override int Tier => 4;
    public override string Icon => "Tier4 Tack Icon";
    public override string Portrait => "Tier4 Tack";

    public override string DisplayName => "Fireworks";
    public override string Description => "Firecrackers are replaced with fireworks that branch into even more explosions.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExpireModel>())
        {
            if (behavior.name.Contains("Fragment"))
            {
                towerModel.GetAttackModel().weapons[0].projectile.RemoveBehavior(behavior);
            }
        }


        var firework = towerModel.GetAttackModel().weapons[0].projectile.Duplicate();    
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExpireModel("Firework", firework, new ArcEmissionModel("FragmentEmmision_", 8, 0, 360, null, true, false), false));
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
        towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.BombShooter, 3).GetAttackModel().weapons[0].projectile.display;

        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExpireModel>())
        {
            if (behavior.name.Contains("Explosion"))
            {
                behavior.projectile.GetDamageModel().damage += 4;
            }

            if (behavior.name.Contains("Firework"))
            {
                var firework = behavior.projectile.Duplicate();
                behavior.projectile.AddBehavior(new CreateProjectileOnExpireModel("Firework", firework, new ArcEmissionModel("FragmentEmmision_", 8, 0, 360, null, true, false), false));


                foreach (var fireworkBehavior in behavior.projectile.GetBehaviors<CreateProjectileOnExpireModel>())
                {
                    if (fireworkBehavior.name.Contains("Explosion"))
                    {
                        behavior.projectile.GetDamageModel().damage += 2;
                    }
                }
            }
        }
    }
}