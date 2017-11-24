using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Sma.Stm.Common.PlugIns
{
    public class PluginLoader<T>
        where T : class
    {
        public List<T> Plugins { get; set; }

        public PluginLoader(string Path)
        {
            Plugins = new List<T>();

            var Directory = new DirectoryInfo(Path);
            if (!Directory.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            var files = Directory.GetFiles("*.dll");
            if (files != null && files.Length > 0)
            {
                foreach (FileInfo fi in files)
                {
                    var pluginsInAssembly = LoadPluginFromAssembly(fi.FullName);
                    if (pluginsInAssembly != null && pluginsInAssembly.Count > 0)
                    {
                        Plugins.AddRange(pluginsInAssembly);
                    }
                }
            }
        }

        private List<T> LoadPluginFromAssembly(string Filename)
        {
            var results = new List<T>();
            var pluginType = typeof(T);

            Assembly assembly = Assembly.LoadFrom(Filename);
            if (assembly == null)
            {
                return results;
            }

            Type[] types = assembly.GetExportedTypes();
            foreach (Type t in types)
            {
                if (!t.IsClass || t.IsNotPublic)
                {
                    continue;
                }

                if (pluginType.IsAssignableFrom(t))
                {
                    var instance = Activator.CreateInstance(t);
                    if (instance != null && instance is T plugin)
                    {
                        results.Add(plugin);
                    }
                }
            }

            return results;
        }
    }
}