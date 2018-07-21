using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VDJ.BuilderGame.Objects
{
    public class BoatPlayerCrossing : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            var playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.OnTouchedBoat(this);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            var playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.OnLeftBoatTouch(this);
            }
        }
    }
}