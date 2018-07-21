using UnityEngine;
using VDJ.BuilderGame.Movement;

namespace VDJ.BuilderGame
{
    [CreateAssetMenu(menuName ="BuilderGame/MovementSettings")]
    public class MovementSettings : ScriptableObject
    {

        public FreeMovement.Settings normalMovementSettings;
        public AnchoredMovement.Settings anchorSettings;
    }
}