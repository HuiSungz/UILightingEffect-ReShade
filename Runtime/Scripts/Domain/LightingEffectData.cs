
using UnityEngine;
using VContainer;

namespace LightingEffect.Domain
{
    public class LightingEffectData
    {
        #region Fields
        
        private readonly LightingEffector _effector;
        
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;
        private Vector3 _lastScale;
        
        public float Speed => _effector.Speed;
        public float CooldownTime => _effector.CooldownTime;
        public float Width => _effector.Width;
        public float Intensity => _effector.Intensity;
        public float Angle => _effector.Angle;
        public Color Color => _effector.Color;
        
        public Vector2 ParentPosition => _effector.ParentPosition;
        public Vector2 ParentSize => _effector.ParentSize;
        public float IsOverlay => _effector.IsOverlay;
        
        #endregion

        [Inject]
        public LightingEffectData(LightingEffector lightingEffector)
        {
            _effector = lightingEffector;
            CacheTransformValues();
        }

        private void CacheTransformValues()
        {
            _lastPosition = _effector.Position;
            _lastRotation = _effector.Rotation;
            _lastScale = _effector.Scale;
        }

        public bool CheckAndUpdateTransform()
        {
            if (_effector.Position != _lastPosition ||
                _effector.Rotation != _lastRotation ||
                _effector.Scale != _lastScale)
            {
                _effector.UpdateTransform();
                CacheTransformValues();
                return true;
            }

            return false;
        }
    }
}
