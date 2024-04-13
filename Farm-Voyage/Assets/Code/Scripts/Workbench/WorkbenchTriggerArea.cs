using Character.Player;
using UnityEngine;

namespace Workbench
{
    [DisallowMultipleComponent]
    public class WorkbenchTriggerArea : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Workbench _workbench;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player _)) return;
            _workbench.UsingWorkbenchStateChangedEvent.Call(this);
        }
    }
}
