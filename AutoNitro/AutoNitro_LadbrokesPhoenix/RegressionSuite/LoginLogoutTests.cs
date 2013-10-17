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
            username = dt.Rows[5]["UserName"].ToString();
            password = dt.Rows[5]["Password"].ToString();
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

                //Check for user locking functionality
                int length = 4;
                for (int i = 1; i <= length; i++)
                {
                    actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, username, "Incorrect Password");
                    Thread.Sleep(2000);
                    if (i < length)
                    {
                        expErrorMessage = "The username and/or password you have provided is incorrect. You have " + (length - i) + " attempts remaining. Please try again.";
                        Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Verification of '" + i + "' time attempt to login with incorrect Password was unsuccessful");
                        Console.WriteLine(i + " time attempt to  login with incorrect Password was verified successfully");
                    }
                    else
                    {
                        expErrorMessage = "The username and/or password provided is incorrect. Your account is locked for security reasons. Please contact Customer Services.";
                        Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Verification of User lock functionality on the forth attempt to login with incorrect Password was Unsuccessful");
                        Console.WriteLine("Vefification of User lock functionality on the forth attempt to login with incorrect Password was successful");
                    }
                }
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
            testDataLst[0] = new TestData(0, "FutureEvents");
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
            testDataLst[0] = new TestData(0, "FutureEvents");
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



        [Test]
        public void VerifyLoginErrorMessage_SelfExclusion()
        {
            ISelenium adminBrowser=null;
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username, actErrorMessage, expErrorMessage;
            bool bStatus;
            username = dt.Rows[7]["UserName"].ToString();
            password = dt.Rows[7]["Password"].ToString();            
            Console.WriteLine("***** Executing Test Case --- 'VerifyLoginErrorMessage_SelfExclusion', Verify the error messages displayed when user attempts to login as Self Excluded Customer *****");
            try
            {
                #region Self EX customer
                //self Excl the customer in OB
                adminBrowser = admincommonObj.LogOnToAdmin();
                bStatus = admincommonObj.SelfExcludedCustomer(adminBrowser, username);
                if (bStatus == true)
                {
                    Console.WriteLine("Customer Self exculded in OB");
                    //Verify the error message
                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, username, password);
                    expErrorMessage = "As part of our standard security and verifications procedures, your account has been linked to a self / company excluded account. As a result, your account has been suspended. For further information, please contact Customer Services.";
                    Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                    Console.WriteLine("Login error message(Self Exclusion) validated successfully");
                }
                else
                {
                    Fail("Failed to Self Exclude the customer '" + username + "'");
                }
                Console.WriteLine("TestCase 'VerifyLoginErrorMessage_SelfExclusion' - Pass");
                #endregion
             }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLoginErrorMessage_SelfExclusion");
                Console.WriteLine("Function 'VerifyLoginErrorMessage_SelfExclusion' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Release Self Exclussion
                admincommonObj.ReleaseSelfExcludedUser(adminBrowser, username);
                admincommonObj.UpdateCustomerStatus(adminBrowser, username, "Active", "-- unset --", "-- unset --");
                LTcommonObj.KillAdminObject();
            }
        }



        [Test]
        public void VerifyLoginErrorMessage_BannedCountry()
        {
            ISelenium adminBrowser=null;
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username, actErrorMessage, expErrorMessage, country = "United States";
            bool bStatus;
            username = dt.Rows[3]["UserName"].ToString();
            password = dt.Rows[3]["Password"].ToString();
            Console.WriteLine("***** Executing Test Case --- 'VerifyLoginErrorMessage_BannedCountry', Verify the error messages displayed when user attempts to login from a Banned Country *****"); 
            try
            {

                //Update customer registration country to a banned country
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();
                bStatus = admincommonObj.UpdateBannedCountryCustomer(adminBrowser, username, country);

                if (bStatus == true)
                {
                    Console.WriteLine("Customer registration country chnaged to Banned country 'US' in OB");
                    //Verify the error message
                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, username, password);
                    expErrorMessage = "We are sorry, but your location is currently prohibited from using the Ladbrokes service. For further information please contact Customer Services";
                    //expErrorMessage = "The supplied user is not active";
                    Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                    Console.WriteLine("Login error message(Banned Country) validated successfully");
                }
                else
                {
                    Fail("Failed to Update Customer's country to a Banned country (" + username + "-" + country + ")");
                }
                Console.WriteLine("TestCase 'VerifyLoginErrorMessage_BannedCountry' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLoginErrorMessage_BannedCountry");
                Console.WriteLine("Function 'VerifyLoginErrorMessage_BannedCountry' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Update customer registration country to a Non-banned country
                admincommonObj.UpdateBannedCountryCustomer(adminBrowser, username, "United Kingdom");
                admincommonObj.UpdateCustomerStatus(adminBrowser, username, "Active", "-- unset --", "-- unset --");
                LTcommonObj.KillAdminObject();
            }
        }



        [Test]
        public void VerifyLoginErrorMessage_SuspendedUser()
        {
            ISelenium adminBrowser=null;
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username, actErrorMessage, expErrorMessage;
            bool bStatus;
            username = dt.Rows[4]["UserName"].ToString();
            password = dt.Rows[4]["Password"].ToString();
            Console.WriteLine("***** Executing Test Case --- 'VerifyLoginErrorMessage_SuspendedUser', Verify the error messages displayed when user attempts to login with a Suspended Account *****");
            try
            {               
                //Suspend the account by failing the Age Verification
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();
                bStatus = admincommonObj.StoreAgeVerification(adminBrowser, username, "Fail", 100, "Test Ref", "Test Notes", "TestNo12345");
                if (bStatus == true)
                {
                    Console.WriteLine("Customer suspended in OB");
                    //Verify the error message
                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, username, password);
                    expErrorMessage = "As part of our standard security and verifications procedures, your account awaits Age Verification. For further information, please contact Customer Services.";
                    //expErrorMessage = "Undefined technical system error.";
                    Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                    Console.WriteLine("Login error message(Suspended Account - Age Verification) validated successfully");
                }
                else
                {
                    Fail("Failed to store Age Verification(Suspend) for customer '" + username + "'");
                }
                Console.WriteLine("TestCase 'VerifyLoginErrorMessage_SuspendedUser' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLoginErrorMessage_SuspendedUser");
                Console.WriteLine("Function 'VerifyLoginErrorMessage_SuspendedUser' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Restore the Age Verification
                admincommonObj.StoreAgeVerification(adminBrowser, username, "Pass", 100, "Test Ref", "Test Notes", "TestNo12345");
                admincommonObj.UpdateCustomerStatus(adminBrowser, username, "Active", "-- unset --", "-- unset --");
                LTcommonObj.KillAdminObject();
            }
        }



        [Test]
        public void VerifyLogin_EliteCustomer()
        {
            ISelenium adminBrowser = null;             
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username;
            bool bStatus;
            username = dt.Rows[0]["UserName"].ToString();
            password = dt.Rows[0]["Password"].ToString();
            Console.WriteLine("***** Executing Test Case --- 'VerifyLogin_EliteCustomer', Verify successful login for Elite Customer *****");           
            try
            {                
                //Update to Elite
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();
                bStatus = admincommonObj.UpdateSegments(adminBrowser, username, "Telephone", "Elite");
                if (bStatus == true)
                {
                    Console.WriteLine("Customer changed to Elite in OB");
                    //Verify Login
                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    LTloginLogoutObj.Login(MyBrowser, username, password);
                    Console.WriteLine("Login successfully validated for Elite Customer");
                }
                else
                {
                    Fail("Failed to update the customer '" + username + "' to Elite");
                }
                Console.WriteLine("TestCase 'VerifyLogin_EliteCustomer' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLogin_EliteCustomer");
                Console.WriteLine("Function 'VerifyLogin_EliteCustomer' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Restore to NonElite
                admincommonObj.UpdateSegments(adminBrowser, username, "Telephone", "-- Unset --");
                LTcommonObj.KillAdminObject();
            }
        }



        [Test]
        public void VerifyLogin_HVCCustomer()
        {
            ISelenium adminBrowser = null;            
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username;
            bool bStatus;
            username = dt.Rows[2]["UserName"].ToString();
            password = dt.Rows[2]["Password"].ToString();
            Console.WriteLine("***** Executing Test Case --- 'VerifyLogin_HVCCustomer', Verify successful login for HVC Customer *****");            
            try
            {
                //update to HVC
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();
                bStatus = admincommonObj.UpdateSegments(adminBrowser, username, "HVC", "Yes");
                if (bStatus == true)
                {
                    Console.WriteLine("Customer changed to HVC in OB");
                    //Verify Login
                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    LTloginLogoutObj.Login(MyBrowser, username, password);
                    Console.WriteLine("Login successfully validated for HVC Customer");
                }
                else
                {
                    Fail("Failed to update the customer '" + username + "' to HVC");
                }
                Console.WriteLine("TestCase 'VerifyLogin_HVCCustomer' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLogin_HVCCustomer");
                Console.WriteLine("Function 'VerifyLogin_HVCCustomer' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Restore to NonHVC
                admincommonObj.UpdateSegments(adminBrowser, username, "HVC", "-- Unset --");
                LTcommonObj.KillAdminObject();
            }
        }



        [Test]
        public void VerifyBetplacement_SelfExcludedCustomer()
        {
            ISelenium adminBrowser = null;
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username, actErrorMessage, expErrorMessage;
            bool bStatus;
            string[] aryOdd = new string[1];
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            username = dt.Rows[7]["UserName"].ToString();
            password = dt.Rows[7]["Password"].ToString();
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_SelfExcludedCustomer', Validate self-excluded customer is not allowed to place bet *****");
            try
            {
                //self Excl the customer in OB
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();
                bStatus = admincommonObj.SelfExcludedCustomer(adminBrowser, username);

                if (bStatus == true)
                {
                    Console.WriteLine("Customer Self Exculded in OB");
                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    // Add and verify selction to betslip
                    LTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");
                    LTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                    LTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);
                    //Enter stake and verify betdetails
                    LTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                    LTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");

                    //Verify the error message
                    LTcommonObj.clickObject(MyBrowser, BetslipControls.placeBet);
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, username, password);
                    expErrorMessage = "As part of our standard security and verifications procedures, your account has been linked to a self / company excluded account. As a result, your account has been suspended. For further information, please contact Customer Services.";
                    Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                    Console.WriteLine("Self-Excluded customer was not allowed to place bets");
                }
                else
                {
                    Fail("Failed to Self Exclude the customer '" + username + "'");
                }
                Console.WriteLine("TestCase 'VerifyBetplacement_SelfExcludedCustomer' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_SelfExcludedCustomer");
                Console.WriteLine("Function 'VerifyBetplacement_SelfExcludedCustomer' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Release Self Exclussion
                admincommonObj.ReleaseSelfExcludedUser(adminBrowser, username);
                admincommonObj.UpdateCustomerStatus(adminBrowser, username, "Active", "-- unset --", "-- unset --");
                LTcommonObj.KillAdminObject();
            }
        }
        


        [Test]
        public void VerifyLogin_BalanceLessThanZero()
        {
            ISelenium adminBrowser=null;                    
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username, actErrorMessage, expErrorMessage, xPath, balance;
            bool bStatus;
            double negBal;
            username = dt.Rows[6]["UserName"].ToString();
            password = dt.Rows[6]["Password"].ToString();
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "FutureEvents"); 
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            Console.WriteLine("***** Executing Test Case --- 'VerifyLogin_BalanceLessThanZero', Verify the error messages displayed when user attempts to login when user's account balance is less than zero *****");        
            try
            {
                //Do manual Adjustment to have negative balance
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();

                #region Check Zero balance Msg
                //Get the Balance for customer
                admincommonObj.SearchCustomer(username, adminBrowser);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                admincommonObj.SelectMainFrame(adminBrowser);
                xPath = "//tr/td[@class='caption' and contains(text(), 'Account balance:')]/following-sibling::td[1]";
                balance = adminBrowser.GetText(xPath);
                Assert.IsTrue(!string.IsNullOrEmpty(balance), "Failed to retrieve balannce for customer '" + username + "'");
                negBal = Convert.ToDouble((Convert.ToDouble(balance) * 2) + 10);
                negBal = System.Math.Abs(negBal) * (-1);
                bStatus = admincommonObj.PerformManualAdjustment(adminBrowser, username, "Test Accounts", negBal.ToString(), "Yes", "Test Accounts", "", "", "", "Today");
                if (bStatus == true)
                {
                    Console.WriteLine("Customer balance reduced to nagative in OB");

                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);

                    //Login
                    LTcommonObj.selectMenuButton(MyBrowser);
                    Assert.IsFalse(MyBrowser.IsVisible(LoginLogoutControls.logoutLink), "Logout link is present in the sidebar(User Logged in");
                    LTcommonObj.clickObject(MyBrowser, LoginLogoutControls.loginOrRegisterLink);
                    LTframeworkCommonObj.WaitUntilElementPresent(MyBrowser, LoginLogoutControls.loginUsernameTextBox, FrameGlobals.ElementLoadTimeout.ToString());
                    LTcommonObj.EnterField(MyBrowser, LoginLogoutControls.loginUsernameTextBox, username);
                    LTcommonObj.EnterField(MyBrowser, LoginLogoutControls.loginPasswordTextBox, password);
                    MyBrowser.FireEvent(LoginLogoutControls.loginSubmitButton, "click");
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);

                    //Verify the Error Message
                    Assert.IsTrue(MyBrowser.IsElementPresent(BetslipControls.negBalanceErrMsg), "Error message for Negative balance was not displayed to user on Login");
                    Console.WriteLine("Login error message(User's account balance is less than zero) validated successfully");
                    LTcommonObj.clickObject(MyBrowser, LoginLogoutControls.CloseButtonInAlertContainer);


                    //Verify error message while placing bet                    
                    LTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");
                    LTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                    LTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                    LTcommonObj.clickObject(MyBrowser, BetslipControls.placeBet);

                    // validate error message
                    Assert.IsTrue(MyBrowser.IsElementPresent(BetslipControls.insufficientFundErMsg), "Insifficient funds error message was not shown to user");
                    LTcommonObj.clickObject(MyBrowser, BetslipControls.backToBetslipBtn);
                    TimeSpan ts = new TimeSpan(0, 0, 5);
                    driver.Manage().Timeouts().ImplicitlyWait(ts);
                    Assert.IsFalse(MyBrowser.IsVisible(BetslipControls.insufficientFundErMsg), "Insifficient funds Alert window failed to close on click on 'Back to Betslip' button");
                }
                else
                {
                    Fail("Failed to do Manual Adjustment for the customer '" + username + "'");
                }
                #endregion
                Console.WriteLine("TestCase 'VerifyLogin_BalanceLessThanZero' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLogin_BalanceLessThanZero");
                Console.WriteLine("Function 'VerifyLogin_BalanceLessThanZero' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Do manual Adjustment to have Positive balance
                admincommonObj.PerformManualAdjustment(adminBrowser, username, "Test Accounts", "100", "Yes", "Test Accounts", "", "", "", "Today");
                LTcommonObj.KillAdminObject();
            }
        }

        [Test]
        public void VerifyLoginErrorMessage_CompanyExclusion()
        {
            ISelenium adminBrowser = null; 
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username, actErrorMessage, expErrorMessage;
            bool bStatus;
            username = dt.Rows[8]["UserName"].ToString();
            password = dt.Rows[8]["Password"].ToString();         
            Console.WriteLine("***** Executing Test Case --- 'VerifyLoginErrorMessage_CompanyExclusion', Verify the error messages displayed when user attempts to login as Company Excluded Customer *****"); 
            try
            {            
                //company Excl the customer in OB
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();
                bStatus = admincommonObj.CompanyExcludedCustomer(adminBrowser, username);

                if (bStatus == true)
                {
                    Console.WriteLine("Customer Company Excluded in OB");
                    //Verify the error message
                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    actErrorMessage = LTloginLogoutObj.CaptureLoginErrorMessage(MyBrowser, username, password);
                    expErrorMessage = "As part of our standard security and verifications procedures, your account has been linked to a self / company excluded account. As a result, your account has been suspended. For further information, please contact Customer Services.";
                    Assert.IsTrue(actErrorMessage.ToLower().Trim() == expErrorMessage.ToLower().Trim(), "Mismatch in Actual and Expected error messages");
                    Console.WriteLine("Login error message(Company Exclusion) validated successfully");
                }
                else
                {
                    Fail("Failed to Company Exclude the customer '" + username + "'");
                }
                Console.WriteLine("TestCase 'VerifyLoginErrorMessage_CompanyExclusion' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLoginErrorMessage_CompanyExclusion");
                Console.WriteLine("Function 'VerifyLoginErrorMessage_CompanyExclusion' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Release Company Exclussion
                admincommonObj.ReleaseCompanyExcludedUser(username, adminBrowser);
                admincommonObj.UpdateCustomerStatus(adminBrowser, username, "Active", "-- unset --", "-- unset --");
                LTcommonObj.KillAdminObject();
            }
        }



        [Test]
        public void VerifyLogin_OBACCustomer()
        {
            ISelenium adminBrowser = null;
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username;
            bool bStatus;
            username = dt.Rows[1]["UserName"].ToString();
            password = dt.Rows[1]["Password"].ToString();
            Console.WriteLine("***** Executing Test Case --- 'VerifyLogin_OBACCustomer', Verify successful login for OBAC Customer *****");                
            try
            {
                //Suspend the account by failing the Age Verification
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();
                admincommonObj.SearchCustomer(username, adminBrowser);
                bStatus = admincommonObj.UpdateCustomerFlag("AUTO_REFER", "Y", adminBrowser);
                if (bStatus == true)
                {
                    Console.WriteLine("Customer turned to OBAC in OB");
                    //Verify Login
                    MyBrowser.WindowFocus();
                    LTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    LTloginLogoutObj.Login(MyBrowser, username, password);
                    Console.WriteLine("Login successfully validated for OBAC Customer");
                }
                else
                {
                    Fail("Failed to update the customer '" + username + "' to OBAC");
                }
                Console.WriteLine("TestCase 'VerifyLogin_OBACCustomer' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyLogin_OBACCustomer");
                Console.WriteLine("Function 'VerifyLogin_OBACCustomer' - Failed");
                Fail(ex.Message);
            }
            finally
            {
                //Restore to NonOBAC
                admincommonObj.SearchCustomer(username, adminBrowser);
                admincommonObj.UpdateCustomerFlag("AUTO_REFER", "N", adminBrowser);
                LTcommonObj.KillAdminObject();
            }
        }



    }//end class
}//end namespace
