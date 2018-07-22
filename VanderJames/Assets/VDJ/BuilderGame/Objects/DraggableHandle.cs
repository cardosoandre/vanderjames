using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using VDJ.BuilderGame.Movement;
using VDJ.Utils;

namespace VDJ.BuilderGame.Objects
{
    public class DraggableHandle : Handle, ICargo
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

                    if (owner.state == State.Grabbed && owner.settings.radius - owner.delta.magnitude < Settings.BorderSize)
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
        private AnchoredMovement anchored;

        public UnityEvent Grabbed;
        public UnityEvent Released;

        //State
        private enum State { Grabbed, Free, Boat}

        State state = State.Free;

        //private bool grabbed = false;

        private IPullProvider pullProvider;

        private Vector3 delta = Vector3.zero;
        private Vector3 gravityDampVel;
        private HandleMoveInput moveInput;
        public AnchoredMovement.Settings anchorSettigns;

        public override bool CanBeGrabbed
        {
            get
            {
                return state == State.Free;
            }
        }

        public override Transform Anchor { get { return anchor; } }

        public override void OnGrab(IPullProvider pullProvider)
        {
            state = State.Grabbed;
            this.pullProvider = pullProvider;
            delta = Vector3.zero;

            gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

            Grabbed.Invoke();
        }

        public override void OnLeave()
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            state = State.Free;

            Released.Invoke();
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

            if (state== State.Grabbed)
            {
                UpdateGrabbed();
            } else if(state == State.Boat)
            {

            }
            else
            {
                delta = Vector3.SmoothDamp(delta, Vector3.zero, ref gravityDampVel, .1f);
            }

            UpdateAnchor();

            
        }
        private void FixedUpdate()
        {
            if (state == State.Boat)
            {
               anchored.MoveFixedUpdate();
            }
            else
            {
                movement.FixedMove();
            }
            Debug.Log(rb.velocity.magnitude);
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

        private void OnCollisionStay(Collision collision)
        {
            Debug.LogFormat("CollisionWith {0}", collision.gameObject);
        }

        private void UpdateAnchor()
        {
            anchor.position = transform.position + delta;
        }

        public void Release()
        {

            anchored.Leave();
            state = State.Free;
        }

        public bool CanGetIntoBoat()
        {
            return true;
        }

        public void GetIntoBoat(BoatMachineState boatMachineState)
        {
            if(state == State.Grabbed)
            {
                ForceLeave();
            }

            state = State.Boat;
            anchored = new AnchoredMovement(boatMachineState.anchor, rb, anchorSettigns);
            anchored.Begin();
        }
    }
}
