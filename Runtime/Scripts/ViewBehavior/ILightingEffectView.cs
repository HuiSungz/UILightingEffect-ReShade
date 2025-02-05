
using UnityEngine;

namespace LightingEffect
{
    public interface ILightingEffectView
    {
        Material SharedMaterial { get; }
        bool IsPlaying { get; }

        void Play();
        void Stop();
        void Cleanup();
    }
}