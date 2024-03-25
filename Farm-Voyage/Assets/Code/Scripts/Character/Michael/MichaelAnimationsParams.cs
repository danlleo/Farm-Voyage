using UnityEngine;

namespace Character.Michael
{
    public static class MichaelAnimationsParams
    {
        public static int IsWalking = Animator.StringToHash(nameof(IsWalking));
        public static int IsWatering = Animator.StringToHash(nameof(IsWatering));
        public static int IsHarvesting = Animator.StringToHash(nameof(IsHarvesting));
    }
}