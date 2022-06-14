using FeatureToggle.Internal;
using Xunit;

namespace FeatureToggle.Tests.Integration
{
    public class AppSettingsProviderForDaysOfWeekShould
    {
        [Fact]
        public void ReadDaysTimePeriod()
        {
            IDaysOfWeekToggleValueProvider sut = new AppSettingsProvider();

            var daysFromConfig = sut.GetDaysOfWeek(new MondayAndFridayToggle());

            var expected = new[] {DayOfWeek.Monday, DayOfWeek.Friday};

            Assert.Equal(expected, daysFromConfig);
        }

        [Fact]
        public void InvalidDayInConfig()
        {
            var sut = new AppSettingsProvider();

            var ex = Assert.Throws<ToggleConfigurationErrorException>(() => sut.GetDaysOfWeek(new InvalidDayToggle()).ToList());
            Assert.Equal("The value 'Sun' in config key 'InvalidDayToggle' is not a valid day of the week. Days should be specified in long format. E.g. Friday and not Fri.", ex.Message);
        }

        [Fact]
        public void ErrorWhenKeyNotInConfig()
        {
            Assert.Throws<ToggleConfigurationErrorException>(() => new AppSettingsProvider().GetDaysOfWeek(new NotInConfig()).ToList());
        }

        private class MondayAndFridayToggle : EnabledOnDaysOfWeekFeatureToggle {}
        private class InvalidDayToggle : EnabledOnDaysOfWeekFeatureToggle {}
        private class NotInConfig : EnabledOnDaysOfWeekFeatureToggle {}
    }
}
