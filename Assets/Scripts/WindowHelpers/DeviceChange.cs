using System;
using System.Collections;
using UnityEngine;

public class DeviceChange : MonoBehaviour
{
    public static event Action<Vector2> OnResolutionChange;
    public static event Action<DeviceOrientation> OnOrientationChange;
    public static float CheckDelay = 0.1f; // How long to wait until we check again.

    static Vector2 resolution;                    // Current Resolution
    static DeviceOrientation orientation;        // Current Device Orientation
    static bool isAlive = true;                    // Keep this script running?

    void Start()
    {
        OnResolutionChange += (vec) =>
        {
            Window.xUnitsPerWidth = 1920 / vec.x;
            Window.yUnitsPerHeight = 1080 / vec.y;
        };
        if (isAlive)
            StartCoroutine(CheckForChange());
        StartCoroutine(AfterFirstFrame());
    }

    IEnumerator AfterFirstFrame()
    {
        yield return new WaitForSeconds(1 / 64f);
        float sWidth = Screen.width;
        float sHeight = Screen.height;
        Window.xUnitsPerWidth = 1920f / sWidth;
        Window.yUnitsPerHeight = 1080f / sHeight;
    }

    IEnumerator CheckForChange()
    {
        Window.xUnitsPerWidth = 1920 / Screen.width;
        Window.yUnitsPerHeight = 1080 / Screen.height;
        resolution = new Vector2(Screen.width, Screen.height);
        orientation = Input.deviceOrientation;
        while (isAlive)
        {

            // Check for a Resolution Change
            if (resolution.x != Screen.width || resolution.y != Screen.height)
            {
                resolution = new Vector2(Screen.width, Screen.height);
                if (OnResolutionChange != null) OnResolutionChange(resolution);
            }

            // Check for an Orientation Change
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Unknown:            // Ignore
                case DeviceOrientation.FaceUp:            // Ignore
                case DeviceOrientation.FaceDown:        // Ignore
                    break;
                default:
                    if (orientation != Input.deviceOrientation)
                    {
                        orientation = Input.deviceOrientation;
                        if (OnOrientationChange != null) OnOrientationChange(orientation);
                    }
                    break;
            }

            yield return new WaitForSeconds(CheckDelay);
        }
    }

    void OnDestroy()
    {
        isAlive = false;
    }

}