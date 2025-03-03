using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Auctions : MonoBehaviour
{
    public GameManager gm;
    public GameObject main;
    public GameObject auctionScene;
    public GameObject sellButton;
    public TextMeshProUGUI value;
    public TextMeshProUGUI YouHave;
    public BigInteger cash = 0;
    public bool sellButtonPressedOnce = false;

    public BigInteger PCValueMulti = 1;
    [Header("View Variables")]
    [SerializeField]
    private BigInteger SellValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //sellButton.GetComponent<Button>().onClick.RemoveAllListeners(); 
        //sellButton.GetComponent<Button>().onClick.AddListener(() => Sell());
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void Sell()
    {
        sellButtonPressedOnce = true;
        sellButton.GetComponent<Button>().onClick.RemoveAllListeners();
        sellButton.GetComponent<Button>().onClick.AddListener(() => Finish());
        sellButton.GetComponentInChildren<TextMeshProUGUI>().text = "RESET";


        BigInteger value = pcValueCalc(gm.bitsPerSecond());
        gm.allowUpdates = false;
        //auctionScene.SetActive(true);
        //StartCoroutine(disableAfterTime());
        cash += value;
    }
    public void makeButtonReset()
    {
        sellButtonPressedOnce = true;
        gm.allowUpdates = false;
        sellButton.GetComponent<Button>().onClick.RemoveAllListeners();
        sellButton.GetComponent<Button>().onClick.AddListener(() => Finish());
        sellButton.GetComponentInChildren<TextMeshProUGUI>().text = "RESET";
    }
    public void makeButtonSell()
    {
        sellButtonPressedOnce = false;
        sellButton.GetComponent<Button>().onClick.RemoveAllListeners();
        sellButton.GetComponent<Button>().onClick.AddListener(() => Sell());
        sellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sell";
        gm.allowUpdates = true;
    }

    void FixedUpdate()
    {
        SellValue = pcValueCalc(gm.bitsPerSecond());
        value.text = Tools.makeSuffixedNumber(SellValue);

        YouHave.text = Tools.makeSuffixedNumber(cash);
    }

    public void Finish()
    {
        sellButtonPressedOnce = false;
        sellButton.GetComponent<Button>().onClick.RemoveAllListeners();
        sellButton.GetComponent<Button>().onClick.AddListener(() => Sell());
        sellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sell";
        gm.allowUpdates = true;
        gm.resetLoop0();
    }
    public BigInteger pcValueCalc(BigInteger bps)
    {
        return Equations.calculate("5^" + (BigInteger.Log(bps, 25) - BigInteger.Log(bps, 400)) + "*" + PCValueMulti);
    }

    public IEnumerator disableAfterTime()
    {
        yield return new WaitForSeconds(10);
        auctionScene.SetActive(false);
        main.SetActive(true);
        Time.timeScale = 0;
    }
}
