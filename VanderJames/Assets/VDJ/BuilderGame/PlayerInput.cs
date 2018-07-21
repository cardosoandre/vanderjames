using UnityEngine;
using Rewired;

namespace VDJ.BuilderGame.Movement
{
    public class PlayerInput : MonoBehaviour, IMoveInput
    {

        public Player player;
        public int playerID;

        private void Awake()
        {
            player = ReInput.players.GetPlayer(playerID);
        }

        public float Horizontal
        {
            get
            {
                return player.GetAxis("Horizontal");
            }
        }

        public float Vertical
        {
            get
            {
                return player.GetAxis("Vertical");
            }
        }

        public bool IsMainButtonPressed
        {
            get
            {
                return player.GetButtonDown("Action");
            }
        }

        public bool IsMainButtonCurrentlyDown
        {
            get
            {
                return player.GetButton("Action");
            }
        }

        public Vector3 MoveVector { get { return new Vector3(Horizontal, 0, Vertical).normalized; } }
    }
}