using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.Objects
{
    public abstract class Handle : MonoBehaviour
    {


        public abstract bool CanBeGrabbed { get; }
        public abstract Transform Anchor { get; }

        public abstract void OnGrab(IPullProvider pullProvider);
        public abstract void OnLeave();

        public interface IPullProvider
        {
            Vector3 Pull { get; }
        }
    }
}
