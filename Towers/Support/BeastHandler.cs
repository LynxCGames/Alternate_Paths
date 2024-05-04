using AlternatePaths;
using AlternatePaths.Displays.Projectiles;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using PathsPlusPlus;

namespace BeastHandler;

public class WildCall : UpgradePlusPlus<HandlerAltPath>
{
    public override int Cost => 400;
    public override int Tier => 1;
    public override string Icon => "Tier1 Handler Icon";
    public override string Portrait => "Tier1 Handler";

    public override string DisplayName => "Call of the Wild";
    public override string Description => "Gains the ability to fire spectral wolves at Bloons. Horned Owl allows them to hit camo Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetAttackModel().range = Game.instance.model.GetTower(TowerType.BeastHandler, 1).GetAttackModel().range;
        towerModel.range = Game.instance.model.GetTower(TowerType.BeastHandler, 1).GetAttackModel().range;

        var wolves = Game.instance.model.GetTower(TowerType.ObynGreenfoot).GetAttackModel().weapons[0].Duplicate();
        wolves.rate /= 1.5f;
        wolves.name = "Wolves";

        if (towerModel.appliedUpgrades.Contains(UpgradeType.HornedOwl))
        {
            wolves.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        }

        towerModel.GetAttackModel().AddWeapon(wolves);
    }
}

public class GuerillaFighter : UpgradePlusPlus<HandlerAltPath>
{
    public override int Cost => 750;
    public override int Tier => 2;
    public override string Icon => "Tier2 Handler Icon";
    public override string Portrait => "Tier2 Handler";

    public override string DisplayName => "Guerilla Fighter";
    public override string Description => "Doubles the maximum power of the handler’s current beast. Only works for tier 3 onward.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (towerModel.appliedUpgrades.Contains(UpgradeType.GreatWhite) || towerModel.appliedUpgrades.Contains(UpgradeType.Velociraptor) || towerModel.appliedUpgrades.Contains(UpgradeType.GoldenEagle))
        {
            towerModel.GetBehavior<BeastHandlerLeashModel>().towerModel.GetBehavior<BeastHandlerPetModel>().basePower *= 2;
            towerModel.GetBehavior<BeastHandlerLeashModel>().towerModel.GetBehavior<BeastHandlerPetModel>().maxPower *= 2;
        }
    }
}

public class PrimalSurge : UpgradePlusPlus<HandlerAltPath>
{
    public override int Cost => 1700;
    public override int Tier => 3;
    public override string Icon => "Tier3 Handler Icon";
    public override string Portrait => "Tier3 Handler";

    public override string DisplayName => "Primal Surge";
    public override string Description => "Conjures more powerful spectral wolves faster.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        foreach (var weapon in towerModel.GetAttackModel().weapons)
        {
            if (weapon.name.Contains("Wolves"))
            {
                weapon.projectile.GetDamageModel().damage += 2;
                weapon.projectile.pierce += 2;
                weapon.rate /= 2;
            }
        }
    }
}

public class HeartofBeast : UpgradePlusPlus<HandlerAltPath>
{
    public override int Cost => 9800;
    public override int Tier => 4;
    public override string Icon => "Tier4 Handler Icon";
    public override string Portrait => "Tier4 Handler";

    public override string DisplayName => "Heart of the Beast";
    public override string Description => "Spectral wolves gain bonus effects based on the tamed beast that get stronger with the beast's tier\n" +
        " - Fish – Wolves deal bonus damage to MOAB and Ceramic Bloons.\n" +
        " - Dino – Wolves gain increased damage and now stun all Bloons hit.\n" +
        " - Bird – Wolves create a spread of spectral orbs when they hit a Bloon.";


    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var orbit = Game.instance.model.GetTower(TowerType.BoomerangMonkey, 5).GetBehavior<OrbitModel>().Duplicate();
        orbit.range /= 1.5f;
        orbit.count = 4;

        if (towerModel.appliedUpgrades.Contains(UpgradeType.Piranha))
        {
            var power = towerModel.GetBehavior<BeastHandlerLeashModel>().towerModel.GetBehavior<BeastHandlerPetModel>().basePower;
            orbit.projectile.ApplyDisplay<BeastOrbFish>();

            towerModel.GetAttackModel().weapons[0].projectile.hasDamageModifiers = true;

            if (power <= 2)
            {
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 6, false, false) { name = "MoabModifier_" });
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 2, false, false) { name = "CeramicModifier_" });
            }
            else if (power > 2)
            {
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 14, false, false) { name = "MoabModifier_" });
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 4, false, false) { name = "CeramicModifier_" });
            }
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.Microraptor))
        {
            var power = towerModel.GetBehavior<BeastHandlerLeashModel>().towerModel.GetBehavior<BeastHandlerPetModel>().basePower;
            orbit.projectile.ApplyDisplay<BeastOrbDino>();

            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new int[] { 0, -1 };

            if (power <= 2)
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 1;
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new FreezeModel("FreezeModel_", 0, 0.75f, "Stun", 999, "Stun", true, new GrowBlockModel("GrowBlockModel_"), null, 0.75f, true, false));
            }
            else if (power > 2)
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 2;
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new FreezeModel("FreezeModel_", 0, 1.5f, "Stun", 999, "Stun", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, false));
            }
        }
        else if (towerModel.appliedUpgrades.Contains(UpgradeType.Gyrfalcon))
        {
            var power = towerModel.GetBehavior<BeastHandlerLeashModel>().towerModel.GetBehavior<BeastHandlerPetModel>().basePower;
            orbit.projectile.ApplyDisplay<BeastOrbBird>();

            var shard = Game.instance.model.GetTower(TowerType.TackShooter).GetAttackModel().weapons[0].projectile.Duplicate();
            shard.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            shard.ApplyDisplay<T5SouldOrb>();
            shard.pierce = 2;

            if (power <= 2)
            {
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("", shard, new ArcEmissionModel("aaa", 6, 0, 360, null, true, false), true, false, false));
            }
            else if (power > 2)
            {
                shard.pierce += 1;
                shard.GetDamageModel().damage += 1;
                shard.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("", shard, new ArcEmissionModel("aaa", 8, 0, 360, null, true, false), true, false, false));
            }
        }
        else
        {
            orbit.projectile.display = Game.instance.model.GetTower(TowerType.DartlingGunner, 5).GetAttackModel().weapons[0].projectile.display;
        }

        towerModel.AddBehavior(orbit);
    }
}

public class PrimalLegend : UpgradePlusPlus<HandlerAltPath>
{
    public override int Cost => 76000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Handler Icon";
    public override string Portrait => "Tier5 Handler";

    public override string DisplayName => "Primal Legend";
    public override string Description => "Ascend and master the true power of the beasts gaining all of their effects.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var orbit1 = towerModel.GetBehavior<OrbitModel>();
        var orbit2 = towerModel.GetBehavior<OrbitModel>().Duplicate();
        var orbit3 = towerModel.GetBehavior<OrbitModel>().Duplicate();

        orbit1.projectile.ApplyDisplay<BeastOrbFish>();
        orbit2.projectile.ApplyDisplay<BeastOrbDino>();
        orbit3.projectile.ApplyDisplay<BeastOrbBird>();

        orbit2.count = 8;
        orbit3.count = 12;

        orbit2.range *= 1.5f;
        orbit3.range *= 2f;

        towerModel.AddBehavior(orbit2);
        towerModel.AddBehavior(orbit3);


        foreach (var weapon in towerModel.GetAttackModel().weapons)
        {
            towerModel.GetAttackModel().RemoveWeapon(weapon);
        }


        var wolves = Game.instance.model.GetTower(TowerType.ObynGreenfoot).GetAttackModel().weapons[0].Duplicate();
        wolves.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        wolves.rate /= 5f;
        wolves.name = "Wolves";
        wolves.projectile.GetDamageModel().damage += 7;
        wolves.projectile.pierce += 5;


        wolves.projectile.hasDamageModifiers = true;
        wolves.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moabs", 1, 56, false, false) { name = "MoabModifier_" });
        wolves.projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 16, false, false) { name = "CeramicModifier_" });


        wolves.projectile.collisionPasses = new int[] { 0, -1 };
        wolves.projectile.AddBehavior(new FreezeModel("FreezeModel_", 0, 2.5f, "Stun", 999, "Stun", true, new GrowBlockModel("GrowBlockModel_"), null, 0, false, true));


        var shard = Game.instance.model.GetTower(TowerType.TackShooter).GetAttackModel().weapons[0].projectile.Duplicate();
        shard.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        shard.ApplyDisplay<T5SouldOrb>();
        shard.GetDamageModel().damage = 5;
        shard.pierce = 6;

        wolves.projectile.AddBehavior(new CreateProjectileOnContactModel("", shard, new ArcEmissionModel("aaa", 16, 0, 360, null, true, false), true, false, false));


        wolves.projectile.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        towerModel.GetAttackModel().AddWeapon(wolves);
    }
}