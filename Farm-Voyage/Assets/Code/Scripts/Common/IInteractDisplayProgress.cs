namespace Common
{
    public interface IInteractDisplayProgress
    {
        public float MaxClampedProgress { get; }
        public float CurrentClampedProgress { get; }
    }
}