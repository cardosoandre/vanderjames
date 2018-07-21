using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDJ.BuilderGame.Movement
{
    public interface IMovement
    {
        void MoveUpdate();
        void MoveFixedUpdate();
        void MoveLateUpdate();
        void Begin();
        void Leave();
    }
}
