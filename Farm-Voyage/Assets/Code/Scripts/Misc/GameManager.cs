using Timespan;
using UnityEngine;
using Zenject;

namespace Misc
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        private DayManager _dayManager;
        
        [Inject]
        private void Construct(DayManager dayManager)
        {
            _dayManager = dayManager;
        }

        private void Start()
        {
            _dayManager.StartDay();
        }
    }
}