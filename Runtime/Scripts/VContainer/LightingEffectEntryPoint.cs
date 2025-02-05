
using LightingEffect.Presenter;
using VContainer;
using VContainer.Unity;

namespace LightingEffect
{
    public class LightingEffectEntryPoint : ITickable
    {
        #region Fields & Constructor
        
        private readonly ILightingEffectPresenter _presenter;
        
        [Inject]
        public LightingEffectEntryPoint(ILightingEffectPresenter presenter)
        {
            _presenter = presenter;
        }

        #endregion

        public void Tick()
        {
            _presenter?.UpdateProcess();
        }
    }
}