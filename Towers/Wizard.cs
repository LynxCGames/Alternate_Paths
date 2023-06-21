using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppSystem.Linq;
using System.Linq;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using PathsPlusPlus;
using AlternatePaths;

namespace Wizard;

public class AdeptWizardry : UpgradePlusPlus<WizardAltPath>
{
    public override int Cost => 220;
    public override int Tier => 1;
    public override string Icon => "Tier1 Wizard Icon";
    public override string Portrait => "Tier1 Wizard";

    public override string DisplayName => "Adept Wizardry";
    public override string Description => "Wizard Monkey shoots magic bolts faster.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.rate /= 1.3f;
        }
    }
}

public class SplitMagic : UpgradePlusPlus<WizardAltPath>
{
    public override int Cost => 640;
    public override int Tier => 2;
    public override string Icon => "Tier2 Wizard Icon";
    public override string Portrait => "Tier2 Wizard";

    public override string DisplayName => "Split Magic";
    public override string Description => "Wizard Monkey learned to split magic in two.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 2, 0, 15, null, false, false);
    }
}

public class PopElemental : UpgradePlusPlus<WizardAltPath>
{
    public override int Cost => 950;
    public override int Tier => 3;
    public override string Icon => "Tier3 Wizard Icon";
    public override string Portrait => "Tier3 Wizard";

    public override string DisplayName => "Pop Elemental";
    public override string Description => "Magic bolts are stronger and can pop all Bloon types.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            weaponModel.projectile.GetDamageModel().damage += 2;
        }
    }
}

public class MagicBurst : UpgradePlusPlus<WizardAltPath>
{
    public override int Cost => 3600;
    public override int Tier => 4;
    public override string Icon => "Tier4 Wizard Icon";
    public override string Portrait => "Tier4 Wizard";

    public override string DisplayName => "Magic Burst";
    public override string Description => "Wizard Monkey attacks even faster and magic is split into 4.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 4, 0, 35, null, false, false);

        towerModel.GetAttackModel().weapons[0].rate /= 2;
    }
}

public class SorcererSupreme : UpgradePlusPlus<WizardAltPath>
{
    public override int Cost => 35000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Wizard Icon";
    public override string Portrait => "Tier5 Wizard";

    public override string DisplayName => "Sorcerer Supreme";
    public override string Description => "Master of pure pop magic.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 8, 0, 65, null, false, false);

        var druid = Game.instance.model.GetTower(TowerType.Druid, 2);
        var lightning = druid.GetAttackModel().weapons.First(w => w.name == "WeaponModel_Lightning").Duplicate();
        lightning.animation = 1;
        lightning.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

        towerModel.GetAttackModel().AddWeapon(lightning);

        towerModel.GetAttackModel().weapons[0].rate /= 1.8f;

        foreach (var weaponModel in towerModel.GetDescendants<WeaponModel>().ToArray())
        {
            weaponModel.projectile.GetDamageModel().damage += 3;
            weaponModel.projectile.pierce += 4;
        }
    }
}