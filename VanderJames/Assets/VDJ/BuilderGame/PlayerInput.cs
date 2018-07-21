using UnityEngine;

namespace VDJ.BuilderGame.Movement
{
    public class PlayerInput : MonoBehaviour, IMoveInput
    {
        public KeyCode mainKey = KeyCode.Space;


        public float Horizontal
        {
            get
            {
                return Input.GetAxisRaw("Horizontal");
            }
        }

        public float Vertical
        {
            get
            {
                return Input.GetAxisRaw("Vertical");
            }
        }

        public bool IsMainButtonPressed
        {
            get
            {
                return Input.GetKeyDown(mainKey);
            }
        }

        public bool IsMainButtonCurrentlyDown
        {
            get
            {
                return Input.GetKey(mainKey);
            }
        }

        public Vector3 MoveVector { get { return new Vector3(Horizontal, 0, Vertical).normalized; } }
    }
}