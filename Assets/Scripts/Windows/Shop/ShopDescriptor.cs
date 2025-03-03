using System;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopDescriptor : MonoBehaviour
{
    public Sprite[] thumbs;
    public Image icon, thumb;
    public TextMeshProUGUI Name, Cost, Description;
    BigInteger cost;
    public Button buyButton;
    bool bought;
    public ShopWindow window;
    Action onBuy;
    public void windowCheck()
    {
        if (window == null)
            window = transform.parent.parent.GetComponent<ShopWindow>();
    }
    void Start()
    {
        windowCheck();
        enabled = false;
    }
    public void Show(descriptorVariables dv)
    {
        enabled = true;
        windowCheck();
        if (window == null)
            return;
        icon.sprite = dv.icon;
        thumb.sprite = thumbs[dv.thumb];
        Name.text = dv.Name;
        cost = dv.Cost;
        Cost.text = "Cost: " + BigIntegerHelp.ToString(cost);
        Description.text = dv.Description;
        onBuy = () =>
        {
            windowCheck();
            if (window.shopMoney >= cost)
                dv.onBuy();
            buyButton.onClick.RemoveListener(() => onBuy());
            updateButton();
        };
        buyButton.onClick.AddListener(() => onBuy());
        buyButton.interactable = !dv.bought;
        updateButton();
        gameObject.SetActive(true);
    }
    public void updateButton()
    {
        windowCheck();
        TextMeshProUGUI bbText = buyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (bought)
        {
            bbText.text = "Bought";
            buyButton.interactable = false;
            return;
        }
        if (bbText.text != "Buy")
            bbText.text = "Buy";
        if (window.shopMoney < cost)
            buyButton.interactable = false;
        else if (window.shopMoney < cost)
            buyButton.interactable = true;

    }
    void FixedUpdate()
    {
        windowCheck();
        if (!bought)
            updateButton();
        gameObject.GetComponent<RectTransform>().sizeDelta = window.viewportSize;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
public class descriptorVariables
{
    public Sprite icon;
    public int thumb;
    public BigInteger Cost;
    public string Name, Description;
    public Action onBuy;
    public bool bought;
    public descriptorVariables(string name, Sprite icon, int thumb, BigInteger cost, Action onBuy, bool bought = false, string Description = "")
    {
        this.icon = icon;
        this.thumb = thumb;
        Cost = cost;
        Name = name;
        this.Description = Description;
        this.onBuy = onBuy;
    }
}
