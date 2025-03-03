using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using BigInteger = System.Numerics.BigInteger;
using System;

public class ShopWindow : Window
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Window thus;

    public List<GameObject> ShopIcons = new List<GameObject>();
    public List<GameObject> ShopPrefabs = new List<GameObject>();
    public int shopState = 0;
    public BigInteger shopMoney = 5;
    public ShopDescriptor sd;

    public float iconSize = 100;
    public float iconSpacing = 25;

    void OnEnable()
    {
        onSizeChange += (a) => refreshUI();
    }
    T CastObject<T>(object input) {   
        return (T) input;   
    }
    public T ConvertObject<T>(object input) {
        return (T) Convert.ChangeType(input, typeof(T));
    }
    void Start()
    {
        refreshUI();
        for (int i = 0; i < ShopIcons.Count; i++)
        {
            var item = ShopIcons[i].GetComponent<ShopItem>();
            if (item.dv != null)
                item.OnPress += () =>
                {
                    sd.Show(item.dv);
                };
        }
    }
    BigInteger[] costs = { 5, 50, 625, 15625, 5000000 };
    public override void onFixedUpdate()
    {
        refreshUI();
    }

    public void refreshUI()
    {
        switch (shopState)
        {
            case 0:
                for (int i = 0; i < ShopPrefabs.Count; i++)
                {
                    if (ShopIcons.Count <= i)
                        ShopIcons.Add(Instantiate(ShopPrefabs[i], Viewport));
                    else if (ShopIcons[i] == null)
                        ShopIcons[i] = Instantiate(ShopPrefabs[i], Viewport);
                    ShopIcons[i].GetComponent<RectTransform>().localPosition = SS0Pos(i);
                }
                //do stuff
                break;
        }
    }
    public Vector2 SS0Pos(int i)
    {
        int inLine = (int)(size.x - iconSpacing) / (int)(iconSize + iconSpacing);
        return new Vector2(
            25 + 125 * (i % inLine),
            -(25 + 125 * (i / inLine))
        );

    }
    public Vector3 SS0Pos3(int i)
    {
        int inLine = (int)(size.x - iconSpacing) / (int)(iconSize + iconSpacing);
        return new Vector2(
            25 + 125 * (i % inLine),
            25 + 125 * (i / inLine)
        );

    }
    // Update is called once per frame

}

[CustomEditor(typeof(ShopWindow))]
public class ShopWindowEditor : WindowEditorTemplate
{

    public override void CustomGUI(SerializedObject obj)
    {
        //custom editor stuff here, everything else is automated
        ShopWindow window = (ShopWindow)obj.targetObject;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(property: obj.FindProperty("ShopPrefabs"), label: new GUIContent("Shop Prefabs"));
        EditorGUILayout.PropertyField(property: obj.FindProperty("ShopIcons"), label: new GUIContent("Shop Icons"));
        window.sd = (ShopDescriptor)EditorGUILayout.ObjectField(label: "Descriptor: ", window.sd, typeof(ShopDescriptor), allowSceneObjects: true);
        window.shopState = EditorGUILayout.IntField("Shop state: ", window.shopState);
        EditorGUILayout.EndVertical();
    }
}