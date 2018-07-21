using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDJ.Utils;

namespace VDJ.BuilderGame.Resources
{
    public class ResourceTokenFinder : ObjectFinder<ResourceToken>
    {
        protected override float TargetScoreFunction(ResourceToken t)
        {
            return 0;
        }

    }
}
