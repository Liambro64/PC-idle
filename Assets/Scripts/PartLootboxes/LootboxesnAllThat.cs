using System.Data;
using System.Numerics;
using TMPro;
using UnityEngine;

public class LootboxesnAllThat : MonoBehaviour
{

    //public variables
    public TextMeshProUGUI SpecialisedLootboxName;
    public TextMeshProUGUI SpecialisedLootboxStats;
    public TextMeshProUGUI RandomLootboxStats;
    public GameManager gameManager;
    public Auctions auctionManager;
    public InventoryController inv;

    //class variables
    public static string[] names = new string[] { "Motherboard", "CPU", "GPU", "RAM", "Storage", "Fans", "Random" };
    int specialisation = -1;
    int box = -1;
    Lootbox[] RandomLootboxes = new Lootbox[] {
        new Lootbox(new double[] {8, 5, 3}),
        new Lootbox(new double[] {8, 7, 5, 3, 1}),
        new Lootbox(new double[] {5, 6, 8, 5, 2}),
        new Lootbox(new double[] {3, 4, 6, 7, 5}),
        new Lootbox(new double[] {3, 4, 6, 8, 7, 0.1d}),
        new Lootbox(new double[] {0, 2, 5, 20, 10, 1, 0.1d})
    };
    BigInteger[] RandLBcosts = { 50, 200, 500, 2000, 10000, 50000 };
    Lootbox[][] specialised = new Lootbox[6][];
    BigInteger[] SpecLBcosts = { 82, 275, 56200, 2750000, 1375000000, 62500000000 };
    Lootbox[] makeLootBoxes(int spec)
    {
        return new Lootbox[] {
            new Lootbox((Lootbox.specialisation)spec, new double[] {8, 5, 3}),
            new Lootbox((Lootbox.specialisation)spec, new double[] {8, 7, 5, 3, 1}),
            new Lootbox((Lootbox.specialisation)spec, new double[] {5, 6, 8, 5, 2}),
            new Lootbox((Lootbox.specialisation)spec, new double[] {3, 4, 6, 7, 5}),
            new Lootbox((Lootbox.specialisation)spec, new double[] {3, 4, 6, 8, 7, 0.1d}),
            new Lootbox((Lootbox.specialisation)spec, new double[] {0, 2, 5, 20, 10, 1, 0.1d})
        };
    }
    public void setSpecialisation(int num)
    {
        specialisation = num;
        if (specialisation != 7)
            SpecialisedLootboxName.text = names[num];
    }
    public void SelectBoxSP(int num)
    {
        double[] db = specialised[specialisation][num].chances;
        SpecialisedLootboxStats.text = "</color=#000000ff>Common: " + db[0].ToString("0.00") + "</color>\t</color=ffdd00ff>uncommon: " + db[1].ToString("0.00") + "</color>\t</color=7a82ee>Rare: " + db[2].ToString("0.00")
                                        + "</color>\n</color=713225>Epic: " + db[3].ToString("0.00") + "</color>\t</color=35342f>Crammed:" + db[4].ToString("0.00") + "</color>\t</color=ff0800>Quantum:" + db[5].ToString("0.00") + "</color>";
        box = num;
    }
    public void SelectBoxR(int num)
    {
        double[] db = RandomLootboxes[num].chances;
        RandomLootboxStats.text = "Common: " + db[0].ToString("0.00") + "    uncommon: " + db[1].ToString("0.00") + "       Rare: " + db[2].ToString("0.00")
                                        + "\nEpic: " + db[3].ToString("0.00") + "     Crammed:" + db[4].ToString("0.00") + "      Quantum:" + db[5].ToString("0.00");
        box = num;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < specialised.Length; i++)
            specialised[i] = makeLootBoxes(i + 1);
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void buy()
    {
        // print("Specialisation: "+specialisation+"\nBox: "+box+
        // "\nCost: "+(specialisation == 7 ? RandLBcosts[box] : SpecLBcosts[box])  
        // +"\nCash: "+auctionManager.cash+"\nCan afford? "+(auctionManager.cash >= SpecLBcosts[box]));
        if (specialisation == 7)
        {
            if (auctionManager.cash >= RandLBcosts[box])
            {
                if (inv.Add(RandomLootboxes[box].Open(gameManager)))
                    auctionManager.cash -= RandLBcosts[box];
            }
            else
                return;


        }
        else if (auctionManager.cash >= SpecLBcosts[box])
        {
            if (inv.Add(specialised[specialisation][box].Open(gameManager)))
                auctionManager.cash -= SpecLBcosts[box];
        }
        else
            return;



    }
    //ORGANIZE PARTS AS UI ELEMENTS IN INVENTORY
    //MAKE MODELS AND PUT IN PC (CAN BE VERY LATER)
}
