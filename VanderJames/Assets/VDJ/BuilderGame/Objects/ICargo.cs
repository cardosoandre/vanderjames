using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VDJ.BuilderGame {
	public interface ICargo {
        bool CanGetIntoBoat();
        void GetIntoBoat(BoatMachineState boatMachineState);
        void Release(Vector3 vector3);
    }
}

