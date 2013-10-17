using MbUnit.Framework;
using System.Diagnostics;
using Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Framework;


namespace AdminSuite
{

    public class AdminBase
    {

        public ISelenium MyBrowser;
        public IWebDriver FfDriver;

        [SetUp]
        // UpdateResourceFile("pradeep","@E:\AdminScripts\ECommerce_CodeBase\PreRequisite_Suite\Resources\Customers.resx");

        /// <summary> To Launch the browser & to login to Open Bet application
        /// </summary>
        ///  Author: Yogesh
        /// Ex: AdminBase.init()
        /// <returns>None</returns>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 
        /// Modification Comments:
        public void Init()
        {
            FfDriver = new FirefoxDriver();
            MyBrowser = new WebDriverBackedSelenium(FfDriver, "https://stg-gib.ladbrokes.com/admin");
            MyBrowser.Start();
            MyBrowser.Open("https://stg-gib.ladbrokes.com/admin");
            MyBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            MyBrowser.Type(AdminSuite.CommonControls.AdminHomePage.UsrNmeTxtBx, "sanjeeva_p");
            MyBrowser.Type(AdminSuite.CommonControls.AdminHomePage.PwdTxtBx, "123456");
            MyBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            MyBrowser.Click(AdminSuite.CommonControls.AdminHomePage.LoginBtn);
            MyBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
        }

        [TearDown]
        public virtual void Cleanup()
        {
            Process[] process = Process.GetProcessesByName("firefox");
            foreach (Process p in process)
            {
                p.Kill();
            }
        }
    }
}
