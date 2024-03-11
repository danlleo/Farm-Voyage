using System;
using UnityEngine;

namespace Farm.Well
{
    [DisallowMultipleComponent]
    public class WaterCanFilledEvent : MonoBehaviour
    {
        public event EventHandler OnWaterCanFilled;

        public void Call(object sender)
        {
            OnWaterCanFilled?.Invoke(sender, EventArgs.Empty);
        }
    }
}