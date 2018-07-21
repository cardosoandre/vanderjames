using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.Objects
{
    public class WaterHazard : MonoBehaviour
    {
        public Transform dummySpawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            var playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
                playerController.OnFell(this, CalculateRespawnPoint(playerController));
        }

        private Vector3 CalculateRespawnPoint(PlayerController playerController)
        {
            return dummySpawnPoint.position;
        }
    }
}
