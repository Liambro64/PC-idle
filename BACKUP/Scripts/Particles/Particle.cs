using System;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using Unity.VisualScripting;
using System.Collections;
using System.ComponentModel;

public class Particle : MonoBehaviour
{
    public Vector3 dir = Vector3.zero;
    public float speed = 1;
    public float initSpeed = 1;
    public float dampening = 2;
    public float minSpeed = 5;
    float beans = 0;
    float startTime = 0;
    float lastTime = 0;
    void Start()
    {
        initSpeed = speed;
        startTime = Time.time;
        lastTime = Time.time;
    }
    void Update()
    {

        float curtime = Time.time;
        if (speedCalc() <= minSpeed)
        {
            Destroy(gameObject);
            print("Distance: " + beans + " after " + (curtime - startTime));
            return;
        }
        float dDist;
        if (Settings.UseIntegralForParticles)
        {
            float elapsed = curtime - startTime;
            float lastElapsed = lastTime - startTime;
            dDist = (
                            (
                                1 / MathF.Pow(
                                    dampening, lastElapsed)
                            )
                            -
                            (
                                1 / MathF.Pow(
                                    dampening, elapsed)
                            )
                            )
                            * initSpeed
                            / MathF.Log(dampening);
        }
        else
        {
            dDist = speed * Time.deltaTime;
            speed *= 1 / Mathf.Pow(dampening, Time.deltaTime);
        }
        beans += dDist;
        transform.position += dir * dDist;
        lastTime = curtime;
    }
    float speedCalc()
    {
        return initSpeed / MathF.Pow(dampening, Time.time - startTime);
    }
}