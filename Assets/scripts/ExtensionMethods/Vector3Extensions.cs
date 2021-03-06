﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        float newX = x.HasValue ? x.Value : original.x;
        float newY = y.HasValue ? y.Value : original.y;
        float newZ = z.HasValue ? z.Value : original.z;
        return new Vector3(newX, newY, newZ);
    }

    public static bool IsBehind(this Vector3 queried, Vector3 forward)
    {
        return queried.x - forward.x < 0;
    }

}
