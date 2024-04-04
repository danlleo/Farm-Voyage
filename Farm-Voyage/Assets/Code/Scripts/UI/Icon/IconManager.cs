using System;
using System.Collections.Generic;
using Attributes.WithinParent;
using Misc;
using UnityEngine;

namespace UI.Icon
{
    [DisallowMultipleComponent]
    public class IconManager : MonoBehaviour
    {
        [Header("External references")] 
        [SerializeField, WithinParent] private Transform _iconsHolder;
        [SerializeField] private ProgressIconUpdater _progressIconUpdaterPrefab;
        
        private readonly Dictionary<Guid, IconView> _iconsDictionary = new();
        private readonly Dictionary<Guid, ProgressIconView> _progressIconsDictionary = new();
        
        private readonly HashSet<Guid> _keysToRemove = new();
        
        private void OnEnable()
        {
            SceneTransition.OnAnySceneTransitionStarted += SceneTransition_OnAnySceneTransitionStarted;
            SceneTransition.OnAnySceneTransitionEnded += SceneTransition_OnAnySceneTransitionEnded;
            IconSO.OnAnyIconVisibilityChanged += IconSO_OnAnyIconVisibilityChanged;
            ProgressIconSO.OnAnyDisplayIconProgressChanged += ProgressIconSO_OnAnyDisplayIconProgressChanged;
            ProgressIconSO.OnAnyDisplayIconProgressResumed += ProgressIconSO_OnAnyDisplayIconProgressResumed;
            ProgressIconSO.OnAnyDisplayIconProgressStopped += ProgressIconSO_OnAnyDisplayIconProgressStopped;
            ProgressIconSO.OnAnyDisplayProgressIconVisibilityChanged +=
                DisplayProgressIconSO_OnAnyDisplayProgressIconVisibilityChanged;
            ProgressIconSO.OnAnyDisplayIconProgressFinished += ProgressIconSO_OnAnyDisplayIconProgressFinished;
            ProgressIconSO.OnAnyDisplayProgressIconRefreshed += ProgressIconSO_OnAnyDisplayProgressIconRefreshed;
        }

        private void OnDisable()
        {
            SceneTransition.OnAnySceneTransitionStarted -= SceneTransition_OnAnySceneTransitionStarted;
            SceneTransition.OnAnySceneTransitionEnded -= SceneTransition_OnAnySceneTransitionEnded;
            IconSO.OnAnyIconVisibilityChanged -= IconSO_OnAnyIconVisibilityChanged;
            ProgressIconSO.OnAnyDisplayIconProgressChanged -= ProgressIconSO_OnAnyDisplayIconProgressChanged;
            ProgressIconSO.OnAnyDisplayIconProgressResumed -= ProgressIconSO_OnAnyDisplayIconProgressResumed;
            ProgressIconSO.OnAnyDisplayIconProgressStopped -= ProgressIconSO_OnAnyDisplayIconProgressStopped;
            ProgressIconSO.OnAnyDisplayProgressIconVisibilityChanged -=
                DisplayProgressIconSO_OnAnyDisplayProgressIconVisibilityChanged;
            ProgressIconSO.OnAnyDisplayIconProgressFinished -= ProgressIconSO_OnAnyDisplayIconProgressFinished;
            ProgressIconSO.OnAnyDisplayProgressIconRefreshed -= ProgressIconSO_OnAnyDisplayProgressIconRefreshed;
        }

        private void Start()
        {
            FindAndCreateAllIcons();
        }

        private void Update()
        {
            RefreshIcons();
            RefreshProgressIcons();
        }

        private void RefreshIcons()
        {
            foreach (KeyValuePair<Guid, IconView> icon in _iconsDictionary)
            {
                if (icon.Value.OwnerTransform == null)
                {
                    DestroyNotPresentIcon(icon);
                    ClearNotPresentKeysFromIconsDictionary();
                    break;
                }

                UpdateIconPosition(icon);
            }
        }
        
        private void RefreshProgressIcons()
        {
            foreach (KeyValuePair<Guid, ProgressIconView> icon in _progressIconsDictionary)
            {
                if (icon.Value.OwnerTransform == null)
                {
                    DestroyNotPresentProgressIcon(icon);
                    ClearNotPresentKeysFromProgressIconsDictionary();
                    break;
                }

                UpdateProgressIconPosition(icon);
            }
        }

        private void FindAndCreateAllIcons()
        {
            _iconsDictionary.Clear();
            _progressIconsDictionary.Clear();
            
            MonoBehaviour[] allMonoBehaviours  = FindObjectsOfType<MonoBehaviour>();
            
            foreach (MonoBehaviour monoBehaviour in allMonoBehaviours)
            {
                switch (monoBehaviour)
                {
                    case IDisplayIcon displayIcon:
                    {
                        RectTransform followRectTransform =
                            Instantiate(displayIcon.Icon.IconRectTransform, _iconsHolder);
                        
                        _iconsDictionary.Add(displayIcon.ID, new IconView(displayIcon.Icon.Offset, followRectTransform,
                            monoBehaviour.transform));
                        
                        break;
                    }
                    case IDisplayProgressIcon displayProgressIcon:
                    {
                        ProgressIconUpdater progressIconUpdater = Instantiate(_progressIconUpdaterPrefab, _iconsHolder);
                        progressIconUpdater.Initialize(
                            displayProgressIcon.ProgressIcon.InitialProgressSprite,
                            displayProgressIcon.ProgressIcon.InProgressSprite,
                            displayProgressIcon.ProgressIcon.StoppedProgressSprite,
                            displayProgressIcon.ProgressIcon.FinishedProgressSprite);

                        _progressIconsDictionary.Add(displayProgressIcon.ID,
                            new ProgressIconView(displayProgressIcon.ProgressIcon.Offset, progressIconUpdater,
                                monoBehaviour.transform));
                        
                        break;
                    }
                }
            }
        }

        private void ClearNotPresentKeysFromIconsDictionary()
        {
            foreach (Guid key in _keysToRemove)
            {
                _iconsDictionary.Remove(key);
            }
        }
        
        private void ClearNotPresentKeysFromProgressIconsDictionary()
        {
            foreach (Guid key in _keysToRemove)
            {
                _progressIconsDictionary.Remove(key);
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
        
        private void UpdateProgressIconPosition(KeyValuePair<Guid, ProgressIconView> icon)
        {
            if (icon.Value.OwnerTransform == null || icon.Value.ProgressIconUpdater.RectTransform == null)
                return;

            Vector3 screenPosition =
                Camera.main.WorldToScreenPoint(icon.Value.OwnerTransform.position + icon.Value.Offset);

            icon.Value.ProgressIconUpdater.RectTransform.position = screenPosition;
        }
        
        private void DestroyNotPresentIcon(KeyValuePair<Guid, IconView> icon)
        {
            _keysToRemove.Add(icon.Key);

            if (icon.Value.FollowRectTransform.gameObject != null)
            {
                Destroy(icon.Value.FollowRectTransform.gameObject);
            }
        }
        
        private void DestroyNotPresentProgressIcon(KeyValuePair<Guid, ProgressIconView> icon)
        {
            _keysToRemove.Add(icon.Key);

            if (icon.Value.ProgressIconUpdater.gameObject != null)
            {
                Destroy(icon.Value.ProgressIconUpdater.gameObject);
            }
        }
        
        private void ChangeIconVisibility(Guid id, bool isActive)
        {
            if (_iconsDictionary.TryGetValue(id, out IconView iconView))
            {
                iconView.FollowRectTransform.gameObject.SetActive(isActive);
            }
        }

        private void ChangeIconsVisibility(bool isActive)
        {
            _iconsHolder.gameObject.SetActive(isActive);
        }

        private void RefreshProgressIcon(Guid id)
        {
            if (_progressIconsDictionary.TryGetValue(id, out ProgressIconView progressIconView))
            {
                progressIconView.ProgressIconUpdater.RefreshProgressIcon();
            }
        }
        
        private void UpdateProgressIcon(Guid id, float progress, float timeToFillInSeconds = 0f)
        {
            if (_progressIconsDictionary.TryGetValue(id, out ProgressIconView progressIconView))
            {
                progressIconView.ProgressIconUpdater.UpdateProgress(progress, timeToFillInSeconds);
            }
        }
        
        private void SetResumedProgressIcon(Guid id)
        {
            if (_progressIconsDictionary.TryGetValue(id, out ProgressIconView progressIconView))
            {
                progressIconView.ProgressIconUpdater.SetResumedProgressIcon();
            }
        }
        
        private void SetStoppedProgressIcon(Guid id)
        {
            if (_progressIconsDictionary.TryGetValue(id, out ProgressIconView progressIconView))
            {
                progressIconView.ProgressIconUpdater.SetStoppedProgressIcon();
            }
        }

        private void SetFinishedProgressIcon(Guid id)
        {
            if (_progressIconsDictionary.TryGetValue(id, out ProgressIconView progressIconView))
            {
                progressIconView.ProgressIconUpdater.SetFinishedProgressIcon();
            }
        }
        
        private void ChangeProgressIconVisibility(Guid id, bool isActive)
        {
            if (_progressIconsDictionary.TryGetValue(id, out ProgressIconView progressIconView))
            {
                progressIconView.ProgressIconUpdater.gameObject.SetActive(isActive);
            }
        }
        
        private void SceneTransition_OnAnySceneTransitionStarted()
        {
            ChangeIconsVisibility(false);
        }
        
        private void SceneTransition_OnAnySceneTransitionEnded()
        {
            ChangeIconsVisibility(true);
        }
        
        private void IconSO_OnAnyIconVisibilityChanged(Guid id, bool isActive)
        {
            ChangeIconVisibility(id, isActive);
        }

        private void ProgressIconSO_OnAnyDisplayIconProgressChanged(Guid id, float progress, float timeToFillInSeconds)
        {
            UpdateProgressIcon(id, progress, timeToFillInSeconds);
        }

        private void ProgressIconSO_OnAnyDisplayIconProgressResumed(Guid id)
        {
            SetResumedProgressIcon(id);
        }
        
        private void ProgressIconSO_OnAnyDisplayIconProgressStopped(Guid id)
        {
            SetStoppedProgressIcon(id);
        }
        
        private void DisplayProgressIconSO_OnAnyDisplayProgressIconVisibilityChanged(Guid id, bool isActive)
        {
            ChangeProgressIconVisibility(id, isActive);
        }
        
        private void ProgressIconSO_OnAnyDisplayIconProgressFinished(Guid id)
        {
            SetFinishedProgressIcon(id);
        }
        
        private void ProgressIconSO_OnAnyDisplayProgressIconRefreshed(Guid id)
        {
            RefreshProgressIcon(id);
        }
    }
}