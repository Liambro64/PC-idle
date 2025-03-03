using System;
using UnityEngine;

public class BitMinerShopItem : ShopItem
{
    public override descriptorVariables dv {
        get => new (
            "Bit Miner",
            sprite,
            0,
            5,
            () => {
                DesktopManager.Add(shortcut, (a, b) => {
                    bought = true;
                    return Instantiate(original:a, parent:b);
                });},
            bought,
            "10/10 would buy again"
        );
    }
    public override void extraStart()
    {

    }
}
