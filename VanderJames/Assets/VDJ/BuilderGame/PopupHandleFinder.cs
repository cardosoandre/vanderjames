using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.Objects;
using VDJ.Utils;

namespace VDJ.BuilderGame
{
    public class PopupHandleFinder : ObjectFinder<PopupHandle>
    {
        protected override float TargetScoreFunction(PopupHandle t)
        {
            return ClosenessToMe(t);
        }

        private float ClosenessToMe(PopupHandle t)
        {
            return -Vector3.Distance(transform.position, t.transform.position);
        }
    }
}
