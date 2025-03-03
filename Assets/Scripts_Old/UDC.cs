using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using g = System.Numerics.BigInteger;


public static class UDC //upgrade declogger
{
    const string pos = "-1";

    // -------------------TAB 1----------------------------

    public static void Upgrade4(g level, GameManager gm, Upgrade upg)
    {
        g cost = -1;
        if (cost == -1)
            cost = U4C(level, gm);
        if (gm.bits > cost)
        {
            gm.allowProgramHolder = true; upg.upgradeButton.gameObject.SetActive(false); gm.bits -= cost;
        }


    }
    public static void Upgrade7(g level, GameManager gm, Upgrade upg)
    {
        g cost = -1;
        if (cost == -1)
            cost = U7C(level, gm);
        if (gm.bits > cost)
        { gm.upg.tab2 = true; gm.bits -= cost; upg.upgradeButton.gameObject.SetActive(false); }
    }
    //TAB 1 COST
    public static g U1C(g level, GameManager gm) { return Equations.calculate("5*1.28929^" + (level + 1)) / gm.t1UpgradeCost; }
    public static g U2C(g level, GameManager gm) { return Equations.calculate("10*1.386593^" + (level + 1)) / gm.t1UpgradeCost; }
    public static g U3C(g level, GameManager gm) { return Equations.calculate(level == 0 ? "50" : "1000^" + level) / gm.t1UpgradeCost; }
    public static g U4C(g level, GameManager gm) { return 500 / gm.t1UpgradeCost; }
    public static g U5C(g level, GameManager gm) { return Equations.calculate("50000*2.7^" + level) / gm.t1UpgradeCost; }
    public static g U6C(g level, GameManager gm) { return Equations.calculate("200000*3.3^" + level) / gm.t1UpgradeCost; }
    public static g U7C(g level, GameManager gm) { return (g)5e+13 / gm.t1UpgradeCost; }

    //tab 2



    public static void Upgrade8(g level, GameManager gm, Upgrade upg)
    {
        g cost = U8C(level, gm);
        if (gm.bits > cost)
        { gm.upg.auctions = true; gm.bits -= cost; upg.upgradeButton.gameObject.SetActive(false); }
    }

    //tab 2 costs
    public static g U8C(g level, GameManager gm) { return (g)1e+13; }

    public static g PUC(g level, GameManager gm) { return Equations.calculate("(1.7645^" + (level + 1) + ")+17") / gm.programUpgradeCost; }

    //auction upgrades

    // public static void AUpgrade1(BigInteger level, GameManager gm, Upgrade upg)
    // {
    //     BigInteger cost = AU1C(level, gm);
    //     if (gm.auc.cash > cost){
    //         upg.level++;
    //         gm.auc.cash -= cost;
    //         upg.multiplier.Calculate(new BigInteger[] {level});
    //         gm.refreshMultipliers();}
    // }
    // public static void AUpgrade2(BigInteger level, GameManager gm, Upgrade upg)
    // {
    //     BigInteger cost = AU2C(level, gm);
    //     if (gm.auc.cash > cost){
    //         upg.level++;
    //         gm.auc.cash -= cost;
    //         upg.multiplier.Calculate(new BigInteger[] {level});
    //         gm.refreshMultipliers();}
    // }
    // public static void AUpgrade3(BigInteger level, GameManager gm, Upgrade upg)
    // {
    //     BigInteger cost = AU3C(level, gm);
    //     if (gm.auc.cash > cost){
    //         upg.level++;
    //         gm.auc.cash -= cost;
    //         upg.multiplier.Calculate(new BigInteger[] {level});
    //         gm.refreshMultipliers();}
    // }
    public static g AU1C(g level, GameManager gm) { return Equations.calculate("10*2.333^" + level); }
    public static g AU2C(g level, GameManager gm) { return Equations.calculate("5*1.75^" + level); }
    public static g AU3C(g level, GameManager gm) { return Equations.calculate("8*1.85^" + level); }
}
