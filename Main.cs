﻿using MelonLoader;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Models.Towers;
using PathsPlusPlus;
using AlternatePaths;

[assembly: MelonInfo(typeof(AlternatePaths.AlternatePaths), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AlternatePaths;

public class AlternatePaths : BloonsTD6Mod
{
    //foreach (var behavior in Game.instance.model.GetTower(TowerType.BananaFarm, 0, 0, 5).behaviors)
    //{
    //    ModHelper.Msg<AlternatePaths>(behavior.TypeName());
    //}
}

public class DartMonkeyAltPath : PathPlusPlus
{
    public override string Tower => TowerType.DartMonkey;
    public override int UpgradeCount => 5;
}
public class BoomerangAltPath : PathPlusPlus
{
    public override string Tower => TowerType.BoomerangMonkey;
    public override int UpgradeCount => 5;
}
public class BombAltPath : PathPlusPlus
{
    public override string Tower => TowerType.BombShooter;
    public override int UpgradeCount => 5;
}
public class TackShooterAltPath : PathPlusPlus
{
    public override string Tower => TowerType.TackShooter;
    public override int UpgradeCount => 5;
}
public class IceMonkeyAltPath : PathPlusPlus
{
    public override string Tower => TowerType.IceMonkey;
    public override int UpgradeCount => 5;
}
public class GlueGunnerAltPath : PathPlusPlus
{
    public override string Tower => TowerType.GlueGunner;
    public override int UpgradeCount => 5;
}
public class SniperAltPath : PathPlusPlus
{
    public override string Tower => TowerType.SniperMonkey;
    public override int UpgradeCount => 5;
}
public class SubAltPath : PathPlusPlus
{
    public override string Tower => TowerType.MonkeySub;
    public override int UpgradeCount => 5;
}
public class PlaneAltPath : PathPlusPlus
{
    public override string Tower => TowerType.MonkeyAce;
    public override int UpgradeCount => 5;
}
public class HeliAltPath : PathPlusPlus
{
    public override string Tower => TowerType.HeliPilot;
    public override int UpgradeCount => 5;
}
public class DartlingAltPath : PathPlusPlus
{
    public override string Tower => TowerType.DartlingGunner;
    public override int UpgradeCount => 5;
}
public class WizardAltPath : PathPlusPlus
{
    public override string Tower => TowerType.WizardMonkey;
    public override int UpgradeCount => 5;
}
public class NinjaAltPath : PathPlusPlus
{
    public override string Tower => TowerType.NinjaMonkey;
    public override int UpgradeCount => 5;
}
public class FarmAltPath : PathPlusPlus
{
    public override string Tower => TowerType.BananaFarm;
    public override int UpgradeCount => 5;
}
public class SpactoryAltPath : PathPlusPlus
{
    public override string Tower => TowerType.SpikeFactory;
    public override int UpgradeCount => 5;
}
public class EngineerAltPath : PathPlusPlus
{
    public override string Tower => TowerType.EngineerMonkey;
    public override int UpgradeCount => 5;
}