using TMPro;
using UnityEngine;

namespace UI.Workbench
{
    [DisallowMultipleComponent]
    public class WorkbenchTool : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        
        private string _name;
        private int _level;
        
        private void Start()
        {
            UpdateTexts();
        }

        public void Initialize(string name, int level)
        {
            _name = name;
            _level = level;
        }

        private void UpdateTexts()
        {
            _nameText.text = _name;
            _levelText.text = $"Lvl. {_level}";
        }
    }
}