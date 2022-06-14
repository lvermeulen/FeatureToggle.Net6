using System;
using System.Reflection;
using FeatureToggle.Internal;

namespace FeatureToggle
{
    public abstract class EnabledOnOrAfterAssemblyVersionWhereToggleIsDefinedToggle : IFeatureToggle
    {
        protected EnabledOnOrAfterAssemblyVersionWhereToggleIsDefinedToggle()
        {
            ToggleValueProvider = new AppSettingsProvider();
        }

        public virtual IAssemblyVersionProvider ToggleValueProvider { get; set; }

        public bool FeatureEnabled
        {
            get
            {
                var assemblyVersionOfDerivedToggle = GetAssemblyVersionOfDerivedToggle();

                var configuredToggleVersion = GetConfiguredVersion();

                return assemblyVersionOfDerivedToggle >= configuredToggleVersion;
            }
        }

        private Version GetAssemblyVersionOfDerivedToggle()
        {
            var assemblyName = GetType().GetTypeInfo().Assembly.FullName!;

            var assembly = new AssemblyName(assemblyName);

            return assembly.Version;
        }

        private Version GetConfiguredVersion()
        {
            return ToggleValueProvider.EvaluateVersion(this);
        }
    }
}