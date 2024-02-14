using System;
using Farm.Tool;
using UnityEngine;

namespace Farm
{
    [CreateAssetMenu(fileName = "Resource_", menuName = "Scriptable Objects/Resources/Resource")]
    public class ResourceSO : ScriptableObject
    {
        [field: SerializeField] public GameObject VisualObject { get; private set; }
        [field: SerializeField] public ResourceType ResourceToGather { get; private set; }
        [field: SerializeField, Range(1, 5)] public int InteractAmountToDestroy { get; private set; }

        [SerializeField] private ToolType _requiredToolType;

        public Type GetRequiredTool()
        {
            return _requiredToolType switch
            {
                ToolType.Shovel => typeof(Shovel),
                ToolType.Axe => typeof(Axe),
                ToolType.Pickaxe => typeof(Pickaxe),
                ToolType.WaterCan => typeof(WaterCan),
                ToolType.Scythe => typeof(Scythe),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}