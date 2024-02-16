using ProjetDevSys;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ProjetDevSysGraphical
{
    public static class ResourceHelper
    {

        private static readonly ResourceManager ResourceManager = new ResourceManager("ProjetDevSysGraphical.Messages", typeof(ResourceHelper).Assembly);

        public static string GetString(string key)
        {
            return ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
        }
    }

    [MarkupExtensionReturnType(typeof(string))]
    public class LangageExtension : MarkupExtension
    {

        public string Key { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {

            return ResourceHelper.GetString(Key) ?? Key;
        }
    }

}
