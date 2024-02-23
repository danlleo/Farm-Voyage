using UnityEngine;

namespace UI
{
    [DisallowMultipleComponent]
    public class UI : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private EmmaShopUI _emmaShopUI;
    }
}
