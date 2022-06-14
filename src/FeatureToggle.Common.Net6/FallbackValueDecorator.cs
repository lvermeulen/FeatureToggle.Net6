using System;

namespace FeatureToggle
{
    public class FallbackValueDecorator : IFeatureToggle
    {
        private readonly Action<Exception> _logAction;

        public FallbackValueDecorator(IFeatureToggle primaryToggle, IFeatureToggle fallbackToggle,
            Action<Exception> logAction = null)
        {
            PrimaryToggle = primaryToggle ?? throw new ArgumentNullException(nameof(primaryToggle));
            FallbackToggle = fallbackToggle ?? throw new ArgumentNullException(nameof(fallbackToggle));
            _logAction = logAction;
        }

        public IFeatureToggle PrimaryToggle { get; }
        public IFeatureToggle FallbackToggle { get; }

        public bool FeatureEnabled
        {
            get
            {
                try
                {
                    return PrimaryToggle.FeatureEnabled;
                }
                catch (Exception ex)
                {
                    _logAction?.Invoke(ex);

                    return FallbackToggle.FeatureEnabled;
                }
            }
        }
    }
}