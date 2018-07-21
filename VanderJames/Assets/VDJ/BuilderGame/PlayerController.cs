using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.Movement;
using VDJ.BuilderGame.Objects;

namespace VDJ.BuilderGame
{
    public class PlayerController : MonoBehaviour, ICargo
    {
        public PlayerInput input;
        public FreeMovement playerMovement;
        public Rigidbody rb;
        public HandleFinder HandleFinder;

        public Transform HandleIndicator;

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
            GoToRespawnState(respawnPoint);
        }


        public void OnTouchedBoat(BoatPlayerCrossing boatPlayerCrossing)
        {
            state.OnTouchedBoat(boatPlayerCrossing);
            GoToOnBoatState();
        }

        public void OnLeftBoatTouch(BoatPlayerCrossing boatPlayerCrossing)
        {
            
        }

        public void Load() {
            
        }

        public void Release() {

        }

        #endregion  


        #region Unity Messages

        private void Start()
        {
            ToFreeMove();
            GoToFreeState();
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

        private void SetMovement(IMovement value)
        {
            if (movement != null)
                movement.Leave();

            movement = value;

            if (movement != null)
                movement.Begin();
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


        private void GoToOnBoatState()
        {
            SetState(new OnBoatState(this));
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

            public virtual void OnTouchedBoat(BoatPlayerCrossing boatPlayerCrossing)
            {
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
                if(owner.input.IsMainButtonCurrentlyDown && owner.HandleFinder.Target != null)
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
                if(!owner.input.IsMainButtonCurrentlyDown)
                {
                    owner.GoToFreeState();
                }
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

            public OnBoatState(PlayerController owner):base(owner)
            {

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
                
            }
        }
        #endregion
    }
}
