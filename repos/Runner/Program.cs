using MusalaTests;
using MusalaTests.Configuration;
using MusalaTests.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using System.Diagnostics;

namespace Runner
{
    public enum TestStatus
    {
        NotRun,
        Passed,
        Failed,
        Broken
    }

    public class TestContext
    {
        public Dictionary<string, TestStatus> Results { get; set; } = new Dictionary<string, TestStatus>();


        public List<Action> Tests { get; set; } = new List<Action>();

        public Action Setup { get; set; }

        public Action CleanUp { get; set; }
        public IWebDriver Driver { get; set; }



        public void AddTest(Action test)
        {
            Tests.Add(() =>
            {
                try
                {
                    Setup();
                    test();
                    Results.Add(test.Method.Name, TestStatus.Passed);
                }
                catch (Exception ex)
                {
                    var broken = ex.GetType().Name != "AssertFailedException";
                    if (broken)
                    {
                        Results.Add(test.Method.Name, TestStatus.Broken);
                        Console.WriteLine("The following test is broken: " + test.Method.Name + ". Exception message: " + ex.GetType().Name);
                    }
                    else
                    {
                        Results.Add(test.Method.Name, TestStatus.Failed);
                    }
                }
                finally
                {
                    CleanUp();
                }
            });
        }

        public void RunTests()
        {
            foreach (var test in Tests)
            {
                test();
            }
        }

        public void Report()
        {
            foreach (var result in Results)
            {
                Console.WriteLine(result);
            }
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            IConfig config = new AppConfigReader();
            var baseURL = config.GetBaseURL();
            var ctxChrome = new TestContext();
            var ctxFirefox = new TestContext();
            var chromeTests = new MusalaTest();
            var firefoxtests = new MusalaTest();

            ctxChrome.Setup = () =>
            {
                ctxChrome.Driver = new UtilizationMethods().Initialize(BrowserType.Chrome, baseURL);
                chromeTests.webDriver = ctxChrome.Driver;
            };
            ctxChrome.CleanUp = () => { ctxChrome.Driver.Quit(); };

            ctxFirefox.Setup = () => { ctxFirefox.Driver = new UtilizationMethods().Initialize(BrowserType.Firefox, baseURL); firefoxtests.webDriver = ctxFirefox.Driver; };
            ctxFirefox.CleanUp = () => { ctxFirefox.Driver.Quit(); };

            var rootDir = JsonHelper.GetProjectRootDirectory();

            //single context
            ctxChrome.AddTest(chromeTests.TestCase1);
            ctxChrome.AddTest(chromeTests.TestCase2);
            ctxChrome.AddTest(chromeTests.TestCase3);
            ctxChrome.AddTest(chromeTests.TestCase4);
            //ctxFirefox.AddTest(firefoxtests.TestCase1);
            //ctxFirefox.AddTest(firefoxtests.TestCase2);
            //ctxFirefox.AddTest(firefoxtests.TestCase3);
            //ctxFirefox.AddTest(firefoxtests.TestCase4);
            ctxChrome.RunTests();
            //ctxFirefox.RunTests();
            ctxChrome.Report();
            //ctxFirefox.Report();

            //parallel
            /*var chromeTask = Task.Run(ctxChrome.RunTests);
            var FirefoxTask = Task.Run(ctxFirefox.RunTests);
            Task.WaitAll(chromeTask, FirefoxTask);
            ctxChrome.Report();
            ctxFirefox.Report();*/

            Console.ReadLine();
        }
    }
}