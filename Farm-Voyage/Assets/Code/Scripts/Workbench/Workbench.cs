using UI.Icon;
using UnityEngine;

namespace Workbench
{
    [DisallowMultipleComponent]
    public class Workbench : MonoBehaviour, IDisplayIcon
    {
        [field:SerializeField] public IconSO Icon { get; private set; }
    }
}
