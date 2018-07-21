using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.Objects;
using VDJ.Utils;

namespace VDJ.BuilderGame
{
    public class HandleFinder : ObjectFinder<Handle>
    {
        protected override float TargetScoreFunction(Handle t)
        {
            return ClosenessToMe(t);
        }

        private float ClosenessToMe(Handle t)
        {
            return -Vector3.Distance(transform.position, t.transform.position);
        }
    }
}
