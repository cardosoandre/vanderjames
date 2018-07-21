using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.Utils
{
    public static class MathUtils
    {
        public static Vector3 Clamp(this Vector3 me, float maxMagnitude)
        {
            if(me.magnitude > maxMagnitude)
            {
                return me.normalized * maxMagnitude;
            }
            else
            {
                return me;
            }
        }
    }
}
