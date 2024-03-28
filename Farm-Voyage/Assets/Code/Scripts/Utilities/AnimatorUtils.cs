using UnityEngine;

namespace Utilities
{
    public static class AnimatorUtils
    {
        public static float GetAnimationClipLength(Animator animator, string animationName)
        {
            RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;

            foreach (AnimationClip animationClip in runtimeAnimatorController.animationClips)
            {
                if (animationClip.name.Equals(animationName))
                {
                    return animationClip.length;
                }
            }
            
            Debug.LogWarning($"Animation clip {animationName} not found.");
            return 0f;
        }
    }
}