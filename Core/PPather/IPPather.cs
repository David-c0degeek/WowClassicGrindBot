﻿using Core.PPather;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;

namespace Core
{
    public interface IPPather
    {
        ValueTask<List<Vector3>> FindRoute(int map, Vector3 fromPoint, Vector3 toPoint);
        ValueTask<List<Vector3>> FindRouteTo(AddonReader addonReader, Vector3 wowPoint);
        ValueTask DrawLines(List<LineArgs> lineArgs);
        ValueTask DrawLines();
        ValueTask DrawSphere(SphereArgs args);
    }
}