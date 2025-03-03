using System;
using System.Buffers.Text;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Numerics;

public class Save
{
    public string Path;
    public GameManager g;
    string nl = "~";
    string inl = "-";

    public string SaveString
    {
        get => makeSaveString();
    }
    public string SaveHash
    {
        get => getHash(makeSaveString());
    }
    public Save(string Path, string FileName, GameManager gameManager)
    {
        this.Path = Path + FileName;
        this.g = gameManager;
    }
    public Save(string Path, GameManager gameManager)
    {
        this.Path = Path;
        this.g = gameManager;
    }
    public string getGameManager()
    {
        string GMVal = "BloopyShmoopy" + nl;
        GMVal += g.bits + ";" + g.cpp;
        return GMVal + nl;
    }
    public string getPrograms()
    {
        string str = "PRGMS" + inl;
        for (int i = 0; i < g.programs.Count; i++)
            str += "PRGM;" + i + ";" + (g.programs[i].name == "​" ? "null" : g.programs[i].name) + ";" + g.programs[i].level[0] + ":" + g.programs[i].level[1] + inl;
        return str + nl;
    }
    public string getUpgrades()
    {
        string str = "UPGS" + inl;
        for (int i = 0; i < g.upg.upgrades.Count; i++)
        {
            Upgrade Upg = g.upg.upgrades[i];
            str += "UPG" + i + ";" + Upg.Level + inl;
        }
        return str + nl;
    }
    public string getAuctions()
    {
        string str = "AUC" + inl;
        str += g.auc.cash + ";" + g.auc.sellButtonPressedOnce;
        return str + nl;
    }
    public string getInventory()
    {
        string str = "INV" + inl;
        PartInventory inv = g.LBNallat.inv.inv;
        for (int i = 0; i < inv.Count; i++)
        {
            str += inv[i].DescribeFS() + inl;
        }
        return str + nl;
    }
    public string makeSaveString()
    {
        string SaveString = getGameManager();
        SaveString += getPrograms();
        SaveString += getUpgrades();
        SaveString += getAuctions();
        SaveString += getInventory();
        return SaveString;
    }
    public bool Create(string path = "")
    {
        if (path == "")
            path = Path;
        else
            Path = path;
        try
        {
            if (File.Exists(path))
                File.Delete(path);

            FileStream fs = new FileStream(Path, FileMode.CreateNew, FileAccess.ReadWrite);
            fs.Close();
            string str = makeSaveString();
            StreamWriter sw = new StreamWriter(Path, true);
            sw.WriteLine(str);
            sw.WriteLine(getHash(str));
            sw.Close();
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(path);
            Debug.Log(ex.StackTrace);
            return false;
        }
    }

    string getHash(string str)
    {
        return Convert.ToBase64String(SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(str)));
    }

}

public class Load
{
    public string Path;
    public GameManager g;
    string nl = "~";
    string inl = "-";

    public Load(string Path, GameManager g)
    {
        this.Path = Path;
        this.g = g;
    }
    public Load(Save sav)
    {
        Path = sav.Path;
        g = sav.g;
    }
    string getHash(string str)
    {
        return Convert.ToBase64String(SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(str)));
    }
    public bool LoadSave()
    {
        string[] str = File.ReadAllLines(Path);
        string Save = str[0];
        string Hash = str[1];
        //if (getHash(Save) != Hash)
        //    return false;
        try
        {

            string[] parts = Save.Split(nl[0]);
            if (parts.Length > 1)
                getGM(parts[1]);
            if (parts.Length > 2)
                getPrograms(parts[2]);
            if (parts.Length > 3)
                getUpgrades(parts[3]);
            if (parts.Length > 4)
                getAuctions(parts[4]);
            if (parts.Length > 5)
                getInventory(parts[5]);
            g.refreshMultipliers();
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message);
            return false;
        }
    }
    //BloopyShmoopy~29.42231501;2~PRGMS-PRGM;0;​:0;0-PRGM;1;​:0;0-~UPGS-UPG0;Upgrades(Clone):4-UPG1;Upgrades(Clone):0-UPG2;Upgrades(Clone):1-UPG3;Upgrades(Clone):
    //0-UPG4;Upgrades(Clone):0-UPG5;Upgrades(Clone):0-UPG6;Tab Upgrade(Clone):0-UPG7;Upgrades(Clone):0-~AUC-0~INV-null-null-null-null-null-null-null-null-~
    public void getGM(string part)
    {
        string[] parts = part.Split(';');
        g.bits = BigInteger.Parse(parts[0]);
        g.cpp = int.Parse(parts[1]);
        //g.FixedUpdate();
    }
    public void getPrograms(string part)
    {
        string[] parts = part.Split(inl[0]);
        for (int i = 1; i < parts.Length - 1; i++)
        {
            string[] parts2 = parts[i].Split(';');
            string[] parts3 = parts2[3].Split(':');
            if (g.programs.Count <= i - 1)
                g.makeProgram(i);
            g.programs[i - 1].Level = new BigInteger[] { BigInteger.Parse(parts3[0]), BigInteger.Parse(parts3[1]) };
            g.programs[i - 1].name = parts2[2];
        }
        for (int i = 0; i < g.buyButtons.Count; i++)
        {
            GameObject go = g.buyButtons[0];
            g.buyButtons.Remove(go);

            MonoBehaviour.Destroy(go);
        }
        for (int i = 0; i < g.maxPrograms - g.programs.Count; i++)
            g.makeButton(i + g.programs.Count + 1);
    }
    //UPGS-UPG0;Upgrades(Clone):8-UPG1;Upgrades(Clone):1-UPG2;Upgrades(Clone):1-UPG3;Upgrades(Clone):
    //1-UPG4;Upgrades(Clone):0-UPG5;Upgrades(Clone):0-UPG6;Tab Upgrade(Clone):0-UPG7;Upgrades(Clone):0-
    public void getUpgrades(string part)
    {
        string[] parts = part.Split('-');
        //  g.upg.resetUpgs();
        for (int i = 1; i < parts.Length - 1; i++)
        {
            string[] parts1 = parts[i].Split(';');
            g.upg.upgrades[i - 1].Level = int.Parse(parts1[1]);
        }
    }
    public void getAuctions(string part)
    {
        string[] parts = part.Split('-');
        string[] parts2 = parts[1].Split(";");
        g.auc.cash = BigInteger.Parse(parts2[0]);
        bool b = false;

        if (parts2.Length > 1 && bool.TryParse(parts2[1], out b))
            g.auc.sellButtonPressedOnce = b;
        g.auc.sellButton.GetComponent<Button>().onClick.RemoveAllListeners();
        if (b == true)
        {
            g.auc.makeButtonReset();
        }
        else
        {
            g.auc.makeButtonSell();
        }

    }
    // active+";"+type+";"+rarity+";"+stat1.randomVal+";"+stat2.randomVal;
    public void getInventory(string part)
    {
        string[] parts = part.Split('-');
        for (int i = 1; i < parts.Length - 1; i++)
        {
            if (parts[i] != "null")
            {
                string[] parts2 = parts[i].Split(';');
                PartItem item = g.LBNallat.inv.inv.setGeneric(i - 1);
                item.Active = bool.Parse(parts2[0]);
                item.Type = int.Parse(parts2[1]);
                item.Rarity = int.Parse(parts2[2]);
                if (item.Active)
                    g.LBNallat.inv.setEquipped(item);
                item.makeMultipliers();
                if (parts2[3] != "null" && item.firstMultiplier != null)

                    item.firstMultiplier.randomVal = double.Parse(parts2[3]);
                if (parts2[4] != "null" && item.secondMultiplier != null)
                    item.secondMultiplier.randomVal = double.Parse(parts2[4]);
                item.Calculate();
            }
            else
            {
                g.LBNallat.inv.inv[i - 1].Remove();
            }
        }
    }
}
