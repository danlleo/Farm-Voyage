using System;

namespace Character.Player.Events
{
    public class DiggingPlantAreaStateChangedEvent
    {
        public event EventHandler<PlayerDiggingPlantAreaEventArgs> OnPlayerDiggingPlantStateChangedArea;

        public void Call(object sender, PlayerDiggingPlantAreaEventArgs playerDiggingPlantAreaEventArgs)
        {
            OnPlayerDiggingPlantStateChangedArea?.Invoke(sender, playerDiggingPlantAreaEventArgs);
        }
    }

    public class PlayerDiggingPlantAreaEventArgs : EventArgs
    {
        public readonly bool IsDigging;

        public PlayerDiggingPlantAreaEventArgs(bool isDigging)
        {
            IsDigging = isDigging;
        }
    }
}