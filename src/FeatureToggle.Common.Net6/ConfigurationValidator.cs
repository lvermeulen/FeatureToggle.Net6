using System;

namespace FeatureToggle
{
    public class ConfigurationValidator
    {
        public void ValidateStartAndEndDates(DateTime startDate, DateTime endDate, string toggleNameInConfig)
        {
            if (startDate >= endDate)
            {
                throw new ToggleConfigurationErrorException($"Configuration for {toggleNameInConfig} is invalid - the start date must be less then the end date");
            }
        }
    }
}
