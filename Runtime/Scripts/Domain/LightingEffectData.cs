
using System;
using UnityEngine;

namespace LightingEffect.Domain
{
    [Serializable]
    public class LightingEffectData
    {
        #region Fields

        private readonly LightingEffector _effector;
        
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

        public LightingEffectData(LightingEffector lightingEffector)
        {
            _effector = lightingEffector;
        }
    }
}
