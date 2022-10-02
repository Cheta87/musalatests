using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.IE;
using System.Collections.ObjectModel;
using MusalaTests.Configuration;

namespace MusalaTests
{
    public class UtilizationMethods
    {
        [TestInitialize]
        public WebDriver Initialize(BrowserType browser, string url)
        {
            WebDriver _webDriver = null;
            switch (browser)
            {
                case BrowserType.IExplorer:
                    _webDriver = new InternetExplorerDriver();
                    break;
                case BrowserType.Firefox:

                    _webDriver = new FirefoxDriver("C:\\Users\\nikola.chetoroshka\\source\\repos\\Runner\\bin\\Debug\\net6.0");
                    break;
                case BrowserType.Chrome:
                    _webDriver = new ChromeDriver();
                    break;
            }
            _webDriver.Manage().Window.Maximize();
            _webDriver.Navigate().GoToUrl(url);

            return _webDriver;
        }
        [TestMethod]
        internal void NavigateMainMenu(ReadOnlyCollection<IWebElement> navElements,string navElement)
        {
            foreach (IWebElement el in navElements)
            {
                string navElementText = el.Text;
                if (navElementText == navElement)
                {
                    el.Click();
                    break;
                }
            }
        }

    }
}
