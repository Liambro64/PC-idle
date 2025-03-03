#if UNITY_EDITOR
using System;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

[ExecuteInEditMode]
[CustomPropertyDrawer(typeof(Multiplier))]
public class MultiplierEditor : PropertyDrawer
{
    static bool foldout = false;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        Multiplier.multiTypes multiType = (Multiplier.multiTypes)property.FindPropertyRelative("type").enumValueIndex;
        Multiplier.what multiWhat = (Multiplier.what)property.FindPropertyRelative("For").enumValueIndex;
        Multiplier.addGroups groups = (Multiplier.addGroups)property.FindPropertyRelative("group").enumValueIndex;
        string equation = property.FindPropertyRelative("equation").stringValue;
        bool active = property.FindPropertyRelative("active").boolValue;
        double value = property.FindPropertyRelative("value").doubleValue;
        foldout = EditorGUILayout.Foldout(foldout, new GUIContent("Mutliplier " + property.FindPropertyRelative("name").stringValue));
        if (foldout)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            multiType = (Multiplier.multiTypes)EditorGUILayout.EnumPopup(multiType);
            multiWhat = (Multiplier.what)EditorGUILayout.EnumPopup(multiWhat);
            EditorGUILayout.EndHorizontal();
            if (multiType == Multiplier.multiTypes.additive)
                groups = (Multiplier.addGroups)EditorGUILayout.EnumPopup(groups);
            EditorGUILayout.BeginHorizontal();
            equation = EditorGUILayout.TextField("Equation", equation);
            active = EditorGUILayout.Toggle(new GUIContent("Active"), active);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button(new GUIContent("Calculate Value")))
            {
                if (property.serializedObject.targetObject.IsConvertibleTo(typeof(Upgrade), false))
                    ((Upgrade)property.serializedObject.targetObject).Calculate();
            }
            EditorGUILayout.LabelField(new GUIContent("Value:\t" + value));


            EditorGUI.indentLevel--;
        }
        property.FindPropertyRelative("type").enumValueIndex = (int)multiType;
        property.FindPropertyRelative("For").enumValueIndex = (int)multiWhat;
        property.FindPropertyRelative("group").enumValueIndex = (int)groups;
        property.FindPropertyRelative("equation").stringValue = equation;
        property.FindPropertyRelative("active").boolValue = active;
        property.serializedObject.ApplyModifiedProperties();
        //base.OnGUI(position, property, label);
    }

    // public override void OnGUI()
    // {

    // 	M.type = (Multiplier.multiTypes)EditorGUILayout.EnumPopup(M.type, GUILayout.Width(105));
    // 	M.For = (Multiplier.what)EditorGUILayout.EnumPopup(M.For, GUILayout.Width(105));
    // 	EditorGUILayout.EndHorizontal();
    // 	if (M.type == Multiplier.multiTypes.additive)
    // 		M.group = (Multiplier.addGroups)EditorGUILayout.EnumPopup(M.group);
    // 	M.equation = EditorGUILayout.TextField(text: M.equation, label: new GUIContent("Equation"));
    // 	M.randomVal = EditorGUILayout.DoubleField(value: M.randomVal, label: new GUIContent("random val (~1 in equation)"));
    // 	if (GUILayout.Button("Calculate Value"))
    // 		M.Calculate(new double[] { item.Rarity, M.randomVal });
    // 	EditorGUILayout.BeginHorizontal();
    // 	EditorGUILayout.LabelField("Value: " + M.value);
    // 	M.active = EditorGUILayout.Toggle(new GUIContent("active"), M.active);
    // 	EditorGUILayout.EndHorizontal();
    // }
}
#endif