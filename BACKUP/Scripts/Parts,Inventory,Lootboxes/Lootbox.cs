using UnityEngine;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;

public class Lootbox
{
    public enum specialisation { None, motherboard, cpu, gpu, ram, strg, fan }
    specialisation spec;
    public double[] chances;
    public Lootbox(specialisation specs, double[] chances)
    {
        this.chances = chances;
        this.spec = specs;
        mathname();
    }
    public Lootbox(double[] chances)
    {
        this.chances = chances;
        spec = 0;
        mathname();
    }
    void mathname()
    {
        double total = 0;
        double total2 = 0;
        for (int i = 0; i < chances.Length; i++)
            total += chances[i];
        Console.WriteLine("Total is" + total);
        double[] chances2 = new double[chances.Length];
        for (int i = 0; i < chances.Length; i++)
        {
            chances2[i] = 100 / (total / chances[i]);
            Console.WriteLine("New chance " + i + ": " + chances2[i]);
            total2 += chances2[i];
        }
        chances = chances2;
    }
    public PartItem Open(GameManager gm)
    {
        double random = UnityEngine.Random.Range(0, 100);
        double cur = 0;
        int i = 0;
        for (i = 0; i < chances.Length; i++)
        {
            cur += chances[i];
            if (cur >= random)
                break;
        }
        if (spec == 0)
        {
            PartItem itemReturn = new PartItem(UnityEngine.Random.Range(0, 6), i);
            itemReturn.makeMultipliers(gm.mp);
            return itemReturn;
        }
        PartItem item = new PartItem((int)(spec - 1), i);
        item.makeMultipliers(gm.mp);
        return item;
    }
}