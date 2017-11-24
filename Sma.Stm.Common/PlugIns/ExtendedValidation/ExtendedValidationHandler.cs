using System;
using System.Collections.Generic;
using System.Text;

namespace Sma.Stm.Common.PlugIns.ExtendedValidation
{
    public class ExtendedValidationHandler
    {
        private PluginLoader<IExtendedValidator> _pluginsLoader;

        public ExtendedValidationHandler (string pluginPath)
        {
            _pluginsLoader = new PluginLoader<IExtendedValidator>(pluginPath);
        }

        public List<string> Validate(string xml)
        {
            var result = new List<string>();

            foreach (var validator in _pluginsLoader.Plugins)
            {
                var vr = validator.Validate(xml);
                result.AddRange(vr);
            }

            return result;
        }
    }
}