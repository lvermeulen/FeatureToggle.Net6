using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace FeatureToggle.Internal
{
    public sealed class AppSettingsProvider : IBooleanToggleValueProvider, IDateTimeToggleValueProvider, ITimePeriodProvider, IDaysOfWeekToggleValueProvider, IAssemblyVersionProvider
    {
        private const string KeyNotFoundInAppsettingsMessage = "The key '{0}' was not found in AppSettings";

        private IConfigurationRoot _configuration;

        public IConfigurationRoot Configuration
        { 
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appSettings.json");
                    Configuration = builder.Build();
                }
               
                return _configuration;               
            }
            set => _configuration = value;
        }

        public bool EvaluateBooleanToggleValue(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            var configValue = GetConfigValue(key);

            return ParseBooleanConfigString(configValue, key);  
        }

        public DateTime EvaluateDateTimeToggleValue(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            var configValue = GetConfigValue(key);            

            var parser = new ConfigurationDateParser();

            return parser.ParseDateTimeConfigString(configValue, key);
        }

        public IEnumerable<DayOfWeek> GetDaysOfWeek(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            var configValues = GetConfigValue(key).Split(new[] {','}).Select(x => x.Trim());

            foreach (var configValue in configValues)
            {
                var isValidDay = Enum.TryParse(configValue, true, out DayOfWeek day);

                if (isValidDay)
                {
                    yield return day;
                }
                else
                {
                    throw new ToggleConfigurationErrorException($"The value '{configValue}' in config key '{key}' is not a valid day of the week. Days should be specified in long format. E.g. Friday and not Fri.");
                }
            }
        }

        public Tuple<DateTime, DateTime> EvaluateTimePeriod(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);


            var configValues = GetConfigValue(key).Split(new[] {'|'});

            var parser = new ConfigurationDateParser();

            var startDate = parser.ParseDateTimeConfigString(configValues[0].Trim(), key);
            var endDate = parser.ParseDateTimeConfigString(configValues[1].Trim(), key);

            var v = new ConfigurationValidator();

            v.ValidateStartAndEndDates(startDate, endDate, key);

            return new Tuple<DateTime, DateTime>(startDate, endDate);
        }

        public Version EvaluateVersion(IFeatureToggle toggle)
        {
            var key = ExpectedAppSettingsKeyFor(toggle);

            ValidateKeyExists(key);

            string configuredVersion = GetConfigValue(key);

            return Version.Parse(configuredVersion);
        }

        private void ValidateKeyExists(string key)
        {
            var allKeys = GetAllConfigKeys();
            if (!allKeys.Contains(key))
            {
                throw new ToggleConfigurationErrorException(string.Format(KeyNotFoundInAppsettingsMessage, key));
            }                
        }

        private static string ExpectedAppSettingsKeyFor(IFeatureToggle toggle)
        {
            return toggle.GetType().Name;
        }

        private static bool ParseBooleanConfigString(string valueToParse, string configKey)
        {
            try
            {
                return bool.Parse(valueToParse);
            }
            catch (Exception ex)
            {
                throw new ToggleConfigurationErrorException(
                    $"The value '{valueToParse}' cannot be converted to a boolean as defined in config key '{configKey}'",
                    ex);
            }
        }

        private string GetConfigValue(string key)
        {
            return Configuration.GetSection(ToggleConfigurationSettings.Prefix.Replace(".", ""))[key];
        }

        private IEnumerable<string> GetAllConfigKeys()
        {
            var allToggleSettings = Configuration.GetSection(ToggleConfigurationSettings.Prefix.Replace(".",""))
                .GetChildren();

            return allToggleSettings.Select(setting => setting.Key).ToArray();
        }
    }
}
