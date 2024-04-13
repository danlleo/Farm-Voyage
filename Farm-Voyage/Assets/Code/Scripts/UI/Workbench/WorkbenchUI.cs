using System;
using System.Collections.Generic;
using Attributes.WithinParent;
using Character.Player;
using DG.Tweening;
using Farm.Tool;
using Level;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Workbench
{
    [RequireComponent(typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public class WorkbenchUI : MonoBehaviour
    {
        private const float StartFadeValue = 0f;
        
        public event Action OnClosed;
        
        [Header("External references")] 
        [SerializeField, WithinParent] private Transform _workbenchToolContainer;
        [SerializeField] private Button _closeButton;
        [SerializeField] private WorkbenchToolUIElement _workbenchToolUIElementPrefab;
        
        [Header("Settings")]
        [SerializeField, Min(0)] private float _timeToFadeInSeconds = 0.35f;
        [SerializeField, Range(0f, 1f)] private float _endFadeValue = 1f;
        
        private readonly Dictionary<Tool, WorkbenchToolUIElement> _toolsToWorkbenchMappings = new();

        private CanvasGroup _canvasGroup;
        private PlayerInventory _playerInventory;
        private Economy _economy;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory, Economy economy)
        {
            _playerInventory = playerInventory;
            _economy = economy;
        }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(CloseUI);
            DisplayTools();
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveAllListeners();
            KillFadeAnimation();
        }

        private void SetDefaultCanvasParams()
        {
            _canvasGroup.alpha = 0f;
        }
        
        private void PlayFadeAnimation(float targetValue, Action onComplete = null)
        {
            _canvasGroup.DOFade(targetValue, _timeToFadeInSeconds).OnComplete(() => onComplete?.Invoke());
        }
            
        private void KillFadeAnimation()
        {
            SetDefaultCanvasParams();
            _canvasGroup.DOKill();
        }
        
        private void DisplayTools()
        {
            foreach (Tool tool in _playerInventory.GetAllTools())
            {
                if (_toolsToWorkbenchMappings.ContainsKey(tool)) continue;
                
                WorkbenchToolUIElement workbenchToolUIElement = Instantiate(_workbenchToolUIElementPrefab, _workbenchToolContainer);
                workbenchToolUIElement.Initialize(_economy, tool, tool.Name, tool.Level);
                
                _toolsToWorkbenchMappings.Add(tool, workbenchToolUIElement);
            }
        }
        
        private void CloseUI()
        {
            _closeButton.onClick.RemoveAllListeners();
            PlayFadeAnimation(StartFadeValue, () => OnClosed?.Invoke());
        }
    }
}
