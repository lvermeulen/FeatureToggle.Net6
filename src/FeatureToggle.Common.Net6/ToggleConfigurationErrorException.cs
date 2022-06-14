using System;

namespace FeatureToggle
{
    public class ToggleConfigurationErrorException : Exception
    {
        public ToggleConfigurationErrorException(string message) : base(message)
        { }

        public ToggleConfigurationErrorException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
