using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System;
using System.Configuration;
using MusalaTests.Interfaces;
using MusalaTests.Configuration;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace MusalaTests
{
    [TestClass]
    public class MusalaTest
    {
        UtilizationMethods utilizationMethods = new UtilizationMethods();
        IConfig config = new AppConfigReader();
        public IWebDriver webDriver { get; set; }


        [TestInitialize]
        public void SetUp()
        {
            var browser = config.GetBrowser();
            var baseUrl = config.GetBaseURL();
            webDriver = utilizationMethods.Initialize(browser, baseUrl);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Console.WriteLine("From cleanup");
        }
        

        [TestMethod]
        public void TestCase1()
        {
            List<string> emails = JsonHelper.GetTestDataArray("C:\\Users\\nikola.chetoroshka\\source\\repos\\MusalaTests\\EmailTestData.json", "emailData");
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
            var contactUsButton = webDriver.FindElement(By.CssSelector("button[class='contact-label btn btn-1b']"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", contactUsButton);
            contactUsButton.Click();

            var contactName = webDriver.FindElement(By.Id("cf-1"));
            var contactEmail = webDriver.FindElement(By.Id("cf-2"));
            var contactMobile = webDriver.FindElement(By.Id("cf-3"));
            var contactSubject = webDriver.FindElement(By.Id("cf-4"));
            var contactMessage = webDriver.FindElement(By.Id("cf-5"));
            var contactSubmit = webDriver.FindElement(By.CssSelector("input[class='wpcf7-form-control has-spinner wpcf7-submit btn-cf-submit']"));
            var closeFormBtn = webDriver.FindElement(By.CssSelector("#fancybox-close"));
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));

            int index = 0;
            foreach (var email in emails)
            {                            
                if (index > 0)
                {
                    contactUsButton.Click();
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("cf-2")));
                    contactName.Clear();
                    contactEmail.Clear();
                    contactMobile.Clear();
                    contactSubject.Clear();
                    contactMessage.Clear();
                }
                index++;
                contactName.SendKeys("test");
                contactEmail.SendKeys(email);
                contactMobile.SendKeys("123");
                contactSubject.SendKeys("testing");
                Assert.AreEqual("The e-mail address entered is invalid.", webDriver.FindElement(By.CssSelector("span.wpcf7-not-valid-tip")).Text);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("fancybox-close")));
                closeFormBtn.Click();
            }
        }

        [TestMethod]
        public void TestCase2()
        {
            var companyNavElements = webDriver.FindElements(By.CssSelector("#menu-main-nav-1 > li > a"));
            utilizationMethods.NavigateMainMenu(companyNavElements, "COMPANY");
            var url = webDriver.Url;
            Assert.AreEqual("https://www.musala.com/company/", url);
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
            var leadershipSection = webDriver.FindElement(By.CssSelector(".company-members"));
            Assert.IsNotNull(leadershipSection);

            var footerSection = webDriver.FindElement(By.CssSelector(".footerEl-wrapper"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", footerSection);
            var acceptCookiesBtn = webDriver.FindElement(By.Id("wt-cli-accept-all-btn"));
            if (acceptCookiesBtn != null)
            {
                acceptCookiesBtn.Click();
            }
            var externalLinks = webDriver.FindElements(By.CssSelector(".links-buttons > a"));
            foreach (IWebElement link in externalLinks)
            {
                var linkHref = link.GetAttribute("href");
                if (linkHref.Contains("facebook"))
                {
                    link.Click();
                    break;
                }
            }
            var browserTabs = new List<string>(webDriver.WindowHandles);
            var newTab = browserTabs[1];
            var facebookUrl = webDriver.SwitchTo().Window(newTab).Url;
            var facebookTitle = webDriver.Title;
            if (facebookTitle.Contains("Musala Soft"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
            Assert.AreEqual("https://www.facebook.com/MusalaSoft?fref=ts", facebookUrl);
        }

        [TestMethod]
        public void TestCase3()
        {
            var companyNavElements = webDriver.FindElements(By.CssSelector("#menu-main-nav-1 > li > a"));
            utilizationMethods.NavigateMainMenu(companyNavElements, "CAREERS");
            webDriver.FindElement(By.CssSelector("section.link_field > div.link-wrapper > a")).Click();
            var joinUsUrl = webDriver.Url;
            Assert.AreEqual("https://www.musala.com/careers/join-us/", joinUsUrl);
            var locations = webDriver.FindElement(By.Name("get_location"));
            var selectElement = new SelectElement(locations);
            selectElement.SelectByValue("Anywhere");
            webDriver.FindElement(By.CssSelector("a.card-jobsHot__link")).Click();

            var mainSections = webDriver.FindElements(By.CssSelector(".content-title > h2"));
            foreach (var section in mainSections)
            {
                if (section.Text == "General description" || section.Text == "Requirements" || section.Text == "Responsibilities" || section.Text == "What we offer")
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail();
                }
            }
            IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;
            var applyBtn = webDriver.FindElement(By.CssSelector("div.btn-apply-container > a > input"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", applyBtn);
            Assert.IsNotNull(applyBtn);
            applyBtn.Click();

            var cvName = webDriver.FindElement(By.Id("cf-1"));
            var cvEmail = webDriver.FindElement(By.Id("cf-2"));
            var cvMobile = webDriver.FindElement(By.Id("cf-3"));
            var cvUploadFile = webDriver.FindElement(By.Id("uploadtextfield"));
            var cvConsent = webDriver.FindElement(By.Id("adConsentChx"));
            var cvMessage = webDriver.FindElement(By.Id("cf-6"));
            var cvSubmitBtn = webDriver.FindElement(By.CssSelector("div.btn-cf-wrapper > p > input"));
            var cvClosePopUpForm = webDriver.FindElement(By.CssSelector("button.close-form"));
            var cvCloseFormBtn = webDriver.FindElement(By.CssSelector("#fancybox-close"));

            cvName.SendKeys("test");
            cvMobile.SendKeys("123");
            cvConsent.Click();
            cvSubmitBtn.Click();
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button.close-form")));
            cvClosePopUpForm.Click();
            var cvErrorMsg = webDriver.FindElement(By.CssSelector("span.wpcf7-not-valid-tip")).Text;
            Assert.AreEqual("The field is required.", cvErrorMsg);

            cvCloseFormBtn.Click();
            applyBtn.Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("cf-2")));
            cvName.Clear();
            cvMobile.Clear();
            cvName.SendKeys("test");
            cvEmail.SendKeys("test@test");
            cvMobile.SendKeys("123");
            cvMessage.SendKeys("testing");
            cvUploadFile.SendKeys(@"C:\Users\nikola.chetoroshka\Desktop\cv_test.txt");
            cvSubmitBtn.Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button.close-form")));
            cvClosePopUpForm.Click();
            var cvMailErrorMsg = webDriver.FindElement(By.CssSelector("span.wpcf7-not-valid-tip")).Text;
            Assert.AreEqual("The e-mail address entered is invalid.", cvMailErrorMsg);
        }

        [TestMethod]
        public void TestCase4()
        {
            var companyNavElements = webDriver.FindElements(By.CssSelector("#menu-main-nav-1 > li > a"));
            utilizationMethods.NavigateMainMenu(companyNavElements,"CAREERS");
            webDriver.FindElement(By.CssSelector("section.link_field > div.link-wrapper > a")).Click();
            var location = webDriver.FindElement(By.Name("get_location"));
            var selectElement = new SelectElement(location);
            selectElement.SelectByValue("Sofia");
            var sofiaOpenPositions = webDriver.FindElements(By.CssSelector(".card-jobsHot"));
            Console.Write("Sofia: \n");
            foreach (var position in sofiaOpenPositions)
            {
                var positionUrl = position.FindElement(By.CssSelector("div > a")).GetAttribute("href");
                var positionTitle = position.FindElement(By.CssSelector("h2.card-jobsHot__title")).Text;
                Console.WriteLine("Position: " + positionTitle + "\nMore Info: "+ positionUrl);
            }
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("get_location")));
            location = webDriver.FindElement(By.Name("get_location"));
            selectElement = new SelectElement(location);
            selectElement.SelectByValue("Skopje");
            var skopjeOpenPositions = webDriver.FindElements(By.CssSelector(".card-jobsHot"));
            Console.Write("Skopje: \n");
            foreach (var position in skopjeOpenPositions)
            {
                var positionUrl = position.FindElement(By.CssSelector("div > a")).GetAttribute("href");
                var positionTitle = position.FindElement(By.CssSelector("h2.card-jobsHot__title")).Text;
                Console.WriteLine("Position: " + positionTitle + "\nMore Info: " + positionUrl);
            }
        }
    }
}
