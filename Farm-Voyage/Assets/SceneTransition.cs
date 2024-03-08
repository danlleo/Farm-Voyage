using System;
using System.Collections;
using Misc;
using UnityEngine;
using Zenject;

public class SceneTransition
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