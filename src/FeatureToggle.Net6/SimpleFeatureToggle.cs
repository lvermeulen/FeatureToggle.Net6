using FeatureToggle.Internal;

namespace FeatureToggle
{
    public abstract class SimpleFeatureToggle : IFeatureToggle
    {
        protected SimpleFeatureToggle()
        {
            ToggleValueProvider = new AppSettingsProvider();
        }

        public virtual IBooleanToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => ToggleValueProvider.EvaluateBooleanToggleValue(this);

        public override string ToString() => GetType().Name;
    }
}