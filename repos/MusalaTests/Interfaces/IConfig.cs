using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MusalaTests.Configuration;

namespace MusalaTests.Interfaces
{
    public interface IConfig
    {
        BrowserType GetBrowser();
        string GetBaseURL();
    }
}
