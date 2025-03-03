using System;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using wm = WindowManager;

public class Shortcut : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Image sp;
    TextMeshProUGUI text;
    Button button;
    RectTransform rt;
    public Sprite sprite{
        get => ((sp == null) ? getsp() : sp).sprite;
        set => ((sp == null) ? getsp() : sp).sprite = value;
    }
    public Image getsp() {
        return sp = gameObject.GetComponent<Image>();
    }
    public string Name {
        get => ((text == null) ? getText() : text).text;
        set => ((text == null) ? getText() : text).text = value;
    }
    public TextMeshProUGUI getText() {
        return text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    public event Action onPress{
        add     => ((button == null) ? getButton() : button).onClick.AddListener(() => value());
        remove  => ((button == null) ? getButton() : button).onClick.RemoveListener(() => value());
    }
    public Button getButton() {
        return button = gameObject.GetComponent<Button>();
    }
    public GameObject window;
    public string windowName{
        get => window.name;
    }
    void OnEnable()
    {
        button = gameObject.GetComponent<Button>();
        sp = gameObject.GetComponent<Image>();
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
        onPress += () => {
            wm.Open(windowName, window);
        };
    }
    public virtual Type windowType() {
        return typeof(Window);
    }
    void Start()
    {
        DesktopManager.Add(gameObject);
        rt.localPosition = DesktopManager.pos(DesktopManager.Icons.Count - 1);
    }
}

[CustomEditor(typeof(Shortcut))]
public class ShortcutEditor : Editor
{
    public UnityEngine.Object obj;
    public override void OnInspectorGUI()
    {
        Shortcut shortcut = (Shortcut)serializedObject.targetObject;
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();
        shortcut.window = (GameObject)  EditorGUILayout.ObjectField (label:"Window: ", shortcut.window, typeof(GameObject),    allowSceneObjects:true);
        shortcut.sprite = (Sprite)      EditorGUILayout.ObjectField (label:"Sprite: ", shortcut.sprite, typeof(Sprite),        allowSceneObjects:true);
        shortcut.Name =                 EditorGUILayout.TextField   (label:"Name: ", text:shortcut.Name);
        EditorGUILayout.ObjectField(label:"Window Type: ", shortcut.window.GetComponent<Window>(), typeof(Window), allowSceneObjects:false);
        EditorGUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}