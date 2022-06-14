using System;
using System.Linq;

namespace FeatureToggle
{
    public class CompositeOrDecorator : IFeatureToggle
    {
        public IFeatureToggle[] WrappedToggles { get; }

        public CompositeOrDecorator(params IFeatureToggle[] togglesToWrap)
        {
            if (togglesToWrap == null)
            {
                throw new ArgumentNullException(nameof(togglesToWrap));
            }

            if (!togglesToWrap.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(togglesToWrap), "At least one toggle must be supplied");
            }

            WrappedToggles = togglesToWrap;
        }

        public bool FeatureEnabled
        {
            get
            {
                return WrappedToggles.Any(x => x.FeatureEnabled);                
            }
        }
    }
}