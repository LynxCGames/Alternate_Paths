using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Linq;
using System.Linq;
using Il2Cpp;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using PathsPlusPlus;
using AlternatePaths;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;

namespace Engineer;

public class TemperedNails : UpgradePlusPlus<EngineerAltPath>
{
    public override int Cost => 460;
    public override int Tier => 1;
    public override string Icon => "Tier1 Engineer Icon";
    public override string Portrait => "Tier1 Engineer";

    public override string DisplayName => "Tempered Nails";
    public override string Description => "Heated nails allow the Engineer and sentries to pop Lead Bloons and deal increased damage to Ceramic Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.hasDamageModifiers = true;
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SentryGun))
        {
            foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
            {
                if (weaponModel.projectile.GetBehavior<CreateTowerModel>() != null)
                {
                    weaponModel.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.hasDamageModifiers = true;

                    weaponModel.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    weaponModel.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.AddBehavior
                        (new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" });
                }
            }
        }
    }
}

public class BurstFire : UpgradePlusPlus<EngineerAltPath>
{
    public override int Cost => 540;
    public override int Tier => 2;
    public override string Icon => "Tier2 Engineer Icon";
    public override string Portrait => "Tier2 Engineer";

    public override string DisplayName => "Burst Fire";
    public override string Description => "Nail gun now fires a spread of nails instead.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 25, null, false, false);
    }
}

public class Prototypes : UpgradePlusPlus<EngineerAltPath>
{
    public override int Cost => 1100;
    public override int Tier => 3;
    public override string Icon => "Tier3 Engineer Icon";
    public override string Portrait => "Tier3 Engineer";

    public override string DisplayName => "Prototype Sentries";
    public override string Description => "Creates tesla turrets that fire electricity at Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var behavior in Game.instance.model.GetTowerFromId("EngineerMonkey-100").GetAttackModels().ToArray())
        {
            if (behavior.name.Contains("Spawner"))
            {
                var spawner = behavior.Duplicate();
                spawner.range = towerModel.range;
                spawner.name = "Electric_Place";
                spawner.weapons[0].projectile.RemoveBehavior<CreateTowerModel>();
                spawner.weapons[0].projectile.AddBehavior(new CreateTowerModel("SentryPlace", GetTowerModel<ElectricSentryV1>().Duplicate(), 0f, true, false, false, true, true));

                if (towerModel.appliedUpgrades.Contains(UpgradeType.FasterEngineering))
                {
                    spawner.weapons[0].Rate = 5f;
                }
                else
                {
                    spawner.weapons[0].Rate = 7f;
                }

                towerModel.AddBehavior(spawner);
            }
        }
    }
}

public class AerialAdvancements : UpgradePlusPlus<EngineerAltPath>
{
    public override int Cost => 4200;
    public override int Tier => 4;
    public override string Icon => "Tier4 Engineer Icon";
    public override string Portrait => "Tier4 Engineer";

    public override string DisplayName => "Aerial Support";
    public override string Description => "Tesla turrets are much stronger and Engineer calls upon flying laser drones to help attack Bloons. Faster Engineering increases the drone's attack speed.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var attack in towerModel.GetAttackModels())
        {
            if (attack.name.Contains("Place"))
            {
                attack.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower = GetTowerModel<ElectricSentryV2>().Duplicate();
            }
        }

        var drones = Game.instance.model.GetTower(TowerType.Etienne).GetBehavior<DroneSupportModel>().Duplicate();
        drones.count = 2;
        drones.droneModel.range = towerModel.range;

        var droneAttackModel = drones.droneModel.GetAttackModel();
        droneAttackModel.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartlingGunner-300").GetAttackModel().weapons[0].projectile.display;
        droneAttackModel.range = towerModel.range;
        droneAttackModel.weapons[0].projectile.pierce = 6;
        droneAttackModel.weapons[0].projectile.GetDamageModel().damage = 3;
        droneAttackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.FasterEngineering))
        {
            droneAttackModel.weapons[0].rate /= 1.75f;
        }
        else
        {
            droneAttackModel.weapons[0].rate /= 1.35f;
        }

        towerModel.AddBehavior(drones);
    }
}

public class Overcharge : UpgradePlusPlus<EngineerAltPath>
{
    public override int Cost => 50000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Engineer Icon";
    public override string Portrait => "Tier5 Engineer";

    public override string DisplayName => "Overcharge";
    public override string Description => "Laser drones are much stronger and tesla turrets fire balls of pure electricity to zap Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var drones = towerModel.GetBehavior<DroneSupportModel>();
        drones.count += 2;

        var droneAttackModel = drones.droneModel.GetAttackModel();
        droneAttackModel.weapons[0].projectile.pierce *= 2;
        droneAttackModel.weapons[0].projectile.GetDamageModel().damage += 2;
        droneAttackModel.weapons[0].rate /= 1.6f;


        foreach (var attack in towerModel.GetAttackModels())
        {
            if (attack.name.Contains("Place"))
            {
                attack.weapons[0].projectile.GetBehavior<CreateTowerModel>().tower = GetTowerModel<ElectricSentryV3>().Duplicate();
            }
        }
    }
}

public class ElectricSentryV1 : ModTower
{
    public override string Portrait => "Electric Sentry";
    public override string Name => "LightningSentryv1";
    public override TowerSet TowerSet => TowerSet.Support;
    public override string BaseTower => TowerType.SentryEnergy;

    public override bool DontAddToShop => true;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 0;
    public override int BottomPathUpgrades => 0;


    public override string DisplayName => "Lightning Sentry ";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        var druid = Game.instance.model.GetTower(TowerType.Druid, 2);
        var lightning = druid.GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").Duplicate();
        lightning.animation = 1;
        lightning.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

        lightning.rate = .75f;

        var lightningProj = lightning.projectile;
        lightningProj.pierce = 9;
        lightningProj.GetBehavior<LightningModel>().splitRange = towerModel.range / 2f;
        lightningProj.GetBehavior<LightningModel>().splits = 1;

        towerModel.GetAttackModel().weapons[0] = lightning;

        towerModel.isSubTower = true;
        towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 20f, 5, false, false));
    }

    public class V1SentryDisplay : ModTowerDisplay<ElectricSentryV1>
    {
        public override float Scale => 1f;
        public override string BaseDisplay => GetDisplay(TowerType.SentryCold);

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }
}

public class ElectricSentryV2 : ModTower
{
    public override string Portrait => "Electric Sentry";
    public override string Name => "LightningSentryv2";
    public override TowerSet TowerSet => TowerSet.Support;
    public override string BaseTower => TowerType.SentryEnergy;

    public override bool DontAddToShop => true;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 0;
    public override int BottomPathUpgrades => 0;


    public override string DisplayName => "Lightning Sentry V2";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        var druid = Game.instance.model.GetTower(TowerType.Druid, 2);
        var lightning = druid.GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").Duplicate();
        lightning.animation = 1;
        lightning.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;

        lightning.rate = .65f;

        var lightningProj = lightning.projectile;
        lightningProj.GetDamageModel().damage += 1;
        lightningProj.pierce = 15;
        lightningProj.GetBehavior<LightningModel>().splitRange = towerModel.range / 1.5f;
        lightningProj.GetBehavior<LightningModel>().splits = 1;

        towerModel.GetAttackModel().weapons[0] = lightning;

        towerModel.isSubTower = true;
        towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 20f, 5, false, false));
    }

    public class V2SentryDisplay : ModTowerDisplay<ElectricSentryV2>
    {
        public override float Scale => 1f;
        public override string BaseDisplay => GetDisplay(TowerType.SentryCold);

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }
}

public class ElectricSentryV3 : ModTower
{
    public override string Portrait => "Electric Sentry";
    public override string Name => "LightningSentryv3";
    public override TowerSet TowerSet => TowerSet.Support;
    public override string BaseTower => TowerType.SentryEnergy;

    public override bool DontAddToShop => true;
    public override int Cost => 0;

    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 0;
    public override int BottomPathUpgrades => 0;


    public override string DisplayName => "Overcharge Sentry";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        var ballLightning = Game.instance.model.GetTower(TowerType.Druid, 4).GetAttackModel().weapons[1].Duplicate();

        ballLightning.rate = 2.5f;

        towerModel.GetAttackModel().weapons[0] = ballLightning;

        towerModel.isSubTower = true;
        towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 20f, 5, false, false));
    }

    public class V3SentryDisplay : ModTowerDisplay<ElectricSentryV3>
    {
        public override float Scale => 1f;
        public override string BaseDisplay => GetDisplay(TowerType.SentryCold);

        public override bool UseForTower(int[] tiers)
        {
            return true;
        }
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }
}