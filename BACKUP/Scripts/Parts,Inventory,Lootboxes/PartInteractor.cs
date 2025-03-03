using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using System.ComponentModel;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System;
using Unity.Mathematics;
using System.Numerics;
public class PartInteractor : MonoBehaviour
{
    public FormatItem formatItem;
    public PartItem toInteract
    {
        get => formatItem.Item;
        set { formatItem.Item = value; if (value != null) { update(); } }
    }
    public GameManager gameManager;
    public InventoryController inventoryController;

    [Header("UI Fields")]

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Stat1;
    public TextMeshProUGUI Compare1;
    public TextMeshProUGUI Stat2;
    public TextMeshProUGUI Compare2;
    public TextMeshProUGUI ButtonText;
    public Button button;
    public Button SellButton;
    public TextMeshProUGUI sellButtonText;
    string[] Names = { "Motherboard", "CPU", "GPU", "RAM", "Storage", "Fans" };
    Color[] colors = {new Color(1, 1, 1, 1), new Color(1, 0.867f, 0, 1), new Color(0.478f, 0.509f, 0.933f),
                      new Color(0.433f, 0.196f, 0.145f), new Color(0.203f, 0.203f,  0.184f), new Color(1, 0.031f, 0)};
    public void update()
    {
        button.onClick.RemoveAllListeners();
        try
        {
            Name.text = Names[toInteract.Type];
            Name.color = colors[toInteract.Rarity];
            if (toInteract.firstMultiplier != null)
                Stat1.text = toInteract.firstMultiplier.ToString();
            else
                Stat1.text = "";
            if (toInteract.secondMultiplier != null)
                Stat2.text = toInteract.secondMultiplier.ToString();
            else
                Stat2.text = "";
            if (inventoryController.isEquipped(toInteract))
            {
                SellButton.onClick.RemoveAllListeners();
                Compare1.text = "=";
                if (toInteract.secondMultiplier != null)
                    Compare2.text = "=";
                else Compare2.text = "";
                ButtonText.text = "Unequip";
                SellButton.onClick.AddListener(() => makeSure());
                sellButtonText.text = "Unequip first";
                button.onClick.AddListener(() =>
                {
                    inventoryController.removeEquipped(toInteract.Type);
                    update();
                });
            }
            else
            {
                sellButtonText.text = "Sell for " + sellValue().ToString("0.0");
                SellButton.onClick.AddListener(() => makeSure());
                PartItem item = inventoryController.getEquipped(toInteract.Type);
                if (item != null)
                {
                    Compare1.text = tOperator(toInteract.firstMultiplier.value, item.firstMultiplier.value);
                    if (toInteract.secondMultiplier != null)
                    {
                        if (item.secondMultiplier != null)
                        {
                            Compare2.text = tOperator(toInteract.secondMultiplier.value, item.secondMultiplier.value);
                        }
                        else
                        {
                            Compare2.text = ">";
                        }
                    }
                    else
                    {
                        Compare2.text = item.secondMultiplier != null ? "<" : "";
                    }
                }
                else
                {

                    Compare1.text = ">";
                    if (toInteract.secondMultiplier != null)
                        Compare2.text = ">";
                    else Compare2.text = "";
                }

                ButtonText.text = "Equip";
                button.onClick.AddListener(() => Equip());
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Type: " + toInteract.Type + "\nRarity: " + toInteract.Rarity + "\nException:\n" + ex.StackTrace);
        }
    }
    public void Equip()
    {
        PartItem item = inventoryController.getEquipped(toInteract.Type);
        inventoryController.setEquipped(toInteract);
        ButtonText.text = "Unequip";
        toInteract.Active = true;
        if (item != null)
            item.Active = false;
        button.onClick.RemoveAllListeners();
        update();
        gameManager.refreshMultipliers();
        button.onClick.AddListener(() =>
        {
            inventoryController.removeEquipped(toInteract.Type);
            update();
        });
    }
    public void makeSure()
    {
        sellButtonText.text = "Are you sure?";
        SellButton.onClick.RemoveAllListeners();
        SellButton.onClick.AddListener(() => Sell());
    }
    public void Sell()
    {
        SellButton.onClick.RemoveAllListeners();
        gameManager.auc.cash += sellValue();
        inventoryController.Remove(toInteract);
        toInteract.delete();
        toInteract = null;
        gameObject.SetActive(false);
    }
    public BigInteger sellValue()
    {
        BigInteger Val = 0;
        if (toInteract.firstMultiplier != null)
            Val += (BigInteger)BigInteger.Log10(BigInteger.Pow(toInteract.firstMultiplier.value, 3));
        if (toInteract.secondMultiplier != null)
            Val += (BigInteger)BigInteger.Log10(BigInteger.Pow(toInteract.secondMultiplier.value, 2)) / 2;
        return BigInteger.Pow(2 * (toInteract.Rarity + 1), (int)Val / 2) + 17;
    }

    string tOperator(BigInteger a, BigInteger b)
    {
        try
        {
            if (a > b)
                return ">";
            if (a < b)
                return "<";
            return "=";
        }
        catch { return "!"; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
