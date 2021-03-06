﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TechTalk.SpecFlow.Configuration
{
    //TODO: merge it with the real RuntimeConfiguration
    public class RuntimeConfigurationForGenerator
    {
        private List<string> _additionalStepAssemblies = new List<string>();

        //language settings
        public CultureInfo ToolLanguage { get; set; }
        public CultureInfo BindingCulture { get; set; }

        //unit test framework settings
        //public Type RuntimeUnitTestProviderType { get; set; }

        //runtime settings
        public bool DetectAmbiguousMatches { get; set; }
        public bool StopAtFirstError { get; set; }
        public MissingOrPendingStepsOutcome MissingOrPendingStepsOutcome { get; set; }

        //tracing settings
        //public Type TraceListenerType { get; set; }
        public bool TraceSuccessfulSteps { get; set; }
        public bool TraceTimings { get; set; }
        public TimeSpan MinTracedDuration { get; set; }

        public IEnumerable<string> AdditionalStepAssemblies
        {
            get
            {
                return _additionalStepAssemblies;
            }
        }

        public RuntimeConfigurationForGenerator()
        {
            ToolLanguage = CultureInfo.GetCultureInfo(ConfigDefaults.FeatureLanguage);
            BindingCulture = null;

            //SetUnitTestDefaultsByName(ConfigDefaults.UnitTestProviderName);

            DetectAmbiguousMatches = ConfigDefaults.DetectAmbiguousMatches;
            StopAtFirstError = ConfigDefaults.StopAtFirstError;
            MissingOrPendingStepsOutcome = ConfigDefaults.MissingOrPendingStepsOutcome;

            //TraceListenerType = typeof(DefaultListener);
            TraceSuccessfulSteps = ConfigDefaults.TraceSuccessfulSteps;
            TraceTimings = ConfigDefaults.TraceTimings;
            MinTracedDuration = TimeSpan.Parse(ConfigDefaults.MinTracedDuration);
        }

        internal RuntimeConfigurationForGenerator UpdateFromConfigFile(ConfigurationSectionHandler configSection)
        {
            if (configSection == null) throw new ArgumentNullException("configSection");

            var config = this;
            if (configSection.Language != null)
            {
                config.ToolLanguage = string.IsNullOrEmpty(configSection.Language.Tool) ?
                    CultureInfo.GetCultureInfo(configSection.Language.Feature) :
                    CultureInfo.GetCultureInfo(configSection.Language.Tool);
            }

            if (configSection.BindingCulture.ElementInformation.IsPresent)
            {
                config.BindingCulture = CultureInfo.GetCultureInfo(configSection.BindingCulture.Name);
            }

//            if (configSection.UnitTestProvider != null)
//            {
//                config.SetUnitTestDefaultsByName(configSection.UnitTestProvider.Name);
//
//                if (!string.IsNullOrEmpty(configSection.UnitTestProvider.RuntimeProvider))
//                    config.RuntimeUnitTestProviderType = GetTypeConfig(configSection.UnitTestProvider.RuntimeProvider);
//
                //TODO: config.CheckUnitTestConfig();
//            }

            if (configSection.Runtime != null)
            {
                config.DetectAmbiguousMatches = configSection.Runtime.DetectAmbiguousMatches;
                config.StopAtFirstError = configSection.Runtime.StopAtFirstError;
                config.MissingOrPendingStepsOutcome = configSection.Runtime.MissingOrPendingStepsOutcome;
            }

            if (configSection.Trace != null)
            {
//                if (!string.IsNullOrEmpty(configSection.Trace.Listener))
//                    config.TraceListenerType = GetTypeConfig(configSection.Trace.Listener);

                config.TraceSuccessfulSteps = configSection.Trace.TraceSuccessfulSteps;
                config.TraceTimings = configSection.Trace.TraceTimings;
                config.MinTracedDuration = configSection.Trace.MinTracedDuration;
            }

            foreach (var element in configSection.StepAssemblies)
            {
                var stepAssembly = ((StepAssemblyConfigElement)element).Assembly;
                config._additionalStepAssemblies.Add(stepAssembly);
            }

            return config;
        }
    }
}