using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys
{
    public static class ResourceHelper
    {

        private static readonly ResourceManager ResourceManager = new ResourceManager("ProjetDevSys.Messages", typeof(ResourceHelper).Assembly);

        public static string GetString(string key)
        {
            return ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
        }
    }
}
