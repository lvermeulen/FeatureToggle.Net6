using FeatureToggle.Internal;
using Xunit;

namespace FeatureToggle.Tests.Integration
{
    public class AppSettingsProviderBooleanShould
    {
        [Fact]
        public void ReadBooleanTrueFromConfig()
        {
            Assert.True(new AppSettingsProvider().EvaluateBooleanToggleValue(new SimpleFeatureTrue()));
        }

        [Fact]
        public void ReadBooleanFalseFromConfig()
        {
            Assert.False(new AppSettingsProvider().EvaluateBooleanToggleValue(new SimpleFeatureFalse()));
        }

        [Fact]
        public void ErrorWhenCannotConvertConfig()
        {
            var ex = Assert.Throws<ToggleConfigurationErrorException>(
                () =>
                    new AppSettingsProvider().EvaluateBooleanToggleValue(new NotABooleanValue()));

            Assert.Equal(typeof(FormatException), ex.InnerException?.GetType());
        }

        [Fact]
        public void ErrorWhenKeyNotInConfig()
        {
            Assert.Throws<ToggleConfigurationErrorException>(() => new AppSettingsProvider().EvaluateBooleanToggleValue(new NotInConfig()));
        }        

        private class NotABooleanValue : SimpleFeatureToggle { }

        private class NotInConfig : SimpleFeatureToggle { }

        private class SimpleFeatureFalse : SimpleFeatureToggle { }

        private class SimpleFeatureTrue : SimpleFeatureToggle { }
    }
}
