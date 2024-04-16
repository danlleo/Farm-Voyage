using Misc;
using UnityEngine;
using Zenject;

namespace Timespan
{
    public sealed class TimeManager : ITickable
    {
        public readonly TimeService Service;
        
        private AsyncProcessor _asyncProcessor;

        public TimeManager(TimeSettingsSO timeSettings)
        {
            Service = new TimeService(timeSettings);
        }
        
        public void Tick()
        {
            UpdateTimeOfDay();
        }

        private void UpdateTimeOfDay()
        {
            Service.UpdateTime(Time.deltaTime);
        }
    }
}
