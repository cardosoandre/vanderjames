using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.Movement
{
    public class AnchoredMovement : IMovement
    {
        [Serializable]
        public class Settings
        {
            public float dampSmoothTime = .1f;
        }

        //Dependencies
        private Transform Anchor;
        private Rigidbody rb;
        private Settings settings;

        //State
        private bool stuck;
        private static float stickingDistance = .01f;
        private Vector3 dampVel;


        public AnchoredMovement(Transform anchor, Rigidbody rb, Settings settings)
        {
            Anchor = anchor;
            this.rb = rb;
            this.settings = settings;
        }



        #region IMovement Implementation

        public void MoveUpdate()
        {
            //Intentionally left Blank
        }
        public void MoveFixedUpdate()
        {
            if(stuck)
            {
                UpdateStuck();
            }
        }

        public void MoveLateUpdate()
        {
            if (stuck)
            {
                UpdateStuck();
            }
            else
            {
                UpdateBeforeStuck();
            }
        }

        public void Begin()
        {
            rb.velocity = Vector3.zero;
        }

        public void Leave()
        {
        }
        #endregion

        private void UpdateBeforeStuck()
        {
            if (Vector3.Distance(Transform.position, Anchor.position) < stickingDistance)
            {
                stuck = true;
                return;
            }

            Transform.position = Vector3.SmoothDamp(Transform.position, Anchor.position, ref dampVel, settings.dampSmoothTime);
        }

        private void UpdateStuck()
        {
            Transform.position = Vector3.SmoothDamp(Transform.position, Anchor.position, ref dampVel, settings.dampSmoothTime);
        }


        private Transform Transform
        {
            get
            {
                return rb.transform;
            }
        }
    }
}
