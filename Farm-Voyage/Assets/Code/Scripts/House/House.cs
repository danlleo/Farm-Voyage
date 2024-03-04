using Character.Player;
using UnityEngine;
using Zenject;

namespace House
{
    [DisallowMultipleComponent]
    public class House : MonoBehaviour
    {
        private Timespan.Day _day;

        private bool _canSleep;
        
        [Inject]
        private void Construct(Timespan.Day day)
        {
            _day = day;
        }

        private void OnEnable()
        {
            _day.OnDayEnded += Day_OnDayEnded;
        }

        private void OnDisable()
        {
            _day.OnDayEnded -= Day_OnDayEnded;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_canSleep) return;
            if (other.TryGetComponent(out Player player))
            {
                Debug.Log("Sleeping time");
            }
        }
        
        private void Day_OnDayEnded()
        {
            _canSleep = true;
        }
    }
}