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


namespace TestRepository.Betslip
{
    public class BetslipFunctions : BaseTest
    {

        TestRepository.Common BFcommonObj = new TestRepository.Common();
        Framework.Common.Common BFframeworkCommonObj = new Framework.Common.Common();


        // <summary>
        ///This method Returns No of Selections present in betslip.
        /// <param name="browser">Selenium Browser</param>
        /// <returns>Selection count in Betslip if, on Exception returns -1 /// </returns>
        /// <example>GetBetslipCount(browser) </example>
        public int GetBetslipCount(ISelenium browser)
        {
            string betSlipCount = null;
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                BFcommonObj.WaitForLoadingIcon(browser, FrameGlobals.IconLoadTimeout);
                betSlipCount = driver.FindElement(By.Id("betslip-value")).Text;
                if (betSlipCount == null)
                {
                    Fail("failed to fetch the Betslip count");
                }
                return Convert.ToInt32(betSlipCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'GetBetslipCount' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return -1;
            }
        }


        //<summary>
        //This method Returns the account balance.
        //<example>GetBalance(browser) </example>
        public double GetBalance(ISelenium browser)
        {
            string balance;
            string[] itemArray = null;
            char[] stakeSymbols = new char[3];

            try
            {
                Assert.IsTrue(browser.IsVisible(LoginLogoutControls.balance), "balance element is not displayed");
                balance = browser.GetText(LoginLogoutControls.balance);
                stakeSymbols[0] = '£';
                stakeSymbols[1] = '*';
                stakeSymbols[2] = '$';

                itemArray = balance.Split(stakeSymbols);
                return Convert.ToDouble(itemArray[1], CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'GetBalance' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return 0.0;
            }
        }


        //<summary>
        //This method removes the desireds selection from betslip
        //<example>RemoveSelFromBetslip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, false) </expample>
        //<example>RemoveSelFromBetslip(MyBrowser, "", "", true) </expample>
        public void RemoveSelFromBetslip(ISelenium browser, string eventName, string selection, bool removeAll)
        {
            try
            {
                int betSlpCnt, betSlpCntafterRemoval;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                Assert.IsTrue(browser.IsElementPresent(BetslipControls.betslipBanner), "Betslip Page is not displayed.");
                betSlpCnt = GetBetslipCount(browser);
                if (betSlpCnt > 0)
                {
                    if (removeAll == true)
                    {
                        BFcommonObj.clickObject(browser, BetslipControls.removeAllSels);
                        betSlpCntafterRemoval = GetBetslipCount(browser);
                        if (betSlpCntafterRemoval == 0 && browser.IsTextPresent("Your betslip is empty"))
                        {
                            Console.WriteLine("All selections in Betslip were removed successfully");
                        }
                        else
                        {
                            Fail("Betslip count is not 0, 'Your betslip is empty' message is not present, Failed to remove all selections in Betslip");
                        }
                    }
                    else
                    {
                        string removeSel, betslipEvent;
                        removeSel = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selection + "')]/following::a[@class='bxc slip-remove'][1]";
                        betslipEvent = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selection + "')]";

                        BFcommonObj.clickObject(browser, removeSel);
                        betSlpCntafterRemoval = GetBetslipCount(browser);

                        TimeSpan ts = new TimeSpan(0, 0, 5);
                        driver.Manage().Timeouts().ImplicitlyWait(ts);
                        Assert.IsFalse(browser.IsElementPresent(betslipEvent), "Failed to remove Event-Selection " + eventName + "-" + selection + "'");
                        Assert.IsTrue(betSlpCntafterRemoval.Equals(betSlpCnt - 1), "Betslip count failed to reduce after selection removal");
                        Console.WriteLine("Event-Selection  '" + eventName + "-" + selection + "' was removed successfully");
                        Console.WriteLine("Betslip counter was updated on removing a selection");
                    }
                }
                else
                {
                    Fail("There are no selections available in the Betslip");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'RemoveSelFromBetslip' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Calculate the Expected Totalstake
        /// </summary>
        /// <param name="browser">Browser Instance</param>
        /// <param name="betType">Type of theBet</param>
        /// <param name="stake">Stake Entered</param>
        /// <returns>Expected Stake </returns>
        /// <example>CalculateTotalStake(browser,"double",2)</example>
        public double CalculateTotalStake(ISelenium browser, string betType, double stake)
        {
            try
            {
                double totalStake = 0.00;
                int numberofSelections = 0;
                int numberOfLines;

                numberOfLines = CalculateBetLines(browser, betType.ToUpper());
                numberofSelections = GetBetslipCount(browser);
                totalStake = numberOfLines * stake;
                return totalStake;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'CalculateTotalStake' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return 0.00;
            }
        }



        /// <summary>
        /// Generates the odds in actual format
        /// </summary>
        /// <Author>Pradeep</Author>
        /// <Date>26 July 2012</Date>
        /// <param name="oddsArray">List of odds </param>
        public void FGenerateOdds(ref string[] oddsArray)
        {
            int cnt = 0;
            double ActualOdd = 0;
            cnt = 0;
            Type arraytype;
            bool isArrayFlag;
            string[] odds;

            arraytype = oddsArray.GetType();
            isArrayFlag = arraytype.IsArray;

            if (!isArrayFlag)
            {
                Console.WriteLine("GetPotentialReturnForMultiples : Error: No Odds provided. Please provide Odd(s) in array format for calculating potential return.");
                return;
            }

            foreach (string str in oddsArray)
            {
                int indexvalue = str.ToString(CultureInfo.InvariantCulture).IndexOf("/", System.StringComparison.Ordinal);
                if (indexvalue != -1)
                {
                    odds = str.Split('/');
                    ActualOdd = Convert.ToDouble(odds[0], CultureInfo.InvariantCulture) / Convert.ToDouble(odds[1], CultureInfo.CurrentCulture);
                    oddsArray[cnt] = Convert.ToString((ActualOdd) + Convert.ToDouble(1));
                }
                else
                {
                    oddsArray[cnt] = Convert.ToString(str);
                }
                cnt = cnt + 1;
            }
        }



        /// <summary>
        /// Gets the expected potential return for multi bets or for a single bet.
        /// </summary>
        /// <Author>Sudhir</Author>
        /// <Date>26 July 2012</Date>
        /// <param name="oddsArray">odds values as array</param>
        /// <param name="stake">Stake entered</param>
        /// <param name="betType">Bet Type</param>
        /// <returns>Potential value in double rounded to 2 decimals</returns>
        public double GetPotentialReturnForMultiples(string[] oddsArray, double stake, string betType)
        {
            int cnt, oddsCount;
            double potentialRet = 0.00, dbl1, dbl2, dbl3, dbl4, dbl5, dbl6, dbl7, dbl8, dbl9, dbl10, dbl11, dbl12, dbl13, dbl14, dbl15;
            double trbl1, trbl2, trbl3, trbl4, trbl5, trbl6, trbl7, trbl8, trbl9, trbl10, trbl11, trbl12, trbl13, trbl14, trbl15, trbl16, trbl17, trbl18, trbl19, trbl20;
            double acc4 = 0.00, acc4_1, acc4_2, acc4_3, acc4_4, acc4_5, acc4_6, acc4_7, acc4_8, acc4_9, acc4_10, acc4_11, acc4_12, acc4_13, acc4_14, acc4_15;
            double acc5 = 0.00, acc6 = 0.00, acc5_1, acc5_2, acc5_3, acc5_4, acc5_5, acc5_6 = 0.00;
            double singles = 0.00;
            double trebles = 0.00;
            double dbls = 0.00;

            // Getting array type
            Type arraytype = oddsArray.GetType();
            bool isArrayFlag = arraytype.IsArray;

            if (!isArrayFlag)
            {
                Console.WriteLine("GetPotentialReturnForMultiples : Error: No Odds provided. Please provide Odd(s) in array format for calculating potential return.");
            }

            // Converting the fractional odds strings to its actual value(+1)
            FGenerateOdds(ref oddsArray);
            oddsCount = (oddsArray.GetUpperBound(0)) + 1;

            //Calculating the total singles of all the odds
            for (cnt = 0; cnt <= oddsArray.GetUpperBound(0); cnt++)
            {
                singles = singles + stake * (Convert.ToDouble(oddsArray[cnt], CultureInfo.InvariantCulture));
            }
            //            singles = ((double)((int)(singles * 100.0))) / 100.0;            

            // Gathering the required multi (s) based on number of odds provided
            if (oddsCount == 2)
            {
                dbls = (stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture);
                //                dbls = ((double)((int)(dbls * 100.0))) / 100.0;
            }
            else if (oddsCount == 3)
            {
                dbl1 = (stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture);
                dbl2 = (stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture);
                dbl3 = (stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture);
                dbls = dbl1 + dbl2 + dbl3;
                //                dbls = ((double)((int)(dbls * 100.0))) / 100.0;

                trebles = ((stake * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture);
                //                trebles = ((double)((int)(trebles * 100.0))) / 100.0;
            }
            else if (oddsCount == 4)
            {
                dbl1 = (stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture);
                dbl2 = (stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture);
                dbl3 = (stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3], CultureInfo.InvariantCulture);
                dbl4 = (stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture);
                dbl5 = (stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture);
                dbl6 = (stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture);
                dbls = dbl1 + dbl2 + dbl3 + dbl4 + dbl5 + dbl6;
                //                dbls = ((double)((int)(dbls * 100.0))) / 100.0;

                trbl1 = ((stake * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture);
                trbl2 = ((stake * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[3], CultureInfo.InvariantCulture);
                trbl3 = ((stake * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture);
                trbl4 = ((stake * Convert.ToDouble(oddsArray[3], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture);
                trebles = trbl1 + trbl2 + trbl3 + trbl4;
                //                trebles = ((double)((int)(trebles * 100.0))) / 100.0;

                acc4 = (((stake * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                //                acc4 = ((double)((int)(acc4 * 100.0))) / 100.0;
            }
            else if (oddsCount == 5)
            {
                dbl1 = (stake * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture);
                dbl2 = (stake * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture);
                dbl3 = (stake * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[3], CultureInfo.InvariantCulture);
                dbl4 = (stake * Convert.ToDouble(oddsArray[0], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[4], CultureInfo.InvariantCulture);
                dbl5 = (stake * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture);
                dbl6 = (stake * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[3], CultureInfo.InvariantCulture);
                dbl7 = (stake * Convert.ToDouble(oddsArray[1], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[4], CultureInfo.InvariantCulture);
                dbl8 = (stake * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[3], CultureInfo.InvariantCulture);
                dbl9 = (stake * Convert.ToDouble(oddsArray[2], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[4], CultureInfo.InvariantCulture);
                dbl10 = (stake * Convert.ToDouble(oddsArray[3], CultureInfo.InvariantCulture)) * Convert.ToDouble(oddsArray[4], CultureInfo.InvariantCulture);
                dbls = dbl1 + dbl2 + dbl3 + dbl4 + dbl5 + dbl6 + dbl7 + dbl8 + dbl9 + dbl10;
                //                dbls = ((double)((int)(dbls * 100.0))) / 100.0;

                trbl1 = ((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2]);
                trbl2 = ((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                trbl3 = ((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                trbl4 = ((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                trbl5 = ((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                trbl6 = ((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[0]);
                trbl7 = ((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                trbl8 = ((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[0]);
                trbl9 = ((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[1]);
                trbl10 = ((stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[0]);
                trebles = trbl1 + trbl2 + trbl3 + trbl4 + trbl5 + trbl6 + trbl7 + trbl8 + trbl9 + trbl10;
                //                trebles = ((double)((int)(trebles * 100.0))) / 100.0;

                acc4_1 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                acc4_2 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                acc4_3 = (((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                acc4_4 = (((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[0]);
                acc4_5 = (((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1]);
                acc4 = acc4_1 + acc4_2 + acc4_3 + acc4_4 + acc4_5;
                //                acc4 = ((double)((int)(acc4 * 100.0))) / 100.0;

                acc5 = ((((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                //                acc5 = ((double)((int)(acc5 * 100.0))) / 100.0;
            }
            else if (oddsCount == 6)
            {
                dbl1 = (stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1]);
                dbl2 = (stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[2]);
                dbl3 = (stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[3]);
                dbl4 = (stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[4]);
                dbl5 = (stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[5]);
                dbl6 = (stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2]);
                dbl7 = (stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[3]);
                dbl8 = (stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[4]);
                dbl9 = (stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[5]);
                dbl10 = (stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                dbl11 = (stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[4]);
                dbl12 = (stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[5]);
                dbl13 = (stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                dbl14 = (stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[5]);
                dbl15 = (stake * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                dbls = dbl1 + dbl2 + dbl3 + dbl4 + dbl5 + dbl6 + dbl7 + dbl8 + dbl9 + dbl10 + dbl11 + dbl12 + dbl13 + dbl14 + dbl15;
                //                dbls = ((double)((int)(dbls * 100.0))) / 100.0;

                trbl1 = ((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2]);
                trbl2 = ((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                trbl3 = ((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                trbl4 = ((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                trbl5 = ((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                trbl6 = ((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                trbl7 = ((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                trbl8 = ((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[0]);
                trbl9 = ((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                trbl10 = ((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                trbl11 = ((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[0]);
                trbl12 = ((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[1]);
                trbl13 = ((stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                trbl14 = ((stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[0]);
                trbl15 = ((stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1]);
                trbl16 = ((stake * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1]);
                trbl17 = ((stake * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2]);
                trbl18 = ((stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[1]);
                trbl19 = ((stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[2]);
                trbl20 = ((stake * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[2]);
                trebles = trbl1 + trbl2 + trbl3 + trbl4 + trbl5 + trbl6 + trbl7 + trbl8 + trbl9 + trbl10 + trbl11 + trbl12 + trbl13 + trbl14 + trbl15 + trbl16 + trbl17 + trbl18 + trbl19 + trbl20;
                //                trebles = ((double)((int)(trebles * 100.0))) / 100.0;

                acc4_1 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                acc4_2 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[4]);
                acc4_3 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[5]);
                acc4_4 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                acc4_5 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[5]);
                acc4_6 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                acc4_7 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                acc4_8 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[5]);
                acc4_9 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                acc4_10 = (((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                acc4_11 = (((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                acc4_12 = (((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[5]);
                acc4_13 = (((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                acc4_14 = (((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                acc4_15 = (((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                acc4 = acc4_1 + acc4_2 + acc4_3 + acc4_4 + acc4_5 + acc4_6 + acc4_7 + acc4_8 + acc4_9 + acc4_10 + acc4_11 + acc4_12 + acc4_13 + acc4_14 + acc4_15;
                //                acc4 = ((double)((int)(acc4 * 100.0))) / 100.0;

                acc5_1 = ((((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4]);
                acc5_2 = ((((stake * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                acc5_3 = ((((stake * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[0]);
                acc5_4 = ((((stake * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1]);
                acc5_5 = ((((stake * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2]);
                acc5_6 = ((((stake * Convert.ToDouble(oddsArray[5])) * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3]);
                acc5 = acc5_1 + acc5_2 + acc5_3 + acc5_4 + acc5_5 + acc5_6;
                //                acc5 = ((double)((int)(acc5 * 100.0))) / 100.0;

                acc6 = (((((stake * Convert.ToDouble(oddsArray[0])) * Convert.ToDouble(oddsArray[1])) * Convert.ToDouble(oddsArray[2])) * Convert.ToDouble(oddsArray[3])) * Convert.ToDouble(oddsArray[4])) * Convert.ToDouble(oddsArray[5]);
                //                acc6 = ((double)((int)(acc6 * 100.0))) / 100.0;
            }

            string multiBetType = (betType.ToUpper(CultureInfo.CurrentCulture)).Trim();
            switch (multiBetType.ToUpper())
            {
                case "SINGLE":
                    potentialRet = singles;
                    break;
                case "DOUBLE":
                    potentialRet = dbls;
                    break;
                case "TREBLE":
                    potentialRet = trebles;
                    break;
                case "ACCUMULATOR (4)":
                    potentialRet = acc4;
                    break;
                case "TRIXIE":
                    potentialRet = dbls + trebles;
                    break;
                case "PATENT":
                    potentialRet = singles + dbls + trebles;
                    break;
                case "YANKEE":
                    potentialRet = dbls + trebles + acc4;
                    break;
                case "LUCKY 15":
                    potentialRet = singles + dbls + trebles + acc4;
                    break;
                case "CANADIAN":
                    potentialRet = dbls + trebles + acc4 + acc5;
                    break;
                case "LUCKY 31":
                    potentialRet = singles + dbls + trebles + acc4 + acc5;
                    break;
                case "ACCUMULATOR (5)":
                    potentialRet = Math.Round(acc5, 2); ;
                    break;
                case "HEINZ":
                    potentialRet = dbls + trebles + acc4 + acc5 + acc6;
                    break;
                case "LUCKY 63":
                    potentialRet = singles + dbls + trebles + acc4 + acc5 + acc6;
                    break;
                case "FORECAST":
                    break;
                case "REVERSE FORECAST":
                    break;
                case "COMBINATION FORECAST":
                    break;
                case "TRICAST":
                    break;
                case "COMBINATION TRICAST":
                    break;
                default:
                    //Console.WriteLine("GetPotentialReturnForMultiples : Error: Provided bet type:" + multiBetType + "not found. Please provide a valid multi bet type.");
                    //potentialRet = singles;
                    //potentialRet = Math.Round(potentialRet, 2);
                    //Console.WriteLine(potentialRet);
                    break;
            } //end of switch

            //Console.WriteLine("GetPotentialReturnForMultiples :Success: Potential return successfully returned for " + multiBetType);
            //            potentialRet = Math.Round(potentialRet, 2);
            //            potentialRet = (potentialRet * 100.0) / 100.0;
            potentialRet = FormatNumber(potentialRet);
            return potentialRet;
        }



        /// <summary>
        /// Calculate Accumulated odds for Fraction and Decimal odds
        /// <Author>Pradeep</Author>
        /// <Date>26 July 2012</Date>
        /// <param name="browser">Browser Instace</param>
        /// <param name="oddsArray">odds array</param>
        /// <param name="isOddsInFraction">
        /// true- if odds in Fraction
        /// false- if odds in Decimal
        /// </param>
        /// <returns>Accumlated odds</returns>
        /// <example>CalculateAccumulatedOdds(browser,oddsArray,true)</example>
        public double CalculateAccumulatedOdds(ISelenium browser, string[] oddsArray, bool isOddsInFraction)
        {
            double accumulatedOdd = 0.00;
            switch (oddsArray.Length.ToString())
            {
                case "2":
                    if (isOddsInFraction)
                    {
                        accumulatedOdd = (Convert.ToDouble((oddsArray[0]) + 1) * (Convert.ToDouble(oddsArray[1]) + 1));
                    }
                    else
                    {
                        accumulatedOdd = (Convert.ToDouble(oddsArray[0]) * Convert.ToDouble((oddsArray[1])));
                    }
                    break;

                case "3":
                    if (isOddsInFraction)
                    {
                        accumulatedOdd = ((Convert.ToDouble(oddsArray[0]) + 1) * (Convert.ToDouble(oddsArray[1]) + 1) * (Convert.ToDouble(oddsArray[2]) + 1));
                    }
                    else
                    {
                        accumulatedOdd = ((Convert.ToDouble(oddsArray[0])) * (Convert.ToDouble(oddsArray[1])) * (Convert.ToDouble(oddsArray[2])));
                    }
                    break;
                case "4":
                    if (isOddsInFraction)
                    {
                        accumulatedOdd = (Convert.ToDouble((oddsArray[0]) + 1) * (Convert.ToDouble(oddsArray[1]) + 1) * (Convert.ToDouble(oddsArray[2]) + 1) * (Convert.ToDouble(oddsArray[3]) + 1));
                    }
                    else
                    {
                        accumulatedOdd = (Convert.ToDouble(oddsArray[0]) * Convert.ToDouble(oddsArray[1]) * Convert.ToDouble(oddsArray[2]) * Convert.ToDouble(oddsArray[3]));
                    }
                    break;
                case "5":
                    if (isOddsInFraction)
                    {
                        accumulatedOdd = (Convert.ToDouble((oddsArray[0]) + 1) * (Convert.ToDouble(oddsArray[1]) + 1) * (Convert.ToDouble(oddsArray[2]) + 1) * (Convert.ToDouble(oddsArray[3]) + 1) * (Convert.ToDouble(oddsArray[4]) + 1));
                    }
                    else
                    {
                        accumulatedOdd = (Convert.ToDouble(oddsArray[0]) * Convert.ToDouble(oddsArray[1]) * Convert.ToDouble(oddsArray[2]) * Convert.ToDouble(oddsArray[3]) * Convert.ToDouble(oddsArray[4]));
                    }
                    break;
                case "6":
                    if (isOddsInFraction)
                    {
                        accumulatedOdd = ((Convert.ToDouble(oddsArray[0]) + 1) * (Convert.ToDouble(oddsArray[1]) + 1) * (Convert.ToDouble(oddsArray[2]) + 1) * (Convert.ToDouble(oddsArray[3]) + 1) * (Convert.ToDouble(oddsArray[4]) + 1) * (Convert.ToDouble(oddsArray[5]) + 1));
                    }
                    else
                    {
                        accumulatedOdd = (Convert.ToDouble(oddsArray[0]) * Convert.ToDouble(oddsArray[1]) * Convert.ToDouble(oddsArray[2]) * Convert.ToDouble(oddsArray[3]) * Convert.ToDouble(oddsArray[4]) * Convert.ToDouble(oddsArray[5]));
                    }
                    break;
                case "7":
                    if (isOddsInFraction)
                    {
                        accumulatedOdd = ((Convert.ToDouble(oddsArray[0]) + 1) * (Convert.ToDouble(oddsArray[1]) + 1) * (Convert.ToDouble(oddsArray[2]) + 1) * (Convert.ToDouble(oddsArray[3]) + 1) * (Convert.ToDouble(oddsArray[4]) + 1) * (Convert.ToDouble(oddsArray[5]) + 1) * (Convert.ToDouble(oddsArray[6]) + 1));
                    }
                    else
                    {
                        accumulatedOdd = (Convert.ToDouble(oddsArray[0]) * Convert.ToDouble(oddsArray[1]) * Convert.ToDouble(oddsArray[2]) * Convert.ToDouble(oddsArray[3]) * Convert.ToDouble(oddsArray[3]) * Convert.ToDouble(oddsArray[4]) * Convert.ToDouble(oddsArray[5]) * Convert.ToDouble(oddsArray[6]));
                    }

                    break;
                case "8":
                    if (isOddsInFraction)
                    {
                        accumulatedOdd = ((Convert.ToDouble(oddsArray[0]) + 1) * (Convert.ToDouble(oddsArray[1]) + 1) * (Convert.ToDouble(oddsArray[2]) + 1) * (Convert.ToDouble(oddsArray[3]) + 1) * (Convert.ToDouble(oddsArray[4]) + 1) * (Convert.ToDouble(oddsArray[5]) + 1) * (Convert.ToDouble(oddsArray[6]) + 1) * (Convert.ToDouble(oddsArray[7]) + 1));
                    }
                    else
                    {
                        accumulatedOdd = (Convert.ToDouble(oddsArray[0]) * Convert.ToDouble(oddsArray[1]) * Convert.ToDouble(oddsArray[2]) * Convert.ToDouble(oddsArray[3]) * Convert.ToDouble(oddsArray[4]) * Convert.ToDouble(oddsArray[5]) * Convert.ToDouble(oddsArray[6]) * Convert.ToDouble(oddsArray[7]));
                    }
                    break;
                default:
                    accumulatedOdd = 0.0;
                    break;
            }

            return Math.Round(accumulatedOdd, 2);
        }



        /// <summary>
        /// Get the expected number of bet lines
        /// </summary>
        /// <Author>Pradeep</Author>
        /// <Date>26 July 2012</Date>
        /// <param name="myBrowser">Selenium Instances</param>
        /// <param name="selection"> Selection Name</param>
        /// <example>betLines = CalculateBetLines("Double")</example> 
        public int CalculateBetLines(ISelenium browser, string selection)
        {
            decimal numerator;
            decimal denom;

            //'Taking the betslip count for knowing the number of selections added to the betslip so that we can calculate the total stake for different bet types
            int betSlipCount = GetBetslipCount(browser);
            int betLines = 0;

            switch (selection.ToUpper())
            {
                case "SINGLE":
                    return betLines = 1;//betSlipCount;
                case "DOUBLE":
                    numerator = Factorial(betSlipCount);
                    denom = Factorial(2) * Factorial(betSlipCount - 2);
                    betLines = Convert.ToInt32((numerator / denom));
                    return betLines;
                case "TREBLE":
                    numerator = Factorial(betSlipCount);
                    denom = Factorial(3) * Factorial(betSlipCount - 3);
                    betLines = Convert.ToInt32((numerator / denom));
                    return betLines;
                case "ACCUMULATOR (4)":
                    numerator = Factorial(betSlipCount);
                    denom = Factorial(4) * Factorial(betSlipCount - 4);
                    betLines = Convert.ToInt32((numerator / denom));
                    return betLines;

                case "TRIXIE":
                    return betLines = 4;
                case "PATENT":
                    return betLines = 7;
                case "YANKEE":
                    return betLines = 11;
                case "LUCKY 15":
                    return betLines = 15;
                case "CANADIAN":
                    return betLines = 26;
                case "LUCKY 31":
                    return betLines = 31;
                case "ACCUMULATOR (5)":
                    numerator = Factorial(betSlipCount);
                    denom = Factorial(5) * Factorial(betSlipCount - 5);
                    betLines = Convert.ToInt32((numerator / denom));
                    return betLines;
                case "HEINZ":
                    return betLines = 57;
                case "LUCKY 63":
                    return betLines = 63;
                case "FORECAST":
                case "REVERSE FORECAST":
                case "COMBINATION FORECAST":
                case "TRICAST":
                case "COMBINATION TRICAST":
                default:
                    return betLines = 1;
            }
        }



        /// <summary>
        ///  This method Calculates the factorial of a number
        /// </summary>
        /// <Author>Pradeep</Author>
        /// <Date>26 July 2012</Date>
        /// <param name="anyNum">Any Number</param>
        /// <returns> Factorial of the given Number</returns>
        /// <example>Factorial(9)</example>
        public decimal Factorial(int anyNum)
        {
            try
            {
                //Non-recursive factorial for
                //-170< anyNum >170
                int i;
                decimal factorial = 0;

                if (anyNum < -170 || anyNum > 170)
                {
                    return factorial = 0;

                }
                if (System.Math.Abs(anyNum) != anyNum)
                {
                    factorial = -1;
                    for (i = -2; i >= anyNum; i--)
                    {
                        factorial = factorial * i;
                    }
                    return factorial;
                }
                else
                {
                    factorial = 1;

                    for (i = 2; i <= anyNum; i++)
                    {
                        factorial = factorial * i;
                    }
                    return factorial;
                }
            }
            catch (AutomationException ex)
            {
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }


        ///<summary>
        /// This method switched the odd type between fractional and decimal 
        /// <example>OddTypeSwitch(browser, "decimal") </example>
        /// <example>OddTypeSwitch(browser, "fractional") </example>
        public void OddTypeSwitch(ISelenium browser, string type)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                BFcommonObj.selectMenuButton(browser);

                if (type.ToLower() == "fractional")
                {
                    Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.fractionalOdd), "Fractional odd switcher was not displayed in the sidebar");
                    js.ExecuteScript("document.getElementById('oddsTypeFractional_index').click()");
                    Thread.Sleep(1000);
                    IWebElement switcher = driver.FindElement(By.Id("oddsTypeFractional_index"));
                    Assert.IsTrue(switcher.GetAttribute("class").Contains("oddstypeselected"), "Failed to switch to Fractional Oddd type");
                }
                else
                {
                    Assert.IsTrue(browser.IsElementPresent(LoginLogoutControls.decimalOdd), "Decimal odd switcher was not displayed in the sidebar");
                    js.ExecuteScript("document.getElementById('oddsTypeDecimal_index').click()");
                    Thread.Sleep(1000);
                    IWebElement switcher = driver.FindElement(By.Id("oddsTypeDecimal_index"));
                    Assert.IsTrue(switcher.GetAttribute("class").Contains("oddstypeselected"), "Failed to switch to Fractional Oddd type");
                }
                js.ExecuteScript("document.getElementById('menu-button').click()");
                BFframeworkCommonObj.PageSync(browser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'OddTypeSwitch' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        ///This method navigates to the specified event details page and adds and evrifies a selction to Betslip
        /// <example>EWterms = BTbetslipObj.AddAndVerifySelectionInBetslip(MyBrowser, "", "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false) </example>
        public string AddAndVerifySelectionInBetslip(ISelenium browser, string sidebarLink, string navPanel, string className, string typeName, string subTypeName, string eventName, string marketName, string selection, string odds, bool returnEW)
        {
            try
            {
                string returnVal = null;
                odds = String.Format("{0:0.00}", double.Parse(odds));
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                NavigateToEventDetailsPage(browser, sidebarLink, navPanel, className, typeName, subTypeName, eventName, marketName);
                returnVal = AddSelectionToBetslip(browser, "", eventName, marketName, selection, odds, returnEW);
                //verify the selection added to betslip   
                VerifySelectionInBetslip(browser, "", eventName, marketName, selection, odds);

                Console.WriteLine("Event-Selection-Odd  '" + eventName + "-" + selection + "-" + odds + "' was added and verified successsfully to the betslip ");
                return returnVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'AddAndVerifySelectionInBetslip' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }



        /// <summary>
        ///This method navigates to the specified sports page
        /// <example>NavigateToSportsPage(MyBrowser, "", "Competition", "Football") </example>
        public void NavigateToSportsPage(ISelenium browser, string sidebarLink, string navPanel, string className)
        {
            try
            {
                string xPath;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                //Navigate to sports page
                if (string.IsNullOrEmpty(sidebarLink))
                {
                    //check if the A-Z element is expanded
                    BFcommonObj.SelectLinksFromSideBar(browser, "A-Z Betting", "A-Z Betting");
                    IWebElement AZexpanded = driver.FindElement(By.Id("p_az_list_allSportsGroup"));
                    if (!bool.Parse(AZexpanded.GetAttribute("expanded")))
                    {
                        AZexpanded.Click();
                        Thread.Sleep(1000);
                    }
                    // select class Name
                    xPath = "//div[@id='expandable_p_az_list_allSportsGroup']//div[@class='bxcl bxf ml5']";
                    BFcommonObj.clickObjectInColl(browser, xPath, className);
                }
                else
                {
                    BFcommonObj.SelectLinksFromSideBar(browser, sidebarLink, sidebarLink);
                }


                //Select the niavigation panel (cupons,competition)
                if (className.ToLower() == "horse racing")
                {
                    xPath = "//span[@class='t7 page-title' and contains(text(), '" + className + "')]/following::a[starts-with(@class, 'bxc bxf tab') and contains(text(), '" + navPanel + "')]";
                }
                else
                {
                    xPath = "//span[@class='t7 page-title' and contains(text(), '" + className + "')]/following::span[starts-with(@class, 'bxc bxf tab') and contains(text(), '" + navPanel + "')]";
                }
                BFcommonObj.clickObject(browser, xPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'NavigateToSportsPage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        ///This method navigates to the specified event details page
        /// <example>NavigateToEventDetailsPage(MyBrowser, "", "Competition", Football", "Fa Cup", "EVentA", "Cup Winner")</example>
        public void NavigateToEventDetailsPage(ISelenium browser, string sidebarLink, string navPanel, string className, string typeName, string subTypeName, string eventName, string marketName)
        {
            try
            {
                string pageTitle, xPath;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                NavigateToSportsPage(browser, sidebarLink, navPanel, className);

                // Expand the inplay tab 
                string[] arrLiveEvents = navPanel.Split('|');
                if (arrLiveEvents.Length > 1)
                {
                    xPath = "//div[@class='bxcl bxf ml5' and contains(text(), '" + arrLiveEvents[1] + "')]/..";
                    IWebElement inplayEvent = driver.FindElement(By.XPath(xPath));
                    if (!bool.Parse(inplayEvent.GetAttribute("expanded")))
                    {
                        inplayEvent.Click();
                        Thread.Sleep(1000);
                    }
                    //click on the more arrow
                    xPath = "//span[@class='bxcl event-name' and contains(text(), '" + eventName + "')]//following::span[contains(text(),'More')][1]";
                    BFcommonObj.clickObject(browser, xPath);
                }
                // For nonlive/upcoming events
                else
                {
                    //check if the Type is expanded
                    xPath = "//div[@id = '" + className + "-" + navPanel.ToLower().Trim() + "']//div[@class='bxcl expandableHeader']//div[@class='bxcl bxf ml5' and contains(text(), '" + typeName + "')]/..";
                    IWebElement Type = driver.FindElement(By.XPath(xPath));
                    if (!bool.Parse(Type.GetAttribute("expanded")))
                    {
                        Type.Click();
                        Thread.Sleep(1000);
                    }

                    if (!string.IsNullOrEmpty(subTypeName))
                    {
                        //Select the subTypeName
                        xPath = "//div[@class='bxcl bxf ml5' and text()='" + typeName + "']//following::div[@class='expandable' and @expanded='true']//div[@class='bxcl bxf ml5']";
                        BFcommonObj.clickObjectInColl(browser, xPath, subTypeName);

                        //Perform only if MORE text is present
                        TimeSpan ts = new TimeSpan(0, 0, 5);
                        driver.Manage().Timeouts().ImplicitlyWait(ts);

                        // code to handle conditions where event name contains "vs", xPath is different
                        string strFootballEventName = null;
                        if (eventName.ToLower().Contains("vs"))
                        {
                            strFootballEventName = eventName;
                        }
                        if (browser.IsElementPresent("//span[@class='bxcl event-name' and contains(text(), '" + eventName + "')]") || browser.IsTextPresent(strFootballEventName))
                        {
                            //check for the Type-Subtype banner
                            pageTitle = "//span[@class='t7 page-title' and contains(text(), '" + typeName + " - " + subTypeName + "')]";
                            Assert.IsTrue(browser.IsElementPresent(pageTitle), typeName + " - " + subTypeName + " banner was not displayed");
                            Thread.Sleep(2000);

                            //Navigate to event details page by tapping 'More' button of a  specified Event                                       
                            // code to handle conditions where event name contains "vs", xPath is different
                            if (eventName.ToLower().Contains("vs"))
                            {
                                string[] arr = eventName.Replace("vs", "-").Split('-');     //eventName.Split('-');

                                xPath = "//span[@class='t7 page-title' and contains(text(), '" + typeName + " - " + subTypeName + "')]/following::span[@class='bxcl event-name' and contains(text()[1], '" + arr[0].Trim() + "') and contains(text()[2], '" + arr[1].Trim() + "')]/following::span[contains(text(),'More')][1]";
                            }
                            else
                            {
                                xPath = "//span[@class='t7 page-title' and contains(text(), '" + typeName + " - " + subTypeName + "')]/following::span[@class='bxcl event-name' and contains(text(), '" + eventName + "')]//following::span[contains(text(),'More')][1]";
                            }
                            BFcommonObj.clickObject(browser, xPath);
                        }
                    }
                    else
                    {
                        xPath = "//div[@class='bxcl bxf ml5' and text()='" + typeName + "']//following::div[@class='expandable' and @expanded='true']//div[@class='bxcl bxf ml5']";
                        BFcommonObj.clickObjectInColl(browser, xPath, eventName);
                    }
                }


                //check for the Event banner
                // code to handle conditions where event name contains "vs", xPath is different
                if (eventName.ToLower().Contains("vs"))
                {
                    pageTitle = "//span[@class='t7 page-title' and contains(text(), '" + eventName.Replace("vs", "-") + "')]";
                }
                else
                {
                    pageTitle = "//span[@class='t7 page-title' and contains(text(), '" + eventName + "')]";
                }
                Assert.IsTrue(browser.IsElementPresent(pageTitle), eventName + " - banner was not displayed");

                //check if the market expanded and select it                                          
                xPath = "//div[@class='bxcl bxf ml5' and text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + marketName.ToLower() + "')]]/..";
                Assert.IsTrue(browser.IsElementPresent(xPath), marketName + " market is not present");
                IWebElement market = driver.FindElement(By.XPath(xPath));
                if (!bool.Parse(market.GetAttribute("expanded")))
                {
                    market.Click();
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'NavigateToEventDetailsPage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        ///This method adds and verifies a selction to betslip
        /// <example>AddSelectionToBetslip(MyBrowser, "EVentA", "Sel1", "1.25", true) </example>
        public string AddSelectionToBetslip(ISelenium browser, string typeName, string eventName, string marketName, string selection, string odds, bool returnEW)
        {
            try
            {
                string xPath, returnVal = null;
                odds = String.Format("{0:0.00}", double.Parse(odds));
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                // code to handle conditions where event name contains "vs", xPath is different
                if (eventName.ToLower().Contains("vs"))
                {
                    eventName = eventName.Replace("vs", "-");
                }

                // get the Each way term
                if (returnEW == true)
                {
                    xPath = "//span[@class='t7 page-title' and contains(text(), '" + eventName + "')]/following::span[@class='ew-terms']";
                    returnVal = browser.GetText(xPath);
                }

                // Get the Betslip Count
                int initBetslipCnt = GetBetslipCount(browser);

                //Add selection to Betslip
                xPath = "//span[@class='t7 page-title' and contains(text(), '" + eventName + "')]//following::div[contains(@class, 'odds-text') and contains(text(), '" + selection + "')]//following::span[@class='odds-convert' and contains(text(),'" + odds + "')]";
                BFcommonObj.clickObject(browser, xPath);

                //verify the selection added to betslip   
                //BFcommonObj.clickObject(browser, BetslipControls.betslipButton);
                //VerifySelectionInBetslip(browser, "", eventName, marketName, selection, odds, false);

                BFcommonObj.clickObject(browser, BetslipControls.betslipButton);
                int laterBetslipCnt = GetBetslipCount(browser);
                Assert.IsTrue(initBetslipCnt + 1 == laterBetslipCnt, "Mismatch in Betslip count on adding a selction, Expected:" + initBetslipCnt + 1 + ", Actual:" + laterBetslipCnt + ".");

                return returnVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'AddSelectionToBetslip' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }



        /// <summary>
        ///This method enters the stake for a specified selection
        /// <example>EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].Odds, testDataLst[0].Stake, "Single", true);</example>
        /// <example>EnterStake(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].Odds, testDataLst[0].Stake, "Double", false);</example>
        public void EnterStake(ISelenium browser, string eventName, string selection, string marketName, string odds, string stake, string multiBetType, bool EW)
        {
            try
            {
                string stakeXPath = null;
                string ewXpath = null;
                string selID;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                if (!string.IsNullOrEmpty(odds)) odds = String.Format("{0:0.00}", double.Parse(odds));

                //Check if stake is to be entered for Single bet
                if (String.IsNullOrEmpty(multiBetType) || multiBetType.ToLower().Contains("single"))
                {
                    //verify the selection added to betslip                    
                    selID = GetSelectionId(browser, eventName, selection, marketName, odds, "");
                    IWebElement inputStake = driver.FindElement(By.Id("slip-odds-stake-SGL_" + selID));
                    inputStake.Clear();
                    inputStake.SendKeys(stake);
                    browser.Click(BetslipControls.betslipBanner);
                    Thread.Sleep(1000);

                    //check the Each Way checkbox
                    if (EW == true)
                    {
                        ewXpath = "//input[@id='price_updater_checkbox_SGL_" + selID + "']";
                        BFcommonObj.SelectCheckbox(browser, ewXpath, "ON");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    switch (multiBetType.ToLower())
                    {
                        case "double":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-DBL_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-DBL_')]";
                            break;

                        case "treble":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-TBL_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-TBL_')]";
                            break;

                        case "trixie":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-TRX')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-TRX_')]";
                            break;

                        case "patent":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-PAT')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-PAT_')]";
                            break;

                        case "accumulator (4)":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-ACC4_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-ACC4_')]";
                            break;

                        case "yankee":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-YAN_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-YAN_')]";
                            break;

                        case "lucky 15":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-L15_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-L15_')]";
                            break;

                        case "accumulator (5)":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-ACC5_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-ACC5_')]";
                            break;

                        case "canadian":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-CAN_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-CAN_')]";
                            break;

                        case "lucky 31":
                            stakeXPath = "//input[starts-with(@id, 'slip-odds-stake-L31_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-L31_')]";
                            break;
                    }
                    Assert.IsTrue(browser.IsElementPresent(stakeXPath), multiBetType + " bet type is not presnt in betslip");
                    IWebElement inputStake = driver.FindElement(By.XPath(stakeXPath));
                    inputStake.SendKeys(stake);
                    browser.Click(BetslipControls.betslipBanner);
                    Thread.Sleep(1000);

                    //check the Each Way checkbox
                    if (EW == true)
                    {
                        BFcommonObj.SelectCheckbox(browser, ewXpath, "ON");
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'EnterStake' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// Returns the actual potential return displayed for a selection from betslip
        /// <example>GetPotentialReturnFromBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].Odds, "Single")</example>
        /// <example>GetPotentialReturnFromBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].Odds, "Double")</example>
        public double GetPotentialReturnFromBetSlip(ISelenium browser, string eventName, string selection, string marketName, string odds, string multiBetType)
        {
            try
            {
                string[] itemArr;
                string val, selId;
                if (!string.IsNullOrEmpty(odds)) odds = String.Format("{0:0.00}", double.Parse(odds));
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                if (String.IsNullOrEmpty(multiBetType) || multiBetType.ToLower().Contains("single"))
                {
                    selId = GetSelectionId(browser, eventName, selection, marketName, odds, "");
                    IWebElement potReturnElement = driver.FindElement(By.Id("potential_return_SGL_" + selId));
                    val = potReturnElement.Text;
                }
                else
                {
                    string Xpath = null;
                    switch (multiBetType.ToLower())
                    {
                        case "double":
                            Xpath = "//span[starts-with(@id, 'potential_return_DBL_')]";
                            break;

                        case "treble":
                            Xpath = "//span[starts-with(@id, 'potential_return_TBL_')]";
                            break;

                        case "trixie":
                            Xpath = "//span[starts-with(@id, 'potential_return_TRX_')]";
                            break;

                        case "patent":
                            Xpath = "//span[starts-with(@id, 'potential_return_PAT_')]";
                            break;

                        case "accumulator (4)":
                            Xpath = "//span[starts-with(@id, 'potential_return_ACC4_')]";
                            break;

                        case "yankee":
                            Xpath = "//span[starts-with(@id, 'potential_return_YAN_')]";
                            break;

                        case "lucky 15":
                            Xpath = "//span[starts-with(@id, 'potential_return_L15_')]";
                            break;

                        case "accumulator (5)":
                            Xpath = "//span[starts-with(@id, 'potential_return_ACC5_')]";
                            break;

                        case "canadian":
                            Xpath = "//span[starts-with(@id, 'potential_return_CAN_')]";
                            break;

                        case "lucky 31":
                            Xpath = "//span[starts-with(@id, 'potential_return_L31_')]";
                            break;
                    }
                    Assert.IsTrue(browser.IsElementPresent(Xpath), multiBetType + " bet type is not presnt in betslip");
                    IWebElement potReturnElement = driver.FindElement(By.XPath(Xpath));
                    val = potReturnElement.Text;
                }

                itemArr = val.Split('£');
                return Convert.ToDouble(itemArr[itemArr.Length - 1], CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'GetPotentialReturnFromBetSlip' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return 0.0;
            }
        }



        /// <summary>
        /// Returns the selection id of a perticular Selection in the BetSlip
        /// <example>GetSelectionId(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].Odds, "Single/Double");
        public string GetSelectionId(ISelenium browser, string eventName, string selection, string marketName, string odds, string multiBetType)
        {
            try
            {
                string xPath, selIDstring, selID;
                string[] selIDArr;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                if (String.IsNullOrEmpty(multiBetType) || multiBetType.ToLower() == "single")
                {
                    //verify the selection added to betslip                    
                    VerifySelectionInBetslip(browser, "", eventName, marketName, selection, odds);

                    // code to handle conditions where event name contains "vs", xPath is different
                    if (eventName.ToLower().Contains("vs"))
                    {
                        eventName = eventName.Replace("vs", "-");
                    }

                    //Get the Selection ID
                    xPath = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selection + "')]/following::select[1]";

                    IWebElement selectionID = driver.FindElement(By.XPath(xPath));
                    selIDstring = selectionID.GetAttribute("id");
                    selIDArr = selIDstring.Split('_');
                    selID = selIDArr[selIDArr.Length - 1];
                    return selID;
                }
                else
                {
                    selID = "";
                    return selID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'GetSelectionId' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }


        //Converts EW
        public double ConvertEWOdds(string EachwayTerms)
        {
            string EWoddstr1 = GetEachWayOdd(EachwayTerms);
            string[] arrEWodds = EWoddstr1.Split('/');
            double EWodds = double.Parse(arrEWodds[0]) / double.Parse(arrEWodds[1]);
            return Math.Round(EWodds, 2);
        }



        /// <summary>
        /// Returns the potential returns for EW
        /// <example>GetPotentialReturnsForEW(aryOdd, EWterms,  testDataLst[0].Stake, "Single/Double");</example>
        public double GetPotentialReturnsForEW(string[] oddsArray, string EWterms, double stake, string multiBetType)
        {
            try
            {
                double returnValue = 0.0;
                string[] ewSplit = EWterms.Split('|');
                double EWodds = ConvertEWOdds(ewSplit[0]);
                double odds = Convert.ToDouble(oddsArray[0]);

                if (multiBetType.ToLower() == "single" || multiBetType.ToLower() == "")
                {
                    //Potential Returns (Singles with each way) = 2*Stake+ (Stake*latest available odds)+(Fraction*Stake* latest available odds)
                    returnValue = (2 * stake + (stake * (odds - 1)) + (EWodds * stake * (odds - 1)));
                }
                else
                {
                    double ewReturn;
                    double EWodds2, odds2, EWodds3, odds3;
                    double multiPotentialRet = GetPotentialReturnForMultiples(oddsArray, stake, multiBetType);
                    multiPotentialRet = ((double)((int)(multiPotentialRet * 100.0))) / 100.0;
                    switch (multiBetType.ToLower())
                    {
                        case "double":
                            EWodds2 = ConvertEWOdds(ewSplit[1]);
                            odds2 = Convert.ToDouble(oddsArray[1]);

                            //(((FA* EW) +1)*((B*EW) +1)*stake)---2
                            ewReturn = ((((odds - 1) * EWodds) + 1) * (((odds2 - 1) * EWodds2) + 1) * stake);
                            ewReturn = ((double)((int)(ewReturn * 100.0))) / 100.0;
                            returnValue = multiPotentialRet + ewReturn;
                            break;

                        case "treble":
                            EWodds2 = ConvertEWOdds(ewSplit[1]);
                            odds2 = Convert.ToDouble(oddsArray[1]);
                            EWodds3 = ConvertEWOdds(ewSplit[2]);
                            odds3 = Convert.ToDouble(oddsArray[2]);

                            //(((FA* EW) +1)*((B*EW) +1)*((C*EW)+1)*stake)---2
                            ewReturn = ((((odds - 1) * EWodds) + 1) * (((odds2 - 1) * EWodds2) + 1) * (((odds3 - 1) * EWodds3) + 1) * stake);
                            ewReturn = ((double)((int)(ewReturn * 100.0))) / 100.0;
                            returnValue = multiPotentialRet + ewReturn;
                            break;
                    }
                }
                returnValue = ((double)((int)(returnValue * 100.0))) / 100.0;
                return returnValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'GetPotentialReturnsForEW' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return 0.0;
            }
        }



        /// <summary>
        /// Returns the potential return/Total stake from betlip
        /// <example>GetTotalsFromBetslip(browser, "stake"</example>
        /// /// <example>GetTotalsFromBetslip(browser, "potential"</example>
        public double GetTotalsFromBetslip(ISelenium browser, string stakeORreturn)
        {
            try
            {
                string[] itemArr;
                string val;
                if (stakeORreturn.ToLower().Contains("stake"))
                {
                    // Return Total Stake
                    Assert.IsTrue(browser.IsElementPresent(BetslipControls.totalStake), "Total Stake element is not present in the Betslip");
                    val = browser.GetText(BetslipControls.totalStake);
                }
                else
                {
                    // Return potential return
                    Assert.IsTrue(browser.IsElementPresent(BetslipControls.totalPotentialReturns), "Total Potential Returns element is not present in the Betslip");
                    val = browser.GetText(BetslipControls.totalPotentialReturns);
                }
                itemArr = val.Split('£');
                return Convert.ToDouble(itemArr[itemArr.Length - 1], CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'GetTotalsFromBetslip' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return 0.0;
            }
        }




        /// <summary>
        /// This method places a bet
        /// <example>BetPlacement(browser, "reuse")</example>
        /// <example>BetPlacement(browser, "home")</example>
        public void BetPlacement(ISelenium browser, string totalStake, int betNos, string actionButton, bool isInPlayEvent)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                Assert.IsTrue(browser.IsElementPresent(BetslipControls.betslipBanner), "Betslip page is not displayed");

                double initBalance, laterBalance;
                string xPath;

                initBalance = GetBalance(browser);
                //Place Bet
                //--Issue with clicking the button
                while (browser.IsVisible(BetslipControls.placeBet))
                {
                    driver.FindElement(By.XPath(BetslipControls.placeBet)).Click();
                    BFcommonObj.WaitForLoadingIcon(browser, FrameGlobals.IconLoadTimeout);
                }

                //Check for the timer if the event is live
                if (isInPlayEvent == true)
                {
                    Assert.IsTrue(browser.IsTextPresent("Please wait while we place your In-Play bet"), "InPlay bet counter was not displayed for betplacement on Live Event");
                }
                if (isInPlayEvent == false)
                    Assert.IsFalse(browser.IsTextPresent("Please wait while we place your In-Play bet"), "InPlay bet counter was displayed for betplacement on Live Event");
                BFcommonObj.WaitForLoadingIcon(browser, FrameGlobals.IconLoadTimeout);
                BFframeworkCommonObj.WaitUntilElementPresent(browser, BetslipControls.betReceiptBanner, FrameGlobals.ElementLoadTimeout.ToString());
                Assert.IsTrue(browser.IsTextPresent("Bets placed successfully"), "Bets placement was unsuccessful");

                //Validate the Total Stake & Total Bets
                xPath = "//span[@class='rel t2' and contains(text(), 'Total stake: £ " + totalStake + "')]";
                Assert.IsTrue(browser.IsElementPresent(xPath), "'Total stake: £ " + totalStake + "' text was is not present in the Bet Receipt");
                xPath = "//div[@class='bxcl bg25 h35 ffdc fs15px c1 ttu lh39 pl10' and contains(text(), 'Your bets (" + betNos + ")')]";
                Assert.IsTrue(browser.IsElementPresent(xPath), "'Your bets (" + betNos + ")' text was is not present in the Bet Receipt");
                Console.WriteLine("Bet Placement was successful");

                //Click on the Action button
                if (actionButton.ToLower().Contains("reuse"))
                {
                    BFcommonObj.clickObject(browser, BetslipControls.reUseSelection);
                    Assert.IsTrue(browser.IsElementPresent(BetslipControls.betslipBanner), "'ReUse Selection' on Bet Receipt failed to navigate back to the Betslip page");
                }
                else if (actionButton.ToLower().Contains("home"))
                {
                    BFcommonObj.clickObject(browser, BetslipControls.backToHomeButton);
                    Assert.IsTrue(driver.FindElement(By.Id("carousel")).Displayed, "'Back to Home button' on Bet Receipt failed to navigate back to the home page (Balance is not displayed on Login)");
                }

                //validate the balance
                laterBalance = GetBalance(browser);
                Assert.IsTrue(laterBalance == (initBalance - Convert.ToDouble(totalStake)), "Balance failed to update on Bet Placement. Expected:" + laterBalance + ", Actual:" + initBalance + ".");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'BetPlacement' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// This method validtaes all details in bet receipt
        /// <example>ValidateBetReceipt(MyBrowser, "", testDataLst[0].EventName, testDataLst[0].MarketName, "Wales|Arsenel", aryOdd, testDataLst[0].Stake, EWterms, false, "Double", 1);
        public void ValidateBetReceipt(ISelenium browser, string typeName, string eventName, string marketName, string selectionName, string[] oddsArr, string stake, string eachWay, bool SP, string multiBetType, int betNos)
        {
            try
            {
                int numLines;
                string actualString = "", eachWayTerm;
                double intialBalance, laterBalance, totalStake, individualStake;
                string odds, textToVerify, currDate, betReceiptContainer;

                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                BFframeworkCommonObj.PageSync(browser);
                // Getting the Intial Balance
                intialBalance = GetBalance(browser);

                Assert.IsTrue(browser.IsElementPresent(BetslipControls.betslipBanner), "Betslip page is not displayed");
                totalStake = GetTotalsFromBetslip(browser, "stake");
                //Getting acutal Number of lines
                numLines = CalculateBetLines(browser, multiBetType);

                //Place Bet
                //--Issue with clicking the button
                while (browser.IsVisible(BetslipControls.placeBet))
                {
                    driver.FindElement(By.XPath(BetslipControls.placeBet)).Click();
                    BFcommonObj.WaitForLoadingIcon(browser, FrameGlobals.IconLoadTimeout);
                }
                BFframeworkCommonObj.WaitUntilElementPresent(browser, BetslipControls.betReceiptBanner, FrameGlobals.ElementLoadTimeout.ToString());
                Thread.Sleep(2000);
                Assert.IsTrue(browser.IsElementPresent(BetslipControls.betSuccessfulMsg), "'Bets placed successfully' text was is not present in the Bet Receipt");
                Console.WriteLine("Bet Placement was successful");

                //Receipt container varies for betslip container
                if (String.IsNullOrEmpty(multiBetType) || multiBetType.ToLower().Contains("single"))
                {
                    betReceiptContainer = "//div[@class='bxv']//div[@class='bxv ffa fs15px']";
                }
                else
                {
                    betReceiptContainer = "//div[@class='bxcl pa10']//div[@class='bxv ffa fs15px']";
                }
                // Checking for Bet Receipt container
                Assert.IsTrue(browser.IsElementPresent(betReceiptContainer), "Bet Receipt container is not present");
                ReadOnlyCollection<IWebElement> element = driver.FindElements(By.XPath(betReceiptContainer));
                actualString = element[0].Text.ToLower();

                //Checking for BetTime                        
                DateTime dateTime = DateTime.Now.Date;//.UtcNow.Date;
                currDate = dateTime.ToString("dd/MM/yyyy");
                //textToVerify = "time:\r\n" + currDate + " - " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                textToVerify = "time:\r\n" + currDate + " - ";
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");

                //Checking for Bet Receipts No 
                textToVerify = "receipt no:\r\no/";
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");

                //Checking for Betlines
                // Temp work around for Eachway -- Ask sudhir to fix the Common function
                if (!string.IsNullOrEmpty(eachWay))
                {
                    numLines = numLines * 2;
                }
                if (numLines > 1)
                {
                    textToVerify = numLines + " lines at £ " + String.Format("{0:0.00}", double.Parse(stake)) + " per line";
                }
                else
                {
                    textToVerify = numLines + " line at £ " + String.Format("{0:0.00}", double.Parse(stake)) + " per line";
                }
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");

                //Checking for Stake for Each way                  
                if (!string.IsNullOrEmpty(eachWay))
                {
                    individualStake = Convert.ToDouble(stake, CultureInfo.CurrentCulture) * 2;
                }
                else
                {
                    individualStake = Convert.ToDouble(stake, CultureInfo.CurrentCulture) * numLines;
                }
                textToVerify = "total stake for this bet:\r\n£ " + String.Format("{0:0.00}", individualStake);
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + individualStake + "' text was NOT displyed in the Bet Receipt");

                // Getting Potential Return only if Each way is false
                double potentialReturn;
                if (!string.IsNullOrEmpty(eachWay))
                {
                    potentialReturn = GetPotentialReturnsForEW(oddsArr, eachWay, double.Parse(stake), multiBetType);
                }
                else
                {
                    potentialReturn = GetPotentialReturnForMultiples(oddsArr, double.Parse(stake), multiBetType);
                }
                textToVerify = "potential return:\r\n£ " + String.Format("{0:0.00}", potentialReturn);
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");


                if (String.IsNullOrEmpty(multiBetType) || multiBetType.ToLower().Contains("single"))
                {
                    //Checking for Event Details
                    // For HR and GH, Typename is displayed in the betslip
                    if (typeName != "")
                    {
                        textToVerify = multiBetType.ToLower() + " - " + eventName.ToLower() + " " + typeName.ToLower() + "\r\n" + selectionName.ToLower() + " - " + marketName.ToLower();
                    }
                    else
                    {
                        // code to handle conditions where event name contains "vs", xPath is different
                        if (eventName.ToLower().Contains("vs"))
                        {
                            eventName = eventName.Replace("vs", "-");
                        }

                        textToVerify = multiBetType.ToLower() + " - " + eventName.ToLower() + "\r\n" + selectionName.ToLower() + " - " + marketName.ToLower();
                        //Market doesn't appear for WDW market
                        if (string.IsNullOrEmpty(marketName))
                        {
                            textToVerify = multiBetType.ToLower() + " - " + eventName.ToLower() + "\r\n" + selectionName.ToLower();
                        }
                    }
                    Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");

                    // Checking for Each way
                    if (!string.IsNullOrEmpty(eachWay))
                    {
                        eachWayTerm = GetEachWayOdd(eachWay);
                        textToVerify = "each way " + eachWayTerm;
                        Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");
                    }

                    // Checking for SP
                    if (SP == true)
                    {
                        textToVerify = "sp";
                        Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");
                    }
                    else
                    //Checking for Odds 
                    {
                        odds = String.Format("{0:0.00}", double.Parse(oddsArr[0]));
                        textToVerify = "odds:\r\n" + odds;
                        Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");
                    }
                }
                else
                {
                    //Checking for BetType
                    textToVerify = multiBetType.ToLower();
                    Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");
                    string[] selArr = selectionName.Split('|');
                    string betTyeAbbreviation = null;
                    switch (multiBetType.ToLower())
                    {
                        case "double":
                            betTyeAbbreviation = "dbl";
                            break;
                        case "treble":
                            betTyeAbbreviation = "tbl";
                            break;
                        case "trixie":
                            betTyeAbbreviation = "trx";
                            break;
                        case "patent":
                            betTyeAbbreviation = "pat";
                            break;
                        case "accumulator (4)":
                            betTyeAbbreviation = "acc4";
                            break;
                        case "yankee":
                            betTyeAbbreviation = "yan";
                            break;
                        case "lucky 15":
                            betTyeAbbreviation = "l15";
                            break;
                        case "accumulator (5)":
                            betTyeAbbreviation = "acc5";
                            break;
                        case "canadian":
                            betTyeAbbreviation = "can";
                            break;
                        case "lucky 31":
                            betTyeAbbreviation = "l31";
                            break;
                    }
                    //Checking for selection Details
                    textToVerify = betTyeAbbreviation + "\r\n";
                    Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");

                    // Checking for Each way
                    string[] ewText = new string[selArr.Length];
                    if (!string.IsNullOrEmpty(eachWay))
                    {
                        string[] arrEW = eachWay.Split('|');
                        for (int i = 0; i < arrEW.Length; i++)
                        {
                            eachWayTerm = GetEachWayOdd(arrEW[i]);
                            ewText[i] = " , e/w terms: " + eachWayTerm;
                        }
                    }
                    //Verify the selections
                    for (int i = 0; i < selArr.Length; i++)
                    {
                        textToVerify = (i + 1) + ". " + selArr[i].ToLower() + ewText[i];
                        Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");
                    }
                }

                // Looking for Total stake
                string xPath = "//span[@class='rel t2' and contains(text(), 'Total stake: £ " + totalStake + "')]";
                Assert.IsTrue(browser.IsElementPresent(xPath), "'Total stake: £ " + totalStake + "' text was is not present in the Bet Receipt");

                xPath = "//div[@class='bxcl bg25 h35 ffdc fs15px c1 ttu lh39 pl10' and contains(text(), 'Your bets (" + betNos + ")')]";
                Assert.IsTrue(browser.IsElementPresent(xPath), "'Your bets (" + betNos + ")' text was is not present in the Bet Receipt");
                Console.WriteLine("Bet Receipt validation was successful");


                //Check Balance after bet placement
                laterBalance = GetBalance(browser);
                Assert.IsTrue((laterBalance == (intialBalance - totalStake)), "Balance Validation failed after Bet Placement");                                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'ValidateBetReceipt' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method fetches the EW odd
        /// <example>GetEachWayOdd("EachWay 1/5 1,2,3") </example>
        public string GetEachWayOdd(string EWTerm)
        {
            try
            {
                string[] EWTermArr;
                EWTermArr = EWTerm.Split(' ');
                return EWTermArr[2];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'GetEachWayOdd' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }



        ///<summary>
        /// This method switched the odd type between fractional and decimal 
        /// <example>VerifyBetslipInfo(MyBrowser, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, EWterms)</example>
        public void VerifyBetslipInfo(ISelenium browser, string eventName, string marketName, string selectionName, string odds, string EWterms)
        {
            try
            {
                string textToVerify, actualString, xPath;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                //click on the info button
                xPath = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selectionName + "')]/../../div[@class='bxc slip-info']";
                BFcommonObj.clickObject(browser, xPath);

                //Check for the info hearder
                xPath = "//div[@id='alert3-one' and contains(text(), '" + eventName + "')]";
                Assert.IsTrue(browser.IsVisible(xPath), "Info pop up for Event '" + eventName + "' was not displayed on clicking the Info button in betslip");

                // Checking for info container
                Assert.IsTrue(browser.IsElementPresent(BetslipControls.betInfoContainer), "Bet Info container is not present");
                ReadOnlyCollection<IWebElement> element = driver.FindElements(By.XPath(BetslipControls.betInfoContainer));
                actualString = element[0].Text.ToLower();
                Thread.Sleep(1000);

                //Checking for Event Details
                textToVerify = "event: " + eventName.ToLower();
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was not displyed in the Info Container");

                //Checking for Date Details
                textToVerify = "date:";
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was not displyed in the Info Container");

                //Checking for Selection
                textToVerify = "selection: " + selectionName.ToLower();
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was not displyed in the Info Container");

                //Checking for Market
                textToVerify = "market: " + marketName.ToLower();
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was not displyed in the Info Container");

                //check only if uers logged in
                if (driver.FindElement(By.Id("balance")).Displayed)
                {
                    //Checking for Min Stake Details
                    textToVerify = "min stake: £";
                    Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was not displyed in the Info Container(user logged in)");

                    //Checking for Max Stake Details
                    textToVerify = "max stake: £";
                    Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was not displyed in the Info Container(user logged in)");
                }
                else
                {
                    //Checking for Min Stake Details
                    textToVerify = "min stake: £";
                    Assert.IsTrue(!actualString.Contains(textToVerify), "'" + textToVerify + "' text is displyed in the Info Container(user logged out)");
                    //Checking for Max Stake Details
                    textToVerify = "max stake: £";
                    Assert.IsTrue(!actualString.Contains(textToVerify), "'" + textToVerify + "' text is displyed in the Info Container(user logged out)");
                }

                //Checking for Odds
                textToVerify = "odds: " + double.Parse(odds);
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was not displyed in the Info Container");

                //Checking for EW odds
                //textToVerify = "each way " + EWodd;
                textToVerify = EWterms.ToLower();
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was not displyed in the Info Container");

                BFcommonObj.clickObject(browser, BetslipControls.closeButtonInInfoContainer);
                Assert.IsFalse(browser.IsVisible(xPath), "Info pop up for Event '" + eventName + "' failed to close on clicking the Close button");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyBetslipInfo' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method gets the mix/max stake predefined value
        /// <example>GetMinMaxStakeValue(MyBrowser, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, EWterms)</example>
        public string GetMinMaxStakeValue(ISelenium browser, string eventName, string selectionName, string MinMaxReturn)
        {
            try
            {
                string actualString, xPath, minValue, maxValue;
                string returnVal = null;
                string[] stakeArr;
                string[] valArr;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                //click on the info button
                xPath = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selectionName + "')]/../../div[@class='bxc slip-info']";
                BFcommonObj.clickObject(browser, xPath);

                //Check for the info hearder
                xPath = "//div[@id='alert3-one' and contains(text(), '" + eventName + "')]";
                Assert.IsTrue(browser.IsVisible(xPath), "Info pop up for Event '" + eventName + "' was not displayed on clicking the Info button in betslip");

                // Checking for info container
                Assert.IsTrue(browser.IsElementPresent(BetslipControls.betInfoContainer), "Bet Info container is not present");
                ReadOnlyCollection<IWebElement> element = driver.FindElements(By.XPath(BetslipControls.betInfoContainer));
                actualString = element[0].Text.ToLower();
                Thread.Sleep(1000);

                //check only if uers is logged in
                if (driver.FindElement(By.Id("balance")).Displayed)
                {
                    stakeArr = actualString.Split('£');
                    //Fetch the Min/Max Value
                    if (MinMaxReturn.ToLower() == "min")
                    {
                        minValue = stakeArr[1];
                        valArr = minValue.Split('\r');
                        minValue = valArr[0];
                        returnVal = minValue.Trim();
                    }
                    else
                    {
                        maxValue = stakeArr[2];
                        valArr = maxValue.Split('\r');
                        maxValue = valArr[0];
                        returnVal = maxValue.Trim();
                    }
                }
                else
                {
                    Fail("Unable to fetch Min/Max value as user is not Logged in");
                }
                BFcommonObj.clickObject(browser, BetslipControls.closeButtonInInfoContainer);
                return returnVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'GetMinMaxStakeValue' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }



        ///<summary>
        /// This method captures the error message displayed in Betslip
        /// <example>CaptureBetslipErrorMessage(browser, xPath)</example>
        public string CaptureBetslipErrorMessage(ISelenium browser, string xPath)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                Assert.IsTrue(browser.IsElementPresent(xPath), "Element not present");
                return driver.FindElement(By.XPath(xPath)).Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'CaptureBetslipErrorMessage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }



        ///<summary>
        /// This method verifies the Bet details in betslip after entering the stake
        /// <example>VerifyBetDetails(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, arrOdd, testDataLst[0].Stake, EWterms, "Single")</example>
        public void VerifyBetDetails(ISelenium browser, string eventName, string selection, string marketName, string[] arrOdds, string stake, string EWterms, string multiBetType, string prevTotalStake, string prevTotalPR)
        {
            try
            {
                double expPotentialReturn, expTotalPotentialReturn, actTotalStake, actPotentialReturn, actTotalPotentialReturns, expTotalStake;
                int betLines;

                // Check Betdetails for EW Multiples
                if (!string.IsNullOrEmpty(EWterms) && !String.IsNullOrEmpty(multiBetType) && !multiBetType.ToLower().Contains("single"))
                {
                    VerifyBetDetailsEWmultiples(browser, stake, multiBetType, prevTotalStake);
                }
                else
                {
                    //Check if the Betslip page is displayed
                    Assert.IsTrue(browser.IsVisible(BetslipControls.betslipBanner), "Betslip page is not displayed");

                    //Get the potential returns displayed along the selection side in betslip
                    actPotentialReturn = GetPotentialReturnFromBetSlip(browser, eventName, selection, marketName, arrOdds[0], multiBetType);
                    //Get the total stake from betslip
                    actTotalStake = GetTotalsFromBetslip(browser, "stake");
                    //Get the total potential returns from betslip
                    actTotalPotentialReturns = GetTotalsFromBetslip(browser, "potential");


                    //Getting expected Number of lines
                    betLines = CalculateBetLines(browser, multiBetType);
                    //Get the expected potentials returns(based on EW)
                    if (!string.IsNullOrEmpty(EWterms))
                    {
                        expPotentialReturn = GetPotentialReturnsForEW(arrOdds, EWterms, double.Parse(stake), multiBetType);
                        betLines = betLines * 2;
                    }
                    else
                    {
                        expPotentialReturn = GetPotentialReturnForMultiples(arrOdds, double.Parse(stake), multiBetType);
                    }
                    //Get the expected total stake
                    expTotalStake = Convert.ToDouble(stake) * betLines;


                    //Add previous PR and Stake if passsed
                    if (!string.IsNullOrEmpty(prevTotalStake))
                    {
                        expTotalStake = Convert.ToDouble(prevTotalStake) + expTotalStake;
                    }
                    expTotalPotentialReturn = expPotentialReturn;
                    if (!string.IsNullOrEmpty(prevTotalPR))
                    {
                        expTotalPotentialReturn = Convert.ToDouble(prevTotalPR) + expPotentialReturn;
                    }
                    // Verify the Total Stake in betslip
                    Assert.IsTrue(actTotalStake == Math.Round(expTotalStake, 2), "Mismatch in Actual and Expected Total stake in Betslip. Expected-'" + expTotalStake + "', Actual-'" + actTotalStake + "'");
                    //Console.WriteLine("Total stake '" + actTotalStake + "' in Betslip was validated successfully");

                    // Verify the potential returns in betslip
                    Assert.IsTrue(actPotentialReturn == Math.Round(expPotentialReturn, 2), "Mismatch in Actual and Expected Potential Return in Betslip. Expected-'" + expPotentialReturn + "', Actual-'" + actPotentialReturn + "'");
                    //Console.WriteLine("Potential Returns '" + actPotentialReturn + "' in Betslip was validated successfully");

                    // Verify the total potential returns 
                    Assert.IsTrue(actTotalPotentialReturns == Math.Round(expTotalPotentialReturn, 2), "Mismatch in Actual and Expected Total Potential Return in Betslip . Expected-'" + expTotalPotentialReturn + "', Actual-'" + actTotalPotentialReturns + "'");
                    //Console.WriteLine("Total Potential Returns '" + expPotentialReturn + "' in Betslip was validated successfully");

                    Console.WriteLine("Bet details was verified successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyBetDetails' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }




        ///<summary>
        /// This method verifies the Bet details in betslip after entering the stake
        /// <example>VerifyBetDetailsEWmultiples(MyBrowser, "1", "Double", prevStake)
        public void VerifyBetDetailsEWmultiples(ISelenium browser, string stake, string multiBetType, string prevStake)
        {
            try
            {
                double actTotalStake, expTotalStake;
                string Xpath = null, expPotentialReturn, actPotentialReturn, actTotalPotentialReturns;
                int betLines;

                //Check if the Betslip page is displayed
                Assert.IsTrue(browser.IsVisible(BetslipControls.betslipBanner), "User is not taken to betslip page on Login");

                //Get the potential returns displayed along the selection side in betslip
                switch (multiBetType.ToLower())
                {
                    case "double":
                        Xpath = "//span[starts-with(@id, 'potential_return_DBL_')]";
                        break;

                    case "treble":
                        Xpath = "//span[starts-with(@id, 'potential_return_TBL_')]";
                        break;

                    case "trixie":
                        Xpath = "//span[starts-with(@id, 'potential_return_TRX_')]";
                        break;

                    case "patent":
                        Xpath = "//span[starts-with(@id, 'potential_return_PAT_')]";
                        break;

                    case "accumulator (4)":
                        Xpath = "//span[starts-with(@id, 'potential_return_ACC4_')]";
                        break;

                    case "yankee":
                        Xpath = "//span[starts-with(@id, 'potential_return_YAN_')]";
                        break;

                    case "lucky 15":
                        Xpath = "//span[starts-with(@id, 'potential_return_L15_')]";
                        break;

                    case "accumulator (5)":
                        Xpath = "//span[starts-with(@id, 'potential_return_ACC5_')]";
                        break;

                    case "canadian":
                        Xpath = "//span[starts-with(@id, 'potential_return_CAN_')]";
                        break;

                    case "lucky 31":
                        Xpath = "//span[starts-with(@id, 'potential_return_L31_')]";
                        break;
                }

                actPotentialReturn = browser.GetText(Xpath);
                //Get the total stake from betslip
                actTotalStake = GetTotalsFromBetslip(browser, "stake");
                //Get the total potential returns from betslip
                actTotalPotentialReturns = browser.GetText(BetslipControls.totalPotentialReturns);

                //Getting expected Number of lines
                betLines = CalculateBetLines(browser, multiBetType);
                //Get the expected total stake
                expTotalStake = Convert.ToDouble(stake) * betLines * 2;
                if (!string.IsNullOrEmpty(prevStake))
                {
                    expTotalStake = expTotalStake + Convert.ToDouble(prevStake);
                }

                // Verify the Total Stake in betslip
                Assert.IsTrue(actTotalStake == expTotalStake, "Mismatch in Actual and Expected Total stake in Betslip. Expected-'" + expTotalStake + "', Actual-'" + actTotalStake + "'");
                Console.WriteLine("Total stake '" + actTotalStake + "' in Betslip was validated successfully");

                // Verify the potential returns in betslip
                expPotentialReturn = "N/A";
                Assert.IsTrue(actPotentialReturn == expPotentialReturn, "Mismatch in Actual and Expected Potential Return in Betslip. Expected-'" + expPotentialReturn + "', Actual-'" + actPotentialReturn + "'");
                Console.WriteLine("Potential Returns '" + actPotentialReturn + "' in Betslip was validated successfully");

                // Verify the total potential returns 
                expPotentialReturn = "N/A";
                Assert.IsTrue(actTotalPotentialReturns == expPotentialReturn, "Mismatch in Actual and Expected Total Potential Return in Betslip . Expected-'" + expPotentialReturn + "', Actual-'" + actTotalPotentialReturns + "'");
                Console.WriteLine("Total Potential Returns '" + expPotentialReturn + "' in Betslip was validated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyBetDetailsEWmultiples' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method verifies all the details in the Bet Slip before entering the satke
        /// <example>VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].Odds, EWterms, "Single", 1)</example>
        /// <example>VerifyBetSlip(MyBrowser, testDataLst[0].EventName, testDataLst[0].SelectionName, testDataLst[0].Odds, No, "Single", 1)</example>
        public void VerifyBetSlip(ISelenium browser, string eventName, string selection, string marketName, string Odds, string EWterms, string multiBetType, int MultipleBetCount)
        {
            try
            {
                double expPotentialReturn = 0.0, actTotalStake, actPotentialReturn, expTotalStake = 0.0;
                int betLines;
                string multipleBetCount, ewXpath = null, stakeXpath = null, xPathMinus = null, xPathPlus = null, selID, actTotalPotentialReturns, multiBetDesc;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                Assert.IsTrue(browser.IsVisible(BetslipControls.betslipBanner), "Betslip page is not displayed");
                if (String.IsNullOrEmpty(multiBetType) || multiBetType.ToLower() == "single")
                {
                    //Check Input, quick stakes, EW
                    selID = GetSelectionId(browser, eventName, selection, marketName, Odds, multiBetType);
                    xPathPlus = "//input[@id='slip-odds-stake-SGL_" + selID + "']/following::div[@class='bxc stake-button' and text()='+']";
                    xPathMinus = "//input[@id='slip-odds-stake-SGL_" + selID + "']/../../div[@class='bxc stake-button' and text()='-']";
                    stakeXpath = "//input[@id='slip-odds-stake-SGL_" + selID + "']";
                    ewXpath = "//input[@id='price_updater_checkbox_SGL_" + selID + "']";
                }
                else
                {
                    switch (multiBetType.ToLower())
                    {
                        case "double":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-DBL_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-DBL_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-DBL_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-DBL_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "treble":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-TBL_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-TBL_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-TBL_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-TBL_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "trixie":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-TRX')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-TRX_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-TRX_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-TRX_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "patent":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-PAT')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-PAT_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-PAT_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-PAT_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "accumulator (4)":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-ACC4_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-ACC4_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-ACC4_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-ACC4_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "yankee":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-YAN_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-YAN_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-YAN_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-YAN_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "lucky 15":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-L15_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-L15_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-L15_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-L15_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "accumulator (5)":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-ACC5_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-ACC5_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-ACC5_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-ACC5_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "canadian":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-CAN_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-CAN_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-CAN_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-CAN_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;

                        case "lucky 31":
                            stakeXpath = "//input[starts-with(@id, 'slip-odds-stake-L31_')]";
                            ewXpath = "//input[starts-with(@id, 'slip-item-check-ew-L31_')]";
                            xPathPlus = "//input[starts-with(@id, 'slip-odds-stake-L31_')]/following::div[@class='bxc stake-button' and text()='+']";
                            xPathMinus = "//input[starts-with(@id, 'slip-odds-stake-L31_')]/../../div[@class='bxc stake-button' and text()='-']";
                            break;
                    }
                    //Getting expected Number of lines
                    betLines = CalculateBetLines(browser, multiBetType);
                    multiBetDesc = "//span[@class='b' and contains(text(), '" + multiBetType + "')]/following::span[contains(text(), '" + betLines + "')]";
                    Assert.IsTrue(browser.IsElementPresent(multiBetDesc), multiBetType + "-" + betLines + " was not displayed in the Betslip");

                    multipleBetCount = browser.GetText(BetslipControls.multiplesBanner);
                    Assert.IsTrue(multipleBetCount.Contains("Multiples (" + MultipleBetCount + ")"), "Multiples-" + MultipleBetCount + " was not displayed in the Betslip");
                }

                //Check for the elements existance
                Assert.IsTrue(driver.FindElement(By.XPath(stakeXpath)).Displayed, "Stake Input box for '" + multiBetType + "' (" + eventName + " - " + selection + " - " + Odds + ") is not present in the Betslip");
                Assert.IsTrue(driver.FindElement(By.XPath(xPathPlus)).Displayed, "Quick Stake (+) for '" + multiBetType + "' (" + eventName + " - " + selection + " - " + Odds + ") is not present in the Betslip");
                Assert.IsTrue(driver.FindElement(By.XPath(xPathMinus)).Displayed, "Quick Stake (-) for '" + multiBetType + "' (" + eventName + " - " + selection + " - " + Odds + ") is not present in the Betslip");
                if (!string.IsNullOrEmpty(EWterms))
                {
                    if (EWterms.ToUpper() == "NO")
                    {
                        TimeSpan ts = new TimeSpan(0, 0, 5);
                        driver.Manage().Timeouts().ImplicitlyWait(ts);
                        Assert.IsFalse(browser.IsElementPresent(ewXpath), "Each Way Check box for '" + multiBetType + "' is present in the Betslip");
                    }
                    else
                    {
                        Assert.IsTrue(driver.FindElement(By.XPath(ewXpath)).Displayed, "Each Way Check box for '" + multiBetType + "' is not present in the Betslip");
                    }
                }

                //Get the potential returns displayed along the selection side in betslip
                actPotentialReturn = GetPotentialReturnFromBetSlip(browser, eventName, selection, marketName, Odds, multiBetType);
                //Get the total stake from betslip
                actTotalStake = GetTotalsFromBetslip(browser, "stake");
                //Get the total potential returns from betslip
                actTotalPotentialReturns = browser.GetText(BetslipControls.totalPotentialReturns);


                // Verify the Total Stake in betslip
                Assert.IsTrue(actTotalStake == expTotalStake, "Mismatch in Actual and Expected Total stake in Betslip. Expected-'" + expTotalStake + "', Actual-'" + actTotalStake + "'");
                // Verify the potential returns in betslip
                Assert.IsTrue(actPotentialReturn == expPotentialReturn, "Mismatch in Actual and Expected Potential Return in Betslip. Expected-'" + expPotentialReturn + "', Actual-'" + actPotentialReturn + "'");
                // Verify the total potential returns 
                Assert.IsTrue(actTotalPotentialReturns.Contains("N/A"), "Mismatch in Actual and Expected Total Potential Return in Betslip . Expected-'N/A', Actual-'" + actTotalPotentialReturns + "'");

                //Verify the PR and total stake text
                Assert.IsTrue(browser.IsTextPresent("Total stake"), "'Total stake' text was not present in the betslip");
                Assert.IsTrue(browser.IsTextPresent("Total Potential Returns"), "'Total Potential Returns' text was not present in the betslip");

                Console.WriteLine("Betslip was verified successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyBetSlip' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        ///This method navigates to the specified event details page and adds and verifies a selction to Betslip(Horse Racing)
        /// <example>EWterms = AddAndVerifySelectionInBetslip_HR(MyBrowser, "", "", "Competition", testDataLst[0].ClassName, testDataLst[0].TypeName, testDataLst[0].SubTypeName, testDataLst[0].EventName, testDataLst[0].MarketName, testDataLst[0].SelectionName, testDataLst[0].Odds, false) </example>
        public string AddAndVerifySelectionInBetslip_HR(ISelenium browser, string sidebarLink, string navPanel, string className, string typeName, string subTypeName, string eventName, string marketName, string selection, string odds, bool SP, bool returnEW)
        {
            try
            {
                string returnVal = null;
                odds = String.Format("{0:0.00}", double.Parse(odds));
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                NavigateToEventDetailsPage_HR(browser, sidebarLink, navPanel, className, typeName, subTypeName, eventName, marketName);
                if (navPanel.ToLower() != "future & specials")
                {
                    returnVal = AddSelectionAndVerify_HR(browser, typeName, eventName, marketName, selection, odds, SP, returnEW);
                }
                else
                {
                    returnVal = AddSelectionToBetslip(browser, "", eventName, marketName, selection, odds, returnEW);
                    VerifySelectionInBetslip(browser, "", eventName, marketName, selection, odds);
                }


                Console.WriteLine("Event-Selection-Odd  '" + eventName + "-" + selection + "-" + odds + "' was added and verified successsfully to the betslip ");
                return returnVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'AddAndVerifySelectionInBetslip_HR' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }

        /// <summary>
        ///This method navigates to the specified event details page (Horse Racing)
        /// <example>NavigateToEventDetailsPage(MyBrowser, "", "Competition", Football", "Fa Cup", "EVentA", "Cup Winner")</example>
        public void NavigateToEventDetailsPage_HR(ISelenium browser, string sidebarLink, string navPanel, string className, string typeName, string subTypeName, string eventName, string marketName)
        {
            try
            {
                string xPath;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                NavigateToSportsPage(browser, sidebarLink, navPanel, className);
                switch (navPanel.ToLower())
                {
                    case "next races":
                        xPath = "//li/a/div[contains(text(), '" + eventName + "')]/following-sibling::div[contains(text(), '" + typeName + "')]/following-sibling::div[@class='bxcl mr5 arrow next-arrow']";
                        BFcommonObj.clickObject(browser, xPath);
                        break;

                    case "today":
                        xPath = "//div[@class='bxcl race-header ttu' and contains(text(), '" + typeName + "')]/following::nav[@class='bxc']/a/div[contains(text(), '" + eventName + "')]";
                        BFcommonObj.clickObject(browser, xPath);
                        break;

                    case "tomorrow":
                        xPath = "//div[@class='bxcl race-header ttu' and contains(text(), '" + typeName + "')]/following::nav[@class='bxc']/a/div[contains(text(), '" + eventName + "')]";
                        BFcommonObj.clickObject(browser, xPath);
                        break;

                    default:
                        //check if the market is not expanded and select it          
                        xPath = "//div[@class ='bxcl bxf ml5' and contains(text(), '" + marketName + "')]/..";
                        Assert.IsTrue(browser.IsElementPresent(xPath), marketName + " market is is present");
                        IWebElement market = driver.FindElement(By.XPath(xPath));
                        if (!bool.Parse(market.GetAttribute("expanded")))
                        {
                            market.Click();
                            Thread.Sleep(1000);
                        }
                        //Select the subTypeName name
                        xPath = "//div[@class ='bxcl bxf ml5' and contains(text(), '" + marketName + "')]/following::div[@class='expandable']//div[@class='bxcl bxf ml5']";
                        BFcommonObj.clickObjectInColl(browser, xPath, typeName);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'NavigateToEventDetailsPage_HR' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }

        /// <summary>
        ///This method adds and verifies a selction to betslip(Horse Racing)
        /// <example>AddSelectionAndVerify_HR(MyBrowser, "EVentA", "Sel1", "1.25", false, true) </example>
        public string AddSelectionAndVerify_HR(ISelenium browser, string typeName, string eventName, string marketName, string selection, string odds, bool SP, bool returnEW)
        {
            try
            {
                string xPath, returnVal = null, pgTitle, tabName;
                odds = String.Format("{0:0.00}", double.Parse(odds));
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                string[] splitMarket = marketName.Split(' ');
                tabName = splitMarket[0].ToUpper();   //  + "_" + splitMarket[1].ToUpper();
                //handling condition for W/O markets
                if (marketName.ToLower().Contains("without"))
                {
                    tabName = "WITHOUT";
                }

                //verify the page title
                pgTitle = "//div[@class='bxcv bxf ffa b fs13px']/div[contains(text(), '" + typeName + "')]/following-sibling::div[contains(text(), '" + eventName + "')]";
                Assert.IsTrue(browser.IsElementPresent(pgTitle), pgTitle + " element not found");

                // Get the Betslip Count
                int initBetslipCnt = GetBetslipCount(browser);

                //select the tab
                xPath = "//a[starts-with(@class, 'bxc bxf tab') and contains(@href, '" + tabName + "')]";
                BFcommonObj.clickObject(browser, xPath);
                // get the Each way term
                if (returnEW == true)
                {
                    xPath = "//div[@class='bxcl fs12px pa10 pbn b ffa']";
                    returnVal = browser.GetText(xPath);
                }

                //Add selection to Betslip                
                if (!string.IsNullOrEmpty(odds))
                {
                    xPath = "//div[@class='bxcl b' and contains(text(), '" + selection + "')]/../following-sibling::div[@class='bxcl']/a/div[contains(@id, '" + tabName + "')]/span[@class='odds-convert' and contains(text(), '" + odds + "')]";
                    BFcommonObj.clickObject(browser, xPath);
                }

                //Add SP to Betslip
                if (SP == true)
                {
                    xPath = "";
                    BFcommonObj.clickObject(browser, xPath);
                }

                //verify the selection added to betslip
                BFcommonObj.clickObject(browser, BetslipControls.betslipButton);
                //VerifySelectionInBetslip(browser, typeName, eventName, marketName, selection, odds);

                int laterBetslipCnt = GetBetslipCount(browser);
                Assert.IsTrue(initBetslipCnt + 1 == laterBetslipCnt, "Mismatch in Betslip count on adding a selction, Expected:" + initBetslipCnt + 1 + ", Actual:" + laterBetslipCnt + ".");
                return returnVal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'AddSelectionAndVerify_HR' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }

        //Verifies the selection is present in Betslip
        public void VerifySelectionInBetslip(ISelenium browser, string typeName, string eventName, string marketName, string selection, string odds)
        {
            try
            {
                string xPath;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                Assert.IsTrue(browser.IsElementPresent(BetslipControls.betslipBanner), "Betslip page is not displayed");

                // code to handle conditions where event name contains "vs", xPath is different
                if (eventName.ToLower().Contains("vs"))
                {
                    eventName = eventName.Replace("vs", "-");
                }

                //Type name is displayed for HR events
                if (typeName != "")
                {
                    xPath = "//div[@class='slip-item']//span[normalize-space(contains(text(), '" + eventName + " " + typeName + "'))]/following-sibling::span[contains(text(), '" + selection + "')]/following-sibling::span[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + marketName.ToLower() + "')]]";
                }
                else
                {
                    xPath = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selection + "')]/following-sibling::span[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + marketName.ToLower() + "')]]";
                }
                Assert.IsTrue(browser.IsElementPresent(xPath), "Event-Selection-Market '" + eventName + "-" + selection + "-" + marketName + "' was not found in the Betslip");

                // odds are displayed in a different format for HR events
                string OddSPxPath = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selection + "')]/following-sibling::span[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + marketName.ToLower() + "')]]/following::span[@class='bxcl select-container']";
                TimeSpan ts = new TimeSpan(0, 0, 5);
                driver.Manage().Timeouts().ImplicitlyWait(ts);
                if (browser.IsElementPresent(OddSPxPath))
                {
                    xPath = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selection + "')]/following-sibling::span[starts-with(text(), text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + marketName.ToLower() + "')])]/following::span[@class='bxcl select-container']/select/option[contains(text(), '" + odds + "')]";
                }
                else
                {
                    xPath = "//div[@class='slip-item']//span[contains(text(), '" + eventName + "')]/following-sibling::span[contains(text(), '" + selection + "')]/following-sibling::span[starts-with(text(), text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + marketName.ToLower() + "')])]/following::span[contains(text(), '" + odds + "')]";
                }
                Assert.IsTrue(browser.IsElementPresent(xPath), "Event-Selection-Market-Odd '" + eventName + "-" + selection + "-" + marketName + "-" + odds + "' was not found in the Betslip");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifySelectionInBetslip' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }


        //formats the number to 0.00
        public double FormatNumber(double number)
        {
            string returnVal = null, num = Convert.ToString(number);
            num = num.Trim();
            if (num.Contains('.'))
            {
                string[] digArr = num.Split('.');
                string digitsAfterDecimal = digArr[1];
                if (digArr[1].Length > 2)
                {
                    digitsAfterDecimal = digArr[1].Substring(0, 2);
                }
                returnVal = digArr[0] + "." + digitsAfterDecimal;
            }
            else
            {
                returnVal = num;
            }
            return Convert.ToDouble(returnVal);
        }

        //verifies a perticular text is present in the betslip
        public void VerifySelectionDetailsInBetslip(ISelenium browser, string textToVerify, string multiBetType)
        {
            try
            {
                string betReceiptContainer, actualString;
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                //Receipt container varies for betslip container
                if (String.IsNullOrEmpty(multiBetType) || multiBetType.ToLower().Contains("single"))
                {
                    betReceiptContainer = "//div[@class='bxv']//div[@class='bxv ffa fs15px']";
                }
                else
                {
                    betReceiptContainer = "//div[@class='bxcl pa10']//div[@class='bxv ffa fs15px']";
                }
                // Checking for Bet Receipt container
                Assert.IsTrue(browser.IsElementPresent(betReceiptContainer), "Bet Receipt container is not present");
                ReadOnlyCollection<IWebElement> element = driver.FindElements(By.XPath(betReceiptContainer));
                actualString = element[0].Text.ToLower();
                Thread.Sleep(2000);

                //Verify the text is present in Betslip
                Assert.IsTrue(actualString.Contains(textToVerify), "'" + textToVerify + "' text was NOT displyed in the Bet Receipt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifySelectionDetailsInBetslip' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }

        /// <summary>
        /// Method to add SP selection to betslip
        /// </summary>
        public string AddSPSelectionToBetslip(ISelenium browser, string sidebarLink, string navPanel, string className, string typeName, string subTypeName, string eventName, string marketName, string selection, string odds, bool SP, bool returnEW)
        {
            string returnVal = null;
            string xPath;
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

            NavigateToEventDetailsPage_HR(browser, sidebarLink, navPanel, className, typeName, subTypeName, eventName, marketName);
            // code to handle conditions where event name contains "vs", xPath is different
            if (eventName.ToLower().Contains("vs"))
            {
                eventName = eventName.Replace("vs", "-");
            }

            // get the Each way term
            if (returnEW == true)
            {
                xPath = "//span[@class='t7 page-title' and contains(text(), '" + eventName + "')]/following::span[@class='ew-terms']";
                returnVal = browser.GetText(xPath);
            }

            // Get the Betslip Count
            int initBetslipCnt = GetBetslipCount(browser);

            //Add selection to Betslip
            xPath = "//div[@class='bxcl bg2 mb4' and contains(string(),'" + selection + "')]//a[@selectionpricetype='STARTING_PRICE']";
            BFcommonObj.clickObject(browser, xPath);

            //verify the selection added to betslip   
            //BFcommonObj.clickObject(browser, BetslipControls.betslipButton);
            //VerifySelectionInBetslip(browser, "", eventName, marketName, selection, odds, false);

            BFcommonObj.clickObject(browser, BetslipControls.betslipButton);
            int laterBetslipCnt = GetBetslipCount(browser);
            Assert.IsTrue(initBetslipCnt + 1 == laterBetslipCnt, "Mismatch in Betslip count on adding a selction, Expected:" + initBetslipCnt + 1 + ", Actual:" + laterBetslipCnt + ".");

            return returnVal;
        }

        public string GetReturnsFromBetslipForSP(ISelenium browser, string stakeOrReturn)
        {
            string[] itemArr = null;
            string val;
            if (stakeOrReturn.ToLower().Contains("stake"))
            {
                // Return Total Stake
                Assert.IsTrue(browser.IsElementPresent(BetslipControls.totalStake), "Total Stake element is not present in the Betslip");
                val = browser.GetText(BetslipControls.totalStake);
            }
            else
            {
                // Return potential return
                Assert.IsTrue(browser.IsElementPresent(BetslipControls.totalPotentialReturns), "Total Potential Returns element is not present in the Betslip");
                val = browser.GetText(BetslipControls.totalPotentialReturns);
                if (val.Contains("£"))
                {
                    itemArr = val.Split('£');
                    val = Convert.ToDouble(itemArr[itemArr.Length - 1], CultureInfo.CurrentCulture).ToString();
                }
            }

            return val;
        }


    }//end class
}//end namespace
