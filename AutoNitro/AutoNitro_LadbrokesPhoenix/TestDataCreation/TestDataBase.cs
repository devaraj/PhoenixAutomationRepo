using Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using MbUnit.Framework;
using System.Diagnostics;
using Framework;

namespace TestDataCreation
{
    public class TestDataBase
    {
        public ISelenium MyBrowser;
        public IWebDriver WebDriverObj;

        /// <summary>
        /// To Launch the browser & to login to Open Bet application
        /// </summary>
        ///  Author: 
        /// <returns>None</returns>
        /// Created Date: 16-Jan-2012
        /// Modified Date: 
        /// Modification Comments:
        [SetUp]
        public void Init()
        {
            //Deleting cookies in ie browser through command line.
            /*var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2");
            var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
            proc.Start();
            WebDriverObj = new InternetExplorerDriver();
            */
            string URL = "https://stg-gib.ladbrokes.com/admin";
            WebDriverObj = new FirefoxDriver();
            MyBrowser = new WebDriverBackedSelenium(WebDriverObj, URL);
            MyBrowser.Start();
            WebDriverObj.Manage().Window.Maximize();
            MyBrowser.WindowMaximize();
            
            MyBrowser.Open(URL);
            MyBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut); 
            MyBrowser.Refresh();
            MyBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            MyBrowser.Type(TestDataCreation.ControlsRepository.UsrNmeTxtBx, "Automation1");
            MyBrowser.Type(TestDataCreation.ControlsRepository.PwdTxtBx, "aditi123");
            MyBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            MyBrowser.Click(TestDataCreation.ControlsRepository.LoginBtn);
            MyBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);            
        }

        [TearDown]
        public virtual void Cleanup()
        {
            Process[] process = new Process[1];
            //process = Process.GetProcessesByName("iexplore");
            process = Process.GetProcessesByName("firefox");
            foreach (Process p in process)
            {
                p.Kill();
            }

        }
    }
}
