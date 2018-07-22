using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.Movement
{
    public class NoMovement : IMovement
    {
        private Rigidbody rb;

        public NoMovement(Rigidbody rb)
        {
            this.rb = rb;
        }

        public void Begin()
        {
            rb.velocity = Vector3.zero; 
        }

        public void Leave()
        {
            //Intentionally left blank
        }

        public void MoveFixedUpdate()
        {
            rb.velocity = Vector3.zero;
        }

        public void MoveLateUpdate()
        {
            //Intentionally left blank
        }

        public void MoveUpdate()
        {
            //Intentionally left blank
        }
    }
}
