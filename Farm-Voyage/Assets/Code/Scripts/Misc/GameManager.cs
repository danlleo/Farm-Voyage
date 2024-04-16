using Timespan;
using UnityEngine;
using Zenject;

namespace Misc
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        private TimeManager _timeManager;
        private SceneTransition _sceneTransition;
        
        [Inject]
        private void Construct(TimeManager timeManager, SceneTransition sceneTransition)
        {
            _timeManager = timeManager;
            _sceneTransition = sceneTransition;
        }

        private void Start()
        {
            _sceneTransition.StartTransition();
        }
    }
}