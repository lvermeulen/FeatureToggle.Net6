namespace FeatureToggle.Tests.TestToggles
{
    public class AnErroringToggle : IFeatureToggle
    {
        public bool FeatureEnabled => throw new ToggleConfigurationErrorException("Simulated toggle exception");
    }
}
