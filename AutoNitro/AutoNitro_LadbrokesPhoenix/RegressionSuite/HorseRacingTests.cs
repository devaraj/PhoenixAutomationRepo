using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.Threading;
using Framework;
using Framework.Common;
using Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using TestRepository.ControlsRepository;

namespace RegressionSuite
{
    [TestFixture(ApartmentState = ApartmentState.STA, TimeOut = FrameGlobals.TestCaseTimeout)]
    public class HorseRacingTests : BaseTest
    {
        TestRepository.LoginLogout.LoginLogoutFunctions loginLogOutObj = new TestRepository.LoginLogout.LoginLogoutFunctions();
        TestRepository.HorseRacing.HorseRacingFuntions horseRacingFuncObj = new TestRepository.HorseRacing.HorseRacingFuntions();
        TestRepository.Common testRepositoryCommonObj = new TestRepository.Common();
        Framework.Common.Common frameWorkCommonObj = new Framework.Common.Common();
        TestRepository.Betslip.BetslipFunctions btFunctionsObj = new TestRepository.Betslip.BetslipFunctions();
        AdminSuite.Common adminCommonObj = new AdminSuite.Common();

        /// <summary>
        /// Validate Event status updated on Racecard accessed via Next Races tab under HR hompage
        /// To validate Event display is updated for the event on the Racecard accessed via Next Races tab under HR hompage
        /// To validate Event display is updated for the event on the Next Races Module
        /// To validate Event status is updated for events on the Next Race Module
        /// </summary>
        /// <TestCaseId>148, 159, 274, 275</TestCaseId>
        /// <TestCasesCovered>3</TestCasesCovered>
        [Test]
        public void ValidateEventStatus_DisplayStatusInNextRacesTab()
        {
            ISelenium adminBrowser = null;
            TestData[] testData = new TestData[1];
            testData[0] = new TestData(0, "LiveEvents");
            string navPanel = "Next Races";
            string eventName = string.Empty;

            if (testData[0].EventName.Count() > 5)
                eventName = horseRacingFuncObj.GetEventNameFromDateTime_HR(MyBrowser, testData[0].DateTime);
            else
                eventName = testData[0].EventName;
            try
            {
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                loginLogOutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                btFunctionsObj.OddTypeSwitch(MyBrowser, "Decimal");

                adminBrowser = adminCommonObj.LogOnToAdmin();

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 148',  validate Event status is updated for the event on the Racecard accessed via Next Races tab under HR hompage*****");
                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 275', validate Event status is updated for events on the Next Race Module *****");
                horseRacingFuncObj.EventStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel);
                Console.WriteLine("" + testData[0].EventName + " is updated on the Racecard accessed via Next Races tab under HR hompage");

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 159', validate Event display is updated for the event on the Racecard accessed via Next Races tab under HR hompage *****");
                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 274', validate Event display is updated for the event on the Next Races Module *****");
                horseRacingFuncObj.EventDisplayStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, eventName);
                Console.WriteLine("Event display status is updated on the Racecard accessed via Next Races tab under HR homepage");

                Console.WriteLine("TestCase 'ValidateEventStatus_DisplayStatusInNextRacesTab' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateEventStatus_DisplayStatusInNextRacesTab");
                Console.WriteLine("TestCase 'ValidateEventStatus_DisplayStatusInNextRacesTab' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                adminCommonObj.UpdateEvents(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, "Active");
                adminCommonObj.UpdateEventsDisplaySts(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, "Yes");
                testRepositoryCommonObj.KillAdminObject();
            }
        }

        /// <summary>
        /// Method to validate Selection price updated for event on Racecard accessed via Next Races tab under HRhompage
        /// To validate Market display is updated for the event on Racecard accessed via Next Races tab under HR hompage
        /// To validate Selection status is updated for the event on the Racecard accessed via Next Races tab under HR hompage
        /// </summary>
        /// <TestCaseId>151, 152, 158</TestCaseId>
        /// <TestCasesCovered>3</TestCasesCovered>
        [Test]
        public void ValidateSelectionPrice_MarketDisplay_SelectionStatusInNextRaces()
        {
            ISelenium adminBrowser = null;
            TestData[] testData = new TestData[1];
            testData[0] = new TestData(12, "UpcomingEvents");
            string odds = "1.51";
            string navPanel = "Next Races";
            string eventName = string.Empty;

            if (testData[0].EventName.Count() > 5)
                eventName = horseRacingFuncObj.GetEventNameFromDateTime_HR(MyBrowser, testData[0].DateTime);
            else
                eventName = testData[0].EventName;

            try
            {
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                loginLogOutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                btFunctionsObj.OddTypeSwitch(MyBrowser, "Decimal");

                adminBrowser = adminCommonObj.LogOnToAdmin();

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 151', validate Selection price is updated for the event on the Racecard accessed via Next Races tab under HR hompage*****");
                horseRacingFuncObj.SelectionPriceUpdateValidation(MyBrowser, adminBrowser, testData[0], navPanel, odds, eventName);
                Console.WriteLine("" + odds + " selection price is updated on the Racecard accessed via Next Races tab under HR hompage");

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 158', validate Selection status is updated for the event on the Racecard accessed via Next Races tab under HR hompage*****");
                horseRacingFuncObj.SelectionStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, odds);
                Console.WriteLine("Selection Status is updated on the Racecard accessed via Next Races tab under HR homepage");

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 152', validate Market display is updated for the event on Racecard accessed via Next Races tab under HR hompage*****");
                horseRacingFuncObj.MarketDisplayStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, eventName);

                Console.WriteLine("TestCase 'ValidateSelectionPrice_MarketDisplay_SelectionStatusInNextRaces' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateSelectionPrice_MarketDisplay_SelectionStatusInNextRaces");
                Console.WriteLine("TestCase 'ValidateSelectionPrice_MarketDisplay_SelectionStatusInNextRaces' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                adminCommonObj.UpdateMarketDisplayStatus(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, "Yes");
                testRepositoryCommonObj.KillAdminObject();
            }
        }

        /// <summary>
        /// To validate Event status is updated for the event on Racecard accessed via Today tab under HR hompage
        /// To validate Selection price is updated for the event on Racecard accessed via Today tab under HR hompage
        /// </summary>
        /// <TestCaseId>215, 233</TestCaseId>
        /// <TestCasesCovered>2</TestCasesCovered>
        [Test]
        public void ValidateEventStatus_SelectionPriceInTodayTab()
        {
            ISelenium adminBrowser = null;
            TestData[] testData = new TestData[1];
            testData[0] = new TestData(13, "UpcomingEvents");
            string odds = "1.23";
            string navPanel = "Today";
            string eventName = string.Empty;

            if (testData[0].EventName.Count() > 5)
                eventName = horseRacingFuncObj.GetEventNameFromDateTime_HR(MyBrowser, testData[0].DateTime);
            else
                eventName = testData[0].EventName;

            try
            {
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                loginLogOutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                btFunctionsObj.OddTypeSwitch(MyBrowser, "Decimal");

                adminBrowser = adminCommonObj.LogOnToAdmin();

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 215',validate Event status is updated for the event on the Racecard accessed via Today tab under HR hompage *****");
                horseRacingFuncObj.EventStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel);
                Console.WriteLine("" + testData[0].EventName + " is updated on the Racecard accessed via Today tab under HR hompage");

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 233', validate Selection price is updated for the event on the Racecard accessed via Today tab under HR hompage *****");
                horseRacingFuncObj.SelectionPriceUpdateValidation(MyBrowser, adminBrowser, testData[0], navPanel, odds, eventName);
                Console.WriteLine("" + odds + " selection price is updated on the Racecard accessed via Today tab under HR hompage");

                Console.WriteLine("TestCase 'ValidateEventStatus_SelectionPriceInTodayTab' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateEventStatus_SelectionPriceInTodayTab");
                Console.WriteLine("TestCase 'ValidateEventStatus_SelectionPriceInTodayTab' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                adminCommonObj.UpdateEvents(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, "Active");
                testRepositoryCommonObj.KillAdminObject();
            }
        }

        /// <summary>
        /// To validate market status is updated for event on the Racecard accessed via Today tab under HR hompage
        /// To validate market display status is updated for event on the Racecard accessed via Today tab under HR hompage
        /// To validate Selection status is updated for event on the Racecard accessed via Today tab under HR hompage
        /// </summary>
        /// <TestCaseId>234, 244, 245</TestCaseId>
        /// <TestCasesCovered>3</TestCasesCovered>
        [Test]
        public void ValidateMarket_MktDisplay_SelectionStatusInTodayTab()
        {
            ISelenium adminBrowser = null;
            TestData[] testData = new TestData[1];
            testData[0] = new TestData(13, "UpcomingEvents");
            string navPanel = "Today";
            string eventName = string.Empty;

            if (testData[0].EventName.Count() > 5)
                eventName = horseRacingFuncObj.GetEventNameFromDateTime_HR(MyBrowser, testData[0].DateTime);
            else
                eventName = testData[0].EventName;

            try
            {
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                loginLogOutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                btFunctionsObj.OddTypeSwitch(MyBrowser, "Decimal");

                adminBrowser = adminCommonObj.LogOnToAdmin();

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 234', validate market status is updated for event on the Racecard accessed via Today tab under HR hompage*****");
                horseRacingFuncObj.MarketStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, eventName);
                Console.WriteLine("" + testData[0].MarketName + " is displayed and in suspended status in the event details page");
                
                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 244', validate market display status is updated for event on the Racecard accessed via Today tab under HR hompage*****");
                horseRacingFuncObj.MarketDisplayStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, eventName);
                Console.WriteLine("" + testData[0].MarketName + " display status is updated in event details page navigated through Today tab from HR home page");
                
                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 245', validate Selection status is updated for event on the Racecard accessed via Today tab under HR hompage*****");
                horseRacingFuncObj.SelectionStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, eventName);
                Console.WriteLine("" + testData[0].EventName + " selections were suspended and updated in event details page navigated through Today tab from Hr home page");
                
                Console.WriteLine("ValidateMarket_MktDisplay_SelectionStatusInTodayTab - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateMarket_MktDisplay_SelectionStatusInTodayTab");
                Console.WriteLine("TestCase 'ValidateMarket_MktDisplay_SelectionStatusInTodayTab' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                adminCommonObj.UpdateMarketStatus(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, "Active");
                adminCommonObj.UpdateMarketDisplayStatus(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, "Yes");
                testRepositoryCommonObj.KillAdminObject();
            }
        }

        /// <summary>
        /// To validate market status is updated for event on Racecard accessed via Tomorrow tab under HR hompage
        /// To validate Market display is updated for event on Racecard accessed via Tomorrow tab under HR hompage
        /// </summary>
        /// <TestCaseId>247, 273</TestCaseId>
        /// <TestCasesCovered>2</TestCasesCovered>
        [Test]
        public void VerifyMarketStatus_MarketDisplay_EventDisplayInTomorrowTab()
        {
            ISelenium adminBrowser = null;
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            TestData[] testData = new TestData[1];
            testData[0] = new TestData(3, "UpcomingEvents");
            System.Data.DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "UpcomingEvents");
            string DateTime = dt.Rows[3]["DATE/TIME"].ToString();
            string navPanel = "Tomorrow";
            string xPath = string.Empty;
            string eventName = string.Empty;

            if (testData[0].EventName.Count() > 5)
                eventName = horseRacingFuncObj.GetEventNameFromDateTime_HR(MyBrowser, testData[0].DateTime);
            else
                eventName = testData[0].EventName;

            try
            {
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                loginLogOutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                btFunctionsObj.OddTypeSwitch(MyBrowser, "Decimal");

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 247', validate market status is updated for event on Racecard accessed via Tomorrow tab under HR hompage*****");

                #region Market Status validation

                adminBrowser = adminCommonObj.LogOnToAdmin();
                horseRacingFuncObj.MarketStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, eventName);
                Console.WriteLine("Market Status validation is verified successfully");

                #endregion

                #region Market Display validation

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 273', validate Market display is updated for event on Racecard accessed via Tomorrow tab under HR hompage*****");
                horseRacingFuncObj.MarketDisplayStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, eventName);
                Console.WriteLine("Market Display Status validation is verified successfully");
                
                #endregion

                Console.WriteLine("TestCase 'VerifyMarketStatus_MarketDisplay_EventDisplayInTomorrowTab' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyMarketStatus_MarketDisplay_EventDisplayInTomorrowTab");
                Console.WriteLine("TestCase 'VerifyMarketStatus_MarketDisplay_EventDisplayInTomorrowTab' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                adminCommonObj.UpdateMarketStatus(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, "Active");
                adminCommonObj.UpdateMarketDisplayStatus(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, "Yes");
                testRepositoryCommonObj.KillAdminObject();
            }
        }

        /// <summary>
        /// To validate market status is updated for event on Racecard accessed via Next Races tab under HR hompage
        /// </summary>
        /// <TestCaseId>217</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        public void VerifyMarketStatusInNextRacesTab()
        {
            ISelenium adminBrowser = null;
            TestData[] testData = new TestData[1];
            testData[0] = new TestData(12, "UpcomingEvents");
            string navPanel = "Next Races";
            string eventName = string.Empty;

            if (testData[0].EventName.Count() > 5)
                eventName = horseRacingFuncObj.GetEventNameFromDateTime_HR(MyBrowser, testData[0].DateTime);
            else
                eventName = testData[0].EventName;

            try
            {
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                loginLogOutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                testRepositoryCommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                btFunctionsObj.OddTypeSwitch(MyBrowser, "Decimal");

                Console.WriteLine("***** Executing Test Case --- 'HorseRacing : 217', validate market status is updated for event on Racecard accessed via Next Races tab under HR hompage *****");

                adminBrowser = adminCommonObj.LogOnToAdmin();
                horseRacingFuncObj.MarketStatusValidation(MyBrowser, adminBrowser, testData[0], navPanel, eventName);
                Console.WriteLine("TestCase 'VerifyMarketStatus_MarketDisplay_EventDisplayInTomorrowTab' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyMarketStatusInNextRacesTab");
                Console.WriteLine("TestCase 'VerifyMarketStatusInNextRacesTab' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                adminCommonObj.UpdateMarketStatus(adminBrowser, testData[0].CategoryName, testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, "Active");
                testRepositoryCommonObj.KillAdminObject();
            }
        }


    }
}
