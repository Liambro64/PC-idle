
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using UnityEditor.MemoryProfiler;
using UnityEngine.iOS;
using Mono.Cecil;
using UnityEditor.Build.Content;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;

[CustomEditor(typeof(InventoryController))]
public class InventoryEditor : Editor
{
	InventoryController icObj;
	PartInventory pi;

	SerializedProperty sInv;
	SerializedProperty slots;
	public void OnEnable()
	{

		icObj = serializedObject.targetObject as InventoryController;
		sInv = serializedObject.FindProperty("inv");
		pi = (PartInventory)sInv.managedReferenceValue;
		slots = sInv.FindPropertyRelative("slot");
		//Debug.Log(str);
		//Debug.Log(describe(slots));


	}
	string describe(SerializedProperty prop, bool child = false)
	{
		string description =
			prop.propertyPath + " is called " +
			prop.name + " and is of type " +
			prop.type;
		if (prop.isArray)
		{
			description += "\n" + prop.name + " is an array " + prop.arraySize + " big of type " + prop.arrayElementType;
		}
		return description;
	}
	void UpdateValues()
	{
		icObj = serializedObject.targetObject as InventoryController;
		pi = icObj.inv;
	}

	void SetValues()
	{
		icObj.inv = pi;
		serializedObject.FindProperty("inv").managedReferenceValue = pi;
		serializedObject.ApplyModifiedProperties();
		SaveChanges();
	}


	enum types { Motherboard, CPU, GPU, RAM, Storage, Fans }
	enum rarities { Common, Uncommon, Rare, Epic, Crammed, Quantumn }
	List<bool> partsOpen = new List<bool>();

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		serializedObject.UpdateIfRequiredOrScript();
		UpdateValues();
		//this.DrawDefaultInspector();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.ObjectField(serializedObject.FindProperty("gm"), label: new GUIContent("Game Manager"));
		EditorGUILayout.ObjectField(serializedObject.FindProperty("inventoyPrefab"), label: new GUIContent("Inventory Spot Prefab"));
		EditorGUILayout.ObjectField(serializedObject.FindProperty("inventoryInUI"), label: new GUIContent("Inventory Spot Parent"));
		EditorGUILayout.ObjectField(serializedObject.FindProperty("partInteractor"), label: new GUIContent("Part Interactor"));
		pi.Space = EditorGUILayout.IntField(value: pi.Space, label: "Space");
		EditorGUILayout.LabelField("Inventory");
		EditorGUI.indentLevel += 2;
		for (int i = 0; i < pi.Space; i++)
		{
			if (partsOpen.Count <= i)
				partsOpen.Add(false);
			if (GUILayout.Button(content: new GUIContent((partsOpen[i] ? "close " : "open ") + "part " + i), GUILayout.ExpandWidth(true), GUILayout.Width(100)))
				partsOpen[i] = !partsOpen[i];
			if (partsOpen[i])
			{
				if (pi[i].Empty || pi[i][null] == null)
				{
					if (GUILayout.Button(content: new GUIContent("Add Part"), GUILayout.Width(65)))
						pi.addGeneric(i);
				}
				else
				{
					PartItem item = pi[i][null];
					EditorGUILayout.BeginHorizontal();
					item.Type = (int)(types)EditorGUILayout.EnumPopup((types)item.Type);
					item.Rarity = (int)(rarities)EditorGUILayout.EnumPopup((rarities)item.Rarity);
					EditorGUILayout.EndHorizontal();
					if (GUILayout.Button(new GUIContent("Make mutlis from type")))
						item.makeMultipliers();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Multiplier 1", GUILayout.MaxWidth(100));
					Multiplier M = item.firstMultiplier;
					if (M == null)
					{
						if (GUILayout.Button(content: new GUIContent("Add Multiplier")))
							item.firstMultiplier = new Multiplier(0, 0, "", "");
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						if (GUILayout.Button(content: new GUIContent("Remove Multiplier"), GUILayout.MaxWidth(115)))
							item.firstMultiplier = null;
						M.type = (Multiplier.multiTypes)EditorGUILayout.EnumPopup(M.type, GUILayout.Width(105));
						M.For = (Multiplier.what)EditorGUILayout.EnumPopup(M.For, GUILayout.Width(105));
						EditorGUILayout.EndHorizontal();
						if (M.type == Multiplier.multiTypes.additive)
							M.group = (Multiplier.addGroups)EditorGUILayout.EnumPopup(M.group);
						M.equation = EditorGUILayout.TextField(text: M.equation, label: new GUIContent("Equation")	);
						M.randomVal = EditorGUILayout.DoubleField(value: M.randomVal, label: new GUIContent("random val (~1 in equation)"));
						if (GUILayout.Button("Calculate Value"))
							M.Calculate(new object[] {item.Rarity, M.randomVal});
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Value: " + M.value);
						M.active = EditorGUILayout.Toggle(new GUIContent("active"), M.active);
						EditorGUILayout.EndHorizontal();
					}
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Multiplier 2", GUILayout.MaxWidth(100));
					M = item.secondMultiplier;
					if (M != null)
					{
						if (GUILayout.Button(content: new GUIContent("Remove Multiplier"), GUILayout.MaxWidth(115)))
							item.secondMultiplier = null;
						M.type = (Multiplier.multiTypes)EditorGUILayout.EnumPopup(M.type, GUILayout.Width(105));
						M.For = (Multiplier.what)EditorGUILayout.EnumPopup(M.For, GUILayout.Width(105));
						EditorGUILayout.EndHorizontal();
						if (M.type == Multiplier.multiTypes.additive)
							M.group = (Multiplier.addGroups)EditorGUILayout.EnumPopup(M.group);
						M.equation = EditorGUILayout.TextField(text: M.equation, label: new GUIContent("Equation"));
						M.randomVal = EditorGUILayout.DoubleField(value: M.randomVal, label: new GUIContent("random val (~1 in equation)"));
						if (GUILayout.Button("Calculate Value"))
							M.Calculate(new object[] { (double)item.Rarity, M.randomVal });
						EditorGUILayout.LabelField("Value: " + M.value);
					}
					else
					{
						if (GUILayout.Button(content: new GUIContent("Add Multiplier")))
							item.secondMultiplier = new Multiplier(0, 0, "", "");
						EditorGUILayout.EndHorizontal();
					}
					GUIStyle style = new GUIStyle();
					if (GUILayout.Button(content: new GUIContent("RemovePart")))
						pi.Remove(i);
					//has an item
				}
			}
		}
		EditorGUILayout.LinkButton("i am the link button");
		if (EditorGUI.EndChangeCheck())
			SetValues();
		EditorGUILayout.EndVertical();	
	}

}
#endif