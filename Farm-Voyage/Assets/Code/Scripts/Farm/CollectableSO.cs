using System;
using UnityEngine;

namespace Farm
{
    [CreateAssetMenu(fileName = "Collectable_", menuName = "Scriptable Objects/Farm/Collectables")]
    public class CollectableSO : ScriptableObject
    {
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField] public Sprite Icon { get; private set; }
        
        public string ID { get; private set; }

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(ID)) return;
            
            ID = Guid.NewGuid().ToString();
                
            // Set the asset dirty to ensure the new ID is saved
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
