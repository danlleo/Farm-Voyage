using System;
using System.Collections.Generic;
using Attributes.WithinParent;
using UnityEngine;

namespace UI.Icon
{
    [DisallowMultipleComponent]
    public class IconManager : MonoBehaviour
    {
        [Header("External references")] 
        [SerializeField, WithinParent] private Transform _iconsHolder;
        
        private readonly Dictionary<Guid, IconView> _iconsDictionary = new();
        private readonly HashSet<Guid> _keysToRemove = new();
        
        private void OnEnable()
        {
            SceneTransition.OnAnySceneTransitionStarted += SceneTransition_OnAnySceneTransitionStarted;
            SceneTransition.OnAnySceneTransitionEnded += SceneTransition_OnAnySceneTransitionEnded;
            IconSO.OnAnyIconVisibilityChanged += IconSO_OnAnyIconVisibilityChanged;
        }

        private void OnDisable()
        {
            SceneTransition.OnAnySceneTransitionStarted -= SceneTransition_OnAnySceneTransitionStarted;
            SceneTransition.OnAnySceneTransitionEnded -= SceneTransition_OnAnySceneTransitionEnded;
            IconSO.OnAnyIconVisibilityChanged -= IconSO_OnAnyIconVisibilityChanged;
        }

        private void Start()
        {
            FindAndCreateAllIcons();
        }

        private void Update()
        {
            RefreshIcons();
        }

        private void RefreshIcons()
        {
            foreach (KeyValuePair<Guid, IconView> icon in _iconsDictionary)
            {
                if (icon.Value.OwnerTransform == null)
                {
                    DestroyNotPresentIcon(icon);
                    ClearNotPresentIconsFromDictionary();
                    break;
                }

                UpdateIconPosition(icon);
            }
        }

        private void FindAndCreateAllIcons()
        {
            _iconsDictionary.Clear();

            MonoBehaviour[] allMonoBehaviours  = FindObjectsOfType<MonoBehaviour>();
            
            foreach (MonoBehaviour monoBehaviour in allMonoBehaviours)
            {
                if (monoBehaviour is not IDisplayIcon displayIcon) continue;

                RectTransform followRectTransform = Instantiate(displayIcon.Icon.IconRectTransform, _iconsHolder);
                _iconsDictionary.Add(displayIcon.ID, new IconView(displayIcon.Icon.Offset, followRectTransform,
                    monoBehaviour.transform));
            }
        }

        private void ClearNotPresentIconsFromDictionary()
        {
            foreach (Guid key in _keysToRemove)
            {
                _iconsDictionary.Remove(key);
            }
        }
        
        private void UpdateIconPosition(KeyValuePair<Guid, IconView> icon)
        {
            if (icon.Value.OwnerTransform == null || icon.Value.FollowRectTransform == null)
                return;

            Vector3 screenPosition =
                Camera.main.WorldToScreenPoint(icon.Value.OwnerTransform.position + icon.Value.Offset);

            icon.Value.FollowRectTransform.position = screenPosition;
        }
        
        private void DestroyNotPresentIcon(KeyValuePair<Guid, IconView> icon)
        {
            _keysToRemove.Add(icon.Key);

            if (icon.Value.FollowRectTransform.gameObject != null)
            {
                Destroy(icon.Value.FollowRectTransform.gameObject);
            }
        }
        
        private void ChangeIconVisibility(Guid id, bool isActive)
        {
            if (_iconsDictionary.TryGetValue(id, out IconView iconView))
            {
                iconView.FollowRectTransform.gameObject.SetActive(isActive);
            }
        }
        
        private void SceneTransition_OnAnySceneTransitionEnded()
        {
            _iconsHolder.gameObject.SetActive(true);
        }

        private void SceneTransition_OnAnySceneTransitionStarted()
        {
            _iconsHolder.gameObject.SetActive(false);
        }
        
        private void IconSO_OnAnyIconVisibilityChanged(Guid id, bool isActive)
        {
            ChangeIconVisibility(id, isActive);
        }
    }
}