using UnityEngine;

namespace Character.Player
{
    public static class PlayerAnimationParams
    {
        // Movement
        public static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
        
        // Actions
        public static readonly int IsMining = Animator.StringToHash(nameof(IsMining));
        public static readonly int IsDigging = Animator.StringToHash(nameof(IsDigging));
        public static readonly int IsChopping = Animator.StringToHash(nameof(IsChopping));
        public static readonly int IsMowing = Animator.StringToHash(nameof(IsMowing));
    }
}
