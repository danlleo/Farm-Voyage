using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Misc
{
    public sealed class SceneTransition
    {
        private const float TransitionTimeInSeconds = 1f;
    
        public static event Action OnAnySceneTransitionStarted;
        public static event Action OnAnySceneTransitionEnded;

        public static event Action<float> OnAnyTransitionNormalizedProgressChanged;

        private AsyncProcessor _asyncProcessor;
    
        [Inject]
        private void Construct(AsyncProcessor asyncProcessor)
        {
            _asyncProcessor = asyncProcessor;
        }
    
        public void StartTransition()
        {
            _asyncProcessor.StartCoroutine(StartTransitionRoutine());
        }

        private IEnumerator StartTransitionRoutine()
        {
            float timer = 0f;

            OnAnySceneTransitionStarted?.Invoke();
        
            while (timer <= TransitionTimeInSeconds)
            {
                timer += Time.deltaTime;
                float t = timer / TransitionTimeInSeconds;
            
                OnAnyTransitionNormalizedProgressChanged?.Invoke(t);
                yield return null;
            }
        
            OnAnySceneTransitionEnded?.Invoke();
        }
    }
}