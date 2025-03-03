using System;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Upgrade : MonoBehaviour
{

    //"Control" variables
    [Header("Control Variables")]
    public string Name;
    public string suffix, prefix;
    int level;
    int maxLevel;
    int upgradeAmount = 0;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            if (maxLevel != 1) Calculate();
            else if (level == 1) upgradeFunc();
        }
    }
    public int MaxLevel
    {
        get => maxLevel;
        set => maxLevel = value;
    }
    Multiplier multi;
    public Multiplier multiplier
    {
        get => multi;
        set => multi = value;
    }
    public BigInteger mVal
    {
        get => multi.value;
        set => Calculate();
    }
    Func<BigInteger, BigInteger> costFunc;
    Action upgradeFunc;

    //Class variables / dependencies
    GameManager gm;



    //Text Mesh Pro variables

    [Header("Prefab Variables")]
    public TextMeshProUGUI LevelTxt;
    public TextMeshProUGUI CostTxt;
    public TextMeshProUGUI MultiplierTXT;
    public TextMeshProUGUI NameTXT;
    public Button upgradeButton;



    public void FixedUpdate()
    {
        updateUI();
    }
    public void Init(UpgradeStorageClass usc, GameManager gameManager)
    {
        level = 0;
        Name = usc.Name;
        maxLevel = usc.maxLevel;
        suffix = usc.suffix;
        prefix = usc.prefix;
        gm = gameManager;
        upgradeButton.onClick.RemoveAllListeners();
        costFunc = (level) => usc.costFunction(level, gm);
        if (usc.maxLevel == 1)
        {
            multiplier = null;
            upgradeFunc = () => usc.upgradeFunction(gm);
            upgradeButton.onClick.AddListener(() =>
            {
                if (gm.allowUpdates)
                {
                    if (costWithBuyMulti() <= gm.bits)
                    {
                        upgradeFunc();
                        gm.bits -= costWithBuyMulti();
                        level = 1;
                        upgradeButton.gameObject.SetActive(false);
                    }
                }
            });
        }
        else
        {
            multiplier = usc.multiplier;
            upgradeButton.onClick.AddListener(() =>
            {
                if (gm.allowUpdates)
                {
                    BigInteger cost = costWithBuyMulti();
                    if (gm.bits >= cost)
                    {
                        gm.bits -= cost;
                        Level += upgradeAmount;
                    }
                }
            });
            if (gm.mp.isIn(multiplier))
            {
                gm.mp.Remove(multiplier);
            }
            gm.mp.Add(multiplier);
        }
        upgradeButton.gameObject.SetActive(true);
        if (multiplier != null)
            Calculate();
        updateUI();
        upgradeButton.onClick.AddListener(() => updateUI());
        upgradeButton.onClick.AddListener(() => gameManager.refreshMultipliers());
    }
    BigInteger costWithBuyMulti()
    {
        int multi = gm.upg.buyMulti;
        if (maxLevel == 1)
            return costFunc(level);
        if (level < maxLevel)
        {
            BigInteger cost = costFunc(level);
            if (multi != -1)
            {
                int i = 1;
                for (i = 1; i < multi && i + level <= maxLevel; i++)
                {
                    cost += costFunc(level + i);
                }
                upgradeAmount = i;
            }
            else
            {
                int i = 1;
                while (cost + costFunc(level + i) <= gm.bits && level + i <= maxLevel)
                    cost += costFunc(level + i++);
                upgradeAmount = i;
            }
            return cost;
        }
        else return 0;
    }
    public void updateUI()
    {
        if (level >= maxLevel)
            upgradeButton.gameObject.SetActive(false);
        LevelTxt.text = "Lvl: " + ((maxLevel > 0 && level >= maxLevel) ? "MAX" : Tools.makeSuffixedNumber(level));
        CostTxt.text = "Cost: " + Tools.makeSuffixedNumber(costWithBuyMulti());
        if (MaxLevel != 1)
            MultiplierTXT.text = prefix + Tools.makeSuffixedNumber(multiplier.value) + suffix;
        else if (level == 1)
        {
            if (multiplier != null) MultiplierTXT.text = Tools.makeSuffixedNumber(multiplier.value) + suffix;
            else MultiplierTXT.text = "UNLOCKED";
        }
        else MultiplierTXT.text = "BUY IT FIRST";
        NameTXT.text = Name;
    }

    public void Calculate()
    {
        multi.Calculate(new object[] { level });
    }
}