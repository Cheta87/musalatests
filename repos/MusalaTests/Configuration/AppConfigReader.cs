using MusalaTests.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusalaTests.Configuration
{
    public class AppConfigReader : IConfig
    {

        public string GetBaseURL()
        {
            return ConfigurationManager.AppSettings.Get(AppConfigKeys.BaseURL);
        }

        public BrowserType GetBrowser()
        {
            string browser = ConfigurationManager.AppSettings.Get(AppConfigKeys.Browser);
            return (BrowserType)Enum.Parse(typeof(BrowserType), browser); 
        }
    }
}
