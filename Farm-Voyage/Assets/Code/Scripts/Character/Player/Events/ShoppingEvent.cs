using System;

namespace Character.Player.Events
{
    public class ShoppingEvent
    {
        public event EventHandler<PlayerShoppingEventArgs> OnPlayerShopping;

        public void Call(object sender, PlayerShoppingEventArgs playerShoppingEventArgs)
        {
            OnPlayerShopping?.Invoke(sender, playerShoppingEventArgs);
        }
    }

    public class PlayerShoppingEventArgs : EventArgs
    {
        public readonly bool IsShopping;

        public PlayerShoppingEventArgs(bool isShopping)
        {
            IsShopping = isShopping;
        }
    }
}