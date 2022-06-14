namespace FeatureToggle
{
    public class AlwaysOnFeatureToggle : IFeatureToggle
    {
        public bool FeatureEnabled => true;
    }
}