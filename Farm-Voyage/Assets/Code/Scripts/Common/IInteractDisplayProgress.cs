using Misc;

namespace Common
{
    public interface IInteractDisplayProgress
    {
        public float MaxClampedProgress { get; }
        public Observable<float> CurrentClampedProgress { get; }
    }
}