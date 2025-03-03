using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
public class FormatItem : MonoBehaviour
{
    private PartItem item;
    public PartItem Item
    {
        get => item;
        set { item = value; Format(); }
    }
    string[] Names = { "Motherboard", "CPU", "GPU", "RAM", "Storage", "Fans" };
    public Sprite img;
    [Header("Prefab Variables")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Multis;
    public Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Format()
    {
        if (item != null && item.Type != -1)
        {
            image.color = new Color(1, 1, 1, 1);
            Name.text = Names[item.Type];
            image.sprite = img;
            string multiString = "";
            if (item.firstMultiplier != null)
            {
                Multiplier M = item.firstMultiplier;
                multiString += M.ToString();
            }
            if (item.secondMultiplier != null)
            {
                Multiplier M = item.secondMultiplier;
                multiString += "\n" + M.ToString();
            }
            Multis.text = multiString;
        }
        else
        {
            item = null;
            Name.text = "";
            Multis.text = "";
            image.color = new Color(0, 0, 0, 0);
            image.sprite = null;
        }
    }
    public void SetPos(Vector2 pos)
    {
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(pos.x, pos.y);
    }
    public void delete()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
