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
using PathsPlusPlus;
using AlternatePaths;

namespace DartMonkey;

public class BloontoniumDarts : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 180;
    public override int Tier => 1;
    public override string Icon => "Tier1 Dart Icon";
    public override string Portrait => "Tier1 Dart";

    public override string DisplayName => "Bloontonium Darts";
    public override string Description => "Darts gain the ability to pop Lead Bloons. Spike-o-Pult gains increased damage.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.immuneBloonProperties = BloonProperties.None;
        }

        if (towerModel.appliedUpgrades.Contains(UpgradeType.SpikeOPult))
        {
            towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
        }
    }
}

public class RubberSeeking : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 330;
    public override int Tier => 2;
    public override string Icon => "Tier2 Dart Icon";
    public override string Portrait => "Tier2 Dart";

    public override string DisplayName => "Rubber Seeking Darts";
    public override string Description => "Darts can now seek out Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var seeking = Game.instance.model.GetTowerFromId("WizardMonkey-500").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate();

        seeking.distance = 999;
        seeking.constantlyAquireNewTarget = true;


        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.AddBehavior(seeking);
            weaponModel.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 4;
            weaponModel.projectile.pierce += 1;
        }
    }
}

public class DartGunner : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 560;
    public override int Tier => 3;
    public override string Icon => "Tier3 Dart Icon";
    public override string Portrait => "Tier3 Dart";

    public override string DisplayName => "Dart Gunner";
    public override string Description => "Dart gun shoots faster and pops 2 layers.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.range += 5;
        towerModel.GetAttackModel().range += 5;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.rate /= 1.4f;
        }

        foreach (var damageModel in towerModel.GetDescendants<DamageModel>().ToArray())
        {
            damageModel.damage += 1;
        }


        //if (IsHighestUpgrade(towerModel))
        //{
        //towerModel.display = towerModel.GetBehavior<DisplayModel>().display =
        //Game.instance.model.GetTower(TowerType.SniperMonkey, 0, 0, 3).display;
        //}
    }
}

public class Rifleman : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 2400;
    public override int Tier => 4;
    public override string Icon => "Tier4 Dart Icon";
    public override string Portrait => "Tier4 Dart";

    public override string DisplayName => "Rifleman";
    public override string Description => "Dart gun now fires incredibly fast and deals more damage to Ceramic Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var damage = new DamageModifierForTagModel("aaa", "Ceramic", 1, 3, false, false) { name = "CeramicModifier_" };

        towerModel.GetAttackModel().weapons[0].projectile.hasDamageModifiers = true;

        towerModel.range += 8;
        towerModel.GetAttackModel().range += 8;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.rate /= 4;

            weaponModel.projectile.AddBehavior(damage);
            weaponModel.projectile.collisionPasses = new int[] { -1, 0, 1 };
            weaponModel.projectile.pierce += 2;
        }
    }
}

public class DartTank : UpgradePlusPlus<DartMonkeyAltPath>
{
    public override int Cost => 28600;
    public override int Tier => 5;
    public override string Icon => "Tier5 Dart Icon";
    public override string Portrait => "Tier5 Dart";

    public override string DisplayName => "Plasma tank";
    public override string Description => "Machine gun attack fires deadly plasma and Plasma Tank periodically fires a Bloontonium rocket that obliterates MOAB-class Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.range += 12;
        towerModel.GetAttackModel().range += 12;

        towerModel.GetAttackModel().weapons[0].projectile.pierce *= 2;
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 3;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative += 7;
        towerModel.GetAttackModel().weapons[0].projectile.display = Game.instance.model.GetTower(TowerType.SuperMonkey, 2).GetAttackModel().weapons[0].projectile.display;


        var missileWeapon = Game.instance.model.GetTower(TowerType.BombShooter, 1, 2).GetAttackModel().weapons[0].Duplicate();
        missileWeapon.rate = Game.instance.model.GetTower(TowerType.DartMonkey).GetAttackModel().weapons[0].rate;
        missileWeapon.projectile.display = Game.instance.model.GetTower(TowerType.DartlingGunner, 0, 3).GetAttackModel().weapons[0].projectile.display;
        missileWeapon.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);


        var missile = missileWeapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
        missile.GetDamageModel().damage = 8;
        missile.hasDamageModifiers = true;
        missile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 56, false, false) { name = "MoabModifier_" });
        missile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        missile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        towerModel.GetAttackModel().AddWeapon(missileWeapon);


        //var ability = Game.instance.model.GetTower(TowerType.BombShooter, 0, 5).GetAbility();
        //ability.description = "Fires a powerful warhead at the strongest MOAB-class Bloon on screen.";
        //ability.Cooldown += 8;

        //towerModel.AddBehavior(ability);
    }
}