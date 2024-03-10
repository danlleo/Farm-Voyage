using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerExtractingWaterEvent : MonoBehaviour
    {
        public event EventHandler OnPlayerExtractingWater;

        public void Call(object sender)
        {
            OnPlayerExtractingWater?.Invoke(sender, EventArgs.Empty);
        }
    }
}