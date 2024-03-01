using System.Collections.Generic;
using Attributes.WithinParent;
using Character.Player;
using Farm.Tool;
using Level;
using UnityEngine;
using Zenject;

namespace UI.Workbench
{
    [DisallowMultipleComponent]
    public class WorkbenchUI : MonoBehaviour
    {
        [Header("External references")] 
        [SerializeField, WithinParent] private Transform _workbenchToolContainer;
        [SerializeField] private WorkbenchToolUIElement _workbenchToolUIElementPrefab;

        private readonly Dictionary<Tool, WorkbenchToolUIElement> _toolsToWorkbenchMappings = new();

        private PlayerInventory _playerInventory;
        private Economy _economy;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory, Economy economy)
        {
            _playerInventory = playerInventory;
            _economy = economy;
        }
        
        private void OnEnable()
        {
            DisplayTools();
        }
        
        private void OnDisable()
        {
            
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
    }
}
