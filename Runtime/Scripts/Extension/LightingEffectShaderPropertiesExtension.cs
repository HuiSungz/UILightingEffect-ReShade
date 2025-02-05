
using UnityEngine;

namespace LightingEffect.Extension
{
    internal static class LightingEffectShaderPropertiesExtension
    {
        private static readonly string ScreenCoord = "_UseScreenCoordinates";
        private static readonly string Color = "_LightColor";
        private static readonly string Width = "_LightWidth";
        private static readonly string Intensity = "_LightIntensity";
        private static readonly string Angle = "_LightAngle";
        private static readonly string ParentPos = "_ParentPosition";
        private static readonly string ParentSize = "_ParentRect";
        private static readonly string Progress = "_Progress";
        
        internal static void SetLightingEffectProperties(this Material material, Color lightColor, float lightWidth, float lightIntensity, float lightAngle, Vector2 parentPosition, Vector2 parentSize)
        {
            material.SetColor(Color, lightColor);
            material.SetFloat(Width, lightWidth);
            material.SetFloat(Intensity, lightIntensity);
            material.SetFloat(Angle, lightAngle);
            material.SetVector(ParentPos, new Vector4(parentPosition.x, parentPosition.y, 0, 0));
            material.SetVector(ParentSize, new Vector4(parentSize.x, parentSize.y, 0, 0));
        }
        
        internal static void SetProgress(this Material material, float progress)
        {
            material.SetFloat(Progress, progress);
        }
        
        internal static void SetScreenCoordinates(this Material material, float isOverlay)
        {
            material.SetFloat(ScreenCoord, isOverlay);
        }
    }
}