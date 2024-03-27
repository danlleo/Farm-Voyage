using UnityEngine;

namespace Character.Michael
{
    [System.Serializable]
    public struct MichaelTransformPoints
    {
        [field:SerializeField] public Transform[] GardeningPoints { get; private set; }
        [field:SerializeField] public Transform EmmaStorePoint { get; private set; }
    }
}