using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Numerics;
using UnityEngine;

public class UpgradeStorageClass
{
    public string Name;
    public int maxLevel;
    public Action<GameManager> upgradeFunction;
    public Func<BigInteger, GameManager, BigInteger> costFunction;
    public Multiplier multiplier;
    public string suffix, prefix;
    public UpgradeStorageClass(string name, int maxlevel, Action<GameManager> upgradefnc, Func<BigInteger, GameManager, BigInteger> costfnc, Multiplier multi, string prefix = "", string suffix = "")
    {
        Name = name;
        maxLevel = maxlevel;
        upgradeFunction = upgradefnc;
        costFunction = costfnc;
        multiplier = multi;
        this.prefix = prefix;
        this.suffix = suffix;
    }
}