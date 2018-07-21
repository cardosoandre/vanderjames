using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.GameState;
using VDJ.BuilderGame.Movement;
using VDJ.BuilderGame.Objects;

namespace VDJ.BuilderGame
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInput input;
        public FreeMovement playerMovement;
        public Rigidbody rb;
        public HandleFinder HandleFinder;
        public Transform HandleIndicator;

        private PlayerConfig data;

        [Space]
        public MovementSettings settings;


        State state;
        IMovement movement;

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
            SetMovement(new FreeMovement(input, settings.normalMovementSettings, rb));
        }

        private void ToAnchor(Transform anchor)
        {
            SetMovement(new AnchoredMovement(anchor, rb, settings.anchorSettings));
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
        }
        #endregion
    }
}
