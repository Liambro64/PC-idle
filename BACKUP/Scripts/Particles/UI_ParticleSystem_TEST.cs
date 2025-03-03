using System;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class UI_ParticleSystem_TEST : MonoBehaviour
{
    /*
    public GameObject[] UIItem;
    public GameObject sphere;
    public GameObject empty;
    public float angle;
    public bool gravity;
    public int amount;
    public int STamount = 250;
    public float speed;
    public float speedMulti = 50;
    public double duration;
    public double howFar;
    public int[] totals;

    public void Init(GameObject particle, int amount, float speed, double duration, Vector2[] range = null, bool gravity = false)
    {
        this.gravity = gravity;
        this.amount = amount;
        UIItem = new GameObject[1] { particle };
        this.speed = speed;
        this.duration = duration;
        howFar = Math.Log(duration, 2) * speed;
    }
    public void Init(GameObject[] particle, int amount, float speed, double duration, Vector2[] range = null, bool gravity = false)
    {
        this.gravity = gravity;
        this.amount = amount;
        UIItem = particle;
        this.speed = speed;
        this.duration = duration;
        howFar = Math.Log(duration, 2) * speed;
    }
    GameObject[] gameObj;
    public const float cSpeed = 100 / 9.122F;
    public void dampingTest()
    {
        GameObject gameObject;
        gameObj = new GameObject[amount];
        gameObject = Instantiate(UIItem[0], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        double[] d = GSPD(howFar, duration);
        Quaternion Angle = gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        Vector3 dir = Angle * Vector3.right;
        Rigidbody2D rig = gameObject.GetComponent<Rigidbody2D>();
        rig.linearDamping = (float)d[0];
        rig.AddForce(dir * (float)d[1] * cSpeed);
        Particle p = gameObject.GetComponent<Particle>();
        p.speed = (float)d[1];
        p.d2 = (value, value2, value3, g) =>
        {
            GameObject sp = Instantiate(sphere, new Vector3(-(amount * 5) - g * 5, value2 * 5, value3 / 20), Quaternion.Euler(0, 0, 0));
            sp.name = "Time; j:" + g + ", time: " + value2 + ", speed: " + value3;
            sp.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
            sp = Instantiate(sphere, new Vector3(-(amount * 5) - g * 5, value / 10, value3 / 20), Quaternion.Euler(0, 0, 0));
            sp.name = "Distance; j:" + g + ", distance: " + value + ", speed: " + value3;
            sp.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0);
        };
    }
    public void angleTest(int angles)
    {
        for (int i = 0; i < 360; i += 360 / angles)
        {
            GameObject gameObject;
            gameObj = new GameObject[amount];
            gameObject = Instantiate(UIItem[0], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            gameObject.name = "angle: " + i;
            double[] d = GSPD(howFar, duration);
            Quaternion Angle = gameObject.transform.rotation = Quaternion.Euler(0, 0, i);
            Vector3 dir = Angle * Vector3.right;
            Rigidbody2D rig = gameObject.GetComponent<Rigidbody2D>();
            rig.linearDamping = (float)d[0] - 1;
            rig.AddForce(dir * (float)d[1] * cSpeed);
            Particle p = gameObject.GetComponent<Particle>();
            p.speed = (float)d[1];
        }
    }
    public void angledSpeedTest(int amount)
    {
        for (int i = 1; i < amount + 1; i++)
        {
            GameObject gameObject;
            gameObj = new GameObject[amount];
            gameObject = Instantiate(UIItem[0], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            float angl = UnityEngine.Random.Range(0f, 360f);
            gameObject.name = "angle: " + angl;
            double[] d = GSPD(howFar * i, duration);
            Quaternion Angle = gameObject.transform.rotation = Quaternion.Euler(0, 0, angl);
            Vector3 dir = Angle * Vector3.right;
            Rigidbody2D rig = gameObject.GetComponent<Rigidbody2D>();
            rig.linearDamping = (float)d[0];
            rig.AddForce(dir * (float)d[1]);
            //rig.AddForce(dir * (float)d[1] * cSpeed * i);
            Particle p = gameObject.GetComponent<Particle>();
            p.speed = (float)d[1];
        }
    }
    public float tDamping = 1;
    public void singleParticleTest()
    {
        GameObject gameObject;
        gameObject = Instantiate(UIItem[0], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Rigidbody2D rig = gameObject.GetComponent<Rigidbody2D>();
        rig.linearDamping = tDamping;
        rig.AddForce(Vector2.right * speed);
        Particle p = gameObject.GetComponent<Particle>();
        p.speed = speed;
        p.single = true;
    }
    public float max = 3;
    public float increase = 0.1f;
    public void ParticleTest()
    {
        
        for (int i = (int)(max/increase)+1; i > 0; i--)
        {
            GameObject gameObject;
            gameObject = Instantiate(UIItem[0], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Rigidbody2D rig = gameObject.GetComponent<Rigidbody2D>();
            rig.linearDamping = i*increase;
            rig.AddForce(Vector2.right * speed);
            Particle p = gameObject.GetComponent<Particle>();
            p.Distance = (v1, v2) => {
                //plotting as:
                //x = time - dampening (v2)
                //y = difference of my calculation from the real distance / 10, calculated as realdistance-calcdistance
                GameObject gm = Instantiate(sphere);
                gm.transform.position = new Vector3(v2, 1/v2);
                gm.name = "X: "+v2+", Y: "+1/v2;
            };
            p.speed = speed;
            p.single = false;
        }
    }
    [SerializeField]
    public const string defaultFile = "C:\\STUFF\\particletest.csv";
    public static bool useInt = false;
    public static int iterator = 0;
    float last = 1;
    float last2 = 1;
    public void ParticleTestWithFile(string filepath = defaultFile  )
    {
        if (filepath ==  "")
            filepath = defaultFile;
        if (useInt) {
            filepath += "" + iterator;
            iterator++;
        }
        print("Does file exist? "+(File.Exists(filepath) ? "yes" : "no"));
        if (!File.Exists(filepath))
            File.Create(filepath);
        //StreamWriter writer = new StreamWriter(filepath);
        //writer.WriteAsync("Distance Offset,Time,Dampening\n");
        for (int i = (int)(max/increase)+1; i > 0; i--)
        {
            GameObject gameObject;
            gameObject = Instantiate(UIItem[0], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Rigidbody2D rig = gameObject.GetComponent<Rigidbody2D>();
            rig.linearDamping = i*increase;
            rig.AddForce(Vector2.right * speed);
            Particle p = gameObject.GetComponent<Particle>();
            p.physicsEveryFrame = true;
            float k = i*increase;
            p.Distance = (v1, v2) => {
                //plotting as:
                //v1: calculation offset from real distance 
                //v2: time
                //l : dampening
                float l = k + 0;
                float c = v1 / (v2*l);
                print("offset: "+v1+", time: "+v2+", dampening: "+l+"\n"+
                      "time - dampening: "+(v2 - l)+", offset / (time * dampening) & backwards: "+c+" "+ 1/c+", diff: "+(c/last));
                      last2 = last / c;
                      last = c;
                //writer.WriteLine(v1+","+v2+","+l);
                GameObject gm = Instantiate(sphere);
                gm.transform.position = new Vector3(v1, v2);
                gm.name = "X: "+v1+", Y: "+v2;
            };
            p.speed = speed;
            p.single = false;
        }
    }
    public void Spit(int particle)
    {
        GameObject gameObject;
        for (int i = 0; i < amount; i++)
        {
            float angle = UnityEngine.Random.Range(-360, 360);
            Vector3 speedT = Quaternion.Euler(0, 0, angle) * new Vector3(speedMulti, speedMulti);
            gameObject = Instantiate(UIItem[particle], speedT, Quaternion.Euler(0, 0, angle), transform);
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            Rigidbody2D rig = gameObject.GetComponent<Rigidbody2D>();
            double[] dub = GSPD(howFar, duration);
            rig.AddForce(speedT);
            gameObject.AddComponent<Particle>();
        }
    }
    //get speed and dampening
    [Header("Control variables for GSPD")]
    public float a = 200;
    public float numBase = 8;
    public float div = 2.5f;

    public double[] GSPD(double distance, double time)
    {
        double damping = Math.Log(distance, time);
        double speed = GES(damping, time, distance);
        print("Speed: " + speed + "\nDamping: " + damping + "\nExpected Distance: " + GED(damping, time, speed));
        return new double[2] { damping - 1, speed };
    }
    //get expected speed
    public double GES(double damping, double time, double distance)
    {
        return distance / (((damping + 1) / (damping)) - Math.Pow(1 / damping, time - 1));
    }
    public double GED(double damping, double time, double speed)
    {
        return (((damping + 1) / (damping)) - Math.Pow(1 / damping, time - 1)) * speed;
    }
    */
}
