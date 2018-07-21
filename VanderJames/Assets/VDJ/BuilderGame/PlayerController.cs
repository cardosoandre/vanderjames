using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.Movement;
using VDJ.BuilderGame.Objects;

namespace VDJ.BuilderGame
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInput input;
        public FreeMovement playerMovement;
        public Rigidbody rb;
        public PopupHandleFinder popupHandleFinder;
        public Transform popupHandleIndicator;

        [Space]
        public MovementSettings settings;


        State state;
        IMovement movement;

        #region Unity Messages

        private void Start()
        {
            ToFreeMove();
            GoToFreeState();
            popupHandleFinder.TargetChanged += PopupHandleFinder_TargetChanged;
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




        private void PopupHandleFinder_TargetChanged(Utils.TargetChangeEventData<Objects.PopupHandle> ev)
        {
            popupHandleIndicator.gameObject.SetActive(ev.NewTarget != null);
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
        #endregion

        #region State Changes

        private void GrabHandle(PopupHandle target)
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
                if(owner.input.IsMainButtonCurrentlyDown && owner.popupHandleFinder.Target != null)
                {
                    TryGrab(owner.popupHandleFinder.Target);
                }
            }

            private void TryGrab(PopupHandle target)
            {
                if (target.CanBeGrabbed)
                    owner.GrabHandle(target);
            }
        }


        private class HandleState : State, PopupHandle.IPullProvider
        {
            private PopupHandle handle;

            public HandleState(PopupHandle handle, PlayerController owner) : base(owner)
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
        #endregion
    }
}
