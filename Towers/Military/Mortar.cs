using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

namespace Mortar;

public class LaserDetonation : UpgradePlusPlus<MortarAltPath>
{
    public override int Cost => 280;
    public override int Tier => 1;
    public override string Icon => "Tier1 Mortar Icon";
    public override string Portrait => "Tier1 Mortar";

    public override string DisplayName => "Laser Detonation";
    public override string Description => "Fires special charged mortar shells that release lasers when they explode. Lasers become more powerful with higher tier upgrades.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var mortar = Game.instance.model.GetTower(TowerType.MortarMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();

        var laser = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].projectile.Duplicate();
        laser.display = Game.instance.model.GetTower(TowerType.DartlingGunner, 3).GetAttackModel().weapons[0].projectile.display;
        laser.GetDamageModel().immuneBloonProperties = Game.instance.model.GetTower(TowerType.DartlingGunner, 3).GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.TheBigOne))
        {
            laser.GetDamageModel().damage += 5;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.ArtilleryBattery) || towerModel.appliedUpgrades.Contains(UpgradeType.ShatteringShells))
        {
            laser.GetDamageModel().damage += 2;
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.TheBiggestOne))
        {
            laser.GetDamageModel().damage *= 6;
        }
        if (towerModel.appliedUpgrades.Contains(UpgradeType.PopAndAwe) || towerModel.appliedUpgrades.Contains(UpgradeType.Blooncineration))
        {
            laser.GetDamageModel().damage *= 2;
        }

        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("LaserBomb", laser, new ArcEmissionModel("", 8, 0, 360, null, true, false), mortar.fraction, mortar.durationfraction, true, false, true));
    }
}

public class CentralCommand : UpgradePlusPlus<MortarAltPath>
{
    public override int Cost => 500;
    public override int Tier => 2;
    public override string Icon => "Tier2 Mortar Icon";
    public override string Portrait => "Tier2 Mortar";

    public override string DisplayName => "Central Command";
    public override string Description => "Gains an ability to target the cursor’s position.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var follow = Game.instance.model.GetTowerFromId("StrikerJones 7").GetAbility(1).Duplicate();
        follow.GetBehavior<ActivateTempTargetPrioSupportZoneModel>().canEffectThisTower = true;
        follow.GetBehavior<ActivateTempTargetPrioSupportZoneModel>().maxNumTowersModified = 1;
        follow.GetBehavior<ActivateTempTargetPrioSupportZoneModel>().isGlobal = false;
        follow.GetBehavior<ActivateTempTargetPrioSupportZoneModel>().lifespan = 10;
        follow.GetBehavior<ActivateTempTargetPrioSupportZoneModel>().range = 1;

        follow.Cooldown = 25;

        towerModel.AddBehavior(follow);
    }
}

public class CarpetBomb : UpgradePlusPlus<MortarAltPath>
{
    public override int Cost => 900;
    public override int Tier => 3;
    public override string Icon => "Tier3 Mortar Icon";
    public override string Portrait => "Tier3 Mortar";

    public override string DisplayName => "Carpet Bombing";
    public override string Description => "Shells cause a shockwave of additional explosions in 4 directions.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var bomb = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].projectile.Duplicate();
        var mortar = Game.instance.model.GetTower(TowerType.MortarMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
        var effect = Game.instance.model.GetTower(TowerType.MortarMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
        var sound = Game.instance.model.GetTower(TowerType.MortarMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
        var explosion = Game.instance.model.GetTower(TowerType.BombShooter, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
        var bombEffect = Game.instance.model.GetTower(TowerType.BombShooter, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
        bombEffect.scale /= 2;
        effect.effectModel = bombEffect;

        explosion.radius /= 2;
        explosion.pierce = 16;
        explosion.GetDamageModel().damage = 2;

        bomb.pierce = 9999;
        bomb.GetDamageModel().damage = 0;
        bomb.GetBehavior<TravelStraitModel>().Speed /= 1.75f;
        bomb.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        bomb.display = Game.instance.model.GetTower(TowerType.DartlingGunner, 5).GetAttackModel().weapons[0].projectile.display;

        bomb.AddBehavior(new CreateProjectileOnExpireModel("ExpireExplosion", explosion, new ArcEmissionModel("", 1, 0, 0, null, true, false), false));

        bomb.AddBehavior(effect);
        bomb.AddBehavior(sound);

        var bomb2 = bomb.Duplicate();
        var bomb3 = bomb.Duplicate();

        bomb2.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
        bomb3.GetBehavior<TravelStraitModel>().Lifespan *= 2;

        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("CarpetBomb-1", bomb, new ArcEmissionModel("", 4, 0, 360, null, true, false), mortar.fraction, mortar.durationfraction, true, false, true));
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("CarpetBomb-2", bomb2, new ArcEmissionModel("", 4, 0, 360, null, true, false), mortar.fraction, mortar.durationfraction, true, false, true));
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("CarpetBomb-3", bomb3, new ArcEmissionModel("", 4, 0, 360, null, true, false), mortar.fraction, mortar.durationfraction, true, false, true));
    }
}

public class ClusterShells : UpgradePlusPlus<MortarAltPath>
{
    public override int Cost => 5800;
    public override int Tier => 4;
    public override string Icon => "Tier4 Mortar Icon";
    public override string Portrait => "Tier4 Mortar";

    public override string DisplayName => "Mega Cluster Shells";
    public override string Description => "Shells deal more damage and create stronger shockwaves in 8 directions.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 3;
        
        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>())
        {
            if (behavior.name.Contains("CarpetBomb"))
            {
                behavior.emission = new ArcEmissionModel("", 8, 0, 360, null, true, false);
                behavior.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage += 2;
            }

            if (behavior.name.Contains("LaserBomb"))
            {
                behavior.projectile.GetDamageModel().damage += 2;
            }
        }
    }
}

public class RollingThunder : UpgradePlusPlus<MortarAltPath>
{
    public override int Cost => 38000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Mortar Icon";
    public override string Portrait => "Tier5 Mortar";

    public override string DisplayName => "Rolling Thunder";
    public override string Description => "Carpet bombing creates a powerful aftershock to devastate groups of Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var bomb = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].projectile.Duplicate();
        var effect = Game.instance.model.GetTower(TowerType.MortarMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExpireModel>().Duplicate();
        var sound = Game.instance.model.GetTower(TowerType.MortarMonkey).GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileExhaustModel>().Duplicate();
        var explosion = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate();
        var bombEffect = Game.instance.model.GetTower(TowerType.BombShooter).GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate();
        effect.effectModel = bombEffect;

        explosion.radius += 4;
        explosion.pierce = 16;
        explosion.GetDamageModel().damage = 5;
        explosion.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        bomb.pierce = 9999;
        bomb.GetDamageModel().damage = 0;
        bomb.GetBehavior<TravelStraitModel>().Speed /= 1.75f;
        bomb.display = Game.instance.model.GetTower(TowerType.DartlingGunner, 5).GetAttackModel().weapons[0].projectile.display;

        bomb.AddBehavior(new CreateProjectileOnExpireModel("ThunderBombs", explosion, new ArcEmissionModel("", 1, 0, 0, null, true, false), false));

        bomb.AddBehavior(effect);
        bomb.AddBehavior(sound);

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage += 3;


        foreach (var behavior in towerModel.GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>())
        {
            if (behavior.name.Contains("CarpetBomb"))
            {
                behavior.projectile.AddBehavior(new CreateProjectileOnExpireModel("ThunderExplosion", bomb, new ArcEmissionModel("", 5, 0, 360, null, true, false), false));

                behavior.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.pierce *= 2;
                behavior.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage += 4;
                behavior.projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }

            if (behavior.name.Contains("LaserBomb"))
            {
                behavior.projectile.GetDamageModel().damage *= 3;
            }
        }
    }
}