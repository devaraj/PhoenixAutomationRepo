using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Collections.ObjectModel;

using Selenium;
using OpenQA.Selenium;
using MbUnit.Framework;
using Framework;
using Framework.Common;
using TestRepository.ControlsRepository;


namespace PreProdSuite
{
    [TestFixture(ApartmentState = ApartmentState.STA, TimeOut = FrameGlobals.TestCaseTimeout)]
    public class HomeGlobalTests : BaseTest
    {
        TestRepository.HomeGlobal.HomeGlobalFunctions HGTHomeGlobalObj = new TestRepository.HomeGlobal.HomeGlobalFunctions();
        TestRepository.LoginLogout.LoginLogoutFunctions HGTLoginLogoutObj = new TestRepository.LoginLogout.LoginLogoutFunctions();
        TestRepository.Betslip.BetslipFunctions HGTBetslipObj = new TestRepository.Betslip.BetslipFunctions();
        TestRepository.Common HGTCommonObj = new TestRepository.Common();
        Framework.Common.Common HGTframeworkCommonObj = new Framework.Common.Common();

        /// <summary>
        /// Customer is taken to Contact Us page
        /// Validate promo banner links are clickable
        /// Validate user can navigate to Promotions page via side menu
        /// To validate that user can navigate to side menu from the global header  and all the essential fields are present on the page
        /// </summary>
        /// <TestCaseId>367,617,619,621</TestCaseId>
        /// <TestCasesCovered>4</TestCasesCovered>
        [Test]
        public void VerifySideMenuLinks()
        {
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            string actUrl, expURL, xPath;
            Console.WriteLine("***** Executing Test Case --- 'VerifySideMenuLinks', Check for broken links on the Side Menu *****");
            try
            {
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);

                #region Verify Side menu links
                // Verify the links on Sidebar
                HGTCommonObj.selectMenuButton(MyBrowser);
                Assert.IsTrue(MyBrowser.IsVisible(LoginLogoutControls.quickLinks), "Quick Links static text is not presnt on the the Side Menu");
                Assert.IsTrue(MyBrowser.IsVisible(LoginLogoutControls.oddsInSideMenu), "Odds static text is not presnt on the the Side Menu");

                //Verify the navigation link- More from Ladbrokes
                Assert.IsTrue(MyBrowser.IsVisible(LoginLogoutControls.moreFromLadbrokes), "More from Ladbrokes link is not presnt on the the Side Menu");
                HGTCommonObj.clickObject(MyBrowser, LoginLogoutControls.moreFromLadbrokes);
                HGTCommonObj.SwitchWindow(MyBrowser, "Ladbrokes Mobile | Sports Betting, Casino and Games");
                actUrl = MyBrowser.GetLocation();
                expURL = "https://mobile.ladbrokes.com/lobby/sports/";      // "https://ext-mobile.ladbrokes.com/"; 
                MyBrowser.Close();
                MyBrowser.SelectWindow("null");
                Thread.Sleep(1000);
                Assert.IsTrue(actUrl.ToLower().Trim() == expURL.ToLower().Trim(), "Mismatch in URL's. Actual '" + actUrl + "',  Expected '" + expURL + "'.");
                MyBrowser.Refresh();
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);

                //Verify the Promo banner
                Assert.IsTrue(MyBrowser.IsVisible(LoginLogoutControls.promotionalBanner), "Promo banner(carousel) is not presnt on the the Home Menu");
                HGTCommonObj.clickObject(MyBrowser, LoginLogoutControls.promotionalBanner);
                Assert.IsTrue(MyBrowser.IsVisible(LoginLogoutControls.promoTitleBanner), "Failed to navigate to Promo page on clicking the Promo banner");
                HGTCommonObj.NavigateToHomePage(MyBrowser, "Header");
                Console.WriteLine("Navigation to 'Home Page' page via 'Header link' was successful");

                // Verify broken links on Side Menu                
                HGTCommonObj.selectMenuButton(MyBrowser);
                xPath = "//header[@class='bxcl section-header h35']/following-sibling::ul[1]/li/a/span[@class='ml10']";

                ReadOnlyCollection<IWebElement> element = driver.FindElements(By.XPath(xPath));
                string[] linkToClick = new string[element.Count];
                for (int i = 0; i < element.Count; i++)
                {
                    linkToClick[i] = element[i].Text.Trim();
                }
                HGTCommonObj.clickObject(MyBrowser, LoginLogoutControls.homeLinkOnSideBar);
                for (int i = 0; i < element.Count; i++)
                {
                    MyBrowser.Refresh();
                    HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    HGTCommonObj.SelectLinksFromSideBar(MyBrowser, linkToClick[i], linkToClick[i]);
                    Console.WriteLine("Navigation to '" + linkToClick[i] + "' page via 'Side Menu' was successful");
                }

                HGTCommonObj.SelectLinksFromSideBar(MyBrowser, "A-Z Betting", "A-Z Betting");
                Console.WriteLine("Navigation to 'A-Z Betting' page via 'Side Menu' was successful");

                HGTCommonObj.SelectLinksFromSideBar(MyBrowser, "Promotions", "Promotions");
                Console.WriteLine("UI Verification of Promotions page via 'Side Menu' was successful");

                HGTHomeGlobalObj.VerifyContactusPage(MyBrowser);

                HGTCommonObj.SelectLinksFromSideBar(MyBrowser, "Best Odds Guaranteed", "Best odds guaranteed");
                Assert.IsTrue(MyBrowser.IsTextPresent("We offer Best Odds Guaranteed on all UK and Irish horse races. If you take an early or board price and the SP return is bigger, we'll pay you at the better odds. Available from 9am."), "BOG info text is not present in the BOG page");
                Assert.IsTrue(MyBrowser.IsVisible(HomeGlobalControls.bogbanner), "BoG image was not found in the BOG page");
                Console.WriteLine("UI Verification of BOG page via 'Side Menu' was successful");

                HGTCommonObj.NavigateToHomePage(MyBrowser, "Side Menu");
                Assert.IsTrue(driver.FindElement(By.Id("carousel")).Displayed, "Carousel is not displayed on in Home page");
                Console.WriteLine("Navigation to 'Home Page' page via 'Side Menu' was successful");
                #endregion
                Console.WriteLine("TestCase 'VerifySideMenuLinks' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifySideMenuLinks");
                Console.WriteLine("TestCase 'VerifySideMenuLinks' - FAIL");
                Fail(ex.Message);
            }
        }

        /// <summary>
        /// To validate that the odds preference will be in the menu list section with a toggle
        /// </summary>
        /// <TestCaseId>626</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyBalanceAndOddSwitchFunctionality()
        {            
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "FutureEvents");
            Console.WriteLine("***** Executing Test Case --- 'VerifyBalanceAndOddSwitchFunctionality', Verify Show/Hide balance, Fractional/Decimal odd switchewr functionalities *****"); 
            try
            {
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                //Check Show/Hide functionality
                HGTCommonObj.clickObject(MyBrowser, LoginLogoutControls.LadbrokesHomeLink);
                MyBrowser.Refresh();
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                HGTLoginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                HGTHomeGlobalObj.VerifyShowHideBalanceFunctionality(MyBrowser);

                // Verify the Fractional and decimal odds switches
                HGTBetslipObj.OddTypeSwitch(MyBrowser, "Decimal");
                HGTHomeGlobalObj.VerifyDecimalFractionalOddType(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, "Decimal");
                HGTBetslipObj.OddTypeSwitch(MyBrowser, "Fractional");
                HGTHomeGlobalObj.VerifyDecimalFractionalOddType(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, "Fractional");
                Console.WriteLine("TestCase 'VerifyBalanceAndOddSwitchFunctionality' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBalanceAndOddSwitchFunctionality");
                Console.WriteLine("TestCase 'VerifyBalanceAndOddSwitchFunctionality' - FAIL");
                Fail(ex.Message);
            }
        }

        /// <summary>
        /// Verify the link at the bottom of the homepage labelled Responsible Gaming
        /// While selecting continue a new browser window is opened, directed to: http://help.ladbrokes.com/display/4/kb/article.aspx?aid=1077
        /// Verify the link at the bottom of the homepage labelled Privacy Policy
        /// While selecting continue a new browser window is opened, directed to: http://help.ladbrokes.com/display/4/kb/article.aspx?aid=2665
        /// Verify the link at the bottom of the homepage labelled Cookie Policy
        /// While selecting continue a new browser window is opened, directed to: http://help.ladbrokes.com/display/4/kb/article.aspx?aid=1120#r5
        /// Verify the link at the bottom of the homepage labelled Terms and Conditions
        /// While selecting continue a new browser window is opened, directed to: http://help.ladbrokes.com/display/4/kb/article.aspx?aid=2665
        /// </summary>
        /// <TestCaseId>225,227,228,230,236,237,241,242</TestCaseId>
        /// <TestCasesCovered>8</TestCasesCovered>
        [Test]
        public void VerifyAllPolicies()
        {
            string[] policy = new string[] { "Terms & Conditions", "Responsible Gambling", "Cookie Policy", "Privacy", "Desktop" };
            string[] url = new string[] { "http://help.ladbrokes.com/display/4/kb/article.aspx?aid=2665", "http://help.ladbrokes.com/display/4/kb/article.aspx?aid=1077", "http://help.ladbrokes.com/display/4/kb/article.aspx?aid=1120#r5", "http://help.ladbrokes.com/display/4/kb/article.aspx?aid=1120", "http://mobile.ladbrokes.com/" };
            Console.WriteLine("***** Executing Test Case --- 'VerifyAllPolicies', Verify all the Policies listed in Home Page *****"); 
            try
            {            
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                HGTCommonObj.clickObject(MyBrowser, LoginLogoutControls.LadbrokesHomeLink);
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);

                Console.WriteLine(".....Validate all policies while User NOT logged in");
                HGTHomeGlobalObj.VerifyPolicies(MyBrowser, policy, url);

                Console.WriteLine(".....Validate all policies while User logged in");
                MyBrowser.Refresh();
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                HGTLoginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                HGTHomeGlobalObj.VerifyPolicies(MyBrowser, policy, url);

                Console.WriteLine("TestCase 'VerifyAllPolicies' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyAllPolicies");
                Console.WriteLine("TestCase 'VerifyAllPolicies' - FAIL");
                Fail(ex.Message);
            }
        }

        /// <summary>
        /// To validate that 18+ image is shown at the bottom of the homepage
        /// To validate that on clicking 18+ image  a new browser window is opened
        /// To validate that the user is taken to Government of Gibraltar Information services page by clicking on option "Continue"
        /// To validate that '© Copyright © 2013 Ladbrokes' text is available at the bottom of the homepage
        /// To validate that 'Ladbrokes International plc & Ladbrokes Sportsbook LP, 
        /// suites 6-8, 5th Floor, Europort, Gibraltar are licensed (RGL Nos. 010, 012 & 044) 
        /// by the Government of Gibraltar and regulated by the Gibraltar Gambling Commissioner' text is available
        /// </summary>
        /// <TestCaseId>208,212,219,221,222</TestCaseId>
        /// <TestCasesCovered>5</TestCasesCovered>
        [Test]
        public void VerifyNFRsInHomepage()
        {
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            Console.WriteLine("***** Executing Test Case --- 'VerifyNFRsInHomepage', Verify the NFR's in the Home page *****");
            try
            {                
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                HGTCommonObj.clickObject(MyBrowser, LoginLogoutControls.LadbrokesHomeLink);
                HGTCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                HGTLoginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);

                // Verify NFR (user Logged in)
                Assert.IsTrue(MyBrowser.IsVisible(HomeGlobalControls.ladbrokesAddress), "Ladbrokes address was not displayed in the home page");
                Assert.IsTrue(MyBrowser.IsVisible(HomeGlobalControls.copyRightTxt), "Copyright text was not displayed in the home page");
                HGTHomeGlobalObj.ValidatePopupsofNFR(MyBrowser, HomeGlobalControls.govGibraltar, "Remote Gambling", "https://www.gibraltar.gov.gi/remotegambling?w_id=20120704133407");
                HGTHomeGlobalObj.ValidatePopupsofNFR(MyBrowser, HomeGlobalControls.plus18, "18.gif", "http://media.ladbrokes.com/generic/images/siteImages/footerIcons_various/18.gif");
                Console.WriteLine("Verification of all NFR's was successful(User logged in)");

                // Verify NFR (user NOT Logged in)
                HGTLoginLogoutObj.Logout(MyBrowser);
                Assert.IsTrue(MyBrowser.IsVisible(HomeGlobalControls.ladbrokesAddress), "Ladbrokes address was not displayed in the home page");
                Assert.IsTrue(MyBrowser.IsVisible(HomeGlobalControls.copyRightTxt), "Copyright text was not displayed in the home page");
                HGTHomeGlobalObj.ValidatePopupsofNFR(MyBrowser, HomeGlobalControls.govGibraltar, "Remote Gambling", "https://www.gibraltar.gov.gi/remotegambling?w_id=20120704133407");
                HGTHomeGlobalObj.ValidatePopupsofNFR(MyBrowser, HomeGlobalControls.plus18, "18.gif", "http://media.ladbrokes.com/generic/images/siteImages/footerIcons_various/18.gif");
                Console.WriteLine("Verification of all NFR's was successful(User NOT logged in)");
                Console.WriteLine("TestCase 'VerifyNFRsInHomepage' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyNFRsInHomepage");
                Console.WriteLine("TestCase 'VerifyNFRsInHomepage' - FAIL");
                Fail(ex.Message);
            }
        }




    }//end class
}//end namespace


