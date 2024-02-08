using UnityEngine;

namespace Farm.Plants
{
    [CreateAssetMenu(fileName = "Plant_", menuName = "Scriptable Objects/Plants/Plant")]
    public class PlantSO : ScriptableObject
    {
        [field: SerializeField] public GameObject Visual { get; private set; }
        [field: SerializeField] public PlantType PlantType { get; private set; }
    }
}