using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.Utils
{
    public static class CoroutineUtils
    {

        public static IEnumerator WaitThenDo(Action callback, float time =1.0f)
        {
            yield return new WaitForSeconds(time);

            callback();
        }
    }
}
