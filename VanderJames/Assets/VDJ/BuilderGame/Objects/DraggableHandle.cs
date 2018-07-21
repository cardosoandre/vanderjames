using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.Movement;
using VDJ.Utils;

namespace VDJ.BuilderGame.Objects
{
    public class DraggableHandle : Handle
    {
        public static float ZeroThreshold = 0.1f;
        [Serializable]
        public class Settings
        {
            public float speed = 1;
            public float gravity = 1;
            public float radius = 2;
            public float pullForce = 5;

            public static float BorderSize = 0.0001f;
        }

        public class HandleMoveInput : IMoveInput
        {
            DraggableHandle owner;

            public HandleMoveInput(DraggableHandle owner)
            {
                this.owner = owner;
            }

            public Vector3 MoveVector
            {
                get
                {

                    if (owner.grabbed && owner.settings.radius - owner.delta.magnitude < Settings.BorderSize)
                    {
                        return owner.delta.normalized;
                    } else
                    {
                        return Vector3.zero;
                    }
                }
            }
        }

        //Dependencies
        [SerializeField]
        private Transform anchor;
        [SerializeField]
        private Settings settings;
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private FreeMovement.Settings moveSettings;

        private FreeMovement movement;

        //State
        private bool grabbed = false;
        private IPullProvider pullProvider;

        private Vector3 delta = Vector3.zero;
        private Vector3 gravityDampVel;
        private HandleMoveInput moveInput;

        public override bool CanBeGrabbed
        {
            get
            {
                return !grabbed;
            }
        }

        public override Transform Anchor { get { return anchor; } }

        public override void OnGrab(IPullProvider pullProvider)
        {
            grabbed = true;
            this.pullProvider = pullProvider;
            delta = Vector3.zero;
        }

        public override void OnLeave()
        {
            grabbed = false;
        }

        public void ForceLeave()
        {
            if (pullProvider != null)
                pullProvider.ForceRelease();
        }

        private void Awake()
        {
            moveInput = new HandleMoveInput(this);
            movement = new FreeMovement(moveInput, moveSettings, rb);
        }

        private void Update()
        {
            if (grabbed)
            {
                UpdateGrabbed();
            }
            else
            {
                delta = Vector3.SmoothDamp(delta, Vector3.zero, ref gravityDampVel, .1f);
            }

            UpdateAnchor();

            
        }
        private void FixedUpdate()
        {
            movement.FixedMove();
        }

        private void UpdateGrabbed()
        {

            var pullLength = pullProvider.Pull.magnitude;

            if(pullLength < ZeroThreshold)
            {
                if (delta.magnitude <= ZeroThreshold)
                {
                    delta = Vector3.zero;
                }
                else
                {
                    var gravity = -delta.normalized * settings.gravity;
                    delta += gravity * Time.deltaTime;
                }
            } else
            {
                var pullVec = pullProvider.Pull * settings.speed;
                delta += pullVec * Time.deltaTime;

            }

            delta = delta.Clamp(settings.radius);

        }

        private void UpdateAnchor()
        {
            anchor.position = transform.position + delta;
        }
    }
}
