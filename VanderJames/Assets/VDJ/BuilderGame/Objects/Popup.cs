using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.Objects
{
    public class Popup : MonoBehaviour
    {
        [Serializable]
        public class TransformData
        {
            public Vector3 Position = Vector3.zero;
            public Quaternion Rotation = Quaternion.identity;
            public Vector3 Scale = Vector3.one;
        }

        public AnimationCurve curve ;

        public TransformData start;
        
        public TransformData end;

        [Range(0, 1)]
        public float progress;


        


        public void Update()
        {
            transform.Apply(PopupHelpers.Lerp(start, end,curve.Evaluate(progress)));
        }

        [Button]
        public void SaveStart()
        {
            start = PopupHelpers.SaveFrom(transform);
        }
        [Button]
        public void SaveEnd()
        {
            end = PopupHelpers.SaveFrom(transform);
        }

    }
}
