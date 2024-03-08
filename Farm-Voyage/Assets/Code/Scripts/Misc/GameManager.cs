using Timespan;
using UnityEngine;
using Zenject;

namespace Misc
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        private DayManager _dayManager;
        private SceneTransition _sceneTransition;
        
        [Inject]
        private void Construct(DayManager dayManager, SceneTransition sceneTransition)
        {
            _dayManager = dayManager;
            _sceneTransition = sceneTransition;
        }

        private void Start()
        {
            _dayManager.StartDay();
            _sceneTransition.StartTransition();
        }
    }
}