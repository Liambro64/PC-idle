using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;

[Serializable]
public class InventoryController : MonoBehaviour
{
    public GameManager gm;
    public GameObject inventoyPrefab;
    public RectTransform inventoryInUI;

    public List<FormatItem> itemsInSpace = new List<FormatItem>();
    public GameObject partInteractor;

    [SerializeReference]
    public PartInventory inv = new(space: 10);


    private PartItem[] Equipped = new PartItem[6];

    public void Start()
    {
        if (inv.gameManager == null)
            inv.gameManager = gm;
    }
    public void FixedUpdate()
    {
        if (itemsInSpace.Count < inv.Space)
            while (itemsInSpace.Count != inv.Space)
            {
                FormatItem item = Instantiate(inventoyPrefab, inventoryInUI).GetComponent<FormatItem>();
                itemsInSpace.Add(item);
                Vector2 pos = makePos(itemsInSpace.Count - 1);
                item.SetPos(pos);
                item.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    partInteractor.GetComponent<PartInteractor>().toInteract = item.Item;
                    if (item.Item != null)
                        partInteractor.SetActive(true);
                });
                float down = Mathf.Abs(pos.y) - 193;
                if (inventoryInUI.sizeDelta.y < down)
                    inventoryInUI.sizeDelta = new Vector2(920, down);
            }
        else if (itemsInSpace.Count > inv.Space)
            while (itemsInSpace.Count != inv.Space)
            {
                FormatItem ite = itemsInSpace[itemsInSpace.Count - 1];
                itemsInSpace.Remove(ite);
                ite.delete();
            }
        for (int i = 0; i < itemsInSpace.Count; i++)
        {
            if (this[i] != null && this[i].Type == -1)
                this[i] = null;
            itemsInSpace[i].Item = inv[i].getItem();
        }
    }

    public Vector2 makePos(int i)
    {
        return new Vector2(
            x: 25 + (225 * (i % 4)),
            y: -25 - (225 * (i / 4))
        );
    }
    public bool Add(PartItem item)
    {
        int i = 0;
        while (this[i] != null && i < inv.Space)
            i++;
        if (i == inv.Space)
        {
            print("what the fuck neggar");
            return false;

        }
        this[i] = item;
        return true;
    }
    public void Remove(PartItem item)
    {
        for (int i = 0; i < slot.Count; i++)
        {
            if (this[i] == item)
            {
                this[i] = null;
                itemsInSpace[i].Item = null;
                break;
            }
            if (itemsInSpace[i].Item == item)
            {
                this[i] = null;
                itemsInSpace[i].Item = null;
                break;
            }
        }
    }
    public PartItem this[int index]
    {
        get => inv[index][null];
        set => inv[index][null] = value;
    }
    public List<PartContainer> slot
    {
        get => inv.slot;
        set => inv.slot = value;
    }
    public bool isEquipped(PartItem item)
    {
        for (int i = 0; i < Equipped.Length; i++)
            if (Equipped[i] == item)
                return true;
        return false;
    }
    public PartItem getEquipped(int type)
    {
        return Equipped[type];
    }
    public void setEquipped(PartItem item)
    {
        Equipped[item.Type] = item;
    }
    public void removeEquipped(int type)
    {
        Equipped[type].Active = false;
        Equipped[type] = null;
    }
}
