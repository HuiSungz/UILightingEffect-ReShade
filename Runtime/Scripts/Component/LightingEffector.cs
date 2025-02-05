using System;
using System.Linq;
using UnityEngine;
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
        private bool _isInitialized;
        
        [NonSerialized] public Vector2 ParentPosition;
        [NonSerialized] public Vector2 ParentSize;

        public Vector3 Position
        {
            get
            {
                if (!_isInitialized) Initialize();
                return _rectTransform ? _rectTransform.position : Vector3.zero;
            }
        }
        
        public Quaternion Rotation
        {
            get
            {
                if (!_isInitialized) Initialize();
                return _rectTransform ? _rectTransform.rotation : Quaternion.identity;
            }
        }
        
        public Vector3 Scale
        {
            get
            {
                if (!_isInitialized) Initialize();
                return _rectTransform ? _rectTransform.localScale : Vector3.one;
            }
        }
        
        public float IsOverlay => _parentCanvas ? (_parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? 1f : 0f) : 0f;
        
        #endregion

        #region Unity Lifecycle Methods
        
        private void Awake()
        {
            Initialize();
        }

        // private void OnValidate()
        // {
        //     if (Application.isPlaying)
        //     {
        //         return;
        //     }
        //     
        //     Initialize();
        //     UpdateTransform();
        // }
        
        #endregion

        #region Initialize Methods

        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            
            if (!TryGetComponent(out _rectTransform))
            {
                Debug.LogError($"[{nameof(LightingEffector)}] RectTransform is not found.");
                return;
            }
            
            _parentCanvas = GetComponentInParent<Canvas>();
            if (!_parentCanvas)
            {
                Debug.LogError($"[{nameof(LightingEffector)}] Parent Canvas is not found.");
                return;
            }

            _corners = new Vector3[4];
            _isInitialized = true;
            
            UpdateTransform();
        }
        
        #endregion


        #region Transform Update Methods

        /// <summary>
        /// 월드 코너 포인트를 기반으로 부모의 위치와 크기를 업데이트합니다.
        /// </summary>
        public void UpdateTransform()
        {
            if (!_rectTransform || !_parentCanvas)
            {
                return;
            }
            
            _rectTransform.GetWorldCorners(_corners);
            
            if (_parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                var screenCorners = _corners.Select(corner => 
                    RectTransformUtility.WorldToScreenPoint(null, corner)).ToArray();
                    
                // 좌상단 코너를 기준점으로 사용
                ParentPosition = screenCorners[0];
                // 크기 계산에는 우하단 코너 사용
                ParentSize = screenCorners[2] - screenCorners[0];
                // 스크린 해상도에 맞춰 좌표 보정
                ParentPosition = new Vector2(
                    ParentPosition.x,
                    Screen.height - ParentPosition.y
                );
            }
            else
            {
                ParentPosition = _corners[0];
                ParentSize = _corners[2] - _corners[0];
            }
        }

        #endregion
        
        #region Editor Methods

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_rectTransform || !enabled)
            {
                return;
            }

            // 라이트 이펙트의 방향을 시각화
            var center = transform.position;
            var direction = Quaternion.Euler(0, 0, Angle) * Vector3.right;
            var length = 50f;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(center, center + direction * length);
            
            // 라이트 폭을 시각화
            var width = Width * length;
            var perpendicular = Quaternion.Euler(0, 0, 90) * direction;
            
            Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
            Gizmos.DrawLine(center + perpendicular * width, center - perpendicular * width);
        }
#endif
        
        #endregion
    }
}