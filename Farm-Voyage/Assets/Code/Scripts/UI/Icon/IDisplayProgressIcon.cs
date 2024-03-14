using System;

namespace UI.Icon
{
    public interface IDisplayProgressIcon
    {
        public ProgressIconSO ProgressIcon { get; }
        public Guid ID { get; }
    }
}