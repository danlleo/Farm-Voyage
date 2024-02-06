using UnityEngine;

namespace Farm
{
    [CreateAssetMenu(fileName = "Resource_", menuName = "Scriptable Objects/Resources/Resource")]
    public class ResourceSO : ScriptableObject
    {
        [field: SerializeField] public GameObject VisualObject { get; private set; }
        [field: SerializeField] public ToolType RequiredTool { get; private set; }
        [field: SerializeField] public ResourceType ResourceToGather { get; private set; }
        [field: SerializeField, Range(1, 5)] public int InteractAmountToDestroy { get; private set; }
    }
}