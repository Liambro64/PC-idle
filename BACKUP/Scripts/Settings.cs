using System;
using UnityEngine;

public static class Settings
{
    public static bool UseIntegralForParticles = true;
    static float FixedUpdateTimestepV = 0.1f;
    public static float FixedUpdateTimestep
    {
        get => FixedUpdateTimestepV;
        set { FixedUpdateTimestepV = value; Time.fixedDeltaTime = value; }
    }
    public static float FixedUpdatesPerSecond
    {
        get => 1 / FixedUpdateTimestepV;
        set { FixedUpdateTimestepV = 1 / value; Time.fixedDeltaTime = FixedUpdateTimestepV; }
    }




}