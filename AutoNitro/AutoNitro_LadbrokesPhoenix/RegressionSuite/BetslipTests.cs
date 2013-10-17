using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium.Interactions;

using Selenium;
using OpenQA.Selenium;
using MbUnit.Framework;
using Framework;
using Framework.Common;
using TestRepository.ControlsRepository;
using OpenQA.Selenium.Firefox;


namespace PreProdSuite
{
    [TestFixture(ApartmentState = ApartmentState.STA, TimeOut = FrameGlobals.TestCaseTimeout)]
    public class BetslipTests : BaseTest
    {
        TestRepository.LoginLogout.LoginLogoutFunctions BTloginLogoutObj = new TestRepository.LoginLogout.LoginLogoutFunctions();
        TestRepository.Betslip.BetslipFunctions BTbetslipObj = new TestRepository.Betslip.BetslipFunctions();
        TestRepository.Common BTcommonObj = new TestRepository.Common();
        Framework.Common.Common BTframeworkCommonObj = new Framework.Common.Common();
        AdminSuite.Common AdminCommonObj = new AdminSuite.Common();


        /// <summary>
        /// To check the information icon on the betslip when user adds selection to it, user NOT logged in
        /// To check the information icon on the betslip when user adds selection to it, user is logged in
        /// </summary>
        /// <TestCaseId>283,288</TestCaseId>
        /// <TestCasesCovered>2</TestCasesCovered>
        [Test]
        public void VerifyBetInfoInBetslip()
        {
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            string EWterms;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(14, "FutureEvents");
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetInfoInBetslip', Verify the details on the info frame of the Betslip (user Logged in/Logged out) *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                // Verify the min/max stake is not displayed in Bet Info if user is logout
                EWterms = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, EWterms, "Single", 1);

                BTbetslipObj.VerifyBetslipInfo(MyBrowser, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, EWterms);
                Console.WriteLine("Verification of Bet Info details in Betslip (User Logged Out) was successful");

                // Verify the min/max stake is displayed in the Bet Info on Login
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, EWterms, "Single", 1);
                BTbetslipObj.VerifyBetslipInfo(MyBrowser, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, EWterms);
                Console.WriteLine("Verification of Bet Info details in Betslip (User Logged In) was successful");
                Console.WriteLine("TestCase 'VerifyBetInfoInBetslip' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetInfoInBetslip");
                Console.WriteLine("TestCase 'VerifyBetInfoInBetslip' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// User can remove a single bet or remove all bets from the betslip
        /// User can remove selections from the betslip by retapping selections
        /// To validate empty betslip page
        /// Potential returns re calculated on removal of selections
        /// </summary>
        /// <TestCaseId>301,302,303,439</TestCaseId>
        /// <TestCasesCovered>4</TestCasesCovered>
        [Test]
        public void VerifyRemoveFunctionalityInBetslip()
        {
            string xPath;
            int initBetslipcount, BetslipCnt, Betslipcount;
            TestData[] testDataLst = new TestData[3];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(1, "FutureEvents");
            testDataLst[2] = new TestData(2, "FutureEvents");
            Console.WriteLine("***** Executing Test Case --- 'VerifyRemoveFunctionalityInBetslip', Verify Remove/RemoveAll functionality in Betslip *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region check remove button functionality
                //Verify the Betslip page when no selections are added
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipButton);
                Assert.IsTrue(MyBrowser.IsVisible(BetslipControls.betslipBanner), "Betslip page is not displayed");
                initBetslipcount = BTbetslipObj.GetBetslipCount(MyBrowser);
                Assert.IsTrue(initBetslipcount == 0, "Betslip count is not 0(No selectins are added)");
                Assert.IsTrue(MyBrowser.IsTextPresent("Your betslip is empty"), "'Your betslip is empty' message was not found in betslip when no selections are added");
                BTcommonObj.clickObject(MyBrowser, LoginLogoutControls.LadbrokesHomeLink);
                Console.WriteLine("Verification of Betslip when no selections are added was successful");

                for (int i = 0; i < testDataLst.Length; i++)
                {
                    BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[i].ClassName, testDataLst[i].TypeName, testDataLst[i].SubTypeName, testDataLst[i].EventName, testDataLst[i].MarketName, testDataLst[i].SelectionName, String.Format("{0:0.00}", double.Parse(testDataLst[i].Odds)), false);
                    BTbetslipObj.EnterStake(MyBrowser, testDataLst[i].EventName, testDataLst[i].SelectionName, testDataLst[i].MarketName, testDataLst[i].Odds, testDataLst[i].Stake, "Single", false);

                    //validate the betslip counter                                      
                    Betslipcount = BTbetslipObj.GetBetslipCount(MyBrowser);
                    if (Betslipcount != initBetslipcount + 1)
                    {
                        Fail("Betslip counter failed to update on adding a selection to Betslip");
                    }
                    initBetslipcount = Betslipcount;
                }
                Console.WriteLine("Betslip Counter was updated and validated successfully");

                //Get the total stake and PR
                double totalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                double totalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Potential Returns");

                double PRsel1 = BTbetslipObj.GetPotentialReturnFromBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "Single");
                double PRsel2 = BTbetslipObj.GetPotentialReturnFromBetSlip(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, "Single");
                double PRsel3 = BTbetslipObj.GetPotentialReturnFromBetSlip(MyBrowser, testDataLst[2].EventName, testDataLst[2].SelectionName, testDataLst[2].MarketName, testDataLst[2].Odds, "Single");


                //Validate the Back  button functionality in Betslip                
                Thread.Sleep(2000);
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipBackArrow);
                //                MyBrowser.Refresh();
                //                BTframeworkCommonObj.PageSync(MyBrowser);

                string pageTitle = "//span[@class='t7 page-title' and contains(text(), '" + testDataLst[2].EventName + "')]";
                Assert.IsTrue(MyBrowser.IsVisible(pageTitle), "Failed to naivagate back to the Event details page '" + testDataLst[2].EventName + "' from betslip");

                //Validate the re taping the selected selection removes it from the betslip
                xPath = "//div[@class='bxcl ml10  odds-text mr2  type' and contains(text(), '" + testDataLst[2].SelectionName + "')]//following::span[@class='odds-convert' and contains(text(),'" + String.Format("{0:0.00}", double.Parse(testDataLst[2].Odds)) + "')]";

                BTcommonObj.clickObject(MyBrowser, xPath);
                BetslipCnt = BTbetslipObj.GetBetslipCount(MyBrowser);
                Assert.IsTrue(BetslipCnt.Equals(initBetslipcount - 1), "Betslip counter failed to update on removiong a selection from Betslip");

                BTcommonObj.NavigateToHomePage(MyBrowser, "Side Menu");
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipButton);

                //Verify the total stake and PR is recalculated
                Assert.IsTrue(totalStake - Convert.ToDouble(testDataLst[2].Stake) == BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake"), "Total stake failed to update on removing a selection");
                Assert.IsTrue(totalPR - PRsel3 == BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Potential Returns"), "Total PR failed to update on removing a selection");

                //Validate the Remove functionality in Betslip
                BTbetslipObj.RemoveSelFromBetslip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, false);

                //Verify the total stake and PR is recalculated
                Assert.IsTrue(totalStake - Convert.ToDouble(testDataLst[2].Stake) - Convert.ToDouble(testDataLst[0].Stake) == BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake"), "Total stake failed to update on removing a selection");
                Assert.IsTrue(totalPR - PRsel3 - PRsel1 == BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Potential Returns"), "Total PR failed to update on removing a selection");

                //Remove all selections
                BTbetslipObj.RemoveSelFromBetslip(MyBrowser, "", "", true);
                Console.WriteLine("TestCase 'VerifyRemoveFunctionalityInBetslip' - PASS");
                #endregion
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyRemoveFunctionalityInBetslip");
                Console.WriteLine("TestCase 'VerifyRemoveFunctionalityInBetslip' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// To validate E/W bet placement from race winner market
        /// </summary>
        /// <TestCaseId>374</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyBetplacement_HorseRacingEW()
        {
            string[] aryOdd = new string[1];
            string EWterms;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(14, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_HorseRacingEW', Verify the Bet Placement on Horse Racing event and check Each Way *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                EWterms = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, EWterms, "Single", 1);

                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, EWterms, "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, EWterms, false, "Single", 1);

                Console.WriteLine("Verification of Betplacement on Horse Racing EW event was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_HorseRacingEW' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_HorseRacingEW");
                Console.WriteLine("TestCase 'VerifyBetplacement_HorseRacingEW' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// To check the betslip when user adds selection to it
        /// To validate bet placement from Race Winner markets
        /// Validate single bet placement
        /// Betslip counter shows single bet added to the betslip
        /// </summary>
        /// <TestCaseId>255,373,393,420</TestCaseId>
        /// <TestCasesCovered>4</TestCasesCovered>
        [Test]
        public void VerifyBetplacement_SingleBet()
        {
            string[] aryOdd = new string[1];
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_SingleBet', Verify Single Bet Placement(Outright Market) *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);

                Console.WriteLine("Verification of Betplacement on Single bet was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_UserLoggedIn' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_SingleBet");
                Console.WriteLine("TestCase 'VerifyBetplacement_SingleBet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// To check the betslip when user adds multiple selections from same event to it
        /// </summary>
        /// <TestCaseId>286</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyBetPlacement_MultipleSels_SameEvent()
        {
            string[] aryOdd1 = new string[1];
            string[] aryOdd2 = new string[1];
            string[] aryOdd3 = new string[1];
            double prevTotalStake, prevTotalPR, expTotalStake;
            TestData[] testDataLst = new TestData[3];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(1, "FutureEvents");
            testDataLst[2] = new TestData(2, "FutureEvents");
            aryOdd1[0] = testDataLst[0].Odds;
            aryOdd2[0] = testDataLst[1].Odds;
            aryOdd3[0] = testDataLst[2].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetPlacement_MultipleSels_SameEvent', Verify Bet placement for multiple selections of the same Event *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region VerifyBetDetails&placement
                //Add Selctions and verify betdetails
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[2].ClassName, testDataLst[2].TypeName, testDataLst[2].SubTypeName, testDataLst[2].EventName, testDataLst[2].MarketName, testDataLst[2].SelectionName, testDataLst[2].Odds, false);

                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[0].MarketName, testDataLst[1].Odds, "", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[2].EventName, testDataLst[2].SelectionName, testDataLst[0].MarketName, testDataLst[2].Odds, "", "Single", 1);

                //Enter Stake and Verify Bet Details
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd1, testDataLst[0].Stake, "", "Single", "", "");

                //Enter Stake for 2nd selection and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, testDataLst[1].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOdd2, testDataLst[1].Stake, "", "Single", prevTotalStake.ToString(), prevTotalPR.ToString());

                //Enter Stake for 3rd selection and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[2].EventName, testDataLst[2].SelectionName, testDataLst[2].MarketName, testDataLst[2].Odds, testDataLst[2].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[2].EventName, testDataLst[2].SelectionName, testDataLst[2].MarketName, aryOdd3, testDataLst[2].Stake, "", "Single", prevTotalStake.ToString(), prevTotalPR.ToString());

                IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
                TimeSpan ts = new TimeSpan(0, 0, 5);
                driver.Manage().Timeouts().ImplicitlyWait(ts);
                Assert.IsFalse(MyBrowser.IsElementPresent(BetslipControls.multiplesBanner), "Multi Bets was displayed on adding multiple selections from the same Event");

                expTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                BTbetslipObj.BetPlacement(MyBrowser, expTotalStake.ToString(), 3, "Home", false);
                #endregion
                Console.WriteLine("TestCase 'VerifyBetPlacement_MultipleSels_SameEvent' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetPlacement_MultipleSels_SameEvent");
                Console.WriteLine("TestCase 'VerifyBetPlacement_MultipleSels_SameEvent' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Validate Potential returns
        /// </summary>
        /// <TestCaseId>315</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyBetplacement_DoubleBet()
        {
            string[] aryOdd = new string[2];
            string doubleStake = "1.00";
            string doubleSel;
            TestData[] testDataLst = new TestData[2];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(11, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_DoubleBet', Verify Double Bet Placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add 2 selections to betslip
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);

                //Enter the stake
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "No", "Double", 1);
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", doubleStake, "Double", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, doubleStake, "", "Double", "", "");

                //Place bet and validate the receipt
                doubleSel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName;
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", doubleSel, aryOdd, doubleStake, "", false, "Double", 1);
                Console.WriteLine("Verification of Double Betplacement was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_DoubleBet' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_DoubleBet");
                Console.WriteLine("TestCase 'VerifyBetplacement_DoubleBet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Place multiple bets with each-way enabled.
        /// </summary>
        /// <TestCaseId>409</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyBetplacement_DoubleBetEW()
        {
            string[] aryOdd = new string[2];
            string doubleStake = "1.00";
            string doubleSel, EWterms1, EWterms2;
            TestData[] testDataLst = new TestData[2];
            testDataLst[0] = new TestData(11, "FutureEvents");
            testDataLst[1] = new TestData(14, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_DoubleBetEW', Verify Double Each Way Bet Placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add 2 selections to betslip
                EWterms1 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, EWterms1, "Single", 1);
                EWterms2 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, EWterms2, "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Double", 1);

                //Enter the stake
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", doubleStake, "Double", true);
                //Verify BetDetails for EW Multiples

                string EW = EWterms1 + "|" + EWterms2;
                doubleSel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName;

                //BTbetslipObj.VerifyBetDetailsEWmultiples(MyBrowser, doubleStake, "Double");
                BTbetslipObj.VerifyBetDetails(MyBrowser, "", "", "", aryOdd, doubleStake, EW, "Double", "", "");

                //Place bet and validate the receipt
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", doubleSel, aryOdd, doubleStake, EW, false, "Double", 1);

                Console.WriteLine("Verification of Double Each Way bet placement was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_DoubleBet' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_DoubleBet");
                Console.WriteLine("TestCase 'VerifyBetplacement_DoubleBet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Validate that only if all of the selections  in the betslip are each-way enabled, 
        /// then the multiple bets should also be each-way enabled.
        /// Validate that only if all the selections in the betslip are not each-way enabled, 
        /// then the multiple bets should not be each-way enabled.
        /// </summary>
        /// <TestCaseId>304,305</TestCaseId>
        /// <TestCasesCovered>2</TestCasesCovered>
        [Test]
        public void VerifyEWDisplayForMultipleBets_TrebleBetPlacement()
        {
            string[] aryOdd = new string[3];
            string trebleStake = "1.00";
            string trebleSel, EWterms1, EWterms2, EWterms4, EW;
            TestData[] testDataLst = new TestData[4];
            testDataLst[0] = new TestData(11, "FutureEvents");
            testDataLst[1] = new TestData(14, "FutureEvents");
            testDataLst[2] = new TestData(0, "FutureEvents");
            testDataLst[3] = new TestData(17, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            aryOdd[2] = testDataLst[3].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyEWDisplayForMultipleBets_TrebleBetPlacement', Verify EW is enabled/displayed for different bets when individual selections are EW enabled/displayed. Verify a Treble EW Bet placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region Check EW display
                Console.WriteLine("Verify EW is enabled for Singles(1st Selection is EW enabled).......");
                //Add 1 selections to betslip and verify the EW is enabled
                EWterms1 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, EWterms1, "Single", 1);
                Console.WriteLine("Verification was successful.....");

                Console.WriteLine("Verify EW is enabled for Double Bet(2nd Selection is EW enabled).......");
                //Add another selection and verify EW is enabled for Single and double
                EWterms2 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, EWterms2, "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Double", 1);
                Console.WriteLine("Verification was successful.....");

                Console.WriteLine("Verify EW is disabled for Double & Treble bet(3rd Selection is EW disabled).......");
                //Add another selection and verify EW is disabled Double and Treble
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[2].ClassName, testDataLst[2].TypeName, testDataLst[2].SubTypeName, testDataLst[2].EventName, testDataLst[2].MarketName, testDataLst[2].SelectionName, testDataLst[2].Odds, false);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[2].EventName, testDataLst[2].SelectionName, testDataLst[2].MarketName, testDataLst[2].Odds, "No", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "No", "Double", 4);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "No", "Treble", 4);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "No", "Trixie", 4);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "No", "Patent", 4);
                Console.WriteLine("Verification was successful.....");

                //Remove the selection added previously
                Console.WriteLine("Verify EW is enabled for Double bet(3rd Selection is removed).......");
                BTbetslipObj.RemoveSelFromBetslip(MyBrowser, testDataLst[2].EventName, testDataLst[2].SelectionName, false);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Double", 1);
                Console.WriteLine("Verification was successful.....");

                Console.WriteLine("Verify EW is enabled for Double, Treble, Trixie, Patent bets(4th Selection is EW enabled).......");
                //Add another selection and verify EW is disabled Double and Treble
                EWterms4 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[3].ClassName, testDataLst[3].TypeName, testDataLst[3].SubTypeName, testDataLst[3].EventName, testDataLst[3].MarketName, testDataLst[3].SelectionName, testDataLst[3].Odds, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[3].EventName, testDataLst[3].SelectionName, testDataLst[3].MarketName, testDataLst[3].Odds, EWterms4, "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Double", 4);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Treble", 4);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Trixie", 4);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Patent", 4);
                Console.WriteLine("Verification was successful.....");

                //Place Treble Bet
                Console.WriteLine("Verify Treble EW bet placemnet).......");
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", trebleStake, "Treble", true);

                //Verify BetDetails for EW Multiples               
                trebleSel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName + "|" + testDataLst[3].SelectionName;
                EW = EWterms1 + "|" + EWterms2 + "|" + EWterms4;

                //BTbetslipObj.VerifyBetDetailsEWmultiples(MyBrowser, trebleStake, "Treble");
                BTbetslipObj.VerifyBetDetails(MyBrowser, "", "", "", aryOdd, trebleStake, EW, "Treble", "", "");
                Console.WriteLine("Verification was successful.....");

                //Place bet and validate the receipt
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", trebleSel, aryOdd, trebleStake, EW, false, "Treble", 1);
                Console.WriteLine("TestCase 'VerifyEWDisplayForMultipleBets_TrebleBetPlacement' - PASS");
                #endregion
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyEWDisplayForMultipleBets_TrebleBetPlacement");
                Console.WriteLine("TestCase 'VerifyEWDisplayForMultipleBets_TrebleBetPlacement' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Betslip counter shows multiple bets added to the betslip
        /// </summary>
        /// <TestCaseId>419</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyBetplacement_TrebleBet()
        {
            string[] aryOdd = new string[3];
            string trebleStake = "1.00";
            string trebleSel;
            TestData[] testDataLst = new TestData[3];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(11, "FutureEvents");
            testDataLst[2] = new TestData(14, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            aryOdd[2] = testDataLst[2].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_TrebleBet', Verify Treble Bet Placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add 3 selections to betslip
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[2].ClassName, testDataLst[2].TypeName, testDataLst[2].SubTypeName, testDataLst[2].EventName, testDataLst[2].MarketName, testDataLst[2].SelectionName, testDataLst[2].Odds, false);

                //Verify the Betslip
                string DoubleBetXpath = "//span[@class='b' and contains(text(), 'Double')]/following::span[contains(text(), '3')]";
                Assert.IsTrue(MyBrowser.IsElementPresent(DoubleBetXpath), "Double(3) betType is not present in the Betslip");
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "No", "Treble", 4);

                //Enter the stake
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", trebleStake, "Treble", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, trebleStake, "", "Treble", "", "");

                //Place bet and validate the receipt
                trebleSel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName + "|" + testDataLst[2].SelectionName;
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", trebleSel, aryOdd, trebleStake, "", false, "Treble", 1);

                Console.WriteLine("Verification of Treble Betplacement was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_TrebleBet' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_TrebleBet");
                Console.WriteLine("TestCase 'VerifyBetplacement_TrebleBet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCaseId></TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetplacement_TrixieBet()
        {
            string[] aryOdd = new string[3];
            string trixieStake = "1.00";
            string trixieSel;
            TestData[] testDataLst = new TestData[3];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(11, "FutureEvents");
            testDataLst[2] = new TestData(14, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            aryOdd[2] = testDataLst[2].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_TrixieBet', Verify Trixie Bet Placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add 3 selections to betslip
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[2].ClassName, testDataLst[2].TypeName, testDataLst[2].SubTypeName, testDataLst[2].EventName, testDataLst[2].MarketName, testDataLst[2].SelectionName, testDataLst[2].Odds, false);

                //Enter the stake
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "", "Trixie", 4);
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", trixieStake, "Trixie", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, trixieStake, "", "Trixie", "", "");

                //Place bet and validate the receipt
                trixieSel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName + "|" + testDataLst[2].SelectionName;
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", trixieSel, aryOdd, trixieStake, "", false, "Trixie", 1);

                Console.WriteLine("Verification of Trixie Betplacement was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_TrixieBet' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_TrixieBet");
                Console.WriteLine("TestCase 'VerifyBetplacement_TrixieBet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCaseId></TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetplacement_PatentBet()
        {
            string[] aryOdd = new string[3];
            string patentStake = "1.00";
            string patentSel;
            TestData[] testDataLst = new TestData[3];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(11, "FutureEvents");
            testDataLst[2] = new TestData(14, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            aryOdd[2] = testDataLst[2].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_PatentBet', Verify Patent Bet Placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add 3 selections to betslip               
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[2].ClassName, testDataLst[2].TypeName, testDataLst[2].SubTypeName, testDataLst[2].EventName, testDataLst[2].MarketName, testDataLst[2].SelectionName, testDataLst[2].Odds, false);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "", "Patent", 4);

                //Enter the stake
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", patentStake, "Patent", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, patentStake, "", "Patent", "", "");
                BTcommonObj.clickObject(MyBrowser, BetslipControls.placeBet);

                //Login and verify the Bet details
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, patentStake, "", "Patent", "", "");
                BTcommonObj.NavigateToHomePage(MyBrowser, "Side Manu");
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipCount);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, patentStake, "", "Patent", "", "");

                //Place bet and validate the receipt
                patentSel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName + "|" + testDataLst[2].SelectionName;
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", patentSel, aryOdd, patentStake, "", false, "Patent", 1);

                Console.WriteLine("Verification of Patent Betplacement was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_PatentBet' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_PatentBet");
                Console.WriteLine("TestCase 'VerifyBetplacement_PatentBet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Validate quick stake button
        /// Stake field length
        /// </summary>
        /// <TestCaseId>448,411</TestCaseId>
        /// <TestCasesCovered>2</TestCasesCovered>
        [Test]
        public void VerifyQuickStakeFunctionalityAndStakeFieldLength()
        {
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            string[] aryOdd = new string[1];
            string selID, xPathPlus, xPathMinus, stakeXPath, stakeVal, actStakeVal, expStakeVal;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyQuickStakeFunctionalityAndStakeFieldLength', Verify the Quick stake (+, -) button functionalities in betslip and the Stake field Length *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                #region Verify stake Filed Length
                actStakeVal = "12345678901";
                expStakeVal = "1234567891";
                selID = BTbetslipObj.GetSelectionId(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, String.Format("{0:0.00}", double.Parse(testDataLst[0].Odds)), "");
                IWebElement inputStake = driver.FindElement(By.Id("slip-odds-stake-SGL_" + selID));

                inputStake.SendKeys(actStakeVal);
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipButton);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, expStakeVal, "", "Single", "", "");
                //Refresh the page and verify the betdetails
                MyBrowser.Refresh();
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, expStakeVal, "", "Single", "", "");
                //Navigate to home and return and verify the betdetails
                BTcommonObj.NavigateToHomePage(MyBrowser, "Side Manu");
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipButton);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, expStakeVal, "", "Single", "", "");
                Console.WriteLine("Stake filed length was verified successfully ");
                #endregion

                #region validate+-ButtonFuntionality
                stakeVal = "10";
                xPathPlus = "//input[@id='slip-odds-stake-SGL_" + selID + "']/following::div[@class='bxc stake-button' and text()='+']";
                xPathMinus = "//input[@id='slip-odds-stake-SGL_" + selID + "']/../../div[@class='bxc stake-button' and text()='-']";
                stakeXPath = "//div[@class='slip-item']//span[contains(text(), '" + testDataLst[0].EventName + "')]/following-sibling::span[contains(text(), '" + testDataLst[0].SelectionName + "')]/following::span[contains(text(), '" + String.Format("{0:0.00}", double.Parse(testDataLst[0].Odds)) + "')]";
                inputStake = driver.FindElement(By.Id("slip-odds-stake-SGL_" + selID));
                IWebElement plusElement = driver.FindElement(By.XPath(xPathPlus));
                IWebElement minusElement = driver.FindElement(By.XPath(xPathMinus));

                //Validate the + functionality in betslip
                inputStake.Clear();
                Thread.Sleep(1000);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                plusElement.Click();
                Thread.Sleep(1000);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, stakeVal, "", "Single", "", "");
                Console.WriteLine("'+' button functionality in betslip was validated successfully ");

                //Validate the - functionality in betslip
                minusElement.Click();
                Thread.Sleep(1000);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);
                Console.WriteLine("'-' button functionality in betslip was validated successfully ");
                #endregion
                Console.WriteLine("TestCase 'VerifyQuickStakeFunctionalityAndStakeFieldLength' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyQuickStakeFunctionalityAndStakeFieldLength");
                Console.WriteLine("TestCase 'VerifyQuickStakeFunctionalityAndStakeFieldLength' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Customer is shown error when placing bets for stakes more than the allowed limit
        /// Max Stake
        /// Min Stake
        /// </summary>
        /// <TestCaseId>423,441,442</TestCaseId>
        /// <TestCasesCovered>3</TestCasesCovered>
        [Test]
        public void VerifyMinMaxStakeFunctionality()
        {
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "FutureEvents");
            string minValue, maxValue, xPath, selID, actErrorMsg, expErrorMsg, minStake, maxStake;
            Console.WriteLine("***** Executing Test Case --- 'VerifyMinMaxStakeFunctionality', Verify the Error messages displyed on entering stake Less than Minimum stake and More than Maximun stake. *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region Verify MinMax Stake functionality
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                selID = BTbetslipObj.GetSelectionId(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "Single");
                xPath = "//div[@id='stakeRangeError_SGL_" + selID + "']";

                // Validate the Min Stake functionality
                minValue = BTbetslipObj.GetMinMaxStakeValue(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, "Min");
                minStake = Convert.ToString(double.Parse(minValue) - 0.01);
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, minStake, "", false);
                actErrorMsg = BTbetslipObj.CaptureBetslipErrorMessage(MyBrowser, xPath);
                expErrorMsg = "The minimum stake for this bet is: £ " + minValue + ".";
                Assert.IsTrue(actErrorMsg.ToLower().Trim() == expErrorMsg.ToLower().Trim(), "Mismatch in Actual and Expected error messages (Min Stake)");
                Console.WriteLine("Error message for Minimum stake was validated successfully");

                // Validate the Max Stake functionality
                maxValue = BTbetslipObj.GetMinMaxStakeValue(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, "Max");
                maxStake = Convert.ToString(double.Parse(maxValue) + 0.1);
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, maxStake, "", false);
                actErrorMsg = BTbetslipObj.CaptureBetslipErrorMessage(MyBrowser, xPath);
                expErrorMsg = "The maximum stake for this bet is: £ " + maxValue + ".";
                Assert.IsTrue(actErrorMsg.ToLower().Trim() == expErrorMsg.ToLower().Trim(), "Mismatch in Actual and Expected error messages (Max Stake)");
                Console.WriteLine("Error message for Maximum stake was validated successfully");

                //Click on Place bet and Verify for stake more than allowed limit
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, maxStake, "", false);
                BTcommonObj.clickObject(MyBrowser, BetslipControls.placeBet);
                actErrorMsg = BTbetslipObj.CaptureBetslipErrorMessage(MyBrowser, xPath);
                expErrorMsg = "The maximum stake for this bet is: £ " + maxValue + ".";
                Assert.IsTrue(actErrorMsg.ToLower().Trim() == expErrorMsg.ToLower().Trim(), "Mismatch in Actual and Expected error messages (Max Stake)");
                Console.WriteLine("Error message for validated successfully when user tries to place bet for stale more than the allowed limit");
                #endregion
                Console.WriteLine("TestCase 'VerifyMinMaxStakeFunctionality' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyMinMaxStakeFunctionality");
                Console.WriteLine("TestCase 'VerifyMinMaxStakeFunctionality' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// validate that user cannot place bets without entering stakes
        /// </summary>
        /// <TestCaseId>444</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyErrorMessage_AttemptToPlaceBetWithEmptyStake()
        {
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "FutureEvents");
            string alertXpath;
            Console.WriteLine("***** Executing Test Case --- 'VerifyErrorMessage_AttemptToPlaceBetWithEmptyStake', Verify the Error messages displyed on attempting to Place Bet without entering any Stake *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);

                BTcommonObj.clickObject(MyBrowser, BetslipControls.placeBet);
                alertXpath = "//div[starts-with(@class, 'alert-box')]/div[contains(text(),'Please add a stake before attempting to place a bet.')]";

                Assert.IsTrue(MyBrowser.IsElementPresent(alertXpath), "Error message was not displayed on attempting to Place Bet without entering any Stake");
                BTcommonObj.clickObject(MyBrowser, BetslipControls.closeButtonInInfoContainer);
                Console.WriteLine("Betslip Error message was validated successfully - 'Place Bet without entering any Stake'");
                Console.WriteLine("TestCase 'VerifyErrorMessage_AttemptToPlaceBetWithEmptyStake' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyErrorMessage_AttemptToPlaceBetWithEmptyStake");
                Console.WriteLine("TestCase 'VerifyErrorMessage_AttemptToPlaceBetWithEmptyStake' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetplacement_Accumulator4Bet()
        {
            string[] aryOdd = new string[4];
            string accumulator4Stake = "1.00";
            string accumulator4Sel;
            TestData[] testDataLst = new TestData[4];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(11, "FutureEvents");
            testDataLst[2] = new TestData(14, "FutureEvents");
            testDataLst[3] = new TestData(17, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            aryOdd[2] = testDataLst[2].Odds;
            aryOdd[3] = testDataLst[3].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_Accumulator4Bet', Verify Accumulator (4) Bet Placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add 4 selections to betslip               
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[2].ClassName, testDataLst[2].TypeName, testDataLst[2].SubTypeName, testDataLst[2].EventName, testDataLst[2].MarketName, testDataLst[2].SelectionName, testDataLst[2].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[3].ClassName, testDataLst[3].TypeName, testDataLst[3].SubTypeName, testDataLst[3].EventName, testDataLst[3].MarketName, testDataLst[3].SelectionName, testDataLst[3].Odds, false);

                //Verify Bet Slip
                string DoubleBetXpath = "//span[@class='b' and contains(text(), 'Double')]/following::span[contains(text(), '6')]";
                Assert.IsTrue(MyBrowser.IsElementPresent(DoubleBetXpath), "Double(6) betType is not present in the Betslip");
                string TrebleBetXpath = "//span[@class='b' and contains(text(), 'Treble')]/following::span[contains(text(), '4')]";
                Assert.IsTrue(MyBrowser.IsElementPresent(TrebleBetXpath), "Treble(6) betType is not present in the Betslip");
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "", "Accumulator (4)", 5);

                //Enter the stake               
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", accumulator4Stake, "Accumulator (4)", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, accumulator4Stake, "", "Accumulator (4)", "", "");

                //Place bet and validate the receipt
                accumulator4Sel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName + "|" + testDataLst[2].SelectionName + "|" + testDataLst[3].SelectionName;
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", accumulator4Sel, aryOdd, accumulator4Stake, "", false, "Accumulator (4)", 1);

                Console.WriteLine("Verification of Accumulator (4) Betplacement was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_Accumulator4Bet' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_Accumulator4Bet");
                Console.WriteLine("TestCase 'VerifyBetplacement_Accumulator4Bet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetplacement_YankeeBet()
        {
            string[] aryOdd = new string[4];
            string yankeeStake = "1.00";
            string yankeeSel;
            TestData[] testDataLst = new TestData[4];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(11, "FutureEvents");
            testDataLst[2] = new TestData(14, "FutureEvents");
            testDataLst[3] = new TestData(17, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            aryOdd[2] = testDataLst[2].Odds;
            aryOdd[3] = testDataLst[3].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_YankeeBet', Verify Yankee Bet Placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add 4 selections to betslip               
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[2].ClassName, testDataLst[2].TypeName, testDataLst[2].SubTypeName, testDataLst[2].EventName, testDataLst[2].MarketName, testDataLst[2].SelectionName, testDataLst[2].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[3].ClassName, testDataLst[3].TypeName, testDataLst[3].SubTypeName, testDataLst[3].EventName, testDataLst[3].MarketName, testDataLst[3].SelectionName, testDataLst[3].Odds, false);

                //Enter the stake
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "", "Yankee", 5);
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", yankeeStake, "Yankee", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, yankeeStake, "", "Yankee", "", "");

                //Place bet and validate the receipt
                yankeeSel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName + "|" + testDataLst[2].SelectionName + "|" + testDataLst[3].SelectionName;
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", yankeeSel, aryOdd, yankeeStake, "", false, "Yankee", 1);

                Console.WriteLine("Verification of Yankee Betplacement was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_YankeeBet' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_YankeeBet");
                Console.WriteLine("TestCase 'VerifyBetplacement_YankeeBet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetplacement_Lucky15Bet()
        {
            string[] aryOdd = new string[4];
            string lucky15Stake = "1.00";
            string lucky15Sel;
            TestData[] testDataLst = new TestData[4];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(11, "FutureEvents");
            testDataLst[2] = new TestData(14, "FutureEvents");
            testDataLst[3] = new TestData(17, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            aryOdd[1] = testDataLst[1].Odds;
            aryOdd[2] = testDataLst[2].Odds;
            aryOdd[3] = testDataLst[3].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_Lucky15Bet', Verify Lucky15 Bet Placement *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add 4 selections to betslip               
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[2].ClassName, testDataLst[2].TypeName, testDataLst[2].SubTypeName, testDataLst[2].EventName, testDataLst[2].MarketName, testDataLst[2].SelectionName, testDataLst[2].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[3].ClassName, testDataLst[3].TypeName, testDataLst[3].SubTypeName, testDataLst[3].EventName, testDataLst[3].MarketName, testDataLst[3].SelectionName, testDataLst[3].Odds, false);

                //Enter the stake
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "", "Lucky 15", 5);
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", lucky15Stake, "Lucky 15", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, lucky15Stake, "", "Lucky 15", "", "");

                //Place bet and validate the receipt
                lucky15Sel = testDataLst[0].SelectionName + "|" + testDataLst[1].SelectionName + "|" + testDataLst[2].SelectionName + "|" + testDataLst[3].SelectionName;
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", "", "", lucky15Sel, aryOdd, lucky15Stake, "", false, "Lucky 15", 1);

                Console.WriteLine("Verification of Lucky 15 Betplacement was successful");
                Console.WriteLine("TestCase 'VerifyBetplacement_Lucky15Bet' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_Lucky15Bet");
                Console.WriteLine("TestCase 'VerifyBetplacement_Lucky15Bet' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// To check the betslip when user adds multiple selections to it
        /// </summary>
        /// <TestCaseId>279,389</TestCaseId>
        /// <TestCasesCovered>2</TestCasesCovered>
        [Test]
        public void VerifyBetPlacement_SinglesAndMultipleBetTyes()
        {
            string[] aryOddDBL = new string[2];
            string[] aryOddSGL1 = new string[1];
            string[] aryOddSGL2 = new string[1];
            string doubleStake = "1.00";
            double prevTotalStake, prevTotalPR, expTotalStake;
            TestData[] testDataLst = new TestData[2];
            testDataLst[0] = new TestData(0, "FutureEvents");
            testDataLst[1] = new TestData(11, "FutureEvents");
            aryOddDBL[0] = testDataLst[0].Odds;
            aryOddDBL[1] = testDataLst[1].Odds;
            aryOddSGL1[0] = testDataLst[0].Odds;
            aryOddSGL2[0] = testDataLst[1].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetPlacement_SinglesAndMultipleBetTyes', Verify Bet Placement for Singles and Multiples bet types together) *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region VerifyBetDetails&placement
                //Add 2 Selction and verify betdetails
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);

                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, "", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "", "Double", 1);

                //Enter Stake for 1st selection and Verify Bet Details               
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOddSGL1, testDataLst[0].Stake, "", "Single", "", "");

                //Enter Stake for 2nd selection and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, testDataLst[1].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOddSGL2, testDataLst[1].Stake, "", "Single", prevTotalStake.ToString(), prevTotalPR.ToString());

                //Enter Stake for Double bet and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", doubleStake, "Double", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, "", "", "", aryOddDBL, doubleStake, "", "Double", prevTotalStake.ToString(), prevTotalPR.ToString());

                expTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                BTbetslipObj.BetPlacement(MyBrowser, expTotalStake.ToString(), 3, "Home", false);

                #endregion
                Console.WriteLine("TestCase 'VerifyBetPlacement_SinglesAndMultipleBetTyes' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetPlacement_SinglesAndMultipleBetTyes");
                Console.WriteLine("TestCase 'VerifyBetPlacement_SinglesAndMultipleBetTyes' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Potential returns displayed when all selections are each way
        /// </summary>
        /// <TestCaseId>424</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyBetPlacement_SinglesAndMultipleBetTyes_EW()
        {
            string[] aryOddSGL1 = new string[1];
            string[] aryOddSGL2 = new string[1];
            string[] aryOddDBL = new string[2];
            double prevTotalStake, prevTotalPR, expTotalStake;
            TestData[] testDataLst = new TestData[2];
            testDataLst[0] = new TestData(11, "FutureEvents");
            testDataLst[1] = new TestData(14, "FutureEvents");
            aryOddSGL1[0] = testDataLst[0].Odds;
            aryOddSGL2[0] = testDataLst[1].Odds;
            string doubleStake = ".50";
            aryOddDBL[0] = testDataLst[0].Odds;
            aryOddDBL[1] = testDataLst[1].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetPlacement_MultipleSelsEW', Verify Bet Placement on multiple selections EW enabled *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                Console.WriteLine("Bet placement for multiple selections(EW enabled) - NON Multiple Bet type");
                #region VerifyBetDetails&placement Only singles
                //Add 2 Selction and verify betdetails
                string EWterms1 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, true);
                string EWterms2 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, true);

                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, "", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Double", 1);

                //Enter Stake for 1st selection and Verify Bet Details               
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOddSGL1, testDataLst[0].Stake, EWterms1, "Single", "", "");

                //Enter Stake for 2nd selection and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, testDataLst[1].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOddSGL2, testDataLst[1].Stake, EWterms2, "Single", prevTotalStake.ToString(), prevTotalPR.ToString());

                expTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                BTbetslipObj.BetPlacement(MyBrowser, expTotalStake.ToString(), 2, "Home", false);
                #endregion

                Console.WriteLine("Bet placement for multiple selections(EW enabled) - Multiple Bet type");
                #region VerifyBetDetails&placement Only Multiples
                //Add 2 Selction and verify betdetails
                EWterms1 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, true);
                EWterms2 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, true);

                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, "", "Single", 1);
                BTbetslipObj.VerifyBetSlip(MyBrowser, "", "", "", "", "Yes", "Double", 1);

                //Enter Stake for 1st selection and Verify Bet Details               
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOddSGL1, testDataLst[0].Stake, EWterms1, "Single", "", "");

                //Enter Stake for 2nd selection and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, testDataLst[1].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOddSGL2, testDataLst[1].Stake, EWterms2, "Single", prevTotalStake.ToString(), prevTotalPR.ToString());

                //Enter details for Double Bettype
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, "", "", "", "", doubleStake, "Double", true);
                string EW = EWterms1 + "|" + EWterms2;
                BTbetslipObj.VerifyBetDetails(MyBrowser, "", "", "", aryOddDBL, doubleStake, EW, "Double", prevTotalStake.ToString(), prevTotalPR.ToString());

                expTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                BTbetslipObj.BetPlacement(MyBrowser, expTotalStake.ToString(), 3, "Home", false);
                #endregion

                Console.WriteLine("TestCase 'VerifyBetPlacement_SinglesAndMultipleBetTyes_EW' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetPlacement_SinglesAndMultipleBetTyes_EW");
                Console.WriteLine("TestCase 'VerifyBetPlacement_SinglesAndMultipleBetTyes_EW' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCaseId></TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetPlacement_PlaceOnlyMarket()
        {
            try
            {
                Console.WriteLine("***** Executing Test Case --- 'VerifyBetPlacement_PlaceOnlyMarket', Validate bet placement from Place Only markets *****");
                string[] aryOdd = new string[1];
                TestData[] testDataLst = new TestData[1];
                testDataLst[0] = new TestData(3, "UpcomingEvents");
                aryOdd[0] = testDataLst[0].Odds;

                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Get the Event Start Time
                string evntNameByTime = testDataLst[0].DateTime;
                string[] evntNameByTimeArr = evntNameByTime.Split(' ');
                evntNameByTime = evntNameByTimeArr[1] + evntNameByTimeArr[2];
                evntNameByTime = Convert.ToDateTime(evntNameByTime).ToString("HH:mm");

                //Add Selction and verify betdetails
                BTbetslipObj.AddAndVerifySelectionInBetslip_HR(MyBrowser, "Horse Racing", "Tomorrow", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, evntNameByTime, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false, false);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                //Enter Stake and Verify Bet Details               
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, testDataLst[0].TypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);
                Console.WriteLine("TestCase 'VerifyBetPlacement_PlaceOnlyMarket' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetPlacement_PlaceOnlyMarket");
                Console.WriteLine("TestCase 'VerifyBetPlacement_PlaceOnlyMarket' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCaseId></TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetPlacement_RaceWinnerMarket_EWnonEW()
        {
            string[] aryOdd = new string[1];
            string EWTerms;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(0, "UpcomingEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetPlacement_RaceWinnerMarket_EWnonEW', Validate bet placement for Race Winner markets(EW and nonEW) *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Get the Event Start Time
                string evntNameByTime = testDataLst[0].DateTime;
                string[] evntNameByTimeArr = evntNameByTime.Split(' ');
                evntNameByTime = evntNameByTimeArr[1] + evntNameByTimeArr[2];
                evntNameByTime = Convert.ToDateTime(evntNameByTime).ToString("HH:mm");

                Console.WriteLine("Bet placement for Race Winner market");
                #region Verift Betplacement
                //Add Selction and verify betdetails
                BTbetslipObj.AddAndVerifySelectionInBetslip_HR(MyBrowser, "Horse Racing", "Today", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, evntNameByTime, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false, false);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                //Enter Stake and Verify Bet Details               
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, testDataLst[0].TypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);
                #endregion

                BTcommonObj.NavigateToHomePage(MyBrowser, "Side Menu");
                Console.WriteLine("Bet placement for Race Winner market");
                #region Verift Betplacement - EW
                //Add Selction and verify betdetails
                EWTerms = BTbetslipObj.AddAndVerifySelectionInBetslip_HR(MyBrowser, "Horse Racing", "Today", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, evntNameByTime, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, EWTerms, "Single", 1);

                //Enter Stake and Verify Bet Details               
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, EWTerms, "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, testDataLst[0].TypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, EWTerms, false, "Single", 1);
                #endregion
                Console.WriteLine("TestCase 'VerifyBetPlacement_RaceWinnerMarket_EWnonEW' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetPlacement_RaceWinnerMarket_EWnonEW");
                Console.WriteLine("TestCase 'VerifyBetPlacement_RaceWinnerMarket_EWnonEW' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCaseId></TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetplacement_HandicapMarket()
        {
            string[] aryOdd = new string[1];
            string handicapSel, handiCapVal, handiCapSelInEvDtlsPg, neg, pos;
            double negVal, posVal;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(6, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_HandicapMarket', Verify Bet Placement(Handicap Market) *****");
            try
            {
                //Excel reader cannot read the '+' symbol, hence preceding it to handoicap val
                handiCapVal = "1.0";     // testDataLst[0].HandicapValue;
                if (!testDataLst[0].HandicapValue.Contains('-')) { handiCapVal = "+" + handiCapVal; }
                negVal = (Convert.ToDouble(handiCapVal) - .25); if (!(Math.Sign(negVal) == -1)) { neg = "+" + negVal.ToString(); } else { neg = negVal.ToString(); }
                posVal = (Convert.ToDouble(handiCapVal) + .25); if (!(Math.Sign(posVal) == -1)) { pos = "+" + posVal.ToString(); } else { pos = posVal.ToString(); }
                handiCapVal = neg + "," + pos;

                //concatinating with HandiCap Val to selection(betslip)
                handicapSel = testDataLst[0].SelectionName + "  (" + handiCapVal + ")";
                //Selection appears differently in event details page and betslip               
                handiCapSelInEvDtlsPg = testDataLst[0].SelectionName + " " + handiCapVal;

                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add and Verify selection in betslip
                BTbetslipObj.NavigateToEventDetailsPage(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName);
                BTbetslipObj.AddSelectionToBetslip(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, handiCapSelInEvDtlsPg, testDataLst[0].Odds, false);
                BTbetslipObj.VerifySelectionInBetslip(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, handicapSel, testDataLst[0].Odds);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, handicapSel, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, handicapSel, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, handicapSel, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName + " (" + handiCapVal + ")", testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);
                Console.WriteLine("TestCase 'VerifyBetplacement_HandicapMarket' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_HandicapMarket");
                Console.WriteLine("TestCase 'VerifyBetplacement_HandicapMarket' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCaseId></TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetplacement_WDWMarket()
        {
            string[] aryOdd = new string[1];
            string winSelection;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(3, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            winSelection = testDataLst[0].SelectionName + " Win";
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_WDWMarket', Verify Bet Placement(Win-Draw-Win Market) *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add and Verify selection in betslip
                BTbetslipObj.NavigateToEventDetailsPage(MyBrowser, "Football", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName);
                BTbetslipObj.AddSelectionToBetslip(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, "H", testDataLst[0].Odds, false);
                BTbetslipObj.VerifySelectionInBetslip(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, winSelection, testDataLst[0].Odds);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, winSelection, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, winSelection, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, winSelection, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, "", testDataLst[0].SelectionName + " - Win", aryOdd, testDataLst[0].Stake, "", false, "Single", 1);
                Console.WriteLine("TestCase 'VerifyBetplacement_WDWMarket' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_WDWMarket");
                Console.WriteLine("TestCase 'VerifyBetplacement_WDWMarket' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCaseId></TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void VerifyBetplacement_FirstGoalScorer()
        {
            string[] aryOdd = new string[1];
            string xPath;
            double firstOdd, secondOdd;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(8, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetplacement_FirstGoalScorer', Verify Bet Placement(First Goal Scorer Market) *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "Football", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);

                #region Validate odds display order
                //Move back to event details page and verify the display order of odds
                // Back arrow does not function at times
                //BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipBackArrow);         
                BTcommonObj.NavigateToHomePage(MyBrowser, "Side Menu");
                BTbetslipObj.NavigateToEventDetailsPage(MyBrowser, "Football", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName);

                xPath = "//div[@class='bxcl bxf ml5' and contains(text(), '" + testDataLst[0].MarketName + "')]/following::div[@class='expandable' and @expanded = 'true']//span[@class='odds-convert']";
                IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
                ReadOnlyCollection<IWebElement> lastGoalOddsCol = driver.FindElements(By.XPath(xPath));
                for (int i = 0; i < (lastGoalOddsCol.Count - 1); i++)
                {
                    if (!((i + 1) == (lastGoalOddsCol.Count)))
                    {
                        firstOdd = Convert.ToDouble(lastGoalOddsCol[i].Text);
                        secondOdd = Convert.ToDouble(lastGoalOddsCol[i + 1].Text);
                        Assert.IsTrue(!(firstOdd <= secondOdd), i + " & " + (i + 1) + " odds were not displayed in ascending order");
                    }
                }
                Console.WriteLine("Display order of Odds for " + testDataLst[0].MarketName + " was verified successfully");
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipButton);
                #endregion

                //Verify bet details and place bet               
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);
                Console.WriteLine("TestCase 'VerifyBetplacement_FirstGoalScorer' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetplacement_FirstGoalScorer");
                Console.WriteLine("TestCase 'VerifyBetplacement_FirstGoalScorer' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <TestCaseId></TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        //[Test]
        public void VerifyBetPlacement_WOMarket_EWnonEW()
        {
            string[] aryOdd = new string[1];
            string EWTerms, xPath, typeName;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(19, "UpcomingEvents");
            aryOdd[0] = testDataLst[0].Odds;
            Console.WriteLine("***** Executing Test Case --- 'VerifyBetPlacement_WOMarket_EWnonEW', Validate bet placement for WO markets(EW and NonEW) *****");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                Console.WriteLine("Bet placement for WO market");
                #region VerifyBetDetails&placement
                //Add Selction and verify betdetails
                BTbetslipObj.AddAndVerifySelectionInBetslip_HR(MyBrowser, "Horse Racing", "Today", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false, false);

                // Click on the back button  and verify the market banner
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipBackArrow);
                MyBrowser.Refresh();
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                xPath = "//a[starts-with(@class, 'bxc bxf tab') and contains(@href, 'WITHOUT')]";
                //select the tab
                BTcommonObj.clickObject(MyBrowser, xPath);
                xPath = "//span[@class='rel t2' and text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + testDataLst[0].MarketName.ToLower() + "')]]";
                Assert.IsTrue(MyBrowser.IsElementPresent(xPath), testDataLst[0].MarketName + " banner was not displayed in the Rcae card for W/O market");

                BTcommonObj.NavigateToHomePage(MyBrowser, "Side Menu");
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipButton);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                //Enter Stake and Verify Bet Details               
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");

                //Type name is appended by narket and WO in bet receipt                   
                //check if the type name is displayed in betslip
                typeName = testDataLst[0].TypeName + " - " + testDataLst[0].MarketName + " match betting and w/o";
                xPath = "//span[@class='bxcl fs15px' and text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + testDataLst[0].EventName + " " + typeName.ToLower() + "')]]";
                Assert.IsTrue(MyBrowser.IsElementPresent(xPath), "'" + testDataLst[0].EventName + " " + typeName + "' was not dsiplayed in betslip");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, typeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);
                #endregion

                BTcommonObj.NavigateToHomePage(MyBrowser, "Side Menu");

                Console.WriteLine("Bet placement for WO market -EW");
                #region VerifyBetDetails&placement
                //Add Selction and verify betdetails
                EWTerms = BTbetslipObj.AddAndVerifySelectionInBetslip_HR(MyBrowser, "Horse Racing", "Today", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false, true);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, EWTerms, "Single", 1);

                //Enter Stake and Verify Bet Details               
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, EWTerms, "Single", "", "");

                //Type name is appended by narket and WO in bet receipt                   
                //check if the type name is displayed in betslip
                typeName = testDataLst[0].TypeName + " - " + testDataLst[0].MarketName + " match betting and w/o";
                xPath = "//span[@class='bxcl fs15px' and text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + testDataLst[0].EventName + " " + typeName.ToLower() + "')]]";
                Assert.IsTrue(MyBrowser.IsElementPresent(xPath), "'" + testDataLst[0].EventName + " " + typeName + "' was not dsiplayed in betslip");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, typeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, EWTerms, false, "Single", 1);
                #endregion

                Console.WriteLine("TestCase 'VerifyBetPlacement_WOMarket_EW' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyBetPlacement_WOMarket_EW");
                Console.WriteLine("TestCase 'VerifyBetPlacement_WOMarket_EW' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Validate pre-play bet placement
        /// </summary>
        /// <TestCaseId>394</TestCaseId>
        /// <TestCasesCovered></TestCasesCovered>
        [Test]
        public void ValidatePreplayBetPlacementPrePlayAndLiveEvent()
        {
            IWebDriver driver = ((WebDriverBackedSelenium)MyBrowser).UnderlyingWebDriver;
            TestData[] testData = new TestData[2];
            string[] odds = new string[2];
            double totalStake, totalPR;
            int betNos = 1;
            Console.WriteLine("***** Executing Test Case 394 ***** 'ValidatePreplayBetPlacementPrePlayAndLiveEvent', Validating pre-play bet placement for preplay event");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                for (int i = 0; i <= 1; i++)
                {
                    testData[0] = new TestData(20, "BetSlipTestData");
                    testData[1] = new TestData(21, "BetSlipTestData");

                    if (i == 1)
                    {
                        testData[0] = new TestData(22, "BetSlipTestData");
                        testData[1] = new TestData(23, "BetSlipTestData");
                        BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "In-Play", "", testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, testData[0].SelectionName, testData[0].Odds, false);
                        BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "In-Play", "", testData[1].ClassName, testData[1].TypeName, testData[1].SubTypeName, testData[1].EventName, testData[1].MarketName, testData[1].SelectionName, testData[1].Odds, false);
                    }
                    else
                    {
                        BTbetslipObj.AddAndVerifySelectionInBetslip_HR(MyBrowser, testData[0].ClassName, "Next Races", testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, testData[0].SelectionName, testData[0].Odds, false, false);
                        BTbetslipObj.AddAndVerifySelectionInBetslip_HR(MyBrowser, testData[1].ClassName, "Next Races", testData[1].ClassName, testData[1].TypeName, testData[1].SubTypeName, testData[1].EventName, testData[1].MarketName, testData[1].SelectionName, testData[1].Odds, false, false);
                    }
                    odds[0] = testData[0].Odds;
                    odds[1] = testData[1].Odds;
                    BTbetslipObj.EnterStake(MyBrowser, testData[0].EventName, testData[0].SelectionName, testData[0].MarketName, testData[0].Odds, testData[0].Stake, "Double", false);
                    totalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                    totalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                    BTbetslipObj.VerifyBetDetails(MyBrowser, testData[0].EventName, testData[0].SelectionName, testData[0].MarketName, odds, testData[0].Stake, "", "Double", "", "");

                    if (i == 1)
                    {

                        BTbetslipObj.BetPlacement(MyBrowser, totalStake.ToString(), betNos, "Home", true);
                        //while (MyBrowser.IsVisible(BetslipControls.placeBet))
                        //    driver.FindElement(By.XPath(BetslipControls.placeBet)).Click();
                        Assert.IsTrue(MyBrowser.IsElementPresent("//div[@id='spinner-wrapper']/p[contains(string(),'Please wait while we place your In-Play bet')]"), "Bet Inplay Placement Timer is not shown to the customer");
                        Console.WriteLine("Bet Inplay Placement Timer is shown to the customer");
                    }
                    else
                    {
                        //BTbetslipObj.BetPlacement(MyBrowser, totalStake.ToString(), betNos, "Home", false);
                        driver.FindElement(By.XPath(BetslipControls.placeBet)).Click();
                        Assert.IsTrue(MyBrowser.IsElementPresent("//div[@class='bxcl bg25 h35 ffdc fs15px c1 ttu lh39 pl10' and contains(text(), 'Your bets (" + betNos + ")')]"), "Delay is displayed to the customer");
                        Console.WriteLine("No delay is displayed to the customer");
                    }
                }
                Console.WriteLine("TestCase 'ValidatePreplayBetPlacementPrePlayAndLiveEvent' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidatePreplayBetPlacement_NotLiveEvent");
                Console.WriteLine("TestCase : 394 'ValidatePreplayBetPlacement_NotLiveEvent' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Verify Potential returns displayed when price is changed from SP to fixed price
        /// </summary>
        /// <TestCaseId>425</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyPRDisplayedWhenChangedToFixedPrice()
        {
            ISelenium adminBrowser = null;
            TestData[] testData = new TestData[1];
            testData[0] = new TestData(128, "BetSlipTestData");
            double totalStake, newPR;
            string totalPR;
            string SelectionID;
            string[] odds = new string[1];
            odds[0] = "1.55";
            Console.WriteLine("***** Executing Test Case 425 ***** 'VerifyPRDisplayedWhenChangedToFixedPrice',Potential returns displayed when price is changed from SP to fixed price");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");
                BTbetslipObj.AddSPSelectionToBetslip(MyBrowser, testData[0].ClassName, "Today", testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, testData[0].SelectionName, testData[0].Odds, false, false);
                BTbetslipObj.EnterStake(MyBrowser, testData[0].EventName, testData[0].SelectionName, testData[0].MarketName, testData[0].Odds, testData[0].Stake, "Single", false);
                totalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                totalPR = BTbetslipObj.GetReturnsFromBetslipForSP(MyBrowser, "Returns");
                Assert.Equals(totalPR, "N/A");
                SelectionID = BTcommonObj.GetSelectionIDFromBetslip(MyBrowser, testData[0].SelectionName, testData[0].EventName);

                BTbetslipObj.RemoveSelFromBetslip(MyBrowser, testData[0].EventName, testData[0].SelectionName, true);
                adminBrowser = AdminCommonObj.LogOnToAdmin();
                AdminCommonObj.UpdateSelection(adminBrowser, SelectionID, odds[0], "Active", string.Empty);
                Thread.Sleep(10000);

                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, testData[0].ClassName, "Today", testData[0].ClassName, testData[0].TypeName, testData[0].SubTypeName, testData[0].EventName, testData[0].MarketName, testData[0].SelectionName, odds.ToString(), false);
                BTbetslipObj.EnterStake(MyBrowser, testData[0].EventName, testData[0].SelectionName, testData[0].MarketName, odds.ToString(), testData[0].Stake, "Single", false);
                totalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                newPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");

                BTbetslipObj.VerifyBetDetails(MyBrowser, testData[0].EventName, testData[0].SelectionName, testData[0].MarketName, odds, testData[0].Stake, "", "Single", totalStake.ToString(), newPR.ToString());
                Console.WriteLine("TestCase 'VerifyPRDisplayedWhenChangedToFixedPrice' - PASS");

            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyPRDisplayedWhenChangedToFixedPrice");
                Console.WriteLine("TestCase : 425 'VerifyPRDisplayedWhenChangedToFixedPrice' - FAIL");
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Verify Potential returns is recalculated on price change
        /// </summary>
        /// <TestCaseId>433</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyPRrecalculatedOnPriceChange()
        {
            ISelenium adminBrowser = null;
            string[] aryOdd1 = new string[1];
            string[] aryOdd2 = new string[1];
            string[] aryNewPrice = new string[1];
            double prevTotalStake, prevTotalPR;
            string dispMsg, selID = null, newPrice = "2.55";
            TestData[] testDataLst = new TestData[2];
            testDataLst[0] = new TestData(20, "FutureEvents");
            testDataLst[1] = new TestData(21, "FutureEvents");
            aryOdd1[0] = testDataLst[0].Odds;
            aryOdd2[0] = testDataLst[1].Odds;
            aryNewPrice[0] = newPrice;
            string[] arrNewPrice = new string[] { newPrice };
            Console.WriteLine("***** Executing Test Case 433 ***** 'VerifyPRrecalculatedOnPriceChange',Verify the Potential returns re calculated if price-odds change");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region Verify Price Change Positive
                //Add Selctions and verify betdetails
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);

                //Enter Stake and Verify Bet Details
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd1, testDataLst[0].Stake, "", "Single", "", "");

                //Enter Stake for 2nd selection and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, testDataLst[1].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOdd2, testDataLst[1].Stake, "", "Single", prevTotalStake.ToString(), prevTotalPR.ToString());

                //get the sel ID for the selection
                selID = BTbetslipObj.GetSelectionId(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, "Single");
                //Change price in OB
                adminBrowser = AdminCommonObj.LogOnToAdmin();
                AdminCommonObj.UpdateSelection(adminBrowser, selID, newPrice, string.Empty, string.Empty, string.Empty);
                Console.WriteLine("Price changed in OB");
                Thread.Sleep(300000);

                MyBrowser.Focus(BetslipControls.betslipBanner);
                 // check if the price has chanegd and potential retirn has been calcualetd accordingly       
                // Change the price to 0 for 1st selection
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "0", "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOdd2, testDataLst[1].Stake, "", "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, aryNewPrice, testDataLst[1].Stake, "", false, "Single", 1);                
                #endregion
                

                #region Verify Price Change Negative
                //Add Selctions and verify betdetails                
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, newPrice, false);
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, newPrice, testDataLst[1].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, arrNewPrice , testDataLst[1].Stake, "", "Single", "", "");

                //get the sel ID for the selection
                selID = BTbetslipObj.GetSelectionId(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, newPrice, "Single");
                //Change price in OB
                adminBrowser = AdminCommonObj.LogOnToAdmin();
                AdminCommonObj.UpdateSelection(adminBrowser, selID, testDataLst[1].Odds, string.Empty, string.Empty, string.Empty);
                Console.WriteLine("Price changed in OB");
                Thread.Sleep(60000);

                MyBrowser.Focus(BetslipControls.betslipBanner);
                BTcommonObj.clickObject(MyBrowser, BetslipControls.placeBet);
                dispMsg = "Price has changed from " + newPrice + " to " + testDataLst[1].Odds;
                Assert.IsTrue(MyBrowser.IsTextPresent(dispMsg), dispMsg + "' was not diplayed in the betslip on change of price value in OB");
                //Verify Betdetails on price change and place bet
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOdd2, testDataLst[1].Stake, "", "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, aryNewPrice, testDataLst[1].Stake, "", false, "Single", 1);
                #endregion

                Console.WriteLine("TestCase 'VerifyPRrecalculatedOnPriceChange' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyPRrecalculatedOnPriceChange");
                Console.WriteLine("TestCase : 433 'VerifyPRrecalculatedOnPriceChange' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                //Reset to old price
                AdminCommonObj.UpdateSelection(adminBrowser, selID, testDataLst[1].Odds, string.Empty, string.Empty, string.Empty);
                Thread.Sleep(60000);
                BTcommonObj.KillAdminObject();
            }
        }



        /// <summary>
        /// Verify Potential returns re calculated if selection is suspended
        /// </summary>
        /// <TestCaseId>434</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyPRrecalculatedOnSelectionSuspension()
        {
            ISelenium adminBrowser = null;
            string[] aryOdd1 = new string[1];
            string[] aryOdd2 = new string[1];
            double prevTotalStake, prevTotalPR;
            string dispMsg, selID = null;
            TestData[] testDataLst = new TestData[2];
            testDataLst[0] = new TestData(20, "FutureEvents");
            testDataLst[1] = new TestData(22, "FutureEvents");
            aryOdd1[0] = testDataLst[0].Odds;
            aryOdd2[0] = testDataLst[1].Odds;
            Console.WriteLine("***** Executing Test Case 434 ***** 'VerifyPRrecalculatedOnSelectionSuspension',Verify the Potential returns re calculated if selection is suspended");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region Verify selection Suspension
                //Add Selctions and verify betdetails
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false);
                BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, false);

                //Enter Stake and Verify Bet Details
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd1, testDataLst[0].Stake, "", "Single", "", "");

                //Enter Stake for 2nd selection and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, testDataLst[1].Stake, "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOdd2, testDataLst[1].Stake, "", "Single", prevTotalStake.ToString(), prevTotalPR.ToString());

                //get the sel ID for the selection
                selID = BTbetslipObj.GetSelectionId(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, "Single");
                //suspend the selection in OB
                adminBrowser = AdminCommonObj.LogOnToAdmin();
                AdminCommonObj.UpdateSelection(adminBrowser, selID, "", "Suspended", "", "");
                Console.WriteLine("Selection suspended in OB");
                Thread.Sleep(60000);

                BTcommonObj.clickObject(MyBrowser, BetslipControls.placeBet);
                dispMsg = "This selection is suspended.";
                Assert.IsTrue(MyBrowser.IsTextPresent(dispMsg), dispMsg + "' was not diplayed in the betslip on change on suspending a selection in OB");

                //Verify Betdetails on selection suspension
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOdd2, testDataLst[1].Stake, "", "Single", prevTotalStake.ToString(), prevTotalPR.ToString());
                // check the selection status in the Event details page
                string xPath = "//span[@class='t7 page-title' and contains(text(), '" + testDataLst[1].EventName + "')]//following::div[contains(@class, 'odds-text') and contains(text(), '" + testDataLst[1].SelectionName + "')]//following::div[@class='bxc bxf rel t2 ln5 price bxcr mr10' and contains(text(),'SUSP')]";
                BTcommonObj.clickObject(MyBrowser, BetslipControls.betslipBackArrow);
                MyBrowser.Refresh();
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                Assert.IsTrue(MyBrowser.IsElementPresent(xPath), "Selection is not suspended in Event details page");
                Console.WriteLine("Selection ststus was changed to suspended in Event details page");
                #endregion

                Console.WriteLine("TestCase 'VerifyPRrecalculatedOnSelectionSuspension' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyPRrecalculatedOnSelectionSuspension");
                Console.WriteLine("TestCase : 434 'VerifyPRrecalculatedOnSelectionSuspension' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                //reactiovate the selection
                AdminCommonObj.UpdateSelection(adminBrowser, selID, "", "Active", "", "");
                Thread.Sleep(60000); 
                BTcommonObj.KillAdminObject();
            }
        }



        /// <summary>
        /// Verify potential returns is recalculated if EW changes.
        /// </summary>
        /// <TestCaseId>435</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyPRrecalculatedOnEWchange()
        {
            ISelenium adminBrowser = null;
            string[] aryOdd1 = new string[1];
            string[] aryOdd2 = new string[1];
            double prevTotalStake, prevTotalPR;
            string EWterm1, EWterm2, dispMsg, selID = null;
            TestData[] testDataLst = new TestData[2];
            testDataLst[0] = new TestData(14, "FutureEvents");
            testDataLst[1] = new TestData(21, "FutureEvents");
            aryOdd1[0] = testDataLst[0].Odds;
            aryOdd2[0] = testDataLst[1].Odds;
            string newEWterms = "Each Way 1/2 1-2";
            Console.WriteLine("***** Executing Test Case 435 ***** 'VerifyPRrecalculatedOnEWchange',Verify the Potential returns is recalculated if EW changes");
            try
            {
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                #region Verify EW change
                //Add Selctions and verify betdetails
                EWterm1 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, true);
                EWterm2 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, true);

                //Enter Stake and Verify Bet Details
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, aryOdd1, testDataLst[0].Stake, EWterm1, "Single", "", "");

                //Enter Stake for 2nd selection and Verify Bet Details               
                prevTotalStake = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Stake");
                prevTotalPR = BTbetslipObj.GetTotalsFromBetslip(MyBrowser, "Returns");
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, testDataLst[1].Stake, "Single", true);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOdd2, testDataLst[1].Stake, EWterm2, "Single", prevTotalStake.ToString(), prevTotalPR.ToString());

                //get the sel ID for the selection
                selID = BTbetslipObj.GetSelectionId(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, testDataLst[1].Odds, "Single");
                //suspend the selection in OB
                adminBrowser = AdminCommonObj.LogOnToAdmin();
                AdminCommonObj.UpdatePayoutFraction(adminBrowser, selID, "1", "2");
                Console.WriteLine("EW changed in OB");
                Thread.Sleep(300000);

                MyBrowser.Focus(BetslipControls.betslipBanner);
                // check if the price has changed and potential retirn has been calcualetd accordingly       
                // Change the price to 0 for 1st selection
                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "0", "Single", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[1].EventName, testDataLst[1].SelectionName, testDataLst[1].MarketName, aryOdd2, testDataLst[1].Stake, EWterm2, "Single", "", "");
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, aryOdd2, testDataLst[1].Stake, newEWterms, false, "Single", 1);                
        
                //Verify EW change in Event details page
                EWterm2 = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "Competition", testDataLst[1].ClassName, testDataLst[1].TypeName, testDataLst[1].SubTypeName, testDataLst[1].EventName, testDataLst[1].MarketName, testDataLst[1].SelectionName, testDataLst[1].Odds, true);
                Assert.IsTrue(EWterm2.ToLower().Trim().Equals(newEWterms.ToLower().Trim()), "EW value was not changed in Event details page. Expected '" + newEWterms + "', Actual '" + EWterm2 + "'.");
                Console.WriteLine("EW value was changed in Event details page");
                #endregion

                Console.WriteLine("TestCase 'VerifyPRrecalculatedOnEWchange' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyPRrecalculatedOnEWchange");
                Console.WriteLine("TestCase : 435 'VerifyPRrecalculatedOnEWchange' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                //Reset to old price
                AdminCommonObj.UpdatePayoutFraction(adminBrowser, selID, "1", "4");
                Thread.Sleep(60000); 
                BTcommonObj.KillAdminObject();
            }
        }



        /// <summary>
        /// Verify Message displayed on handicap changes.
        /// </summary>
        /// <TestCaseId>435</TestCaseId>
        /// <TestCasesCovered>1</TestCasesCovered>
        [Test]
        public void VerifyMessageOnHandicapChange()
        {
            ISelenium adminBrowser = null;
            string[] aryOdd = new string[1];
            string selID = null, newHandicapVal = null, handicapSel, handiCapVal, handiCapSelInEvDtlsPg, neg, pos;
            double negVal, posVal;
            TestData[] testDataLst = new TestData[1];
            testDataLst[0] = new TestData(6, "FutureEvents");
            aryOdd[0] = testDataLst[0].Odds; 
            Console.WriteLine("***** Executing Test Case 436 ***** 'VerifyMessageOnHandicapChange',Verify Message displayed on handicap changes");
            try
            {
                //Excel reader cannot read the '+' symbol, hence preceding it to handoicap val
                handiCapVal = "1.0";    // testDataLst[0].HandicapValue;
                if (!testDataLst[0].HandicapValue.Contains('-')) { handiCapVal = "+" + handiCapVal; }
                negVal = (Convert.ToDouble(handiCapVal) - .25); if (!(Math.Sign(negVal) == -1)) { neg = "+" + negVal.ToString(); } else { neg = negVal.ToString(); }
                posVal = (Convert.ToDouble(handiCapVal) + .25); if (!(Math.Sign(posVal) == -1)) { pos = "+" + posVal.ToString(); } else { pos = posVal.ToString(); }
                handiCapVal = neg + "," + pos;

                //concatinating with HandiCap Val to selection(betslip)
                handicapSel = testDataLst[0].SelectionName + "  (" + handiCapVal + ")";
                //Selection appears differently in event details page and betslip               
                handiCapSelInEvDtlsPg = testDataLst[0].SelectionName + " " + handiCapVal;

                #region Verify handicap value change
                BTcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                BTloginLogoutObj.Login(MyBrowser, FrameGlobals.UserName, FrameGlobals.PassWord);
                BTbetslipObj.OddTypeSwitch(MyBrowser, "decimal");

                //Add and Verify selection in betslip
                BTbetslipObj.NavigateToEventDetailsPage(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName);
                BTbetslipObj.AddSelectionToBetslip(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, handiCapSelInEvDtlsPg, testDataLst[0].Odds, false);
                BTbetslipObj.VerifySelectionInBetslip(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, handicapSel, testDataLst[0].Odds);
                BTbetslipObj.VerifyBetSlip(MyBrowser, testDataLst[0].EventName, handicapSel, testDataLst[0].MarketName, testDataLst[0].Odds, "", "Single", 1);

                BTbetslipObj.EnterStake(MyBrowser, testDataLst[0].EventName, handicapSel, testDataLst[0].MarketName, testDataLst[0].Odds, testDataLst[0].Stake, "", false);
                BTbetslipObj.VerifyBetDetails(MyBrowser, testDataLst[0].EventName, handicapSel, testDataLst[0].MarketName, aryOdd, testDataLst[0].Stake, "", "Single", "", "");
                
                //get the sel ID for the selection
                selID = BTbetslipObj.GetSelectionId(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].MarketName, testDataLst[0].Odds, "Single");
                //change the handi cap value in OB
                adminBrowser = AdminCommonObj.LogOnToAdmin();
                newHandicapVal = AdminCommonObj.UpdateHandicapValue(adminBrowser, selID, 21);
                Console.WriteLine("Handicap value changed in OB");
                Thread.Sleep(300000);

                MyBrowser.Focus(BetslipControls.betslipBanner);
                if (!newHandicapVal.Contains('-')) { newHandicapVal = "+" + newHandicapVal; }
                negVal = (Convert.ToDouble(newHandicapVal) - .25); if (!(Math.Sign(negVal) == -1)) { neg = "+" + negVal.ToString(); } else { neg = negVal.ToString(); }
                posVal = (Convert.ToDouble(newHandicapVal) + .25); if (!(Math.Sign(posVal) == -1)) { pos = "+" + posVal.ToString(); } else { pos = posVal.ToString(); }
                newHandicapVal = pos + "," + neg;
                //concatinating with HandiCap Val to selection(betslip)
                string newHandicapSel = testDataLst[0].SelectionName + "  (" + newHandicapVal + ")";
                //Selection appears differently in event details page and betslip               
                string newHandiCapSelInEvDtlsPg = testDataLst[0].SelectionName + " " + newHandicapVal;

                //place bet and validate bet receipt
                BTbetslipObj.ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName + " (" + newHandicapVal + ")", testDataLst[0].SelectionName, aryOdd, testDataLst[0].Stake, "", false, "Single", 1);                
                
                //verify the changed Handicap values in EVent details page
                BTbetslipObj.NavigateToEventDetailsPage(MyBrowser, "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName);
                BTbetslipObj.AddSelectionToBetslip(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, newHandiCapSelInEvDtlsPg, testDataLst[0].Odds, false);
                BTbetslipObj.VerifySelectionInBetslip(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, newHandicapSel, testDataLst[0].Odds);
                
                #endregion
                Console.WriteLine("TestCase 'VerifyMessageOnHandicapChange' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "VerifyMessageOnHandicapChange");
                Console.WriteLine("TestCase : 436 'VerifyMessageOnHandicapChange' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                //Reset the handicap value
                AdminCommonObj.UpdateHandicapValue(adminBrowser, selID, 45);
                Thread.Sleep(60000); 
                BTcommonObj.KillAdminObject();
            }
        }




    }//end class
}//end namespace




