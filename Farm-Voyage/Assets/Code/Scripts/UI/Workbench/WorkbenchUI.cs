using System;
using System.Collections.Generic;
using Attributes.WithinParent;
using Character.Player;
using Farm.Tool;
using UnityEngine;
using Zenject;

namespace UI.Workbench
{
    [DisallowMultipleComponent]
    public class WorkbenchUI : MonoBehaviour
    {
        [Header("External references")] 
        [SerializeField, WithinParent] private Transform _workbenchToolContainer;
        [SerializeField] private WorkbenchTool _workbenchToolPrefab;

        private readonly Dictionary<Type, WorkbenchTool> _toolsToWorkbenchMappings = new();

        private PlayerInventory _playerInventory;

        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            _playerInventory = playerInventory;
        }
        
        private void OnEnable()
        {
            UpdateVisuals();
        }
        
        private void OnDisable()
        {
            
        }
            
        private void UpdateVisuals()
        {
            foreach (Tool tool in _playerInventory.GetAllTools())
            {
                if (_toolsToWorkbenchMappings.ContainsKey(tool.GetType())) continue;
                
                WorkbenchTool workbenchTool = Instantiate(_workbenchToolPrefab, _workbenchToolContainer);
                workbenchTool.Initialize(tool.Name, tool.Level);
                
                _toolsToWorkbenchMappings.Add(tool.GetType(), workbenchTool);
            }
        }
    }
}
