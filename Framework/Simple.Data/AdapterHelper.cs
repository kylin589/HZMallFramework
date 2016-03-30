﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simple.Data
{
    class AdapterHelper
    {
        private static IAdapter ComposeAdapter(string contractName)
        {
            using (var container = CreateContainer())
            {
                var export = container.GetExport<IAdapter>(contractName);
                if (export == null) throw new ArgumentException("Unrecognised file.");
                return export.Value;
            }
        }

        private static CompositionContainer CreateContainer()
        {
            var path = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
            path = Path.GetDirectoryName(path);
            if (path == null) throw new ArgumentException("Unrecognised file.");

            var catalog = new DirectoryCatalog(path, "Simple.Data.*.dll");
            return new CompositionContainer(catalog);
        }
    }
}
