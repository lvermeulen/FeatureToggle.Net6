using Xunit;

namespace FeatureToggle.Tests
{
    public class AppSettingsProviderShould
    {
        [Fact]
        public void HaveDefaultConfiguration()
        {
            Assert.True(new A().FeatureEnabled);
        }

        private class A : SimpleFeatureToggle { }
    }
}
