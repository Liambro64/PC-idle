using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;

public class ShopWindow : Window
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Window thus;
    void Start()
    {

    }

    // Update is called once per frame

}

[CustomEditor(typeof(ShopWindow))]
public class ShopWindowEditor : WindowEditorTemplate
{
    public override void CustomGUI()
    {
        //custom editor stuff here, everything else is automated

    }
}