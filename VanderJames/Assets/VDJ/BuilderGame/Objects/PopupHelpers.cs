using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.Objects
{
    public static class PopupHelpers
    {
        public static void Apply(this Transform transform, Popup.TransformData data)
        {
            transform.localPosition = data.Position;
            transform.localRotation = data.Rotation;
            transform.localScale = data.Scale;
        }

        public static Popup.TransformData Lerp(Popup.TransformData d1, Popup.TransformData d2, float t)
        {
            return new Popup.TransformData()
            {
                Position = Vector3.Lerp(d1.Position, d2.Position, t),
                Rotation = Quaternion.Lerp(d1.Rotation, d2.Rotation, t),
                Scale = Vector3.Lerp(d1.Scale, d2.Scale, t)
            };
        }

        public static Popup.TransformData SaveFrom(Transform transform)
        {
            return new Popup.TransformData()
            {
                Position = transform.localPosition,
                Rotation = transform.localRotation,
                Scale = transform.localScale
            };
        }
    }
}

