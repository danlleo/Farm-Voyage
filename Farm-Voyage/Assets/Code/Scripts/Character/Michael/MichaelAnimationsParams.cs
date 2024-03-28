using UnityEngine;

namespace Character.Michael
{
    public static class MichaelAnimationsParams
    {
        public static int IsWalking = Animator.StringToHash(nameof(IsWalking));
        public static int IsWatering = Animator.StringToHash(nameof(IsWatering));
        public static int IsHarvesting = Animator.StringToHash(nameof(IsHarvesting));
        public static int IsSitting = Animator.StringToHash(nameof(IsSitting));
        public static int OnThinking = Animator.StringToHash(nameof(OnThinking));
        public static int OnAngry = Animator.StringToHash(nameof(OnAngry));
        public static int OnLooking = Animator.StringToHash(nameof(OnLooking));
        public static int OnPlanting = Animator.StringToHash(nameof(OnPlanting));
        public static int OnPicking = Animator.StringToHash(nameof(OnPicking));
    }
}