using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Selenium;
using OpenQA.Selenium;
using Framework;
using TestRepository.ControlsRepository;


namespace TestRepository.LoginLogout
{

    public class LoginLogoutFunctions : BaseTest
    {

        TestRepository.Common LFcommonObj = new TestRepository.Common();
        Framework.Common.Common LFframeworkCommonObj = new Framework.Common.Common();


        ///<summary>
        /// This method loggsin to the application
        /// <example>Login(browser, "arun_ecomm_test", "123456") </example>
        public void Login(ISelenium browser, string username, string password)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.LadbrokesHomeLink), "Header(Ladbrokes Logo) is not displayed on Home page");
                Assert.IsTrue(browser.IsVisible(BetslipControls.betslipButton), "Betslip button is not displayed on Home page");

                LFcommonObj.selectMenuButton(browser);
                Assert.IsFalse(browser.IsVisible(LoginLogoutControls.logoutLink), "Logout link is present in the sidebar(User Logged in");
                LFcommonObj.clickObject(browser, LoginLogoutControls.loginOrRegisterLink);

                // Check for the Login page elements
                LFframeworkCommonObj.WaitUntilElementPresent(browser, LoginLogoutControls.loginUsernameTextBox, FrameGlobals.ElementLoadTimeout.ToString());
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.loginBanner), "Login header is not present in mobile's login page");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.LadbrokesHomeLink), "Ladbrokes Logo is not present in mobile's login page");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.loginUsernameTextBox), "User name field is not present in mobile's login page");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.loginPasswordTextBox), "Password field is not present in mobile's login page");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.lostLoginButton), "Lost Login button is not present in mobile's login page");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.newToLadbrokesBanner), "New to Ladbrokes banner is not present in mobile's login page");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.registerButton), "Register button is not present in mobile's login page");
                Assert.IsTrue(browser.IsVisible(BetslipControls.betslipButton), "Betslip button is not present in mobile's login page");

                //Login                
                LFcommonObj.EnterField(browser, LoginLogoutControls.loginUsernameTextBox, username);
                LFcommonObj.EnterField(browser, LoginLogoutControls.loginPasswordTextBox, password);
                //LFcommonObj.clickObject(browser, LoginLogoutControls.loginSubmitButton);
                browser.FireEvent(LoginLogoutControls.loginSubmitButton, "click");
                LFcommonObj.WaitForLoadingIcon(browser, FrameGlobals.IconLoadTimeout);

                //Check elements on Login FrameGlobals.ElementLoadTimeout + 10000
                //LFframeworkCommonObj.WaitUntilElementPresent(browser, LoginLogoutControls.balance, 30000.ToString());
                //Thread.Sleep(10000);
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.balance), "Balance is not displayed on Login");
                Console.WriteLine("Login was successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'Login' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method verifies the all details, links once the user has logged in 
        /// <example>VerifyDetailsOnLogin(browser)</example>
        public void VerifyDetailsOnLogin(ISelenium browser)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                //Check elements on Login
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.balance), "Balance is not displayed on Login");
                LFcommonObj.selectMenuButton(browser);
                IWebElement username = driver.FindElement(By.XPath("//div[@id='sidebar-user-name' and contains(string(), '" + FrameGlobals.UserDisplayName + "')]"));
                Assert.IsTrue(username.Displayed, "User name '" + FrameGlobals.UserDisplayName + "' is not diplayed on Login");
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.logoutLink), "Logout link is not present in the sidebar on Login");

                //Verify the links under account
                LFcommonObj.clickObject(browser, LoginLogoutControls.accountLink);
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.deposit), "Deposit link is not present in the Account section");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.withdraw), "Withdraw link is not present in the Account section");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.histroy), "Histroy link is not present in the Account section");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.viewBalances), "View Balances link is not present in the Account section");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.Transfer), "Transfer link is not present in the Account section");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.redeemFreeBets), "Redeem Free Bets link is not present in the Account section");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.depositLimits), "Deposit Limits link is not present in the Account section");
                LFcommonObj.clickObject(browser, LoginLogoutControls.accountLink);

                //Verify links on Sidebar
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.moreFromLadbrokes), "More from Ladbrokes link is not present in the sidebar");
                LFcommonObj.clickObject(browser, LoginLogoutControls.homeLinkOnSideBar);
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.balance), "Failed to navigate to 'Home Page' on tapping the Home icon in the sidebar");
                Assert.IsFalse(browser.IsVisible(LoginLogoutControls.sidebar), "Side bar remians on navigating on the home page");
                Console.WriteLine("Verification of UI object on Login was successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyDetailsOnLogin' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method logs the user out of the application
        /// <example>Logout(browser)</example>
        public void Logout(ISelenium browser)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                LFcommonObj.selectMenuButton(browser);
                Assert.IsFalse(browser.IsVisible(LoginLogoutControls.loginOrRegisterLink), "User is not logged in");
                //LFcommonObj.clickObject(browser, LoginLogoutControls.logoutLink, "xpath");               
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("logoutObj=document.getElementsByClassName('bxc sidebar-icon logout ml20');logoutObj.item(0).click()");
                //js.ExecuteScript("anf = document.getElementById('sidebar-scroll');divName = anf.childNodes[1];ulList = divName.childNodes[9].childNodes;bb = ulList[17].childNodes;bb.item(1).click()");

                //Actions builder = new Actions(driver);
                //IWebElement helpWebElement = driver.FindElement(By.XPath(LoginLogoutControls.logoutLink));
                //builder.MoveToElement(helpWebElement).Build().Perform();

                LFcommonObj.WaitForLoadingIcon(browser, FrameGlobals.ElementLoadTimeout);
                Assert.IsFalse(browser.IsVisible(LoginLogoutControls.balance), "Balance is displayed after Logout");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.LadbrokesHomeLink), "Home page is not displayed on logout");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.carousel), "Carousel is not found on Home page on logout");
                Console.WriteLine("Logout was successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'Logout' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method verifies the details, links once the user has logged out
        /// <example>VerifyDetailsOnLogout(browser)</example>
        public void VerifyDetailsOnLogout(ISelenium browser)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                Assert.IsFalse(browser.IsVisible(LoginLogoutControls.sidebar), "Sidebar failed to close on Logout");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.LadbrokesHomeLink), "Header(Ladbrokes Logo) is not displayed on Home page after Logout");
                Assert.IsFalse(browser.IsVisible(LoginLogoutControls.balance), "Balance is displayed after Logout");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.carousel), "Carousel is not found on Home page on logout");

                //Assert.IsTrue(browser.IsVisible(LoginLogoutControls.promotionalBanner), "Promotional banner is not displayed on Home page");

                LFcommonObj.selectMenuButton(browser);
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.loginOrRegisterLink), "Login/Register element is not present after Logout");
                Assert.IsFalse(browser.IsVisible(LoginLogoutControls.logoutLink), "Logout link is present after Logout");
                //browser.Click(LoginLogoutControls.menuIcon);
                js.ExecuteScript("document.getElementById('menu-button').click()");
                LFframeworkCommonObj.PageSync(browser);

                Console.WriteLine("Verification of UI object on Logout was successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyDetailsOnLogout' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method captures the error message displayed when attempts to login with invalid credentials
        /// <example>CaptureLoginErrorMessage(browser, "ecomm_test_user", "123456")</example>
        public string CaptureLoginErrorMessage(ISelenium browser, string userName, string password)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                LFcommonObj.selectMenuButton(browser);
                LFcommonObj.clickObject(browser, LoginLogoutControls.loginOrRegisterLink);

                LFframeworkCommonObj.WaitUntilElementEditable(browser, LoginLogoutControls.loginUsernameTextBox, "10000");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.loginBanner), "Login page not displayed");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.loginUsernameTextBox), "Username field not presesnt in Login page");
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.loginPasswordTextBox), "Password field not presesnt in Login page");

                browser.Type(LoginLogoutControls.loginUsernameTextBox, "");
                if (userName != null)
                {
                    browser.Type(LoginLogoutControls.loginUsernameTextBox, userName);
                }

                browser.Type(LoginLogoutControls.loginPasswordTextBox, "");
                if (password != null)
                {
                    browser.Type(LoginLogoutControls.loginPasswordTextBox, password);
                }

                browser.FireEvent(LoginLogoutControls.loginSubmitButton, "click");
                LFcommonObj.WaitForLoadingIcon(browser, FrameGlobals.IconLoadTimeout);
                Thread.Sleep(1000);
                return driver.FindElement(By.XPath(LoginLogoutControls.loginErrorpanel)).Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'CaptureLoginErrorMessage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }



    }//end class
}//end namespace

