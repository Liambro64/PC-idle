using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

[Serializable]
public class PartInventory
{
    [SerializeReference]
    public List<PartContainer> slot = new List<PartContainer>(8);

    public int Space
    {
        get => slot.Capacity;
        set { if (slot.Count > value) { slot.RemoveRange(value, Count - value); slot.Capacity = value; } else { slot.Capacity = value; Fill(); } }
    }
    public int Count { get => slot.Count; }
    public int CountIn { get => CountInContainers(); }
    public PartContainer this[int index]
    {
        get => slot[index];
        set => slot[index] = value;
    }
    public PartItem this[int index, object index2]
    {
        get => slot[index][index2];
        set => slot[index][index2] = value;
    }

    private GameManager gm;
    public GameManager gameManager
    {
        get => gm;
        set { gm = value; mpc = gm.mp; }
    }
    private MultiplierContainer mpc;
    //initialiser
    public PartInventory(int space = 8)
    {
        slot = new List<PartContainer>(space);
        PartInventory cur = this;
        if (!isFilled(ref cur))
            throw new Exception("your computer hates you cause it should have worked");
        try
        {
            gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
            mpc = gm.mp;
        }
        catch { Debug.LogWarning("Couldnt get GameManager or Multiplier Container"); }

    }

    public bool nullCheck(int space = 8)
    {
        if (slot == null)
        {
            slot = new List<PartContainer>(8);
        }
        if (slot == null)
        {
            Debug.Log("literally fucking how");
            return true;
        }
        return false;
    }
    public void addGeneric(int i)
    {
        this[i].addGeneric();
    }
    public PartItem setGeneric(int i)
    {
        Remove(i);
        addGeneric(i);
        return this[i][null];
    }
    public void Remove(int i)
    {
        this[i].Remove();
    }
    public void Add(PartItem item)
    {
        slot.Add(new PartContainer(item));
    }
    public void Add(PartContainer item)
    {
        slot.Add(item);
    }
    public void Add(int type, int rarity)
    {
        slot.Add(new PartContainer(type, rarity));
    }
    public void Fill()
    {
        for (int i = 0; i < slot.Capacity; i++)
            if (this.Count <= i)
                this.Add(new PartContainer());
    }

    public static bool isFilled(ref PartInventory cont, int space = 8)
    {
        if (cont == null)
            cont = new PartInventory(space);
        if (cont.nullCheck())
        {
            Debug.Log("why do you do this to me"); return false;
        }
        if (cont.Count < cont.Space)
            cont.Fill();
        if (cont.Count == cont.Space)
            return true;
        return false;
    }
    int CountInContainers()
    {
        int c = 0;
        for (int i = 0; i < slot.Count; i++)
        {
            if (slot[i].hasItem())
                c++;
        }
        return c;
    }
}