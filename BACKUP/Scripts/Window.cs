using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using Unity.Entities.UniversalDelegates;
using UnityEditor.ShaderGraph.Internal;
using System.Data.Common;
using System.Drawing;

public class Window : MonoBehaviour
{
    //prefab variables
    public static float xUnitsPerWidth = 1920;
    public static float yUnitsPerHeight = 1080;

    public TextMeshProUGUI NameText;
    public RectTransform Top;
    public RectTransform Viewport;
    public RectTransform rt;

    //control variables				  Focus, Maxed
    private Bits State = new Bits(2) { false, false };
    public Bits state
    {
        get => State;
        set { state = value; }
    }
    private Vector2 Size = new Vector2(512, 512);
    //											-4 , -28
    readonly Vector2 edge = new(4, 28);
    public Vector2 viewportSize
    {
        get => size - edge;
        set => size = value + edge;
    }
    void OnEnable()
    {
        rt = GetComponent<RectTransform>();
    }
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    public Vector2 MinSize(Vector2 vec)
    {
        if (vec.x < 256)
            vec.x = 256;
        if (vec.y < 256)
            vec.y = 256;
        return vec;
    }
    public Vector2 size
    {
        get => Size;
        set
        {
            Size = MinSize(value);
            (rt == null ? GetComponent<RectTransform>() : rt).sizeDelta = Size;
            Top.sizeDelta = new Vector2(Size.x, Top.sizeDelta.y);
        }
    }
    new string name = "Program Name";
    public string Name
    {
        get => name;
        set { name = value; NameText.text = name; }
    }
    public bool overTop = false;
    public void Update()
    {
        Vector3 MousePos = Input.mousePosition;
        MousePos.x *= xUnitsPerWidth;
        MousePos.y *= yUnitsPerHeight;
        Vector2 sides = new(5, 5);
        Vector2 realPos = new Vector2(rt.localPosition.x, 1080 - (-rt.localPosition.y) - size.y);
        Vector2 middle = realPos + (size / 2);
        if (Input.GetMouseButtonDown(0))
        {
            if (Within(MousePos, realPos, size) && !Within(MousePos, realPos + sides, size - (sides * 2)))
            {
                Vector2 dir = WithinSides(MousePos, middle, sides);
                StartCoroutine(scale(dir, MousePos));
                return;
            }
            if (overTop)
                StartCoroutine(Move(transform.position - Input.mousePosition));

        }
        makeView();
    }
    public IEnumerator scale(Vector2 dir, Vector2 ogPos)
    {
        print(dir);
        Vector2 MousePos;
        while (Input.GetMouseButton(0))
        {
            MousePos = Input.mousePosition;
            MousePos.x *= xUnitsPerWidth;
            MousePos.y *= yUnitsPerHeight;
            sizeChange(MousePos - ogPos, dir);
            ogPos = MousePos;
            yield return null;
        }
    }
    public bool Within(Vector2 pos, Vector2 start, Vector2 size)
    {
        if (start.x < pos.x && pos.x < (start.x + size.x) && start.y < pos.y && pos.y < (start.y + size.y))
            return true;
        return false;
    }
    public virtual void makeView()
    {

    }
    public void FixedUpdate()
    {
        makeFixedView();
    }
    public virtual void makeFixedView()
    {

    }
    public void Minimise()
    {
        if (State[0])
            State[0] = false;
        else
            state[0] = true;
    }
    public void Maximise()
    {
        if (State[1])
            State[1] = false;
        else
            state[1] = true;
    }
    public void Close()
    {
        //State = new (2) {true, false};
        Destroy(gameObject);
        print("close");
    }
    IEnumerator Move(Vector3 mouseOffset)
    {
        while (Input.GetMouseButton(0))
        {
            transform.position = Input.mousePosition + mouseOffset;
            yield return null;
        }
    }
    public Vector2 WithinSides(Vector2 Pos, Vector2 middle, Vector2 sides)
    {
        Vector2 relPos = Pos - middle;
        Vector2 _relPos = relPos;
        if (relPos.y < 0)
            relPos.y = -relPos.y;
        if (relPos.x < 0)
            relPos.x = -relPos.x;
        Vector2 size2 = size / 2;
        if (Mathf.Pow(Mathf.Abs(relPos.x) - size2.x, 2)
            + Mathf.Pow(Mathf.Abs(relPos.y) - size2.y, 2) < sides.sqrMagnitude / 2)
        {
            if (_relPos.x > 0)
            {
                if (_relPos.y > 0)
                    return new(1, 1);
                return new(1, -1);
            }
            if (_relPos.y > 0)
                return new(-1, 1);
            return new(-1, -1);
        }
        if (_relPos.x > Mathf.Abs(_relPos.y))
            return new(1, 0);
        if (_relPos.y > Mathf.Abs(_relPos.x))
            return new(0, 1);
        if (-_relPos.x > Mathf.Abs(_relPos.y))
            return new(-1, 0);
        return new(0, -1);
    }
    public void sizeChange(Vector2 add, Vector2 dir)
    {
        if (dir == Vector2.zero)
            return;
        //a
        if (dir.x != -1 && dir.y != 1)
        {
            //a1
            if (dir.x == 0)
                size = -add * Vector2.up + size;
            //a2
            else if (dir.y == 0)
                size = add * Vector2.right + size;
            //a3
            else
                size += new Vector2(add.x, -add.y);
        }
        //b
        else
        {
            //b1
            if (dir.y != 1)
            {
                if (dir.y == 0)
                    size -= add * Vector2.right;
                else
                    size -= add;
                rt.position += new Vector3(add.x/xUnitsPerWidth, 0);
            }
            //b2
            else if (dir.x != -1)
            {
                if (dir.x == 0)
                    size += add * Vector2.up;
                else
                    size += add;
                rt.position += new Vector3(0, add.y/yUnitsPerHeight);
            }
            //b3
            else
            {
                size += new Vector2(-add.x, add.y);
                rt.position += new Vector3(add.x/xUnitsPerWidth, add.y/yUnitsPerHeight);
            }
        }

    }

}



//OTHER STUFF




public class Bits : IEnumerable
{
    List<bool> bits = new List<bool>();
    public void Add(bool b)
    {
        bits.Add(b);
    }
    public int Length
    {
        get => bits.Count;
    }
    public int Count
    {
        get => bits.Count;
    }
    public bool this[int i]
    {
        get => bits[i];
        set => bits[i] = value;
    }
    public Bits(int size)
    {
        bits = new List<bool>();
    }
    public bool Equals(bool[] b)
    {
        for (int i = 0; i < Math.Min(Count, b.Length); i++)
            if (bits[i] != b[i])
                return false;
        return true;
    }
    public bool[] Convert()
    {
        bool[] bools = new bool[Count];
        for (int i = 0; i < Count; i++)
            bools[i] = bits[i];
        return bools;
    }
    public void Convert(bool[] b)
    {
        for (int i = 0; i < Math.Min(Count, b.Length); i++)
            bits[i] = b[i];
    }
    public IEnumerator GetEnumerator()
    {
        return bits.GetEnumerator();
    }

}

[CustomEditor(typeof(Window))]
public class WindowEditor : Editor
{
    readonly string[] States = new string[] { "Minimised", "Focused", "huh", "Maximised" };
    Window window;

    public override void OnInspectorGUI()
    {
        if (window != (Window)serializedObject.targetObject)
            window = (Window)serializedObject.targetObject;
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Prefab Variables");
        EditorGUI.indentLevel++;
        window.rt = (RectTransform)EditorGUILayout.ObjectField("Transform: ", window.rt, typeof(RectTransform), true);
        window.NameText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Program Name Obj: ", window.NameText, typeof(TextMeshProUGUI), true);
        window.Top = (RectTransform)EditorGUILayout.ObjectField("Top: ", window.Top, typeof(RectTransform), true);
        window.Viewport = (RectTransform)EditorGUILayout.ObjectField("Viewport: ", window.Viewport, typeof(RectTransform), true);
        EditorGUI.indentLevel--;
        //Control Variables
        EditorGUILayout.LabelField("Control Variables");
        //EditorGUI.indentLevel++;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("State");
        window.state[0] = EditorGUILayout.Toggle(window.state[0]);
        window.state[1] = EditorGUILayout.Toggle(window.state[1]);
        EditorGUILayout.LabelField("\t:\t" + States[BTI(window.state)]);
        EditorGUILayout.EndHorizontal();
        window.Name = EditorGUILayout.TextField("Name: ", window.Name);
        window.size = EditorGUILayout.Vector2Field("Size: ", window.size);
        //window.viewportSize = EditorGUILayout.Vector2Field("Viewport Size: ", window.viewportSize);
        //EditorGUI.indentLevel--;
        EditorGUILayout.Vector2Field("Viewport Size (read-only): ", window.viewportSize);
        EditorGUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            SaveChanges();
        }
        //repaint 
        if (EditorApplication.isPlaying)
            Repaint();
    }
    public static void MakeWindowGUI(Window window)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Prefab Variables");
        EditorGUI.indentLevel++;
        window.rt = (RectTransform)EditorGUILayout.ObjectField("Transform: ", window.rt, typeof(RectTransform), true);
        window.NameText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Program Name Obj: ", window.NameText, typeof(TextMeshProUGUI), true);
        window.Top = (RectTransform)EditorGUILayout.ObjectField("Top: ", window.Top, typeof(RectTransform), true);
        window.Viewport = (RectTransform)EditorGUILayout.ObjectField("Viewport: ", window.Viewport, typeof(RectTransform), true);
        EditorGUI.indentLevel--;
        //Control Variables
        EditorGUILayout.LabelField("Control Variables");
        //EditorGUI.indentLevel++;
        window.Name = EditorGUILayout.TextField("Name: ", window.Name);
        window.size = EditorGUILayout.Vector2Field("Size: ", window.size);
        //window.viewportSize = EditorGUILayout.Vector2Field("Viewport Size: ", window.viewportSize);
        //EditorGUI.indentLevel--;
        EditorGUILayout.Vector2Field("Viewport Size (read-only): ", window.viewportSize);
        EditorGUILayout.EndVertical();
    }

    int BTI(Bits b)
    {
        int num = 0;
        for (int i = 0; i < b.Length; i++)
        {
            num += b[i] == true ? (int)Mathf.Pow(2, i) : 0;
        }
        return num;
    }
}

public class WindowEditorTemplate : Editor
{
    public virtual Window Window {
        get => (Window)serializedObject.targetObject;
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        WindowEditor.MakeWindowGUI(Window);
        CustomGUI();
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            SaveChanges();
        }
        //repaint 
        if (EditorApplication.isPlaying)
            Repaint();

        
    }
    public virtual void CustomGUI()
    {

    }

}

[CustomPropertyDrawer(typeof(Window))]
public class WindowPD : PropertyDrawer
{
    readonly string[] States = new string[] { "Minimised", "Focused", "huh", "Maximised" };
    Window window;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        if (window != (Window)property.objectReferenceValue)
            window = (Window)property.objectReferenceValue;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.ObjectField(property, label);
        EditorGUILayout.LabelField("Prefab Variables");
        EditorGUI.indentLevel++;
        window.rt = (RectTransform)EditorGUILayout.ObjectField("Transform: ", window.rt, typeof(RectTransform), true);
        window.NameText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Program Name Obj: ", window.NameText, typeof(TextMeshProUGUI), true);
        window.Top = (RectTransform)EditorGUILayout.ObjectField("Top: ", window.Top, typeof(RectTransform), true);
        window.Viewport = (RectTransform)EditorGUILayout.ObjectField("Viewport: ", window.Viewport, typeof(RectTransform), true);
        EditorGUI.indentLevel--;
        //Control Variables
        EditorGUILayout.LabelField("Control Variables");
        //EditorGUI.indentLevel++;
        window.Name = EditorGUILayout.TextField("Name: ", window.Name);
        window.size = EditorGUILayout.Vector2Field("Size: ", window.size);
        //window.viewportSize = EditorGUILayout.Vector2Field("Viewport Size: ", window.viewportSize);
        //EditorGUI.indentLevel--;
        EditorGUILayout.Vector2Field("Viewport Size (read-only): ", window.viewportSize);
        EditorGUILayout.EndVertical();
    }

    int BTI(Bits b)
    {
        int num = 0;
        for (int i = 0; i < b.Length; i++)
        {
            num += b[i] == true ? (int)Mathf.Pow(2, i) : 0;
        }
        return num;
    }
}