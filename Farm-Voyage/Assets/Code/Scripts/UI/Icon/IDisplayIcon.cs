using System;

namespace UI.Icon
{
    public interface IDisplayIcon
    {
        public IconSO Icon { get; }
        public Guid ID { get; }
    }
}