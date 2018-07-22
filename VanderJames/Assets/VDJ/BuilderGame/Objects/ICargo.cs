using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VDJ.BuilderGame {
	public interface ICargo {
		void Release();
        bool CanGetIntoBoat();
        void GetIntoBoat(BoatMachineState boatMachineState);
    }
}

