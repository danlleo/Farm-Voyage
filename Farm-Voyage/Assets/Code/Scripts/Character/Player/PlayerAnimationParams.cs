using UnityEngine;

namespace Character.Player
{
    public static class PlayerAnimationParams
    {
        public static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
        public static readonly int IsGathering = Animator.StringToHash(nameof(IsGathering));
    }
}
