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
    public class LoginLogoutTests : BaseTest
    {
        TestRepository.LoginLogout.LoginLogoutFunctions LTloginLogoutObj = new TestRepository.LoginLogout.LoginLogoutFunctions();
        TestRepository.Betslip.BetslipFunctions LTbetslipObj = new TestRepository.Betslip.BetslipFunctions();
        TestRepository.Common LTcommonObj = new TestRepository.Common();
        Framework.Common.Common LTframeworkCommonObj = new Framework.Common.Common();
        AdminSuite.Common admincommonObj = new AdminSuite.Common();


        [Test]
        public void VerifyDetailsOnLoginLogout()
        {
            Console.WriteLine("***** Executing Test Case --- 'VerifyDetailsOnLoginLogout', Verify Details on Login/Logout *****");
            try
            {
                LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                LTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                LTloginLogoutObj.VerifyDetailsOnLogin(MyBrowser);

                LTloginLogoutObj.Logout(MyBrowser);
                LTloginLogoutObj.VerifyDetailsOnLogout(MyBrowser);
                Console.WriteLine("TestCase 'VerifyDetailsOnLoginLogout' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyDetailsOnLoginLogout");
                Console.WriteLine("TestCase 'VerifyDetailsOnLoginLogout' - FAIL");
                Fail(ex.Message);
            }
        }



        [Test]
        public void VerifyLoginErrorMessages()
        {
            ISelenium adminBrowser;
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string username, password, actErrorMessage, expErrorMessage;
            username = dt.Rows[10]["UserName"].ToString();
            password = dt.Rows[10]["Password"].ToString();
            Console.WriteLine("***** Executing Test Case --- 'VerifyLoginErrorMessages', Verifythe error messages displayed on using invalid user credentials *****");
            try
            {            
                LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                LTcommonObj.selectMenuButton(MyBrowser);
                LTcommonObj.clickObject(MyBrowser, LoginLogoutControls.loginOrRegisterLink);

                #region Verify Error messages
                actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, "Incorrect Username", "Incorrect Password");
                expErrorMessage = "The username and/or password you have provided is incorrect. You have 0 attempts remaining. Please try again.";
                Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                Console.WriteLine("Login error message(Attempt to Login with incorrect Username and Password) validated successfully");

                actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, "Incorrect Username", password);
                expErrorMessage = "The username and/or password you have provided is incorrect. You have 0 attempts remaining. Please try again.";
                Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                Console.WriteLine("Login error message(Attempt to Login with incorrect Username) validated successfully");

                actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, "", "");
                expErrorMessage = "username and password cannot be empty.";
                Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                Console.WriteLine("Login error message(Attempt to Login leaving username and Password fields blank) validated successfully");

                actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, "", password);
                expErrorMessage = "username cannot be empty.";
                Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                Console.WriteLine("Login error message(Attempt to login leaving the Username blank) validated successfully");

                actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, username, "");
                expErrorMessage = "password cannot be empty.";
                Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                Console.WriteLine("Login error message(Attempt to login leaving the Password blank) validated successfully");


                actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, username, "Incorrect Password");
                expErrorMessage = "The username and/or password you have provided is incorrect. You have 3 attempts remaining. Please try again.";
                Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                Console.WriteLine("Login error message(Attempt to login with incorrect Password) validated successfully");
                
                Console.WriteLine("Function 'VerifyLoginErrorMessages' - Pass");
                #endregion
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLoginErrorMessages");
                Console.WriteLine("Function 'VerifyLoginErrorMessages' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Unlock the locked customer in OB
                adminBrowser = admincommonObj.LogOnToAdmin();
                admincommonObj.UnLockTheLockedUser(adminBrowser, username);
                LTcommonObj.KillAdminObject();
            }
        }



        [Test]
        public void VerifyBetSlipDetailsWhenUserLoggedinLoggedout()
        {
            string[] aryOdd = new string[1];
            string xPath;
            int BetslipCnt;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "PreProdEvents");
            aryOdd[0] = testDataLst[0].Odds;
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetSlipDetailsWhenUserLoggedinLoggedout', Verify the BetSlip details when user logged in/Logged out *****");
            try
            {                
                LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                LTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region Verify Betslip details
                LTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                LTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                //Enter Stake and verify the bet details
                LTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", false);
                LTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");

                //Login and Verify the bet details                 
                LTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                LTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                Console.WriteLine("Verification of Betslip on User Login was successful");

                //Logout and verify Bet details
                LTloginLogoutObj.Logout(MyBrowser);
                BetslipCnt = LTbetslipObj.GetBetslipCount(MyBrowser);
                Assert.IsTrue(BetslipCnt.Equals(0), "Betslip counter failed to decrease to 0 on Logout");
                LTcommonObj.clickObject(MyBrowser, BetslipControls.betslipButton);
                TimeSpan ts = new TimeSpan(0, 0, 5);
                driver.Manage().Timeouts().ImplicitlyWait(ts);
                xPath = "//div[@class='slip-item']//span[contains(text(), '" + testDataLst[0].EventName + "')]/following-sibling::span[contains(text(), '" + testDataLst[0].SelectionName + "')]/following::span[contains(text(), '" + String.Format("{0:0.00}", double.Parse(testDataLst[0].Odds)) + "')]";
                Assert.IsFalse(MyBrowser.IsElementPresent(xPath), "Failed to removed Event-Selection-Odd '" + testDataLst[0].EventName + "-" + testDataLst[0].SelectionName + "-" + testDataLst[0].Odds + "' on Logout");
                Assert.IsTrue(MyBrowser.IsTextPresent("Your betslip is empty"), "Betslip empty message was not displayed");

                Console.WriteLine("Verification of Betslip on User Login was successful");
                Console.WriteLine("TestCase 'VerifyBetSlipDetailsWhenUserLoggedinLoggedout' - PASS");
                #endregion
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetSlipDetailsWhenUserLoggedinLoggedout");
                Console.WriteLine("TestCase 'VerifyBetSlipDetailsWhenUserLoggedinLoggedout' - FAIL");
                Fail(ex.Message);
            }
        }



        [Test]
        public void VerifyBetplacement_UserNotLoggedIn()
        {
            string[] aryOdd = new string[1];
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "PreProdEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_UserNotLoggedIn', Verify the Bet Placement when user is NOT Logged in *****");
            try
            {
                LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                LTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region Verify Betplacement               
                LTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                LTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);
                LTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                LTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                LTcommonObj.clickObject(MyBrowser, BetslipControls.placeBet);
                
                // Check if the user is taken to Login page
                LTcommonObj.EnterField(MyBrowser, LoginLogoutControls.loginUsernameTextBox, FrameGlobals.UserName);
                LTcommonObj.EnterField(MyBrowser, LoginLogoutControls.loginPasswordTextBox, FrameGlobals.PassWord);
                MyBrowser.FireEvent(LoginLogoutControls.loginSubmitButton, "click");
                LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);

                // Check if the user is taken back to the betslip on Login In 
                Assert.IsTrue(MyBrowser.IsVisible(BetslipControls.betslipBanner), "User is not taken to betslip page on Login");
                string xPath = "//div[@class='slip-item']//span[contains(text(), '" + testDataLst[0].EventName + "')]/following-sibling::span[contains(text(), '" + testDataLst[0].SelectionName + "')]/following::span[contains(text(), '" + testDataLst[0].Odds + "')]";
                Assert.IsTrue(MyBrowser.IsElementPresent(xPath), "Event-Selection-Odd '" + testDataLst[0].EventName + "-" + testDataLst[0].SelectionName + "-" + testDataLst[0].Odds + "' was not found in the Betslip");
                LTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");

                //Place bet and validate the receipt
                //LTbetslipObj.BetPlacement(MyBrowser, "Home");
                LTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);
                Console.WriteLine("TestCase 'VerifyBetplacement_UserNotLoggedIn' - PASS");
                #endregion
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_UserNotLoggedIn");
                Console.WriteLine("TestCase 'VerifyBetplacement_UserNotLoggedIn' - FAIL");
                Fail(ex.Message);
            }
        }



        [Test]
        public void VerifyPageDetailsOnLoginLogout()
        {            
            string eventName, xPath;
            Console.WriteLine("***** Executing Test Case --- 'VerifyPageDetailsOnLoginLogout', Verify User remains on the same page previously navigated to on Login/Logout *****");
            try
            {                
                LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                LTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region Verify HR Page
                Console.WriteLine("Verify Horse Racing Page detials on Login/Logout");
                //Add and event from Horse Racing page-Next races 
                LTcommonObj.SelectLinksFromSideBar(MyBrowser, "Horse Racing", "Horse Racing");
                xPath = "//a[starts-with(@class, 'bxc bxf tab') and contains(text(), 'Next Races')]";
                LTcommonObj.clickObject(MyBrowser, xPath);
                Thread.Sleep(2000);
                //Select any event
                xPath = "//div[@class='pa10']/ul/li[1]/a";
                LTcommonObj.clickObject(MyBrowser, xPath);

                //Get the Event name
                xPath = "//div[@class='bxcv bxf ffa b fs13px']/div[1]";
                Assert.IsTrue(MyBrowser.IsElementPresent(xPath), "Element is not present (No Events under Next Races)");
                eventName = MyBrowser.GetText(xPath);

                //Login and verify the details
                LTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                Assert.IsTrue(MyBrowser.IsVisible(xPath), "Element is not present");
                Assert.IsTrue(eventName == MyBrowser.GetText(xPath), "Failed to redirect to Horse Racing page on Login");

                //Logout and verify the details
                LTloginLogoutObj.Logout(MyBrowser);

                //Login and verify the details
                LTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                Assert.IsTrue(MyBrowser.IsVisible(xPath), "Element is not present");
                Assert.IsTrue(eventName == MyBrowser.GetText(xPath), "Failed redirect to Horse Racing page on Login");
                Console.WriteLine("Verification of details on Horse Racing Page on Login/Logout was successfull");
                #endregion

                LTcommonObj.NavigateToHomePage(MyBrowser, "Side Menu");
                MyBrowser.Refresh();
                LTframeworkCommonObj.PageSync(MyBrowser);

                #region Verify Football Page
                Console.WriteLine("Verify Football Page detials on Login/Logout");
                //Add and event from Home page Inplay 
                xPath = "//span[starts-with(@class, 'bxc bxf tab') and contains(text(), 'In-play')]";
                LTcommonObj.clickObject(MyBrowser, xPath);
                Thread.Sleep(2000);
                //Select any event
                xPath = "//div[@id='featured_inplay']//div[@class='pa10']//span[@class='bxc bxf rel t2 mr6 tac' and contains(text(), 'More')]";
                LTcommonObj.clickObject(MyBrowser, xPath);

                //Get the Event name
                xPath = "//div[@class='bxcv pa10 event-details-header-live']//div[@class='bxcr bxf teams tar']";
                Assert.IsTrue(MyBrowser.IsElementPresent(xPath), "Element is not present (Football Event is not present under In-Play)");
                eventName = MyBrowser.GetText(xPath);

                //Logout and verify the details
                LTloginLogoutObj.Logout(MyBrowser);

                //Login and verify the details
                LTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                Assert.IsTrue(MyBrowser.IsVisible(xPath), "Element is not present");
                Assert.IsTrue(eventName == MyBrowser.GetText(xPath), "Failed to redirect to Football page on Login");

                //Logout and verify the details
                LTloginLogoutObj.Logout(MyBrowser);
                Console.WriteLine("Verification of details on Football Page on Login/Logout was successfull");
                #endregion
                Console.WriteLine("TestCase 'VerifyPageDetailsOnLoginLogout' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyPageDetailsOnLoginLogout");
                Console.WriteLine("TestCase 'VerifyPageDetailsOnLoginLogout' - FAIL");
                Fail(ex.Message);
            }
        }



    }//end class
}//end namespace
