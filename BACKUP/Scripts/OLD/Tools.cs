using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem.Interactions;
using Unity.Mathematics;
using System;
using UnityEngine.Rendering;
using System.Numerics;
using Unity.Entities.UniversalDelegates;
using Vector3 = UnityEngine.Vector3;

public static class Tools
{
    public static List<string> suffixes = new List<string>();
    public static void makeSuffixes()
    {
        string path = Application.dataPath + "/Text Files/Suffixes.txt";
        StreamReader reader = new StreamReader(path);
        for (int i = 0; i < 103; i++)
        {
            suffixes.Add(reader.ReadLine());
        }
    }
    public static void areSuffixesMade()
    {
        if (suffixes.Count == 0)
            makeSuffixes();
    }
    public static string makeSuffixedNumber(BigInteger num)
    {
        int log = (int)BigInteger.Log(num, 1000);
        if (log >= 301)
            return num.ToString();
        if (log >= 1)

        {
            num /= (BigInteger)Math.Pow(1000, log);
            string number = num.ToString();
            number = number.Substring(0, Math.Min(number.Length, 4));
            return number + suffixes[(int)log];
        }
        return num.ToString();
    }
    public static BigInteger makeProgramCost(int i)
    {
        return (BigInteger)Math.Pow(5, i) - 5;
    }
    public static Vector3 makeButtonPosition(int num)
    {
        float x = 37 + 210 * (num / 2);
        float y = -98 + -210 * (num % 2);
        return new Vector3(x, y);
    }


    public static Vector3 makeProgramPosition(int num)
    {
        float x = 15 + 210 * (num / 2);
        float y = -15 + -210 * (num % 2);
        return new Vector3(x, y);
    }
    public static GameManager GetGameManager() { return GameObject.Find("Game Manager").GetComponent<GameManager>(); }
}
