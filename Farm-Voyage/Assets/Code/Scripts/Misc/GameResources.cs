using UI;
using UnityEngine;

namespace Misc
{
    public class GameResources : MonoBehaviour
    {
        private static GameResources s_instance;

        public static GameResources Retrieve
        {
            get
            {
                if (s_instance == null)
                    s_instance = Resources.Load<GameResources>("GameResources");

                return s_instance;
            }
        }

        [field: SerializeField] public PopupText PopupText { get; private set; }
    }
}
