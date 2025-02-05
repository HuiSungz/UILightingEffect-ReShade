
using LightingEffect.Domain;
using LightingEffect.Presenter;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LightingEffect
{
    [AddComponentMenu("")]
    public class LightingEffectLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            if (!TryGetComponent<LightingEffector>(out var effector))
            {
                Debug.LogError("LightingEffector is not found.");
                return;
            }

            builder.RegisterComponent(effector);
            builder.Register<LightingEffectData>(Lifetime.Scoped);
            
            if(!TryGetComponent<LightingEffectView>(out var viewBehavior))
            {
                Debug.LogError("LightingEffectView is not found.");
                return;
            }
            
            builder.RegisterComponent<ILightingEffectView>(viewBehavior);
            
            builder.Register<ILightingEffectPresenter, LightingEffectPresenter>(Lifetime.Scoped);
            builder.RegisterEntryPoint<LightingEffectEntryPoint>(Lifetime.Scoped);
        }
    }
}