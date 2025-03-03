using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;
using static suffix;
using System.IO;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;
using bih = BigIntegerHelp;
using System;

public class BitMinerWindow : Window
{
    public Button buySpeed, buyPower, buyMiner;
    public TextMeshProUGUI speedInfo, powerInfo, minerInfo, bitInfo;
    public BigInteger miners = 1;
    public BigInteger bits = 0;
    public BigInteger BasePower = 1;
    public BigInteger BaseSpeed = 1;
    public BigInteger power = 1;
    public BigInteger speed {
        get => BaseSpeed;
        set => BaseSpeed = value;
    }
    public BigInteger bp {
        get => BasePower;
        set {BasePower = value; power = BasePower*miners;}
    }
    public BigInteger Miners {
        get => miners;
        set => miners = value;
    }
    public BigInteger addMiners {
        get => miners;
        set => miners += value;
    }

    double Progress = 0;
    int[] levels = {0, 0};
    int[] lLevels = {0, 0};
    public int[] Levels {
        get => levels;
        set {
            levels = value;
            refreshLevels();
        }
    }
    public void refreshLevels()
    {
        if (levels[0] != lLevels[0])
            BasePower = BigInteger.Pow(2, levels[0]);
        if (levels[1] != lLevels[1])
            BaseSpeed = BigInteger.Pow(2, levels[1]);
        lLevels = new int[2] {levels[0], levels[1]};
    }
    public void refreshUpgrades() {

    }
    double progress
    {
        get => Progress;
        set {Progress = value; bar.progress = (float)Progress;}
    }
    public ProgressBar bar;
    void OnEnable()
    {
        getPrefabVariables();

    }
    public void Start()
    {
        buySpeed.onClick.AddListener(() => {if (PowerSpeedCost(Levels[1]) >= bits ) Levels[1]++;});
        buyPower.onClick.AddListener(() => {if (PowerSpeedCost(Levels[0]) >= bits ) Levels[0]++;});
        //                                                                      custom set accessor is +=
        buyMiner.onClick.AddListener(() => {if (PowerSpeedCost(Levels[0]) >= bits ) addMiners = 1;});
    }
    public override void onUpdate()
    {
        int fps = (int)math.ceil(1/Time.deltaTime);
        if ((speed / fps) >= 1)
            bits += power * (speed / fps);
        progress += (double)(speed % fps)/fps;
        if (progress >= 1)
            bits += power;
    }
    public override void onFixedUpdate()
    {
        speedInfo.text  = bih.ToString(BaseSpeed);
        powerInfo.text  = bih.ToString(BasePower);
        minerInfo.text  = bih.ToString(miners);
        bitInfo.text    = bih.ToString(bits);
    }
    // calculation Functions
    /// <summary>
    /// Power and Speed cost function, separate for easy changing
    /// </summary>
    /// <param name="i">level</param>
    /// <returns>2^i as a BigInteger</returns>
    public BigInteger pscFunc(int i)
    {
        return BigInteger.Pow(1000, i);
    }
    /// <summary>
    /// Miner cost function, separate for easy changing
    /// </summary>
    /// <param name="i">level</param>
    /// <returns>1000^i as a BigInteger if i > 2, otherwise i == 1 ? 50 : 100 </returns>
    public BigInteger mcFunc(int i)
    {
        if (i == 1)
            return 50;
        if (i == 2)
            return 100;
        return BigInteger.Pow(1000, i);
    }
    public BigInteger PowerSpeedCost (int level, int levels = 1) {
        return CostFunc(level, pscFunc, levels);
    }
    public BigInteger MinerCost(int level, int levels = 1) {
        return CostFunc(level, mcFunc, levels);
    }
    public BigInteger CostFunc(int level, Func<int, BigInteger> cFunc, int levels)
    {
        if (levels == 1)
            return cFunc(level+1);
        BigInteger cost = cFunc(level + 1);
        if (levels == -1)
        {
            int i = 2;
            while (cost < bits)
            {
                cost += cFunc(level+i++);
            }
            return cost - cFunc(level + (--i));
        }
        for (int i = 2; i < levels; i++) {
            cost += cFunc(level + i);
        }
        return cost;
    }
}

[CustomEditor(typeof(BitMinerWindow))]
public class BitMinerWindowWindowEditor : WindowEditorTemplate
{
    public override void CustomGUI(SerializedObject obj)
    {
        //custom editor stuff here, everything else is automated
        BitMinerWindow window = (BitMinerWindow)obj.targetObject;
        EditorGUILayout.BeginVertical();
        window.buySpeed = (Button)EditorGUILayout.ObjectField(label:"Speed Buy Button", window.buySpeed, typeof(Button), allowSceneObjects:true);
        window.buyPower = (Button)EditorGUILayout.ObjectField(label:"Power Buy Button", window.buyPower, typeof(Button), allowSceneObjects:true);
        window.buyMiner = (Button)EditorGUILayout.ObjectField(label:"Miner Buy Button", window.buyMiner, typeof(Button), allowSceneObjects:true);
        window.speedInfo = (TextMeshProUGUI)EditorGUILayout.ObjectField(label:"Speed info text", window.speedInfo, typeof(TextMeshProUGUI), allowSceneObjects:true);
        window.powerInfo = (TextMeshProUGUI)EditorGUILayout.ObjectField(label:"Power info text", window.powerInfo, typeof(TextMeshProUGUI), allowSceneObjects:true);
        window.minerInfo = (TextMeshProUGUI)EditorGUILayout.ObjectField(label:"Miner info text", window.minerInfo, typeof(TextMeshProUGUI), allowSceneObjects:true);
        window.bitInfo = (TextMeshProUGUI)EditorGUILayout.ObjectField(label:"Bits info text", window.bitInfo, typeof(TextMeshProUGUI), allowSceneObjects:true);
        EditorGUILayout.IntField(label: "Bits: ", (int)window.bits);
        EditorGUILayout.IntField(label: "Miners: ", (int)window.miners);
        EditorGUILayout.IntField(label: "Power: ", (int)window.BasePower);
        EditorGUILayout.IntField(label: "speed: ", (int)window.BaseSpeed);
        EditorGUILayout.EndVertical();
    }
}
public static class BigIntegerHelp
{
    public static string ToString(BigInteger a)
    {
        if (a < 100000)
            return a.ToString();
        long log1000 = (long)BigInteger.Log(a, 1000);
        BigInteger b = a / BigInteger.Pow(1000, (int)log1000);
        string str = b.ToString();
        int log10b = (int)BigInteger.Log10(b);
        int digitsTd = 3 - log10b;
        if (digitsTd > 0)
            for (int i = 0; i < digitsTd; i++)
                str += "." + (a / (BigInteger.Pow(1000, (int)log1000) / BigInteger.Pow(10, i + 1)));
        return str + getSuffix(log1000);
    }
}
