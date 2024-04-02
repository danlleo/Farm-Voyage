using System;

namespace Character.Player.Events
{
    public class ExtractingWaterEvent
    {
        public event EventHandler<PlayerExtractingWaterEventArgs> OnPlayerExtractingWater;

        public void Call(object sender, PlayerExtractingWaterEventArgs playerExtractingWaterEventArgs)
        {
            OnPlayerExtractingWater?.Invoke(sender, playerExtractingWaterEventArgs);
        }
    }

    public class PlayerExtractingWaterEventArgs : EventArgs
    {
        public readonly bool IsExtracting;

        public PlayerExtractingWaterEventArgs(bool isExtracting)
        {
            IsExtracting = isExtracting;
        }
    }
}