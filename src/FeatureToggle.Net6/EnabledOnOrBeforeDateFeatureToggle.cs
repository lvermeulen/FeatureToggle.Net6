using System;
using FeatureToggle.Internal;

namespace FeatureToggle
{
    public abstract class EnabledOnOrBeforeDateFeatureToggle : IFeatureToggle
    {
        protected EnabledOnOrBeforeDateFeatureToggle()
        {
            NowProvider = () => DateTime.Now;
            ToggleValueProvider = new AppSettingsProvider();
        }

        public Func<DateTime> NowProvider { get; set; }

        public virtual IDateTimeToggleValueProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled => NowProvider.Invoke() <= ToggleValueProvider.EvaluateDateTimeToggleValue(this);
    }
}
