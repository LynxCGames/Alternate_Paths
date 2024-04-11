using AlternatePaths;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using PathsPlusPlus;

namespace BananaFarm;

public class HealthyBananas : UpgradePlusPlus<FarmAltPath>
{
    public override int Cost => 270;
    public override int Tier => 1;
    public override string Icon => "Tier1 Farm Icon";
    public override string Portrait => "Tier1 Farm";

    public override string DisplayName => "Healthy Bananas";
    public override string Description => "Banana Farm produces 1 life at the end of every round.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var lives = Game.instance.model.GetTower(TowerType.BananaFarm, 0, 0, 5).GetBehavior<BonusLivesPerRoundModel>().Duplicate();
        lives.name = "RoundLives";
        lives.amount = 1;

        towerModel.AddBehavior(lives);
    }
}

public class BananaStock : UpgradePlusPlus<FarmAltPath>
{
    public override int Cost => 900;
    public override int Tier => 2;
    public override string Icon => "Tier2 Farm Icon";
    public override string Portrait => "Tier2 Farm";

    public override string DisplayName => "Banana Stock";
    public override string Description => "Produces $50 at the end of every round plus bonus cash based on the round number.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var cash = Game.instance.model.GetTower(TowerType.BananaFarm, 0, 0, 5).GetBehavior<PerRoundCashBonusTowerModel>().Duplicate();
        cash.cashPerRound = 50;
        cash.cashRoundBonusMultiplier = 5;

        towerModel.AddBehavior(cash);
    }
}

public class StockValue : UpgradePlusPlus<FarmAltPath>
{
    public override int Cost => 3250;
    public override int Tier => 3;
    public override string Icon => "Tier3 Farm Icon";
    public override string Portrait => "Tier3 Farm";

    public override string DisplayName => "Stock Value";
    public override string Description => "Increases the value of all nearby bananas by 15%. Can stack multiple times per Banana Farm.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.range += 5;

        towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound += 25;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum *= 1.5f;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum *= 1.5f;

        var stock = Game.instance.model.GetTower(TowerType.MonkeyVillage, 0, 0, 4).GetBehavior<MonkeyCityIncomeSupportModel>().Duplicate();
        stock.isUnique = false;
        stock.incomeModifier = 1.15f;

        towerModel.AddBehavior(stock);
    }
}

public class HealthierBananas : UpgradePlusPlus<FarmAltPath>
{
    public override int Cost => 11500;
    public override int Tier => 4;
    public override string Icon => "Tier4 Farm Icon";
    public override string Portrait => "Tier4 Farm";

    public override string DisplayName => "Healthier Bananas";
    public override string Description => "Banana Farm now provides 3 lives each round and stock value buff is increased to 30%.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetBehavior<BonusLivesPerRoundModel>().amount += 2;

        towerModel.GetBehavior<MonkeyCityIncomeSupportModel>().incomeModifier += 0.15f;

        towerModel.GetAttackModel().weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count += 3;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum += 20f;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum += 20f;
    }
}

public class StockExchange : UpgradePlusPlus<FarmAltPath>
{
    public override int Cost => 105000;
    public override int Tier => 5;
    public override string Icon => "Tier5 Farm Icon";
    public override string Portrait => "Tier5 Farm";

    public override string DisplayName => "Banana Stock Exchange";
    public override string Description => "Stock value buff increased to 70% extra value and is spread to all Banana Farms.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        towerModel.GetBehavior<MonkeyCityIncomeSupportModel>().incomeModifier += 0.4f;
        towerModel.GetBehavior<MonkeyCityIncomeSupportModel>().isCustomRadius = true;
        towerModel.GetBehavior<MonkeyCityIncomeSupportModel>().customRadius = 9999;

        towerModel.GetBehavior<BonusLivesPerRoundModel>().amount += 9;
        towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound += 175;

        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().maximum *= 2f;
        towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().minimum *= 2f;
    }
}