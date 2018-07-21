using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VDJ.BuilderGame.Movement
{
    public class FreeMovement : IMovement
    {
        [Serializable]
        public class Settings
        {
            public static float StoppingThreshold = 1f;
            public float moveForce = 20.0f;
            public float brakeForce = 30.0f;
            public float speedCap = 10f;
            public float speedCapFrictionPower = 22f;
        }

        private PlayerInput input;
        private Settings settings;
        private Rigidbody rb;


        Vector3 moveAxisInput;
        bool facingLeft = false;

        public FreeMovement(PlayerInput input, Settings settings, Rigidbody rb)
        {
            this.input = input;
            this.settings = settings;
            this.rb = rb;
        }

        public float VelocityMag { get { return rb.velocity.magnitude; } }
        public bool FacingLeft { get { return facingLeft; } }




        #region IMovement Implementation
        public void MoveUpdate()
        {
            //Intentionally left Blank
        }

        public void MoveFixedUpdate()
        {
            moveAxisInput = input.MoveVector;

            Move();
            ApplyFriction();

            CheckFacing();
        }

        public void MoveLateUpdate()
        {
            //Intentionally left Blank
        }

        public void Begin()
        {
        }

        public void Leave()
        {
        }
        #endregion  
        

        private void CheckFacing()
        {
            if (moveAxisInput.x > 0)
                facingLeft = false;
            else if (moveAxisInput.x < 0)
                facingLeft = true;
        }

        private void Move()
        {
            rb.AddForce(moveAxisInput * settings.moveForce, ForceMode.Force);
        }

        private void ApplyFriction()
        {
            if (rb.velocity.magnitude < Settings.StoppingThreshold && moveAxisInput.magnitude <= 0.001f)
            {
                FullStop();
            }
            else
            {
                ApplyBrake();
            }

            if (rb.velocity.magnitude > settings.speedCap)
            {
                rb.AddForce(-rb.velocity.normalized * settings.speedCapFrictionPower);
            }
        }

        private void FullStop()
        {
            rb.velocity = Vector2.zero;
        }

        private void ApplyBrake()
        {
            Vector3 brakeForce = -rb.velocity.normalized * settings.brakeForce;
            if (Mathf.Abs(moveAxisInput.x) > 0.001f)
                brakeForce.x = 0;
            if (Mathf.Abs(moveAxisInput.z) > 0.001f)
                brakeForce.z = 0;
            rb.AddForce(brakeForce);
        }
    }
}