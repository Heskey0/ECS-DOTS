using System;
using UnityEngine;

public static class UnityExtern
{
    public static bool FloatEqual(this float self,float target)
    {
        return Mathf.Abs(self - target) < 0.001f;
    }
}
