﻿using Attributes.WithinParent;
using Character.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Misc
{
    [RequireComponent(typeof(Canvas))]
    public class SceneTransitionView : MonoBehaviour
    {
        private static readonly int s_progress = Shader.PropertyToID("_Progress");

        [Header("External references")]
        [SerializeField, WithinParent] private Image _transitionImage;
    
        private Canvas _canvas;
        private Camera _camera;

        private Material _transitionMaterial;
    
        [Inject]
        private void Construct(PlayerFollowCamera playerFollowCamera)
        {
            _camera = playerFollowCamera.GetComponent<Camera>();
        }
    
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _transitionMaterial = _transitionImage.material;
        }

        private void OnEnable()
        {
            SceneTransition.OnAnyTransitionNormalizedProgressChanged += SceneTransition_OnAnyTransitionNormalizedProgressChanged;
        }

        private void OnDisable()
        {
            SceneTransition.OnAnyTransitionNormalizedProgressChanged -= SceneTransition_OnAnyTransitionNormalizedProgressChanged;
        }

        private void Start()
        {
            InitializeCanvas();
        }
    
        private void InitializeCanvas()
        {
            _canvas.worldCamera = _camera;
            _canvas.planeDistance = 1;
        }
    
        private void SceneTransition_OnAnyTransitionNormalizedProgressChanged(float normalizedProgress)
        {
            _transitionMaterial.SetFloat(s_progress, normalizedProgress);
        }
    }
}