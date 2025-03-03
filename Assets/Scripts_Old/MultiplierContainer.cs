using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Numerics;

[Serializable]
public class MultiplierContainer
{
    [SerializeField]
    List<Multiplier> multipliers = new List<Multiplier>();
    public void Add(Multiplier m) { multipliers.Add(m); }
    public void Add(Multiplier[] m) { multipliers.AddRange(m); }
    public void Remove(Multiplier m) { multipliers.Remove(m); }
    public bool isIn(Multiplier m)
    {
        for (int i = 0; i < multipliers.Count; i++)
        {
            if (multipliers[i].name == m.name)
                return true;
        }
        return false;
    }
    public void Set(int i, BigInteger val) { multipliers[i].value = val; }
    public Multiplier Get(int i) { return multipliers[i]; }
    public Multiplier Get(string name)
    {
        for (int i = 0; i < multipliers.Count; i++)
            if (multipliers[i].name == name) return multipliers[i];
        return null;
    }
    public Multiplier[] GetAll() { return multipliers.ToArray(); }

    public BigInteger getTypeMultiplier(Multiplier.what what)
    {
        BigInteger baseNum = 1;
        BigInteger total = 0;
        BigInteger multiplier = 1;
        BigInteger[] addMulties = new BigInteger[Enum.GetNames(typeof(Multiplier.addGroups)).Length];
        Array.Fill(addMulties, 1);
        for (int i = 0; i < multipliers.Count; i++)
        {

            if (multipliers[i].For == what && multipliers[i].active)

            {
                Debug.Log("adding " + multipliers[i].name + "(" + multipliers[i].value + ") to " + what.ToString() + " with mType of " + multipliers[i].type);
                if (multipliers[i].type == Multiplier.multiTypes.multi) multiplier *= multipliers[i].value;
                else if (multipliers[i].type == Multiplier.multiTypes.additive) addMulties[(int)multipliers[i].group] *= multipliers[i].value;
                else if (multipliers[i].type == Multiplier.multiTypes.addbase) baseNum += multipliers[i].value;
                else if (multipliers[i].type == Multiplier.multiTypes.addtotal) total += multipliers[i].value;
            }
        }
        for (int i = 0; i < addMulties.Length; i++)
            multiplier *= addMulties[i];
        total += baseNum * multiplier;
        Debug.Log("adding " + total + " to " + what.ToString());
        return total;
    }
    public BigInteger[] getMultiTypes()
    {
        int len = Enum.GetNames(typeof(Multiplier.what)).Length;
        BigInteger[] multis = new BigInteger[len];
        for (int i = 0; i < len; i++)
            multis[i] = getTypeMultiplier((Multiplier.what)i);
        return multis;
    }

}
[Serializable]
public class Multiplier
{
    public enum multiTypes { multi, additive, addbase, addtotal }
    public enum what { power, speed, maxPrograms, UpgradesCost, UpgradeTab1Cost, ProgramUpgradeCost, PCValue, maxSpeed, maxPower }
    public enum addGroups { NA, a, b, c }
    public static string[] prefixes = { "", "+", "+", "+" };
    public static string[] suffixes = { "x ", "% ", " base ", " total " };

    public string name;
    [SerializeField]
    public multiTypes type;
    [SerializeField]
    public addGroups group = addGroups.NA;
    [SerializeField]
    public what For;
    public BigInteger value = 1;
    public bool active = true;
    public string equation;
    public double randomVal = 0;
    public Multiplier(multiTypes Type, what For, string name, string equation, double rand = 0, addGroups group = addGroups.NA, bool active = true)
    {
        this.name = name;
        type = Type;
        this.For = For;
        this.group = group;
        this.active = active;
        this.equation = equation;
        randomVal = rand != 0 ? (new System.Random().NextDouble() * (rand + 0.5)) - 0.5d : 0;
        if (Type == multiTypes.addbase || Type == multiTypes.addtotal)
            value = 0;


    }
    public string MultiTypeCallback(string str)
    {
        Debug.Log("multitypecallback");
        List<string> MultiplierTypes = new List<string>() { "Multiply", "Additive Multiply", "Add Base", "Add Total" };
        int i;
        for (i = 0; i < MultiplierTypes.Count; i++)
            if (MultiplierTypes[i] == str)
                break;
        type = (multiTypes)i;
        return str;
    }
    public string MultiGroupCallback(string str)
    {
        Debug.Log("mutliGroupcallback");
        List<string> additiveGroups = new List<string>() { "Base", "a", "b", "c" };
        int i;
        for (i = 0; i < additiveGroups.Count; i++)
            if (additiveGroups[i] == str)
                break;
        group = (addGroups)i;
        return str;
    }
    public string MutliForCallback(string str)
    {
        Debug.Log("MultiForcallback");
        List<string> ForStrs = new List<string>() { "power", "speed", "maxPrograms", "UpgradeTab1Cost", "ProgramUpgradeCost", "PCValue", "maxSpeed", "maxPower" };
        int i;
        for (i = 0; i < ForStrs.Count; i++)
            if (ForStrs[i] == str)
                break;
        For = (what)i;
        return str;
    }
    public string Describe()
    {
        List<string> ForStrs = new List<string>() { "power", "speed", "maxPrograms", "UpgradeTab1Cost", "ProgramUpgradeCost", "PCValue", "maxSpeed", "maxPower" };
        List<string> MultiplierTypes = new List<string>() { "Multiply", "Additive Multiply", "Add Base", "Add Total" };
        List<string> additiveGroups = new List<string>() { "Base", "a", "b", "c" };
        string desc = MultiplierTypes[(int)type] + " by " + value + " for " + ForStrs[(int)For] + (type == multiTypes.additive ? " in group " + additiveGroups[(int)group] : "");
        return desc;
    }
    public override string ToString()
    {
        return prefixes[(int)type] + value.ToString("0.0") + suffixes[(int)type] + For.ToString();
    }
    public void Calculate(object[] format)
    {
        value = Equations.formattedCalculate(equation, format);
    }

}
