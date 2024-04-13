using System;
using System.Collections;
using Attributes.WithinParent;
using Character.Player;
using DG.Tweening;
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
        [SerializeField, WithinParent] private CanvasGroup _barsCanvasGroup;
        
        [Space(10)]
        [SerializeField, WithinParent] private CanvasGroup _filledTextCanvasGroup;
        [SerializeField, WithinParent] private RectTransform _filledTextRectTransform;
        
        [Header("Settings")]
        [SerializeField, Range(0.1f, 0.5f)] private float _fillForce;
        [SerializeField, Range(0.1f, 2f)] private float _resistanceForce;
        [SerializeField, Range(0.1f, 1f)] private float _anyImageBarFillAmountSpeed;

        private WaterCan _waterCan;
        private PlayerPCInput _playerPCInput;
        private Player _player;
        
        private Coroutine _resistanceRoutine;

        private bool _hasFinishedFilling;
        
        [Inject]
        private void Construct(PlayerInventory playerInventory, PlayerPCInput playerPCInput, Player player)
        {
            if (playerInventory.TryGetTool(out WaterCan waterCan))
            {
                _waterCan = waterCan;
            }

            _playerPCInput = playerPCInput;
            _player = player;
        }

        private void Awake()
        {
            HideFieldTextCanvasGroup();
            ClearResistanceBarFillAmount();
            ClearWaterCanFilledBarAmount();
        }

        private void OnEnable()
        {
            _playerPCInput.OnInteract += PlayerPCInput_OnInteract;
            _waterCan.OnWaterAmountChanged += WaterCan_OnWaterAmountChanged;
         
            ResetUI();
            UpdateWaterCanFillAmountBackgroundImage();
        }

        private void OnDisable()
        {
            _playerPCInput.OnInteract -= PlayerPCInput_OnInteract;
            _waterCan.OnWaterAmountChanged -= WaterCan_OnWaterAmountChanged;
        }

        private void UpdateWaterCanFillAmountBackgroundImage()
        {
            _waterCanFilledBarBackgroundImage.DOFillAmount(
                (float)_waterCan.CurrentWaterCapacityAmount / WaterCan.WaterCanCapacityAmount,
                _anyImageBarFillAmountSpeed);
        }

        private void HideFieldTextCanvasGroup()
        {
            _filledTextCanvasGroup.alpha = 0f;
        }
        
        private void ClearResistanceBarFillAmount()
        {
            _forceBarBackgroundImage.fillAmount = 0f;
        }

        private void ClearWaterCanFilledBarAmount()
        {
            _waterCanFilledBarBackgroundImage.fillAmount = 0f;
        }
        
        private void ResetUI()
        {
            _barsCanvasGroup.alpha = 1f;
            _forceBarBackgroundImage.fillAmount = 0f;
        }

        private void AnimatePopupText(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_barsCanvasGroup.DOFade(0f, .25f));
            sequence.Append(_filledTextCanvasGroup.DOFade(1f, .25f));
            sequence.Append(_filledTextRectTransform.DOShakeScale(.4f, 0.6f, 20));
            sequence.AppendInterval(1f);
            sequence.Append(_filledTextCanvasGroup.DOFade(0f, .25f));
            sequence.OnComplete(() => onComplete?.Invoke());
        }
        
        private IEnumerator ResistanceRoutine()
        {
            _forceBarBackgroundImage.fillAmount += _fillForce;

            if (_forceBarBackgroundImage.fillAmount >= 1f)
            {
                _waterCan.FillWaterCan();
                
                if (_waterCan.CurrentWaterCapacityAmount == WaterCan.WaterCanCapacityAmount)
                {
                    _hasFinishedFilling = true;
                    
                    AnimatePopupText(() =>
                    {
                        _player.Events.ExtractingWaterStateChangedEvent.Call(false);
                        _hasFinishedFilling = false;
                    });
                    
                    yield break;
                }
                
                ClearResistanceBarFillAmount();
                
                yield break;
            }
            
            while (_forceBarBackgroundImage.fillAmount < 1f)
            {
                _forceBarBackgroundImage.fillAmount -= Time.deltaTime * _resistanceForce;
                yield return null;
            }
        }
        
        private void WaterCan_OnWaterAmountChanged(int arg1, int arg2)
        {
            UpdateWaterCanFillAmountBackgroundImage();
        }
        
        private void PlayerPCInput_OnInteract()
        {
            if (_hasFinishedFilling) return;
            
            if (_resistanceRoutine != null)
            {
                StopCoroutine(_resistanceRoutine);
            }
            
            _resistanceRoutine = StartCoroutine(ResistanceRoutine());
        }
    }
}
