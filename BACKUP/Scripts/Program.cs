using System;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Program : MonoBehaviour
{
    //control variables
    new public string name = "";
    public GameManager gameManager;
    public BigInteger speed = 1;
    public BigInteger power = 1;
    BigInteger progress = 0;
    public BigInteger speedMAX = (BigInteger)1e6;
    public BigInteger powerMAX = (BigInteger)1e8;
    public int num;
    //power, speed
    public BigInteger[] level = new BigInteger[2];
    public BigInteger[] Level
    {
        get => level;
        set { level = value; UpdateVals(); }
    }
    public BigInteger[] upgradeAmount = new BigInteger[2];
    //object variables
    public RawImage progressBar;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI SpeedUpg;
    public TextMeshProUGUI Power;
    public TextMeshProUGUI PowerUpg;
    public TextMeshProUGUI nameBox;
    public Button powerUpgrade;
    public Button SpeedUpgrade;

    public GameObject Description;
    public GameObject Upgrades;
    public static bool upgrades = false;
    bool init = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!init)
        {
            init = true;
            gameManager.programs.Add(this);
            powerUpgrade.onClick.AddListener(() =>
            {
                BigInteger cost = costWithBuyMulti(0);
                if (gameManager.bits >= cost)
                {
                    gameManager.bits -= cost;
                    level[0] += upgradeAmount[0];
                    power = BigInteger.Pow(2, (int)level[0] / 2);
                }
            });
            SpeedUpgrade.onClick.AddListener(() =>
            {
                BigInteger cost = costWithBuyMulti(1);
                if (gameManager.bits >= cost)
                {
                    gameManager.bits -= cost;
                    level[1] += upgradeAmount[1];
                    speed = BigInteger.Pow(28, (int)level[1] / 2);
                }
            });
            updateMaxes();
        }
    }

    public void Init()
    {
        if (!init)
        {
            init = true;
            gameManager.programs.Add(this);
            powerUpgrade.onClick.AddListener(() =>
            {
                BigInteger cost = costWithBuyMulti(0);
                if (gameManager.bits >= cost)
                {
                    gameManager.bits -= cost;
                    level[0] += upgradeAmount[0];
                    power = BigInteger.Pow(2, (int)level[0] / 2);
                }
            });
            SpeedUpgrade.onClick.AddListener(() =>
            {
                BigInteger cost = costWithBuyMulti(1);
                if (gameManager.bits >= cost)
                {
                    gameManager.bits -= cost;
                    level[1] += upgradeAmount[1];
                    speed = BigInteger.Pow(28, (int)level[1] / 2);
                }
            });
            updateMaxes();
        }
    }

    public void UpdateVals()
    {
        power = BigInteger.Pow(2, (int)level[0] / 2);
        speed = BigInteger.Pow(2, (int)level[1] / 2);
    }
    // Update is called once per frame
    void Update()
    {
        //BigInteger stuff
        if (gameManager.allowUpdates)
        {
            progress += BigInteger.Min((BigInteger)Time.deltaTime * speed * gameManager.baseSpeed, speedMAX);
            if (progress >= 1)
            {
                gameManager.bits += (progress - (progress % 1)) * BigInteger.Min(power * gameManager.basePower, powerMAX);
                progress %= 1;
            }
            progressBar.transform.localScale = new Vector3(1.8f * (float)progress, 0.2f, 1);
        }
    }
    public void updateMaxes()
    {
        speedMAX = (BigInteger)1e6 * gameManager.maxSpeed;
        powerMAX = (BigInteger)1.5e6 * gameManager.maxPower;
    }
    public void FixedUpdate()
    {
        //ui updates

        if (!upgrades)
        {
            Speed.text = "Speed: " + Tools.makeSuffixedNumber(gameManager.baseSpeed) + " *\n" + Tools.makeSuffixedNumber(speed) + " = "
                        + (gameManager.baseSpeed * speed < speedMAX ? Tools.makeSuffixedNumber(gameManager.baseSpeed * speed) : "MAX (" + Tools.makeSuffixedNumber(speedMAX) + ")");
            Power.text = "Power: " + Tools.makeSuffixedNumber(gameManager.basePower) + " *\n" + Tools.makeSuffixedNumber(power) + " = "
                        + (gameManager.basePower * power < powerMAX ? Tools.makeSuffixedNumber(gameManager.basePower * power) : "MAX (" + Tools.makeSuffixedNumber(powerMAX) + ")");
        }
        else
        {
            PowerUpg.text = "Power: " + Tools.makeSuffixedNumber(power) + "\nCost:" + Tools.makeSuffixedNumber(costWithBuyMulti(0));
            SpeedUpg.text = "Speed: " + Tools.makeSuffixedNumber(speed) + "\nCost:" + Tools.makeSuffixedNumber(costWithBuyMulti(1));
        }
        ToggleUpgrades();
        name = nameBox.text;
        gameObject.GetComponent<RectTransform>().localPosition = Tools.makeProgramPosition(num - 1);
    }
    public void ToggleUpgrades()
    {
        Description.SetActive(!upgrades);
        Upgrades.SetActive(upgrades);
    }



    BigInteger costWithBuyMulti(int p)
    {
        int multi = gameManager.upg.programBuyMulti;
        BigInteger cost = UDC.PUC(level[p], gameManager);
        int i = 1;
        if (multi != -1)
            for (i = 1; i < multi; i++)
            {
                cost += UDC.PUC(level[p] + i, gameManager);
            }
        else
            while (cost + UDC.PUC(level[p] + i, gameManager) <= gameManager.bits)
                cost += UDC.PUC(level[p] + i++, gameManager);
        upgradeAmount[p] = i;
        return cost;
    }
}
