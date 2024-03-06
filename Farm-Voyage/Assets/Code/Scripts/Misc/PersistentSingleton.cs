using UnityEngine;
    
namespace Misc
{
    public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance != null) return s_instance;
                s_instance = FindObjectOfType<T>();

                if (s_instance != null) return s_instance;
                
                GameObject singletonObject = new();
                s_instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T) + " (Singleton)";

                DontDestroyOnLoad(singletonObject);

                return s_instance;
            }
        }

        protected virtual void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (s_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

}