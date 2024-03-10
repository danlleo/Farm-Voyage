using Attributes.WithinParent;
using Character.Player;
using Farm.Tool.ConcreteTools;
using InputManagers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class WellUI : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField, WithinParent] private Image _forceBarBackgroundImage;
        [SerializeField, WithinParent] private Image _waterCanFilledBarBackgroundImage;

        [Header("Settings")]
        [SerializeField, Range(0.1f, 0.5f)] private float _fillForce;
        [SerializeField, Range(0.1f, 0.5f)] private float _resistanceForce;
        
        private WaterCan _waterCan;
        private PlayerPCInput _playerPCInput;

        private Coroutine _resistanceRoutine;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory, PlayerPCInput playerPCInput)
        {
            if (playerInventory.TryGetTool(out WaterCan waterCan))
            {
                _waterCan = waterCan;
            }

            _playerPCInput = playerPCInput;
        }

        private void Awake()
        {
            _forceBarBackgroundImage.fillAmount = 0;
        }

        private void OnEnable()
        {
            _playerPCInput.OnInteract += PlayerPCInput_OnInteract;
        }

        private void OnDisable()
        {
            _playerPCInput.OnInteract -= PlayerPCInput_OnInteract;
        }
        
        private void PlayerPCInput_OnInteract()
        {
            _waterCan
        }
    }
}
