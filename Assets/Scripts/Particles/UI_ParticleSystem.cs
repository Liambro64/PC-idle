using System;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.Collections;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class UI_ParticleSystem : MonoBehaviour
{
    public GameObject[] UIItem;
    public float distance = 50;
    public float time = 5;
    public float minSpeed = 1;
    public int frameRate = 10;
    public int amount = 1;
    public float range = 60;


    public void Init(GameObject particle)
    {
        UIItem = new GameObject[1] { particle };
    }
    public void Init(GameObject[] particle)
    {
        UIItem = particle;
    }
    public void Spit(int particle, int amount)
    {
        GameObject gameObject;
        float speed = GetSpeed(distance, time);
        float damping = MathF.Pow(speed, 1 / time);
        for (int i = 0; i < amount; i++)
        {
            float angle = UnityEngine.Random.Range(-range, range);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
            gameObject = Instantiate(UIItem[particle], Vector3.zero, Quaternion.Euler(0, 0, angle), transform);
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Particle p = gameObject.GetComponent<Particle>();
            p.speed = speed;
            p.dampening = damping;
            p.minSpeed = minSpeed;
            p.dir = dir;
        }
    }
    public void SpitOneText(string text, int particle = 0)
    {
        GameObject gameObject;
        float speed = GetSpeed(distance, time);
        float damping = MathF.Pow(speed, 1 / time);
        float angle = UnityEngine.Random.Range(-range, range);
        Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
        gameObject = Instantiate(UIItem[particle], Vector3.zero, Quaternion.Euler(0, 0, angle), transform);
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        gameObject.GetComponent<TextMeshProUGUI>().text = text;
        Particle p = gameObject.GetComponent<Particle>();
        p.dir = dir;
        p.speed = speed;
        p.dampening = damping;
        p.minSpeed = minSpeed;
    }
    public void SpitOneText(string text)
    {
        GameObject gameObject;
        float speed = GetSpeed(distance, time);
        float damping = MathF.Pow(speed, 1 / time);
        float angle = UnityEngine.Random.Range(-range, range);
        Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
        gameObject = Instantiate(UIItem[0], Vector3.zero, Quaternion.Euler(0, 0, angle), transform);
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        gameObject.GetComponent<TextMeshProUGUI>().text = text;
        Particle p = gameObject.GetComponent<Particle>();
        p.dir = dir;
        p.speed = speed;
        p.dampening = damping;
        p.minSpeed = minSpeed;
    }
    public void SpitMultipleText(string text)
    {
        GameObject gameObject;
        float speed = GetSpeed(distance, time);
        float damping = MathF.Pow(speed, 1 / time);
        for (int i = 0; i < amount; i++)
        {
            float angle = UnityEngine.Random.Range(-range, range);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
            gameObject = Instantiate(UIItem[0], Vector3.zero, Quaternion.Euler(0, 0, angle), transform);
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            gameObject.GetComponent<TextMeshProUGUI>().text = text;
            Particle p = gameObject.GetComponent<Particle>();
            p.speed = speed;
            p.dampening = damping;
            p.minSpeed = minSpeed;
            p.dir = dir;
        }
    }
    public void SpitOne(int particle)
    {
        float speed = GetSpeed(distance, time);
        print("Speed: " + speed);
        float damping = MathF.Pow(speed, 1 / time);
        GameObject gameObject;
        float angle = UnityEngine.Random.Range(-range, range);
        Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
        gameObject = Instantiate(UIItem[particle], Vector3.zero, Quaternion.Euler(0, 0, angle), transform);
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Particle p = gameObject.GetComponent<Particle>();
        p.dir = dir;
        p.speed = speed;
        p.dampening = damping;
        p.minSpeed = minSpeed;
    }
    Func<float, float> ln = (x) => MathF.Log(x);

    public float Distance(float speed, float time)
    {
        float d = MathF.Pow(speed, 1 / time);
        //convienience
        float s = speed;
        return (1 - 1 / MathF.Pow(d, ln(s) / ln(d))) * s / ln(d);
    }


    public float GetSpeed(float distance, float time)
    {
        float s = distance;
        float speedDist = Distance(s, time);
        if (speedDist == distance)
            return s;
        else
        {
            bool dbls = true;
            int start = 0;
            float diff = 0;
            while (MathF.Round(speedDist * 256) / 256 != distance)
            {
                if (speedDist > distance)
                {
                    if (dbls && start != 2)
                    {
                        diff = s / 2;
                        s -= diff;
                        start = 1;
                    }
                    else
                    {
                        diff /= 2;
                        s -= diff;
                        dbls = false;
                    }
                }
                else if (speedDist < distance)
                {
                    if (dbls && start != 1)
                    {
                        diff = s;
                        s += diff;
                        start = 2;
                    }
                    else
                    {
                        diff /= 2;
                        s += diff;
                        dbls = false;
                    }
                }
                speedDist = Distance(s, time);
            }
        }
        if (speedDist < distance)
            s = MathF.Ceiling(s);
        else if (speedDist > distance)
            s = MathF.Floor(s);
        return s;
    }
}
