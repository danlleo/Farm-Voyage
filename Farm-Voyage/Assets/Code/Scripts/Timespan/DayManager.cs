using Misc;
using Zenject;

namespace Timespan
{
    public sealed class DayManager
    {
        public Day CurrentDay { get; private set; }

        private AsyncProcessor _asyncProcessor;
        
        [Inject]
        private void Construct(AsyncProcessor asyncProcessor)
        {
            _asyncProcessor = asyncProcessor;
            CurrentDay = new Day(_asyncProcessor);
        }
        
        public void StartDay()
        {
            CurrentDay.StartDay();
        }

        public void EndDay()
        {
            
        }
    }
}
