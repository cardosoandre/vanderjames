﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.GameState;
using VDJ.BuilderGame.Movement;
using VDJ.BuilderGame.Objects;
using VDJ.Utils;

namespace VDJ.BuilderGame
{
    public class PlayerController : MonoBehaviour, ICargo
    {
        public PlayerInput input;
        public FreeMovement playerMovement;
        public Rigidbody rb;
        public HandleFinder HandleFinder;

        public Transform HandleIndicator;

        private PlayerConfig data;


        [Space]
        public MovementSettings MoveSettings;
        public Settings settings;

        [Serializable]
        public class Settings
        {
            public float respawnTime = 2.0f;
        }


        State state;
        IMovement movement;



        #region Events
        public void OnFell(WaterHazard waterHazard, Vector3 respawnPoint)
        {
            state.OnTouchedWater(respawnPoint);
        }

        

        public ICargo GetCargo()
        {
            return state.GetCargo();
        }


        public bool CanGetIntoBoat()
        {
            return state.CanGetIntoBoat();
        }

        public void GetIntoBoat(BoatMachineState boatMachineState)
        {
            GoToOnBoatState(boatMachineState);
        }

        //public void OnLeftBoatTouch(BoatPlayerCrossing boatPlayerCrossing)
        //{

        //}


        public void Release(Vector3 offset) {
            transform.position += offset;
            GoToFreeState();
        }

        #endregion  


        #region Unity Messages

        private void Start()
        {
            if (state == null)
            {
                ToFreeMove();
                GoToFreeState();
            }
            HandleFinder.TargetChanged += HandleFinder_TargetChanged;
        }

        private void Update()
        {
            movement.MoveUpdate();
            state.Update();
        }
        private void LateUpdate()
        {
            movement.MoveLateUpdate();
        }
        private void FixedUpdate()
        {
            movement.MoveFixedUpdate();
        }

        #endregion


        public void Activate()
        {
            GoToFreeState();
        }

        public void GoToInactive()
        {
            SetState(new InactiveState(this));
        }



        private void SetMovement(IMovement value)
        {
            if (movement != null)
                movement.Leave();

            movement = value;

            if (movement != null)
                movement.Begin();
        }

        public void Init(PlayerConfig data)
        {
            this.data = data;
            input.playerID = data.controllerIndex;
        }

        private void SetState(State value)
        {
            if (state != null)
                state.Leave();


            state = value;

            if (state != null)
                state.Begin();
        }


        private void HandleFinder_TargetChanged(Utils.TargetChangeEventData<Handle> ev)
        {
            HandleIndicator.gameObject.SetActive(ev.NewTarget != null);
        }


        #region Movement Changes
        private void ToFreeMove()
        {
            SetMovement(new FreeMovement(input, MoveSettings.normalMovementSettings, rb));
        }

        private void ToAnchor(Transform anchor)
        {
            SetMovement(new AnchoredMovement(anchor, rb, MoveSettings.anchorSettings));
        }


        private void ToNoMove()
        {
            SetMovement(new NoMovement(rb));
        }
        private void ToNoMovement()
        {
            SetMovement(new NoMovement(rb));
        }
        #endregion

        #region State Changes

        private void GrabHandle(Handle target)
        {
            SetState(new HandleState(target, this));
        }

        private void GoToFreeState()
        {
            SetState(new FreeState(this));
        }

        private void GoToRespawnState(Vector3 target)
        {
            SetState(new RespawnState(target, settings.respawnTime, this));
        }



        private void GoToOnBoatState(BoatMachineState boatMachineState)
        {
            SetState(new OnBoatState(this, boatMachineState));
        }

        #endregion

        #region States
        private abstract class State
        {
            protected PlayerController owner;

            protected State(PlayerController owner)
            {
                this.owner = owner;
            }

            public abstract void Update();

            public abstract void Begin();
            public abstract void Leave();
            

            public virtual ICargo GetCargo()
            {
                return owner;
            }

            public virtual bool CanGetIntoBoat()
            {
                return true;
            }

            public virtual void OnTouchedWater(Vector3 respawnPoint)
            {
                owner.GoToRespawnState(respawnPoint);
            }
        }

        private class InactiveState : State
        {
            public InactiveState(PlayerController owner) : base(owner)
            {
            }

            public override void Begin()
            {
                owner.ToNoMovement();
            }

            public override void Leave()
            {
            }

            public override void Update()
            {
            }
            public override bool CanGetIntoBoat()
            {
                return false;
            }
        }

        private class FreeState : State
        {
            public FreeState(PlayerController owner) :base(owner)
            {

            }

            public override void Begin()
            {
                owner.ToFreeMove();
            }
            public override void Leave()
            {
            }


            public override void Update()
            {
                if(owner.input.MainButton && owner.HandleFinder.Target != null)
                {
                    TryGrab(owner.HandleFinder.Target);
                }
            }

            private void TryGrab(Handle target)
            {
                if (target.CanBeGrabbed)
                    owner.GrabHandle(target);
            }
            
        }


        private class HandleState : State, Handle.IPullProvider
        {
            private Handle handle;

            public HandleState(Handle handle, PlayerController owner) : base(owner)
            {
                this.handle = handle;
            }

            public Vector3 Pull
            {
                get
                {
                    return owner.input.MoveVector;
                }
            }

            public override void Begin()
            {
                handle.OnGrab(this);
                owner.ToAnchor(handle.Anchor);
            }

            public void ForceRelease()
            {
                owner.GoToFreeState();
            }

            public override void Leave()
            {
                handle.OnLeave();
            }

            public override void Update()
            {
                if(!owner.input.MainButton)
                {
                    owner.GoToFreeState();
                }
            }

            public override ICargo GetCargo()
            {
                return handle as ICargo;
            }
        }


        private class RespawnState : State
        {
            private float remainingTime;

            private Vector3 spawnTarget;

            public RespawnState(Vector3 spawnTarget, float duration, PlayerController owner) : base(owner)
            {
                this.spawnTarget = spawnTarget;
                this.remainingTime = duration;
            }

            public override void Begin()
            {
                owner.ToNoMove();
            }

            public override void Leave()
            {
            }

            public override void Update()
            {
                remainingTime -= Time.deltaTime;
                if(remainingTime <= 0)
                {
                    owner.transform.position = spawnTarget;
                    owner.GoToFreeState();
                }
            }
        }

        private class OnBoatState : State
        {
            private BoatMachineState boat;

            public OnBoatState(PlayerController owner, BoatMachineState boat):base(owner)
            {
                this.boat = boat;
            }

            public override void Begin()
            {
                print("began onBoatState");
                owner.ToAnchor(boat.Anchor);
            }

            public override void Leave()
            {
                
            }

            public override void Update()
            {
            }
            public override void OnTouchedWater(Vector3 respawnPoint)
            {
                Debug.Log("I don't even care");
                owner.StartCoroutine(CoroutineUtils.WaitThenDo(() => Debug.Log("I Love it"), 1.0f));
            }
        }
        
        #endregion
    }
}
