using System;
using UnityEngine;

namespace InputManagers
{
    public interface IPlayerInput
    {
        public event Action OnInteract;
        public Vector2 MovementInput { get; }
        public float MoveAmount { get; }
    }
}