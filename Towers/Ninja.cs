using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Linq;
using AlternatePaths.Displays.Projectiles;
using System.Linq;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using PathsPlusPlus;
using AlternatePaths;

namespace Ninja;

public class SiaToss : UpgradePlusPlus<NinjaAltPath>
{
    public override int Cost => 420;
    public override int Tier => 1;
    public override string Icon => "Tier1 Ninja Icon";
    public override string Portrait => "Tier1 Ninja";

    public override string DisplayName => "Sai Toss";
    public override string Description => "Throws high damage Sais that can pop multiple Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var sai = Game.instance.model.GetTower(TowerType.DartMonkey, 0).GetAttackModel().weapons[0].Duplicate();
        sai.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        sai.name = "Sai_Toss";

        sai.projectile.GetDamageModel().damage = 3;
        sai.projectile.pierce = 4;
        sai.rate = towerModel.GetAttackModel().weapons[0].rate * 3f;
        sai.projectile.ApplyDisplay<Sai>();

        towerModel.GetAttackModel().AddWeapon(sai);
    }
}

public class DeadlyShurikens : UpgradePlusPlus<NinjaAltPath>
{
    public override int Cost => 480;
    public override int Tier => 2;
    public override string Icon => "Tier2 Ninja Icon";
    public override string Portrait => "Tier2 Ninja";

    public override string DisplayName => "Deadly Shurikens";
    public override string Description => "Shurikens deal more damage and can pop all Bloon types.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var damageModel = towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel();

        damageModel.damage += 1;
        damageModel.immuneBloonProperties = BloonProperties.None;
    }
}

public class Katanas : UpgradePlusPlus<NinjaAltPath>
{
    public override int Cost => 2500;
    public override int Tier => 3;
    public override string Icon => "Tier3 Ninja Icon";
    public override string Portrait => "Tier3 Ninja";

    public override string DisplayName => "Katanas";
    public override string Description => "Sais are replaced with katanas that are even more powerful. ";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            if (weaponModel.name == "Sai_Toss")
            {
                weaponModel.projectile.pierce += 4;
                weaponModel.projectile.GetDamageModel().damage += 2;
                weaponModel.projectile.ApplyDisplay<Katana>();
            }
        }
    }
}

public class RazorBlades : UpgradePlusPlus<NinjaAltPath>
{
    public override int Cost => 4750;
    public override int Tier => 4;
    public override string Icon => "Tier4 Ninja Icon";
    public override string Portrait => "Tier4 Ninja";

    public override string DisplayName => "Razor Blade";
    public override string Description => "Katanas so sharp that they tear right through Moab-Class Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].rate /= 1.4f;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.hasDamageModifiers = true;

            if (weaponModel.name == "Sai_Toss")
            {
                weaponModel.rate = towerModel.GetAttackModel().weapons[0].rate * 2.4f;
                weaponModel.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 25, false, false) { name = "MoabModifier_" });
            }
        }
    }
}

public class DemonBlade : UpgradePlusPlus<NinjaAltPath>
{
    public override int Cost => 56000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Ninja Icon";
    public override string Portrait => "Tier5 Ninja";

    public override string DisplayName => "Demon Blade";
    public override string Description => "Deep from within the underworld, I have arisen.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.hasDamageModifiers = true;

            if (weaponModel.name == "Sai_Toss")
            {
                weaponModel.projectile.scale *= 2;
                weaponModel.projectile.pierce += 2;
                weaponModel.projectile.GetDamageModel().damage += 5;
                weaponModel.rate = towerModel.GetAttackModel().weapons[0].rate * 1.3f;
                weaponModel.projectile.GetBehavior<DamageModifierForTagModel>().damageAddative *= 5;
                weaponModel.projectile.ApplyDisplay<DemonBladeProj>();
                weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            }
        }

        var blades = Game.instance.model.GetTower(TowerType.DartMonkey, 0).GetAttackModel().weapons[0].Duplicate();
        blades.name = "Demon_Blades";
        blades.rate = towerModel.GetAttackModel().weapons[0].rate * 1.3f;

        blades.projectile.hasDamageModifiers = true;
        blades.projectile.GetDamageModel().damage = 8;
        blades.projectile.pierce = 6;
        blades.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 32, false, false) { name = "MoabModifier_" });
        blades.projectile.ApplyDisplay<Katana>();
        blades.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        blades.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        blades.emission = new ArcEmissionModel("aaa", 2, 0, 60, null, false, false);

        towerModel.GetAttackModel().AddWeapon(blades);
    }
}