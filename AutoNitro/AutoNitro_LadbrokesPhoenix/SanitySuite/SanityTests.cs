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
    public class SanityTests : BaseTest
    {
        TestRepository.LoginLogout.LoginLogoutFunctions BVTloginLogoutObj = new TestRepository.LoginLogout.LoginLogoutFunctions();
        TestRepository.Betslip.BetslipFunctions BVTbetslipObj = new TestRepository.Betslip.BetslipFunctions();
        TestRepository.Common BVTcommonObj = new TestRepository.Common();
        Framework.Common.Common BVTframeworkCommonObj = new Framework.Common.Common();


        [Test]
        public void VerifyLoginLogout()
        {
            try
            {
                Console.WriteLine("***** Executing Test Case --- 'VerifyLoginLogout', Verify Details on Login/Logout *****");
                BVTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BVTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BVTloginLogoutObj.VerifyDetailsOnLogin(MyBrowser);
                BVTloginLogoutObj.Logout(MyBrowser);
                BVTloginLogoutObj.VerifyDetailsOnLogout(MyBrowser);
                Console.WriteLine("TestCase 'VerifyLoginLogout' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLoginLogout");
                Console.WriteLine("TestCase 'VerifyLoginLogout' - FAIL");
                Fail(ex.Message);
            }
        }


        [Test]
        public void VerifyBetPlacement()
        {
            string[] aryOdd = new string[1];
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "PreProdEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetPlacement', Verify Single Bet Placement *****");
            try
            {
                BVTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BVTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BVTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                BVTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BVTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                BVTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                BVTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                BVTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);
                Console.WriteLine("TestCase 'VerifyBetPlacement' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetPlacement");
                Console.WriteLine("TestCase 'VerifyBetPlacement' - FAIL");
                Fail(ex.Message);
            }
        }


    }//end class
}//end namespace

