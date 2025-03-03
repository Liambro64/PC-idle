using System;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Windows;
using TMPro;
using UnityEngine.UI;
using System.Text;
using Unity.Collections;
using UnityEngine.InputSystem;
using System.IO;
using Unity.Mathematics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class GameManager : MonoBehaviour
{
    [Header("Control Variables")]
    //control variables
    public BigInteger bits = 0;
    public BigInteger baseSpeed = 1;
    public BigInteger basePower = 1;
    public BigInteger maxPower = 1;
    public BigInteger maxSpeed = 1;
    public int maxPrograms = 1;
    int currentPossiblePrograms = 0;
    public int cpp
    {
        get => currentPossiblePrograms;
        set => currentPossiblePrograms = value;
    }
    public bool allowProgramHolder = false;
    public bool PUbutton
    {
        get => allowProgramHolder;
        set { allowProgramHolder = value; }
    }
    private BigInteger UpgradesCost = 1;
    private BigInteger Tab1UpgradeCost = 1;
    public bool allowUpdates = true;
    public BigInteger t1UpgradeCost
    {
        get => UpgradesCost * Tab1UpgradeCost;
        set => Tab1UpgradeCost = value;
    }
    public BigInteger programUpgradeCost = 1;
    public bool unlockedTab2 = false;
    public BigInteger gamespeed = 1;
    [Header("Object Variables")]
    //object variables
    public TextMeshProUGUI bitText;
    public RectTransform programHolder;
    public GameObject ProgramObject;
    public GameObject ProgramBuyButton;
    public GameObject programUpgradeButton;
    public UpgradeSectionController upg;
    public Button tmp10xbits;
    public Auctions auc;
    //class variables
    public List<Program> programs = new List<Program>();
    public List<GameObject> buyButtons = new List<GameObject>();
    public MultiplierContainer mp = new MultiplierContainer();
    public LootboxesnAllThat LBNallat;
    public Save save;
    public Load load;
    //view variables
    [Header("View Variables")]
    [SerializeField]
    private BigInteger BPS = 0;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()

    {
        Tools.areSuffixesMade();
        save = new Save("C:\\STUFF\\evsa.vas", this);
        load = new Load(save);
        auc.sellButton.GetComponent<Button>().onClick.RemoveAllListeners();
        auc.sellButton.GetComponent<Button>().onClick.AddListener(() => auc.Sell());
        //tmp10xbits.onClick.AddListener(() => this.bits*=10);
    }
    public void refreshMultipliers()
    {
        //update with multiplier types
        //power, speed, maxPrograms, UpgradesCost, UpgradeTab1Cost, ProgramUpgradeCost, PCValue, maxSpeed, maxPower
        BigInteger[] multis = mp.getMultiTypes();
        basePower = multis[0];
        baseSpeed = multis[1];
        maxPrograms = (int)multis[2];
        UpgradesCost = multis[3];
        Tab1UpgradeCost = multis[4];
        programUpgradeCost = multis[5];
        auc.PCValueMulti = multis[6];
        maxSpeed = multis[7];
        maxPower = multis[8];
        for (int i = 0; i < programs.Count; i++)
            programs[i].updateMaxes();
    }
    public BigInteger bitsPerSecond()
    {
        BigInteger bbps = basePower * baseSpeed;
        BigInteger total = 0;

        for (int i = 0; i < programs.Count; i++)
        {
            total += BigInteger.Min(programs[i].power * programs[i].speed * bbps, programs[i].powerMAX * programs[i].speedMAX);
        }
        return total;
    }
    public void toggleProgramUpgrades()
    {
        Program.upgrades = !Program.upgrades;
    }
    // Update is called once per frame
    void Update()
    {
        //Time.timeScale = (float)gamespeed;
        if (UnityEngine.Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("String: " + save.SaveString);
            Debug.Log("Hash: " + save.SaveHash);
            Debug.Log(save.Create() ? "Save created at " + save.Path : "Failed to create save");

        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            load.LoadSave();
        }
        bitText.text = "bits: " + Tools.makeSuffixedNumber(bits);
    }
    public void FixedUpdate()
    {
        BPS = bitsPerSecond();
        //do UI updates here
        while (maxPrograms > currentPossiblePrograms)
        {
            makeButton(++currentPossiblePrograms);
        }
        if (programUpgradeButton.activeSelf != allowProgramHolder)
            programUpgradeButton.SetActive(allowProgramHolder);
    }
    public void makeProgram(int i, Button button = null)
    {
        if (bits >= Tools.makeProgramCost(i))
        {
            if (button != null)
            {
                buyButtons.Remove(button.gameObject);
                bits -= Tools.makeProgramCost(i);
                Destroy(button.gameObject);
            }
            GameObject obj = Instantiate(ProgramObject, new Vector3(1000, 1000), Quaternion.Euler(0, 0, 0), programHolder);
            obj.GetComponent<Program>().gameManager = this;
            obj.GetComponent<Program>().num = i;
            obj.GetComponent<Program>().Init();
            obj.GetComponent<RectTransform>().localPosition = Tools.makeProgramPosition(i - 1);
        }
    }
    public void makeButton(int i)
    {
        Button button = Instantiate(ProgramBuyButton, programHolder)
        .GetComponent<Button>();
        buyButtons.Add(button.gameObject);
        button.name = i + "";
        button.GetComponent<RectTransform>().localPosition = Tools.makeButtonPosition(i - 1);
        if (button.GetComponent<RectTransform>().localPosition.x + 100 > programHolder.GetComponent<RectTransform>().sizeDelta.x)
            programHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(480 + 210 * ((i - 1) / 2), programHolder.GetComponent<RectTransform>().sizeDelta.y);
        button.onClick.AddListener(() => makeProgram(i, button));
        TextMeshProUGUI[] text = button.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int j = 0; j < text.Length; j++)
            if (text[j].gameObject.name == "cost")
                text[j].text = "cost: " + Tools.makeSuffixedNumber(Tools.makeProgramCost(i));
    }
    IEnumerator DestroyAfterFrame(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        Destroy(obj);
    }
    public void resetLoop0()
    {
        bits = 5;
        upg.resetUpgs();
        for (int i = 0; i < programs.Count; i++)
        {
            GameObject obj = programs[i].gameObject;
            Destroy(obj);
        }
        for (int i = 0; i < buyButtons.Count; i++)
            Destroy(buyButtons[i]);
        buyButtons.RemoveRange(0, buyButtons.Count);
        programs.RemoveRange(0, programs.Count);
        allowProgramHolder = false;
        unlockedTab2 = false;
        Program.upgrades = false;
        auc.gameObject.SetActive(false);
        currentPossiblePrograms = 0;
        StartCoroutine(doAfterSeconds(0.1f, refreshMultipliers));
    }
    //past here is a mess so you do not have to look
    public void getProgramPower()
    {
        basePower = mp.getTypeMultiplier(Multiplier.what.power);
    }
    public void getProgramSpeed()
    {
        baseSpeed = mp.getTypeMultiplier(Multiplier.what.speed);
    }
    public IEnumerator doAfterSeconds(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
