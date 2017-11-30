using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sma.Stm.Common.PlugIns
{
    public class PluginLoader<T>
        where T : class
    {
        public List<T> Plugins { get; }

        public PluginLoader(string path)
        {
            Plugins = new List<T>();

            var directory = new DirectoryInfo(path);
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            var files = directory.GetFiles("*.dll");
            if (files == null || files.Length <= 0)
                return;

            foreach (var fi in files)
            {
                var pluginsInAssembly = LoadPluginFromAssembly(fi.FullName);
                if (pluginsInAssembly != null && pluginsInAssembly.Count > 0)
                {
                    Plugins.AddRange(pluginsInAssembly);
                }
            }
        }

        private static List<T> LoadPluginFromAssembly(string filename)
        {
            var results = new List<T>();
            var pluginType = typeof(T);

            var assembly = Assembly.LoadFrom(filename);
            if (assembly == null)
            {
                return results;
            }

            var types = assembly.GetExportedTypes();
            foreach (var t in types)
            {
                if (!t.IsClass || t.IsNotPublic)
                {
                    continue;
                }

                if (!pluginType.IsAssignableFrom(t))
                    continue;

                var instance = Activator.CreateInstance(t);
                if (instance != null && instance is T plugin)
                {
                    results.Add(plugin);
                }
            }

            return results;
        }
    }
}