using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Selenium;
using OpenQA.Selenium;
using Framework;
using TestRepository.ControlsRepository;


namespace TestRepository.HomeGlobal
{
    public class HomeGlobalFunctions : BaseTest
    {
        TestRepository.Betslip.BetslipFunctions HGFbetslipObj = new TestRepository.Betslip.BetslipFunctions();
        TestRepository.Common HGFcommonObj = new TestRepository.Common();
        Framework.Common.Common HGframeworkCommonObj = new Framework.Common.Common();


        ///<summary>
        /// This method verifies the details on COntact Us page
        /// <example>VerifyContactusPage(MyBrowser)</example>        
        public void VerifyContactusPage(ISelenium browser)
        {
            try
            {
                HGFcommonObj.selectMenuButton(browser);
                HGFcommonObj.clickObject(browser, LoginLogoutControls.loginOrRegisterLink);
                Thread.Sleep(1000);
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.lostLoginButton), "Lost Login button is not present in mobile's login page");
                HGFcommonObj.clickObject(browser, LoginLogoutControls.lostLoginButton);

                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.contactUsBanner), "'Contact us' banner is not present in the Contact Us page");
                Assert.IsTrue(browser.IsTextPresent("To speak to our 24 hour Customer Support team please contact us on:"), "Contact Us message is not present in the Contact Us page");
                Assert.IsTrue(browser.IsTextPresent("Call us on:"), "'Call us on' message is not present in the Contact Us page");

                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.UKContacts), "UK contact details are not not present in the Contact Us page");
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.NonUKContacts), "Non UK contact details are not not present in the Contact Us page");
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.emailContacts), "eMail information is not present in the Contact Us page");
                Console.WriteLine("UI Verification of Contact us page via 'Lost Login' was successful");

                HGFcommonObj.SelectLinksFromSideBar(browser, "Contact us", "Contact Us");
                Assert.IsTrue(browser.IsTextPresent("To speak to our 24 hour Customer Support team please contact us on:"), "Contact Us message is not present in the Contact Us page");
                Assert.IsTrue(browser.IsTextPresent("Call us on:"), "'Call us on' message is not present in the Contact Us page");
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.UKContacts), "UK contact details are not not present in the Contact Us page");
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.NonUKContacts), "Non UK contact details are not not present in the Contact Us page");
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.emailContacts), "eMail information is not present in the Contact Us page");
                Console.WriteLine("UI Verification of Contact us page via 'Side Menu' was successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyContactusPage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method verifies the all the policies on Home page 
        /// <example>VerifyPolicies(MyBrowser)</example>  
        public void VerifyPolicies(ISelenium browser, string[] Policies, string[] URLs)
        {
            try
            {
                string url, xPath;
                for (int i = 0; i < Policies.Length; i++)
                {
                    HGframeworkCommonObj.PageSync(browser);
                    xPath = "//a[starts-with(@class, 'bxc tac sec-button small footer') and contains(text(), '" + Policies[i] + "')]";

                    Assert.IsTrue(browser.IsElementPresent(xPath), Policies[i] + " button not found");
                    browser.Click(xPath);
                    HGframeworkCommonObj.PageSync(browser);
                    HGframeworkCommonObj.WaitUntilElementPresent(browser, LoginLogoutControls.alertContainer, "5000");
                    Thread.Sleep(1000);
                    Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.alertContainer), "Alert container was not displayed on tapping the '" + Policies[i] + "' link");
                    Assert.IsTrue(browser.IsTextPresent("You are about to navigate away from the site and any selections you have in the betslip may be lost"), "Warning message[1] is not displayed in the Alert container");
                    Assert.IsTrue(browser.IsTextPresent("Do you want to navigate away?"), "Warning message[2] is not displayed in the Alert container");
                    browser.Click(LoginLogoutControls.CloseButtonInAlertContainer);
                    HGframeworkCommonObj.PageSync(browser);
                    Assert.IsFalse(browser.IsVisible(LoginLogoutControls.alertContainer), "Alert container failed to close on tapping the Cancel button");

                    browser.Click(xPath);
                    HGframeworkCommonObj.WaitUntilElementPresent(browser, LoginLogoutControls.alertContainer, "5000");
                    Thread.Sleep(1000);
                    Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.alertContainer), "Alert container was not displayed on tapping the '" + Policies[i] + "' link");
                    browser.Click(LoginLogoutControls.ContinueButtonInAlertContainer);
                    HGframeworkCommonObj.PageSync(browser);
                    Thread.Sleep(2000);

                    //Title is different for Desktop policy
                    if (Policies[i] == "Desktop")
                    {
                        HGFcommonObj.SwitchWindow(browser, "Ladbrokes Mobile");
                    }
                    else
                    {
                        HGFcommonObj.SwitchWindow(browser, "LBR Customer KB");
                    }

                    url = browser.GetLocation();
                    // url = driver.Url;
                    browser.Close();
                    browser.SelectWindow("null");
                    Thread.Sleep(1000);

                    if (url.ToLower().Trim() == URLs[i].ToLower().Trim())
                    {
                        Console.WriteLine("'" + Policies[i] + "' validated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Failed to validate the '" + Policies[i] + "'");
                        Fail("Mismatch in Actual and Expected URLs");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyPolicies' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method verifies the Show/hide balance functionality
        /// <example>VerifyShowHideBalanceFunctionality(MyBrowser)</example>  
        public void VerifyShowHideBalanceFunctionality(ISelenium browser)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                Assert.IsTrue(driver.FindElement(By.Id("balance")).Displayed, "Balance is not displayed( user not Logged in");

                browser.Click(LoginLogoutControls.balance);
                Thread.Sleep(1000);
                string xPath = "//div[@class='bxc balance-button' and contains(text(),'Show Balance')]";
                IWebElement balance = driver.FindElement(By.XPath(xPath));
                Thread.Sleep(1000);
                if (balance.GetAttribute("style").Contains("display: none"))
                {
                    Fail("Balance failed to HIDE on clicking it.");
                }

                browser.Click(xPath);
                Thread.Sleep(1000);
                balance = driver.FindElement(By.XPath(xPath));
                Thread.Sleep(1000);
                if (!balance.GetAttribute("style").Contains("display: none"))
                {
                    Fail("Balance failed to SHOW on clicking it.");
                }

                Console.WriteLine("Show/Hide balance functionality was verified successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyShowHideBalanceFunctionality' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method checks for broken links on Event details/Event type page
        /// <example>VerifyNavigationsFromEventsPage(MyBrowser, "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, "Go to Horse Racing", "Event Details")</example>  
        /// <example>VerifyNavigationsFromEventsPage(MyBrowser, "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, "Go to A-Z betting", "Type/Subtype")</example>                                 
        public void VerifyNavigationsFromEventsPage(ISelenium browser, string sidebarLink, string navPanel, string className, string typeName, string subTypeName, string eventName, string marketName, string action, string navigationPage)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                string pageTitle, xPath;

                HGFbetslipObj.NavigateToSportsPage(browser, sidebarLink, navPanel, className);

                //Select the type name
                //check if the Type is expanded
                xPath = "//div[@id = '" + className + "-" + navPanel.ToLower().Trim() + "']//div[@class='bxcl expandableHeader']//div[@class='bxcl bxf ml5' and contains(text(), '" + typeName + "')]/..";
                IWebElement Type = driver.FindElement(By.XPath(xPath));
                if (!bool.Parse(Type.GetAttribute("expanded")))
                {
                    Type.Click();
                    Thread.Sleep(1000);
                }
                //xPath = "//div[@id = '" + className + "-" + navPanel.ToLower().Trim() + "']//div[@class='bxcl expandableHeader']//div[@class='bxcl bxf ml5']";
                //HGFcommonObj.clickObjectInColl(browser, xPath, typeName);

                if (!string.IsNullOrEmpty(subTypeName))
                {
                    //Select the subTypeName name
                    xPath = "//div[@class='bxcl bxf ml5' and text()='" + typeName + "']//following::div[@class='expandable' and @expanded='true']//div[@class='bxcl bxf ml5']";
                    HGFcommonObj.clickObjectInColl(browser, xPath, subTypeName);

                    //Check for the page to navigate
                    if (navigationPage.ToLower().Contains("event details"))
                    {
                        //Perform only if MORE text is present
                        TimeSpan ts = new TimeSpan(0, 0, 5);
                        driver.Manage().Timeouts().ImplicitlyWait(ts);
                        if (browser.IsElementPresent("//span[@class='bxcl event-name' and contains(text(), '" + eventName + "')]"))
                        {
                            //check for the Type-Subtype banner
                            pageTitle = "//span[@class='t7 page-title' and contains(text(), '" + typeName + " - " + subTypeName + "')]";
                            Assert.IsTrue(browser.IsElementPresent(pageTitle), typeName + " - " + subTypeName + " banner was not displayed");

                            //Navigate to event details page by tapping 'More' button of a  specified Event           
                            Thread.Sleep(2000);
                            xPath = "//span[@class='bxcl event-name' and contains(text(), '" + eventName + "')]//following::span[contains(text(),'More')]";
                            HGFcommonObj.clickObject(browser, xPath);
                        }
                    }
                }
                else
                {
                    xPath = "//div[@class='bxcl bxf ml5' and text()='" + typeName + "']//following::div[@class='expandable' and @expanded='true']//div[@class='bxcl bxf ml5']";
                    HGFcommonObj.clickObjectInColl(browser, xPath, eventName);
                }

                //Condition to decide which page to navigate
                if (navigationPage.ToLower().Contains("event details"))
                {
                    //check for the Event banner
                    pageTitle = "//span[@class='t7 page-title' and contains(text(), '" + eventName + "')]";
                    Assert.IsTrue(browser.IsElementPresent(pageTitle), eventName + " - banner was not displayed");
                    pageTitle = eventName;
                    xPath = "//span[@class='t7 page-title' and contains(text(), '" + pageTitle + "')]/following::div[@class='bxc sec-button' and contains(text(), '" + action + "')]";
                }
                else
                {
                    pageTitle = typeName + " - " + subTypeName;
                    xPath = "//span[@class='t7 page-title' and contains(text(), '" + pageTitle + "')]/following::div[@class='bxc button' and contains(text(), '" + action + "')]";
                }
                HGFcommonObj.clickObject(browser, xPath);


                //Check if the action navigates to its respective pages                
                if (action.ToLower().Contains(className.ToLower()))
                {
                    xPath = "//span[@class='t7 page-title' and contains(text(), '" + className + "')]";
                    Assert.IsTrue(browser.IsElementPresent(xPath), "Failed to navigate back to " + className + " page from Event Details page");
                }
                else if (action.ToLower().Contains("home"))
                {
                    Assert.IsTrue(driver.FindElement(By.Id("carousel")).Displayed, "User is not take to 'Home' page on Logout");
                }
                else if (action.ToLower().Contains("A-z"))
                {
                    xPath = "//span[@class='t7 page-title' and contains(text(), 'A-Z Betting')]";
                    Assert.IsTrue(browser.IsElementPresent(xPath), "Failed to navigate back to 'A-Z Betting' page from Event Details page");
                }
                Console.WriteLine("Successfully navigated to '" + action + "' page");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyNavigationsFromEventsPage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method verifies the Decimal/Fractional odd type switcher changes the odd thetheir respective format
        /// <example>VerifyDecimalFractionalOddType(MyBrowser, "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, "Decimal/Fractional");                      
        public void VerifyDecimalFractionalOddType(ISelenium browser, string sidebarLink, string navPanel, string className, string typeName, string subTypeName, string eventName, string marketName, string selection, string odds, string oddType)
        {
            try
            {
                string xPath, actOddType;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                HGFbetslipObj.NavigateToEventDetailsPage(browser, sidebarLink, navPanel, className, typeName, subTypeName, eventName, marketName);

                //Verify the odd is displayed in the correct format
                xPath = "//div[@class='bxcl ml10  odds-text mr2  type' and contains(text(), '" + selection + "')]//following::span[1][@class='odds-convert']";
                actOddType = browser.GetText(xPath);
                if (oddType.ToLower() == "decimal")
                {
                    Assert.IsTrue(Convert.ToDouble(odds) == Convert.ToDouble(actOddType), "Odd type failed to switch to odd type '" + oddType + "'");
                    Console.WriteLine("Successfuly validated the Odd type switch to '" + oddType + "'");
                }
                else
                {
                    Assert.IsTrue(actOddType.Contains("/"), "Odd type failed to switch to odd type '" + oddType + "'");
                    Console.WriteLine("Successfuly validated the Odd type switch to '" + oddType + "'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyNavigationsFromEventsPage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method Clicks on the NRF links and validates the URl opened in the new browser
        /// <example>ValidatePopupsofNFR(MyBrowser, "xPath", "Ladbrokes LBR", "https://www.gibraltar.gov.gi/remotegambling");
        public void ValidatePopupsofNFR(ISelenium browser, string xPath, string newWindowTitle, string expURL)
        {
            try
            {
                browser.Click(xPath);
                HGframeworkCommonObj.PageSync(browser);
                HGframeworkCommonObj.WaitUntilElementPresent(browser, LoginLogoutControls.alertContainer, "5000");
                Thread.Sleep(1000);
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.alertContainer), "Alert container was not displayed on tapping the '" + xPath + "' link");
                Assert.IsTrue(browser.IsTextPresent("You are about to navigate away from the site and any selections you have in the betslip may be lost"), "Warning message[1] is not displayed in the Alert container");
                Assert.IsTrue(browser.IsTextPresent("Do you want to navigate away?"), "Warning message[2] is not displayed in the Alert container");
                browser.Click(LoginLogoutControls.CloseButtonInAlertContainer);
                HGframeworkCommonObj.PageSync(browser);
                Assert.IsFalse(browser.IsVisible(LoginLogoutControls.alertContainer), "Alert container failed to close on tapping the Cancel button");

                browser.Click(xPath);
                HGframeworkCommonObj.WaitUntilElementPresent(browser, LoginLogoutControls.alertContainer, "5000");
                Thread.Sleep(1000);
                Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.alertContainer), "Alert container was not displayed on tapping the '" + xPath + "' link");
                browser.Click(LoginLogoutControls.ContinueButtonInAlertContainer);
                HGframeworkCommonObj.PageSync(browser);
                Thread.Sleep(2000);
                HGFcommonObj.SwitchWindow(browser, newWindowTitle);

                string actUrl = browser.GetLocation();
                // url = driver.Url;
                browser.Close();
                browser.SelectWindow("null");
                Thread.Sleep(1000);

                if (actUrl.ToLower().Trim() != expURL.ToLower().Trim())
                {
                    Console.WriteLine("Mismatch in URL's. Actual '" + actUrl + "',  Expected '" + expURL + "'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'ValidatePopupsofNFR' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }




    }//end class
}//end namespace

