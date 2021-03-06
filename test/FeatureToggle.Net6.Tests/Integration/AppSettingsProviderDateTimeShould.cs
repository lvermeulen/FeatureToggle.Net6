using FeatureToggle.Internal;
using Xunit;

namespace FeatureToggle.Tests.Integration
{
    public class AppSettingsProviderDateTimeShould
    {
        [Fact]
        public void ReadDate()
        {
            var sut = new AppSettingsProvider();

            var result = sut.EvaluateDateTimeToggleValue(new BeforeDate());

            var expected = new DateTime(2050, 1, 2, 4, 5, 8);

            Assert.Equal(result, expected);
        }

        [Fact]
        public void ErrorWhenBadDateFormat()
        {
            var ex = Assert.Throws<ToggleConfigurationErrorException>(
                () => new AppSettingsProvider().EvaluateDateTimeToggleValue(new InvalidDateFormat()));

            Assert.Equal(typeof(FormatException), ex.InnerException?.GetType());
        }

        private class BeforeDate : EnabledOnOrBeforeDateFeatureToggle { }     

        private class InvalidDateFormat : EnabledOnOrAfterDateFeatureToggle { }
    }
}
