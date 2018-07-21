using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VDJ.BuilderGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {

        #region Inspector Variables
        public PlayerInput input;
        [Space]
        public float moveForce = 5.0f;
        public bool useSpeedCapFriction = false;
        public bool useBraking = false;
        public FrictionSettings frictionSettings;

        [Serializable]
        public class FrictionSettings
        {
            public static float StoppingThreshold = 1f;
            public float brakeForce = 4.0f;
            public float speedCap = 5f;
            public float speedCapFrictionPower = 5f;
        }

        #endregion



        Rigidbody rb;
        Vector3 moveAxisInput;
        bool facingLeft = false;

        public float VelocityMag { get { return rb.velocity.magnitude; } }
        public bool FacingLeft { get { return facingLeft; } }
        

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            moveAxisInput = input.MoveVector;
            Move();
            if (useSpeedCapFriction || useBraking)
            {
                ApplyFriction();
            }

            CheckFacing();
        }

        private void CheckFacing()
        {
            if (moveAxisInput.x > 0)
                facingLeft = false;
            else if (moveAxisInput.x < 0)
                facingLeft = true;
        }

        private void Move()
        {
            rb.AddForce(moveAxisInput * moveForce, ForceMode.Force);
        }

        private void ApplyFriction()
        {
            if (rb.velocity.magnitude < FrictionSettings.StoppingThreshold && moveAxisInput.magnitude <= 0.001f)
            {
                FullStop();
            }
            else if (useBraking)
            {
                ApplyBrake();
            }

            if (useSpeedCapFriction && rb.velocity.magnitude > frictionSettings.speedCap)
            {
                rb.AddForce(-rb.velocity.normalized * frictionSettings.speedCapFrictionPower);
            }
        }

        private void FullStop()
        {
            rb.velocity = Vector2.zero;
        }

        private void ApplyBrake()
        {
            Vector3 brakeForce = -rb.velocity.normalized * frictionSettings.brakeForce;
            if (Mathf.Abs(moveAxisInput.x) > 0.001f)
                brakeForce.x = 0;
            if (Mathf.Abs(moveAxisInput.z) > 0.001f)
                brakeForce.z = 0;
            rb.AddForce(brakeForce);
        }

    }
}