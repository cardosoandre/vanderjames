﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VDJ.BuilderGame {
    public class BoatMachineState : MonoBehaviour
    {

        public enum Direction { Left, Right };

        public GameObject rightSideDock;
        public GameObject leftSideDock;

        public PlayerObjectFinder playerObjectFinder;
        public SpriteRenderer sr;

        public Transform anchor;

        [Space]
        public float speed = 0.5f;
        public Direction direction = Direction.Right;
        public float waitTime = 2.0f;
        public float cargoShieldTime = 1.0f;
        public float leaveOffset = 2.0f;


        private ICargo myCargo;

        private float timeWhenCargoEntered;

        private float TimeWithCargo { get { return Time.time - timeWhenCargoEntered; } }

        State state;
        private Rigidbody rb;

        public Transform Anchor { get { return anchor; }  }

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            SetState(new Crossing(this));
            playerObjectFinder.TargetChanged += PlayerObjectFinder_TargetChanged;
            UpdateSpriteDirection();
        }

        // Update is called once per frame
        void Update()
        {
            state.Update();
        }

        void PlayerObjectFinder_TargetChanged(Utils.TargetChangeEventData<PlayerController> obj)
        {
            if (obj.NewTarget != null) {
                var cargo = obj.NewTarget.GetCargo();

                state.OnNewCargo(cargo);
            }
        }


        void ChangeDirection()
        {

            if (direction == Direction.Right)
            {
                direction = Direction.Left;
            }
            else
            {
                direction = Direction.Right;
            }
            UpdateSpriteDirection();
        }

        private void UpdateSpriteDirection()
        {
            sr.flipX = direction == Direction.Right;
        }

        private void SetState(State value)
        {
            if (state != null)
                state.Leave();


            state = value;

            if (state != null)
                state.Begin();
        }

        private void GoToDockedState()
        {
            SetState(new Docked(this));
        }

        private void GoToCrossingState()
        {
            SetState(new Crossing(this));
        }

        #region States
        private abstract class State
        {
            protected BoatMachineState owner;

            protected State(BoatMachineState owner)
            {
                this.owner = owner;
            }

            public abstract void Update();

            public abstract void Begin();
            public abstract void Leave();

            public virtual void OnNewCargo(ICargo cargo)
            {
                if (cargo == null)
                    return;
                if (owner.HasCargo)
                {
                    return;
                }

                if (cargo.CanGetIntoBoat())
                {
                    cargo.GetIntoBoat(owner);
                    owner.myCargo = cargo;
                    owner.timeWhenCargoEntered = Time.time;
                }
            }
        }

        private class Docked : State
        {
            private float timer;

            public Docked(BoatMachineState owner) : base(owner)
            {
                timer = owner.waitTime;
            }

            public override void Begin()
            {
                if (owner.TimeWithCargo > owner.cargoShieldTime)
                {
                    owner.ReleaseCargo();
                }
            }

            public override void Leave()
            {

            }

            public override void Update()
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    owner.ChangeDirection();
                    owner.GoToCrossingState();
                }
            }
            
        }

        private void ReleaseCargo()
        {
            if (myCargo != null)
            {
                var delta = DirectionVector();
                myCargo.Release(delta * leaveOffset);
                myCargo = null;
            }
        }

        private Vector3 DirectionVector()
        {
            return direction == Direction.Right ? Vector3.right : Vector3.left;
        }

        private bool HasCargo
        {
            get
            {
                return myCargo != null;
            }
        }

        private class Crossing : State
        {
            public Crossing(BoatMachineState owner) : base(owner)
            {

            }

            public override void Begin()
            {

            }

            public override void Leave()
            {

            }

            public override void Update()
            {
                // The step size is equal to speed times frame time.
                float step = owner.speed * Time.deltaTime;

                // Move our position a step closer to the target.
                if (owner.direction == Direction.Right)
                {
                    owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.rightSideDock.transform.position, step);
                }
                else
                {
                    owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.leftSideDock.transform.position, step);
                }

                if (owner.transform.position == owner.rightSideDock.transform.position)
                {
                    owner.GoToDockedState();
                }
                else if (owner.transform.position == owner.leftSideDock.transform.position)
                {
                    owner.GoToDockedState();
                }
            }
        }
        #endregion
    }
}
