using UnityEngine;
using System;
using Unity.VisualScripting;

[Serializable]
public class PartContainer
{
    [Serialize]
    private PartItem item;
    public PartItem Itemm
    {
        get => getItem();
    }
    public bool HasItem
    {
        get => empty ? hasItem() : false;
    }
    private bool empty = false;
    public bool Empty
    {
        get => empty;
    }

    public PartItem this[object obj]
    {
        get => item == obj ? null : item;
        set => item = value == obj ? (PartItem)PaCoFncs.boolChange(ref empty) : PaCoFncs.boolChange(ref empty, ref value);
    }
    public PartItem getItem()
    {
        return item == null ? null : item;
    }
    public PartContainer(PartItem item = null)
    {
        this.item = item;
        empty = item == null ? true : false;
    }
    public PartContainer(int type, int rarity)
    {
        this.item = new PartItem(type, rarity);
    }
    public void addGeneric()
    {
        if (item == null || empty == true)
        {
            item = new PartItem(0, 0, true);
            empty = false;
        }
    }
    public void add(int type, int rarity)
    {
        if (item == null || empty == true)
        {
            item = new PartItem(type, rarity);
            empty = false;
        }
    }
    public void add(int type, int rarity, bool fromInspector)
    {
        if (item == null)
        {
            item = new PartItem(type, rarity, fromInspector);
            empty = false;
        }
    }
    public void Remove()
    {
        item = null;
        empty = true;
    }
    public bool hasItem()
    {
        if (item != null && item.Type != -1)
            return true;
        return false;
    }
    public string DescribeFS()
    {
        if (item != null)
            return item.DescribeFS();
        else return "null";
    }
}
public static class PaCoFncs
{
    public static object boolChange(ref bool b)
    {
        b = true;
        return null;
    }
    public static PartItem boolChange(ref bool b, ref PartItem ret)
    {
        b = false;
        return ret;
    }
}