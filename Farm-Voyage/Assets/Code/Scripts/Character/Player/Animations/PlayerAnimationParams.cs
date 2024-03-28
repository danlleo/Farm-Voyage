using UnityEngine;

namespace Character.Player.Animations
{
    public static class PlayerAnimationParams
    {
        public static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
        public static readonly int IsMining = Animator.StringToHash(nameof(IsMining));
        public static readonly int IsDigging = Animator.StringToHash(nameof(IsDigging));
        public static readonly int IsHarvesting = Animator.StringToHash(nameof(IsHarvesting));
        public static readonly int IsWatering = Animator.StringToHash(nameof(IsWatering));
        public static readonly int IsChopping = Animator.StringToHash(nameof(IsChopping));
        public static readonly int IsCarrying = Animator.StringToHash(nameof(IsCarrying));
    }
}
