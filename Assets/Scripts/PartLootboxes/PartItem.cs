using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PartItem
{
    //MB, CPU, GPU, RAM, Strg, Fans
    public int position = 0;
    private int type;

    public int Type
    {
        get => type;
        set => type = value;
    }
    private int rarity;

    public int Rarity
    {
        get => rarity;
        set => rarity = value;
    }
    private Multiplier stat1;
    private Multiplier stat2;

    public Multiplier firstMultiplier { get => stat1; set => stat1 = value; }
    public Multiplier secondMultiplier { get => stat2; set => stat2 = value; }
    public bool madeMultis = false;
    private bool active = false;
    public bool Active
    {
        get => active;
        set { active = value; setActiveActive(); }
    }
    public PartItem(int type, int rarity, Multiplier stat1, Multiplier stat2 = null)
    {
        this.type = type;
        this.rarity = rarity;
        this.stat1 = stat1;
        this.stat2 = stat2;
    }
    public PartItem(int type, int rarity)
    {
        this.type = type;
        this.rarity = rarity;
        makeMultipliers();
    }
    public PartContainer Contain()
    {
        return new PartContainer(this);
    }
    public PartItem(int type, int rarity, bool fromInspector)
    {
        this.type = type;
        this.rarity = rarity;
    }
    [SerializeField]
    public Multiplier[] makeMultipliers(MultiplierContainer mp = null)
    {
        switch (type)
        {
            case 0:
                stat1 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.speed, "MBbs", "1+(5^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                if (rarity >= 3)
                    stat2 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.power, "MBbp", "1+(3^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                else stat2 = null;
                break;
            case 1: //cpu
                stat1 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.power, "CPUbp", "1+(4^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                if (rarity >= 3)
                    stat2 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.ProgramUpgradeCost, "CPUpuc", "1+(2^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                else stat2 = null;
                break;
            case 2: //gpu
                stat1 = new Multiplier(Multiplier.multiTypes.multi, Multiplier.what.power, "GPUmp", "1+(4^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                if (rarity >= 3)
                    stat2 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.UpgradesCost, "GPUbuc", "1+(3^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                else stat2 = null;
                break;
            case 3: //RAM
                stat1 = new Multiplier(Multiplier.multiTypes.multi, Multiplier.what.maxPower, "RAMmmp", "1+(10^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                if (rarity >= 3)
                    stat2 = new Multiplier(Multiplier.multiTypes.multi, Multiplier.what.maxSpeed, "RAMmms", "1+(10^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                else stat2 = null;
                break;
            case 4: //Storage
                stat1 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.maxPrograms, "STRGbmp", "~0*(1+~1)", Math.Pow(2, rarity), active: false);
                if (rarity >= 3)
                    stat2 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.maxPrograms, "STRGbmp2", "~0*(1+~1)", Math.Pow(2, rarity), active: false);
                else stat2 = null;
                break;
            case 5: //fans
                stat1 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.maxSpeed, "FANbms", "1+(10^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                if (rarity >= 3)
                    stat2 = new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.maxPower, "FANpuc", "1+(10^~0)*(1+~1)", Math.Pow(2, rarity), active: false);
                else stat2 = null;
                break;
            case object:
                return null;

        }
        madeMultis = true;
        Multiplier[] ms = new Multiplier[2] { stat1, stat2 };
        if (stat1 != null)
        {
            stat1.Calculate(new object[] { rarity, stat1.randomVal });
            mp?.Add(stat1);
        }
        if (stat2 != null)
        {
            stat2.Calculate(new object[] { rarity, stat2.randomVal });
            mp?.Add(stat2);
        }
        return ms;
    }
    public void Calculate()
    {
        if (stat1 != null)
            stat1.Calculate(new object[] { rarity, stat1.randomVal });
        if (stat2 != null)
            stat2.Calculate(new object[] { rarity, stat2.randomVal });
    }
    public void setOppositeActive()
    {
        active = !active;
        if (stat1 != null)
            stat1.active = active;
        if (stat2 != null)
            stat2.active = active;
    }
    public void setActive()
    {
        active = true;
        if (stat1 != null)
            stat1.active = active;
        if (stat2 != null)
            stat2.active = active;
    }
    public void setNotActive()
    {
        active = false;
        if (stat1 != null)
            stat1.active = active;
        if (stat2 != null)
            stat2.active = active;
    }
    public void setActiveActive()
    {
        if (stat1 != null)
            stat1.active = active;
        if (stat2 != null)
            stat2.active = active;
    }
    public void delete()
    {
        this.Active = false;
        stat1 = null;
        stat2 = null;
        type = -1;
        rarity = -1;
    }
    public string DescribeFS()
    {
        string str = active + ";" + type + ";" + rarity + ";" +
        (stat1 != null ? stat1.randomVal : "null") + ";" +
        (stat2 != null ? stat2.randomVal : "null");
        return str;
    }
}
