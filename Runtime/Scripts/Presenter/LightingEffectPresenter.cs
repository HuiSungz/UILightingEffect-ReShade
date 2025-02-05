
using LightingEffect.Domain;
using LightingEffect.Extension;
using UnityEngine;

namespace LightingEffect.Presenter
{
    public class LightingEffectPresenter : ILightingEffectPresenter
    {
        #region Fields & Constructor

        private readonly LightingEffectData _data;
        private readonly ILightingEffectView _view;

        private float _currentCooldown;
        private float _progress;
        private bool _isWaiting;
        
        private const float ProgressMax = 2f;
        
        public LightingEffectPresenter(LightingEffectData data, ILightingEffectView view)
        {
            _data = data;
            _view = view;
        }

        #endregion

        public void UpdateProcess()
        {
            UpdateEffect();
            UpdateView();
        }

        private void UpdateEffect()
        {
            if (_isWaiting)
            {
                UpdateCooldown();
            }
            else
            {
                UpdateProgress();
            }
        }
        
        private void UpdateCooldown()
        {
            _currentCooldown -= Time.deltaTime;

            if (!(_currentCooldown <= 0f))
            {
                return;
            }
            
            _isWaiting = false;
            _progress = 0f;
        }
        
        private void UpdateProgress()
        {
            _progress += Time.deltaTime * _data.Speed;

            if (!(_progress > ProgressMax))
            {
                return;
            }
            
            _isWaiting = true;
            _currentCooldown = _data.CooldownTime;
        }

        #region View Process

        private void UpdateView()
        {
            _data.CheckAndUpdateTransform();
            
            UpdateViewMaterial();
            UpdateViewProgress();
        }

        private void UpdateViewMaterial()
        {
            var sharedMaterial = _view.SharedMaterial;
            if (!sharedMaterial)
            {
                return;
            }

            sharedMaterial.SetScreenCoordinates(_data.IsOverlay);
            sharedMaterial.SetLightingEffectProperties(_data.Color, _data.Width, _data.Intensity, _data.Angle, _data.ParentPosition, _data.ParentSize);
        }

        private void UpdateViewProgress()
        {
            var sharedMaterial = _view.SharedMaterial;
            if (!sharedMaterial || !_view.IsPlaying)
            {
                return;
            }

            sharedMaterial.SetProgress(_progress);
        }

        #endregion
    }
}
