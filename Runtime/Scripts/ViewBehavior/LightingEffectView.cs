
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace LightingEffect
{
    [AddComponentMenu("")]
    public class LightingEffectView : MonoBehaviour, ILightingEffectView
    {
        #region DI Injection

        private LightingEffector _effector;
        
        [Inject]
        public void Construct(LightingEffector effector)
        {
            _effector = effector;

            InitializeMaterial();
        }

        #endregion

        #region Fields

        private Material _sharedMaterial;
        private Image[] _graphicComps;
        private bool _isInitialized;
        private bool _isPlaying;
        
        public Material SharedMaterial => _sharedMaterial;
        public bool IsPlaying => _isPlaying;

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            if (_isInitialized && _effector.PlayOnStart)
            {
                Play();
            }
        }

        private void OnEnable()
        {
            if(_isInitialized && _effector.PlayOnStart)
            {
                Play();
            }
        }

        private void OnDisable()
        {
            Stop();
        }
        
        private void OnDestroy()
        {
            Cleanup();
        }

        #endregion

        #region Construct Initialize

        private void InitializeMaterial()
        {
            if (_isInitialized)
            {
                return;
            }
            
            _graphicComps = _effector.UseChildApply
                ? GetComponentsInChildren<Image>(true)
                : new[] { GetComponent<Image>() };
            
            _sharedMaterial = new Material(Shader.Find("Custom/LightingEffectUnlit"));
            ApplyChildMaterials();
        }

        private void ApplyChildMaterials()
        {
            foreach (var graphic in _graphicComps)
            {
                if (graphic)
                {
                    graphic.material = _sharedMaterial;
                }
            }

            _isInitialized = true;
        }

        #endregion

        #region Implements Management Methods

        public void Play()
        {
            _isPlaying = true;

            foreach (var graphic in _graphicComps)
            {
                if (graphic)
                {
                    graphic.material = _sharedMaterial;
                }
            }
        }
        
        public void Stop()
        {
            _isPlaying = false;

            foreach (var graphic in _graphicComps)
            {
                if (graphic)
                {
                    graphic.material = null;
                }
            }
        }

        public void Cleanup()
        {
            Stop();
            
            if (_sharedMaterial)
            {
                if (Application.isPlaying)
                {
                    Destroy(_sharedMaterial);
                }
                else
                {
                    DestroyImmediate(_sharedMaterial);
                }
            }
            
            _isInitialized = false;
        }

        #endregion
    }
}
