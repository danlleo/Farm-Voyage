using System;

namespace Farm.Well
{
    public class WaterCanFilledEvent
    {
        public event EventHandler OnWaterCanFilled;

        public void Call(object sender)
        {
            OnWaterCanFilled?.Invoke(sender, EventArgs.Empty);
        }
    }
}