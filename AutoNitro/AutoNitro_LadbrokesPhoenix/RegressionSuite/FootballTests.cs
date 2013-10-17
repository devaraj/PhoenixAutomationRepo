using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

using Selenium;
using OpenQA.Selenium;
using MbUnit.Framework;
using Framework;
using Framework.Common;
using TestRepository.ControlsRepository;


namespace PreProdSuite
{
    [TestFixture(ApartmentState = ApartmentState.STA, TimeOut = FrameGlobals.TestCaseTimeout)]
    public class FootballTests : BaseTest
    {
        TestRepository.LoginLogout.LoginLogoutFunctions FTloginLogoutObj = new TestRepository.LoginLogout.LoginLogoutFunctions();
        TestRepository.Betslip.BetslipFunctions FTbetslipObj = new TestRepository.Betslip.BetslipFunctions();
        TestRepository.Common FTcommonObj = new TestRepository.Common();
        Framework.Common.Common FTframeworkCommonObj = new Framework.Common.Common();
        

        /// <summary>
        /// To validate the display of markets and Events in their respective pages
        /// </summary>
        /// <TestCaseId>188</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void Validate_MarketAndEventPage()
        {
            TestData[] testData = new TestData[1];
            testData[0] = new TestData(27, "BetSlipTestData");

            Console.WriteLine("***** Executing Test Case 188 ***** 'Validate_MarketAndEventPage',Potential returns displayed when price is changed from SP to fixed price");
            try
            {
                FTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                FTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                FTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");
                FTbetslipObj.NavigateToSportsPage(MyBrowser, "Football", "Highlights", "");
                Console.WriteLine("TestCase 'Validate_MarketAndEventPage' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "Validate_MarketAndEventPage");
                Console.WriteLine("TestCase : 188 'Validate_MarketAndEventPage' - FAIL");
                Fail(ex.Message);
            }
        }


    }//end class
}//end namespace

