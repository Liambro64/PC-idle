using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSectionController : MonoBehaviour
{
    public GameObject UpgradeButton;
    public GameObject TabUpgrade;

    public int tabOpen = 0;

    public GameObject tab1;
    public bool tab2 = false;
    public Button Tab2;
    public GameObject tab2Obj;
    public bool auctions = false;
    public GameObject auctionButton;
    public GameObject AuctionUpgradeHolder;

    public GameManager gm;
    public int buyMulti = 1;
    public int programBuyMulti = 1;

    public TextMeshProUGUI buyMultiText;
    public TextMeshProUGUI programBuyMultiText;


    UpgradeStorageClass[] TAB1 =
    {
        new UpgradeStorageClass("Program Power", 200, null, UDC.U1C, new Multiplier(Multiplier.multiTypes.multi, Multiplier.what.power, "1mp", "1.11^~0"), prefix: "x"),
        new UpgradeStorageClass("Program Speed", 175, null, UDC.U2C, new Multiplier(Multiplier.multiTypes.multi, Multiplier.what.speed, "1ms", "1.08^~0"), prefix: "x"),
        new UpgradeStorageClass("Max Programs", 20, null, UDC.U3C, new Multiplier(Multiplier.multiTypes.addbase, Multiplier.what.maxPrograms, "1abmp", "~0"), prefix: "+"),
        new UpgradeStorageClass("Program Upgrades???", 1, (gm) => gm.allowProgramHolder = true, UDC.U4C, null),
        new UpgradeStorageClass("Upgrade Costs for Tab 1", 150, null, UDC.U5C, new Multiplier(Multiplier.multiTypes.multi, Multiplier.what.UpgradeTab1Cost, "1mut1c", "1.1^~0"), prefix:"/"),
        new UpgradeStorageClass("Upgrade Costs for Programs", 125, null, UDC.U6C, new Multiplier(Multiplier.multiTypes.multi, Multiplier.what.ProgramUpgradeCost, "1apuc", "1.08^~0"),prefix: "/"),
        new UpgradeStorageClass("Next Tab?", 1, (gm) => gm.upg.tab2 = true, UDC.U7C, null)
    };
    UpgradeStorageClass[] TAB2 =
    {
        new UpgradeStorageClass("I wanna sell it now", 1, (gm) => gm.upg.auctions = true, UDC.U8C, null)
    };
    public static GameManager RANDOMBS()
    {
        return Tools.GetGameManager();
    }
    public List<Upgrade> upgrades = new List<Upgrade>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tools.areSuffixesMade();
        MakeTab1();
        MakeTab2();
        //MakeAuctionUpgrades();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void FixedUpdate()
    {
        if (auctionButton.activeSelf != auctions)
            auctionButton.SetActive(auctions);
        if (Tab2.interactable != tab2)
            Tab2.interactable = tab2;
        switch (tabOpen)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case -1:
                break;

        }
    }

    public void updateUpgrades()
    {
        for (int i = 0; i < upgrades.Count; i++)
            upgrades[i].updateUI();
    }
    public void resetUpgs()
    {
        int tot = 0;
        int i = 0;
        for (i = 0; i < TAB1.Length; i++)
            upgrades[i].Init(TAB1[i], gm);
        tot += i;
        for (i = 0; i < TAB2.Length; i++)
            upgrades[i + tot].Init(TAB2[i], gm);
        tot += i;
        //for (i = 0; i < AUCTIONS.Length; i++)
        //upgrades[i+tot].Init(AUCTIONS[i], gm);
        tab2 = false;
        auctions = false;
    }







    ////UPGRADE TABS


    public void MakeTab1()
    {
        for (int i = 0; i < TAB1.Length - 1; i++)
            upgrades.Add(MakeUpgrade(i, UpgradeButton, tab1.transform, TAB1, MakeUpgradeTabPosition));
        upgrades.Add(MakeUpgrade(6, TabUpgrade, tab1.transform, TAB1, (i) => new Vector3(-2.5f, -265)));
        //tab2
    }

    public void MakeTab2()
    {
        for (int i = 0; i < TAB2.Length; i++)
            upgrades.Add(MakeUpgrade(i, UpgradeButton, tab2Obj.transform, TAB2, MakeUpgradeTabPosition));
    }


    ///TAB 1 STUFF-------------------------------------------------------------------------------------

    public Vector3 MakeUpgradeTabPosition(int i) { return new Vector3(7 + (305 * (i % 3)), -20 - (129 * (i % 6 / 3))); }

    public Vector3 MakeAuctionUpgradePosition(int i) { return new Vector3(25 + (325 * (i % 2)), -25 - (125 * (i / 2))); }





    int[] buyOptions = { 1, 5, 10, 25, 50, 100, -1 };
    public void changeBuyMulti()
    {
        int i;
        for (i = 0; i < buyOptions.Length; i++)
        {
            if (buyMulti == buyOptions[i])
                break;
        }
        if (i >= buyOptions.Length - 1)
            i = -1;
        buyMulti = buyOptions[++i];
        buyMultiText.text = "Buy: " + (buyMulti == -1 ? "MAX" : buyMulti + "x");
        updateUpgrades();
    }
    public void changeProgramBuyMulti()
    {
        int i;
        for (i = 0; i < buyOptions.Length; i++)
        {
            if (programBuyMulti == buyOptions[i])
                break;
        }
        if (i >= buyOptions.Length - 1)
            i = -1;
        programBuyMulti = buyOptions[++i];
        programBuyMultiText.text = "Buy: " + (programBuyMulti == -1 ? "MAX" : programBuyMulti + "x");
        updateUpgrades();
    }

    public Upgrade MakeUpgrade(int i, GameObject src, Transform parent, UpgradeStorageClass[] upgSC, Func<int, Vector3> func)
    {
        GameObject obj = Instantiate(src, parent);
        Upgrade upg = obj.GetComponent<Upgrade>();
        upg.Init(upgSC[i], gm);
        obj.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        obj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        obj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        obj.GetComponent<RectTransform>().localPosition = func(i);
        return upg;
    }















}
