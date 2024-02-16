using UnityEngine;

namespace UI.Icon
{
    public class DisplayIcon
    {
        public readonly RectTransform RectTransform;
        public readonly IconSO Icon;

        public DisplayIcon(RectTransform rectTransform, IconSO icon)
        {
            RectTransform = rectTransform;
            Icon = icon;
        }
    }
}