using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VDJ.Utils;

namespace VDJ.BuilderGame {
    public class PlayerObjectFinder : ObjectFinder<PlayerController>
    {
        protected override float TargetScoreFunction(PlayerController t)
        {
            return ClosenessToMe(t);
        }

        private float ClosenessToMe(PlayerController t)
        {
            return -Vector3.Distance(transform.position, t.transform.position);
        }
    }
}