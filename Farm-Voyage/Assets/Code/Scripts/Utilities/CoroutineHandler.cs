using System.Collections;
using UnityEngine;

namespace Utilities
{
    public static class CoroutineHandler
    {
        public static void StartAndAssignIfNull(MonoBehaviour owner, ref Coroutine coroutine, IEnumerator enumerator)
        {
            coroutine ??= owner.StartCoroutine(enumerator);
        }

        public static void StartAndOverride(MonoBehaviour owner, ref Coroutine coroutine, IEnumerator enumerator)
        {
            coroutine = owner.StartCoroutine(enumerator);
        }
        
        public static void ClearAndStopCoroutine(MonoBehaviour owner, ref Coroutine coroutine)
        {
            if (coroutine == null) return;
            
            owner.StopCoroutine(coroutine);
            coroutine = null;
        }

        public static void ReassignAndRestart(MonoBehaviour owner, ref Coroutine coroutine, IEnumerator enumerator)
        {
            if (coroutine != null)
            {
                owner.StopCoroutine(coroutine);
            }

            coroutine = owner.StartCoroutine(enumerator);
        }

        public static void StopCoroutine(MonoBehaviour owner, Coroutine coroutine)
        {
            if (coroutine != null)
            {
                owner.StopCoroutine(coroutine);
            }   
        }
    }
}