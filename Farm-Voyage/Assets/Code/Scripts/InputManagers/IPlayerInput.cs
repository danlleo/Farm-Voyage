using UnityEngine;

namespace InputManagers
{
    public interface IPlayerInput
    {
        public Vector2 MovementInput { get; }
        public float MoveAmount { get; }
    }
}