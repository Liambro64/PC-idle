using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<float> Frames = new();
    public TextMeshProUGUI text;
    public string Text {
        get => text?.text;
        set {if (text != null) text.text = value;}
    }
    void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        
    }
    float fps = 0;
    float aFps = 0;
    float dTime = 0;
    float aDtime = 0;

    // Update is called once per frame
    void Update()
    {
        fps = 1/(dTime = Time.deltaTime);
        
        Frames.Add(dTime);
        Text = "FPS: "+aFps.ToString("0.0")+"("+fps.ToString("0.0")+")\n"
             + "DTime: "+aDtime.ToString("0.0000")+"("+dTime.ToString("0.0000")+")";
    }
    void FixedUpdate()
    {
        aDtime = 0;
        for (int i = 0; i < Frames.Count; i++)
            aDtime += Frames[i];
        aDtime /= Frames.Count;
        aFps = 1/aDtime;
        Frames = new();
    }
}
