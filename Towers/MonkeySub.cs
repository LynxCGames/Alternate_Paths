using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2Cpp;
using PathsPlusPlus;
using AlternatePaths;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

namespace MonketSub;

public class BetterDarts : UpgradePlusPlus<SubAltPath>
{
    public override int Cost => 90;
    public override int Tier => 1;
    public override string Icon => "Tier1 Sub Icon";
    public override string Portrait => "Tier1 Sub";

    public override string DisplayName => "High IQ Darts";
    public override string Description => "Darts gain better homing to pop Bloons with more efficiency.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TrackTargetModel>().TurnRate *= 3;
    }
}

public class Torpedos : UpgradePlusPlus<SubAltPath>
{
    public override int Cost => 720;
    public override int Tier => 2;
    public override string Icon => "Tier2 Sub Icon";
    public override string Portrait => "Tier2 Sub";

    public override string Description => "Fires torpedos that can pop many Bloons in a small area.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var torpedo = Game.instance.model.GetTower(TowerType.BombShooter, 2).GetWeapon().Duplicate();
        torpedo.projectile.display = Game.instance.model.GetTower(TowerType.BombShooter, 0, 2).GetWeapon().projectile.display;
        torpedo.projectile.GetBehavior<TravelStraitModel>().Lifespan *= 2.5f;
        torpedo.rate = towerModel.GetAttackModel().weapons[0].rate * 2.3f;
        torpedo.projectile.scale /= 1.65f;
        torpedo.name = "Torpedo";

        towerModel.GetAttackModel().AddWeapon(torpedo);
    }
}

public class AssaultSub : UpgradePlusPlus<SubAltPath>
{
    public override int Cost => 1600;
    public override int Tier => 3;
    public override string Icon => "Tier3 Sub Icon";
    public override string Portrait => "Tier3 Sub";

    public override string DisplayName => "Assault Sub";
    public override string Description => "Now fires four darts at a time at a slightly faster speed.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].name = "Dart1";
        towerModel.GetAttackModel().weapons[0].rate /= 1.2f;

        var dart2 = towerModel.GetAttackModel().weapons[0].Duplicate();
        var dart3 = towerModel.GetAttackModel().weapons[0].Duplicate();
        var dart4 = towerModel.GetAttackModel().weapons[0].Duplicate();

        dart2.name = "Dart2";
        dart3.name = "Dart3";
        dart4.name = "Dart4";

        towerModel.GetAttackModel().AddWeapon(dart2);
        towerModel.GetAttackModel().AddWeapon(dart3);
        towerModel.GetAttackModel().AddWeapon(dart4);

        foreach (var weapon in towerModel.GetAttackModel().weapons)
        {
            if (weapon.name == "Dart1")
            {
                weapon.ejectX = 9;
            }
            if (weapon.name == "Dart2")
            {
                weapon.ejectX = 3;
            }
            if (weapon.name == "Dart3")
            {
                weapon.ejectX = -3;
            }
            if (weapon.name == "Dart4")
            {
                weapon.ejectX = -9;
            }
        }
    }
}

public class Battleship : UpgradePlusPlus<SubAltPath>
{
    public override int Cost => 11820;
    public override int Tier => 4;
    public override string Icon => "Tier4 Sub Icon";
    public override string Portrait => "Tier4 Sub";

    public override string Description => "Darts deal more damage and torpedos are fired in a spread.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var dart5 = towerModel.GetAttackModel().weapons[0].Duplicate();
        dart5.name = "Dart5";

        towerModel.GetAttackModel().AddWeapon(dart5);

        foreach (var weapon in towerModel.GetAttackModel().weapons)
        {
            if (weapon.name == "Dart1")
            {
                weapon.ejectX = 10;
                weapon.projectile.GetDamageModel().damage = 3;
            }
            if (weapon.name == "Dart2")
            {
                weapon.ejectX = 5;
                weapon.projectile.GetDamageModel().damage = 3;
            }
            if (weapon.name == "Dart3")
            {
                weapon.ejectX = 0;
                weapon.projectile.GetDamageModel().damage = 3;
            }
            if (weapon.name == "Dart4")
            {
                weapon.ejectX = -5;
                weapon.projectile.GetDamageModel().damage = 3;
            }
            if (weapon.name == "Dart5")
            {
                weapon.ejectX = -10;
                weapon.projectile.GetDamageModel().damage = 3;
            }
            if (weapon.name == "Torpedo")
            {
                weapon.emission = new RandomArcEmissionModel("Torpedo Spread", 6, 0, 0, 75, 0, null);
                weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 4;
                weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                weapon.rate /= 1.6f;
            }
        }
    }
}

public class NauticDestroyer : UpgradePlusPlus<SubAltPath>
{
    public override int Cost => 40600;
    public override int Tier => 5;
    public override string Icon => "Tier5 Sub Icon";
    public override string Portrait => "Tier5 Sub";

    public override string DisplayName => "Nautic Destroyer";
    public override string Description => "The pinnacle of monkey aquatic engineering.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weapon in towerModel.GetAttackModel().weapons)
        {
            if (weapon.name.Contains("Dart"))
            {
                weapon.projectile.GetDamageModel().damage = 8;
            }
            if (weapon.name == "Torpedo")
            {
                weapon.emission = new RandomArcEmissionModel("Torpedo Spread", 16, 0, 0, 135, 0, null);
                weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage *= 3;
                weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.hasDamageModifiers = true;
                weapon.projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 24, false, false) { name = "MoabModifier_" });
                weapon.rate /= 1.2f;
            }
        }
    }
}