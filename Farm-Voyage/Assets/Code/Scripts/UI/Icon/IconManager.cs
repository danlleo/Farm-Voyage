using System.Collections.Generic;
using UnityEngine;

namespace UI.Icon
{
    [DisallowMultipleComponent]
    public class IconManager : MonoBehaviour
    {
        private readonly Dictionary<Transform, DisplayIcon> _iconsDictionary = new();

        private void Awake()
        {
            FindAndCreateAllIcons();
        }

        private void Update()
        {
            foreach (KeyValuePair<Transform, DisplayIcon> icon in _iconsDictionary)
            {
                UpdateIconPosition(icon.Key, icon.Value);
            }
        }

        private void FindAndCreateAllIcons()
        {
            _iconsDictionary.Clear();

            MonoBehaviour[] allMonoBehaviours  = FindObjectsOfType<MonoBehaviour>();
            
            foreach (MonoBehaviour monoBehaviour in allMonoBehaviours)
            {
                if (monoBehaviour is not IDisplayIcon displayIcon) continue;
                
                RectTransform rectTransform = Instantiate(displayIcon.Icon.IconRectTransform, transform);
                _iconsDictionary.Add(monoBehaviour.transform, new DisplayIcon(rectTransform, displayIcon.Icon));
            }
        }
        
        private void UpdateIconPosition(Transform objectToFollow, DisplayIcon displayIcon)
        {
            if (objectToFollow == null && displayIcon.RectTransform == null)
                return;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(objectToFollow.position + displayIcon.Icon.Offset);

            if (screenPosition.z < 0)
            {
                displayIcon.Icon.IconRectTransform.gameObject.SetActive(false);
            }
            else
            {
                displayIcon.RectTransform.gameObject.SetActive(true);
                displayIcon.RectTransform.position = screenPosition;
            }
        }
    }
}