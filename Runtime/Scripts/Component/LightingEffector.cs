
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LightingEffect
{
    [AddComponentMenu("Lighting/Lighting Effector")]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    [RequireComponent(typeof(LightingEffectView), typeof(LightingEffectLifetimeScope))]
    public class LightingEffector : MonoBehaviour
    {
        #region Fields - Options
        
        [Range(0.1f, 3f)] public float Speed = 0.7f;
        [Range(0f, 10f)] public float CooldownTime = 2f;
        
        public Color Color = Color.white;
        [Range(0.01f, 1f)] public float Width = 0.07f;
        [Range(0f, 5f)] public float Intensity = 0.7f;
        [Range(-89f, 89f)] public float Angle;
        
        public bool UseChildApply = true;
        public bool PlayOnStart = true;
        
        private RectTransform _rectTransform;
        private Canvas _parentCanvas;
        private Vector3[] _corners;
        
        [NonSerialized] public Vector2 ParentPosition;
        [NonSerialized] public Vector2 ParentSize;
        
        public float IsOverlay => _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 1f : 0f;
        
        #endregion
        
        private void Awake()
        {
            if (!TryGetComponent(out _rectTransform))
            {
                Debug.LogError("RectTransform is not found.");
                return;
            }
            
            _parentCanvas = GetComponentInParent<Canvas>();
            if (!_parentCanvas)
            {
                Debug.LogError("Parent Canvas is not found.");
            }
            
            InitializeInternal();
        }
        
        private void InitializeInternal()
        {
            _corners = new Vector3[4];
            _rectTransform.GetWorldCorners(_corners);
            
            if (_parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                var screenCorners = _corners.Select(corner => RectTransformUtility.WorldToScreenPoint(null, corner)).ToArray();
                ParentPosition = screenCorners[0];
                ParentSize = new Vector2(screenCorners[2].x - screenCorners[0].x, screenCorners[2].y - screenCorners[0].y);
            }
            else
            {
                ParentPosition = _corners[0];
                ParentSize = new Vector2(_corners[2].x - _corners[0].x, _corners[2].y - _corners[0].y);
            }
        }
    }

}
