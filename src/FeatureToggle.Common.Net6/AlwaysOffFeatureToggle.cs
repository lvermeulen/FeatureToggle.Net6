namespace FeatureToggle
{
    public class AlwaysOffFeatureToggle : IFeatureToggle
    {
        public bool FeatureEnabled => false;
    }
}