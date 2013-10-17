using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;
using System.Globalization;
using Selenium;
using OpenQA.Selenium;
using Framework;
using TestRepository.ControlsRepository;
using Framework.Common;

namespace TestRepository.HorseRacing
{
    public class HorseRacingFuntions : BaseTest
    {
        TestRepository.Common testRepositoryCommonObj = new TestRepository.Common();
        Framework.Common.Common frameWorkCommonObj = new Framework.Common.Common();
        TestRepository.Betslip.BetslipFunctions btCommonObj = new Betslip.BetslipFunctions();
        AdminSuite.Common adminCommonObj = new AdminSuite.Common();
        
        /// <summary>
        /// Method to get the event status of an event
        /// </summary>
        /// <returns>Status of the Event</returns>
        public string GetEventStatus(ISelenium browser, string sidebarLink, string navPanel, TestData testdata)
        {
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
            string[] SelectionPriceType = new string[2];
            string status = string.Empty;
            string eventName = GetEventNameFromDateTime_HR(browser, testdata.DateTime);
            btCommonObj.NavigateToEventDetailsPage_HR(browser, sidebarLink, navPanel, testdata.ClassName, testdata.TypeName, testdata.SubTypeName, eventName, testdata.MarketName);

            ReadOnlyCollection<IWebElement> selectionPriceType = driver.FindElements(By.XPath("//div[@class='bxcl bg2 mb4' and contains(string(),'" + testdata.SelectionName + "')]//a[@selectionpricetype]"));

            for (int i = 0; i < selectionPriceType.Count; i++)
            {
                //string xpath = "//div[@class='bxcl bg2 mb4' and contains(string(),'" + testdata.SelectionName + "')]//a[" + (i + 1) + "]";//@selectionpricetype
                string xpath = "//a[ancestor::div[@class='bxcl bg2 mb4' and contains(string(),'" + testdata.SelectionName + "')]][1]";
                SelectionPriceType[i] = driver.FindElement(By.XPath(xpath)).GetAttribute("selectionpricetype");
                if (SelectionPriceType[i].ToLower().Contains("susp"))
                {
                    status = "Suspended";
                    break;
                }
                else
                    status = "Active";
            }

            return status;
        }

        /// <summary>
        /// Method to get event name from date time
        /// </summary>
        public string GetEventNameFromDateTime_HR(ISelenium browser, string DateTime)
        {
            string[] EventName = new string[2];

            EventName = DateTime.Split(' ');
            EventName[1] = EventName[1].Remove(5);
            return EventName[1];
        }

        /// <summary>
        /// Method to update and validate event status validations
        /// </summary>
        public void EventStatusValidation(ISelenium browser, ISelenium adminBrowser,TestData testData, string navPanel)
        {
            string eventStatus, updatedStatus;
            string alternateStatus = "Suspended";

            try
            {
                #region Event Status validation

                eventStatus = GetEventStatus(browser, testData.ClassName, navPanel, testData);
                if (eventStatus == "Suspended")
                    alternateStatus = "Active";
                adminCommonObj.UpdateEvents(adminBrowser, testData.CategoryName, testData.ClassName, testData.TypeName, testData.SubTypeName, testData.EventName, alternateStatus);
                Thread.Sleep(FrameGlobals.OpenBetReflectTimeOut);
                Console.WriteLine("Event status is updated in admin");

                updatedStatus = GetEventStatus(browser, testData.ClassName, navPanel, testData);
                Assert.IsFalse(Equals(eventStatus, updatedStatus), "Event status is not updated on the page");

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CaptureScreenshot(browser, "EventStatusValidation");
            }
        }

        /// <summary>
        /// Method to update and validate event display status validation
        /// </summary>
        public void EventDisplayStatusValidation(ISelenium browser, ISelenium adminBrowser, TestData testData, string navPanel,string eventName)
        {
            try
            {
                #region Event Display Status Validation

                adminCommonObj.UpdateEventsDisplaySts(adminBrowser, testData.CategoryName, testData.ClassName, testData.TypeName, testData.SubTypeName, testData.EventName, "No");
                Thread.Sleep(FrameGlobals.OpenBetReflectTimeOut);
                Console.WriteLine("Event Display status is updated in admin");

                btCommonObj.NavigateToSportsPage(browser, testData.ClassName, navPanel, testData.ClassName);

                Assert.IsFalse(browser.IsElementPresent("//li/a/div[contains(text(), '" + eventName + "')]/following-sibling::div[contains(text(), '" + testData.TypeName + "')]/following-sibling::div[@class='bxcl mr5 arrow next-arrow']"),
                                                                        "Event is still displayed after update");
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CaptureScreenshot(browser, "EventDisplayStatusValidation");
            }
        }

        /// <summary>
        /// Method to update and validate Market status validation
        /// </summary>
        public void MarketStatusValidation(ISelenium browser,ISelenium adminBrowser,TestData testData,string navPanel,string eventName)
        {
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
            string xPath = string.Empty;

            try
            {
                adminCommonObj.UpdateMarketStatus(adminBrowser, testData.CategoryName, testData.ClassName, testData.TypeName, testData.SubTypeName, testData.EventName, testData.MarketName, "Suspend");

                Thread.Sleep(FrameGlobals.OpenBetReflectTimeOut);
                Console.WriteLine("Market Status is updated to Suspended");

                btCommonObj.NavigateToEventDetailsPage_HR(browser, testData.ClassName, navPanel, testData.ClassName, testData.TypeName, testData.SubTypeName, eventName, testData.MarketName);

                //Add selection to Betslip
                xPath = "//div[@class='bxcl b' and contains(text(), '" + testData.SelectionName + "')]/../following-sibling::div[@class='bxcl']/a//span[contains(string(),'" + testData.Odds + "')]";
                testRepositoryCommonObj.clickObject(browser, xPath);
                testRepositoryCommonObj.WaitForLoadingIcon(browser, FrameGlobals.IconLoadTimeout);
                testRepositoryCommonObj.clickObject(browser, BetslipControls.betslipButton);

                btCommonObj.EnterStake(browser, testData.EventName, testData.SelectionName, testData.MarketName, testData.Odds, testData.Stake, "Single", false);

                driver.FindElement(By.XPath(BetslipControls.placeBet)).Click();
                testRepositoryCommonObj.WaitForLoadingIcon(browser, FrameGlobals.IconLoadTimeout);

                Assert.IsTrue(browser.IsElementPresent(BetslipControls.selectionSuspendedMsg), "Error message for suspended market is not shown in the betslip page");
                Console.WriteLine("Market Status validation is verified successfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CaptureScreenshot(browser, "MarketStatusValidation");
            }
        }


        /// <summary>
        /// Method to update and validate Market display status validation
        /// </summary>
        public void MarketDisplayStatusValidation(ISelenium browser, ISelenium adminBrowser, TestData testData, string navPanel,string eventName)
        {
            string xPath = string.Empty;

            try
            {
                #region Market Display validation

                adminCommonObj.UpdateMarketDisplayStatus(adminBrowser, testData.CategoryName, testData.ClassName, testData.TypeName, testData.SubTypeName, testData.EventName, testData.MarketName, "No");
                Console.WriteLine("Market Display is set to 'NO'");
                Thread.Sleep(FrameGlobals.OpenBetReflectTimeOut + 50000);

                btCommonObj.NavigateToSportsPage(browser, testData.ClassName, navPanel, testData.ClassName);
                xPath = "//div[@class='bxcl race-header ttu' and contains(text(), '" + testData.TypeName + "')]/following::nav[@class='bxc']/a/div[contains(text(), '" + eventName + "')]";
                testRepositoryCommonObj.clickObject(browser, xPath);

                Assert.IsTrue(browser.IsElementPresent("//div[starts-with(@id,'alert') and contains(string(),'Race not found')]"), "Market Display status is not updated in the portal");
                Console.WriteLine("Market Display Status validation is verified successfully");

                #endregion
            }
            catch (AutomationException ex)
            {
                Console.WriteLine(ex.Message);
                CaptureScreenshot(browser, "MarketDisplayStatusValidation");
            }
        }

        /// <summary>
        /// Method to update and validate Selection status validation
        /// </summary>
        public void SelectionStatusValidation(ISelenium browser, ISelenium adminBrowser, TestData testData, string navPanel,string eventName)
        {
            string selectionId = string.Empty;

            try
            {
                btCommonObj.AddAndVerifySelectionInBetslip_HR(browser, testData.ClassName, navPanel, testData.ClassName, testData.TypeName, testData.SubTypeName, testData.EventName, testData.MarketName, testData.SelectionName, testData.Odds, false, false);
                selectionId = testRepositoryCommonObj.GetSelectionIDFromBetslip(browser, testData.SelectionName, testData.EventName);

                adminCommonObj.UpdateSelection(adminBrowser, selectionId, testData.Odds, "Suspended", "");
                Thread.Sleep(FrameGlobals.OpenBetReflectTimeOut);

                btCommonObj.NavigateToEventDetailsPage(browser, testData.ClassName, navPanel, testData.ClassName, testData.TypeName, testData.SubTypeName, eventName, testData.MarketName);
                Assert.IsFalse(browser.IsVisible("//div[@class='bxcl bg2 mb4' and contains(string(),'" + testData.SelectionName + "')]//a"),
                                                                        "Selection is not displayed in the event details page");
                Console.WriteLine("" + eventName + " selections were suspended and updated in event details page navigated through Today tab from Hr home page");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CaptureScreenshot(browser, "SelectionStatusValidation");
            }
            finally
            {
                adminCommonObj.UpdateSelection(adminBrowser, selectionId, testData.Odds, "Active", "");
            }
        }

        /// <summary>
        /// Method to update and validate SelectionPrice Update validation
        /// </summary>
        public void SelectionPriceUpdateValidation(ISelenium browser, ISelenium adminBrowser, TestData testData, string navPanel, string odds,string eventName)
        {
            string selectionId = string.Empty;

            try
            {
                btCommonObj.AddAndVerifySelectionInBetslip_HR(browser, testData.ClassName, navPanel, testData.ClassName, testData.TypeName, testData.SubTypeName, testData.EventName, testData.MarketName, testData.SelectionName, testData.Odds, false, false);
                selectionId = testRepositoryCommonObj.GetSelectionIDFromBetslip(browser, testData.SelectionName, testData.EventName);

                adminCommonObj.UpdateSelection(adminBrowser, selectionId, odds, "Active", "");
                Thread.Sleep(FrameGlobals.OpenBetReflectTimeOut);
                Console.WriteLine("Selection price is updated in admin");

                btCommonObj.NavigateToEventDetailsPage_HR(browser, testData.ClassName, navPanel, testData.ClassName, testData.TypeName, testData.SubTypeName, eventName, testData.MarketName);
                Assert.IsTrue(browser.IsVisible("//div[@class='bxcl bg2 mb4' and contains(string(),'" + testData.SelectionName + "') and contains(string(),'" + odds + "')]"),
                                                        "Selection price is not displayed in the event details page");

                Console.WriteLine("" + odds + " selection price is updated on the Racecard accessed via Today tab under HR hompage");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CaptureScreenshot(browser, "SelectionPriceUpdateValidation");
            }
            finally
            {
                adminCommonObj.UpdateSelection(adminBrowser, selectionId, odds, "Active", "");
            }
        }
    }
}
