using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.Objects.Buildings
{
    public class AutoBuildingStarter : MonoBehaviour
    {
        public BuildingSpot spot;
        private BuildingSpot.Settings settings;

        private void Awake()
        {
            spot.Init(settings);
        }
    }
}
