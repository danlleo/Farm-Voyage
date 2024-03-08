﻿using System.Collections.Generic;
using Attributes.WithinParent;
using UnityEngine;

namespace UI.Icon
{
    [DisallowMultipleComponent]
    public class IconManager : MonoBehaviour
    {
        [Header("External references")] 
        [SerializeField, WithinParent] private Transform _iconsHolder;
        
        private readonly Dictionary<Transform, IconView> _iconsDictionary = new();
        private readonly HashSet<Transform> _keysToRemove = new();
        
        private void OnEnable()
        {
            SceneTransition.OnAnySceneTransitionStarted += SceneTransition_OnAnySceneTransitionStarted;
            SceneTransition.OnAnySceneTransitionEnded += SceneTransition_OnAnySceneTransitionEnded;
        }

        private void OnDisable()
        {
            SceneTransition.OnAnySceneTransitionStarted -= SceneTransition_OnAnySceneTransitionStarted;
            SceneTransition.OnAnySceneTransitionEnded -= SceneTransition_OnAnySceneTransitionEnded;
        }

        private void Start()
        {
            FindAndCreateAllIcons();
        }

        private void Update()
        {
            foreach (KeyValuePair<Transform, IconView> icon in _iconsDictionary)
            {
                if (icon.Key == null)
                {
                    DestroyNotPresentIcon(icon);
                }
                else
                {
                    UpdateIconPosition(icon.Key, icon.Value);
                }
            }
            
            ClearNotPresentIconsFromDictionary();
        }
        
        private void FindAndCreateAllIcons()
        {
            _iconsDictionary.Clear();

            MonoBehaviour[] allMonoBehaviours  = FindObjectsOfType<MonoBehaviour>();
            
            foreach (MonoBehaviour monoBehaviour in allMonoBehaviours)
            {
                if (monoBehaviour is not IDisplayIcon displayIcon) continue;

                RectTransform followRectTransform = Instantiate(displayIcon.Icon.IconRectTransform, _iconsHolder);
                _iconsDictionary.Add(monoBehaviour.transform, new IconView(displayIcon.Icon.Offset, followRectTransform));
            }
        }

        private void ClearNotPresentIconsFromDictionary()
        {
            foreach (Transform key in _keysToRemove)
            {
                _iconsDictionary.Remove(key);
            }
        }
        
        private void UpdateIconPosition(Transform objectToFollow, IconView iconView)
        {
            if (objectToFollow == null || iconView.FollowRectTransform == null)
                return;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(objectToFollow.position + iconView.Offset);

            if (screenPosition.z < 0)
            {
                iconView.FollowRectTransform.gameObject.SetActive(false);
            }
            else
            {
                iconView.FollowRectTransform.gameObject.SetActive(true);
                iconView.FollowRectTransform.position = screenPosition;
            }
        }
        
        private void DestroyNotPresentIcon(KeyValuePair<Transform, IconView> icon)
        {
            _keysToRemove.Add(icon.Key);

            if (icon.Value.FollowRectTransform.gameObject != null)
            {
                Destroy(icon.Value.FollowRectTransform.gameObject);
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
    }
}