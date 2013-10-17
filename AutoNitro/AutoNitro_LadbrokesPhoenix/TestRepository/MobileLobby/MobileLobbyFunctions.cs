using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Selenium;
using OpenQA.Selenium;
using Framework;
using TestRepository.ControlsRepository;


namespace TestRepository.MobileLobby
{
    public class MobileLobbyFunctions : BaseTest
    {
        TestRepository.Common MLcommonObj = new TestRepository.Common();
        Framework.Common.Common MLframeworkCommonObj = new Framework.Common.Common();




        /// <summary>
        /// This method will wait for loader icon to complete loading
        /// <param name="browserObj">Browser Instance</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <example>WaitForLoadingIcon(browser,60)</example>      
        public void WaitForLoadingIcon_MobileLobby(ISelenium browserObj, int timeout)
        {
            MLframeworkCommonObj.PageSync(browserObj);
            DateTime now;

            DateTime delay = DateTime.Now.AddSeconds(timeout);
            while (browserObj.IsVisible(MobileLobbyControls.lbLoadingIcon))
            {
                now = DateTime.Now;
                if (now < delay)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            MLframeworkCommonObj.PageSync(browserObj);
        }





        ///<summary>
        /// This method navigates to the Registration page
        /// <example>NavigateToRegistrationPage(browser) </example>
        public void NavigateToRegistrationPage(ISelenium browser)
        {
            try
            {
                MLcommonObj.selectMenuButton(browser);
                MLcommonObj.clickObject(browser, LoginLogoutControls.loginOrRegisterLink);

                // Check for the Login page elements
                MLframeworkCommonObj.WaitUntilElementPresent(browser, LoginLogoutControls.registerButton, FrameGlobals.ElementLoadTimeout.ToString());
                clickObject_MobileLobby(browser, LoginLogoutControls.registerButton);
                // check for the floowing elements
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.Logo), "Ladbrokes Logo was not found on the Registration page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.registrationTitle), "Failed to navigate to Registration page, Registration title not found");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.closebutton), "Close button was found on the Registration page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.licenseText), "Lisence text was not found on the Registration page");
                Console.WriteLine("Successfully navigated to Lobby registration page");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(browser, "");
                Console.WriteLine("Function 'NavigateToRegistrationPage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method registers a new customer
        /// <example>RegisterCustomer(browser) </example>
        public string RegisterCustomer(ISelenium browser, string promocode, string country, string accountCurrency, string DOByear)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                TimeSpan ts = new TimeSpan(0, 0, 5);
                driver.Manage().Timeouts().ImplicitlyWait(ts);
                Random rnd = new Random();
                int rndNumber = rnd.Next(10000);
                string regMsg, xPath;
                string[] fName = new string[] { "Dylan", "Ethan", "George", "Hary", "Jacob", "John" };
                string[] lName = new string[] { "Brown", "Jones", "Miller", "Roberts", "Taylor", "Wilson" };
                string firstname = "Auto" + fName[rnd.Next(0, 5)];
                string lastname = "Auto" + lName[rnd.Next(0, 5)];
                string username = "AutoUser" + rndNumber;
                EnterRegisterDetails(browser, promocode, country, accountCurrency, DOByear, firstname, lastname, username);

                //Validate registration
                clickObject_MobileLobby(browser, MobileLobbyControls.registerNow);

                // for banned country
                if (country.ToLower() == "united states")
                {
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.failureRgMsg), "Registration failure message was not displayed");
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.registrationTitle), "Registration page title was not found in the header on registration status");
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.Logo), "Ladbrokes Logo was not displayed on registration status");
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.backbutton), "Back button was not displayed on registration status");

                    regMsg = "We are sorry but your country of residence is currently prohibited from using the Ladbrokes service.";
                    xPath = "//ul[@class='error_align']/li[contains(text()[2], '" + regMsg + "')]";
                    Assert.IsTrue(browser.IsElementPresent(xPath), "Registration failure message was not displayed to the user");
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.contactMessage), "Customer contact message was not displayed on failing to create a  customer from banned country");
                    Console.WriteLine("Customer was not registered from a banned country");
                }
                // for below 18 years customers
                else if ((DateTime.Now.Year - int.Parse(DOByear)) < 18)
                {
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.registrationTitle), "Registration Page is not displayed");
                    xPath = "//div[@class='monthformError parentFormundefined formError']/div[@class='formErrorContent' and contains(text(), 'You are under 18')]";
                    Assert.IsTrue(browser.IsElementPresent(xPath), "'You are under 18' error message was not displayed");
                    Console.WriteLine("Error message validated successfully for Customer of age below 18 attaempting to register");
                }
                // for regular customers
                else
                {
                    string balanceCurrXpath;
                    if (country.ToLower() == "united kingdom")
                    {
                        balanceCurrXpath = "//div[@class='samount' and contains(text(), 'Balance: £')]/span[@id='headerBalance' and contains(text(), '0.0')]";
                        regMsg = "Thanks " + firstname + ", your account has now been set up. Your customer ID is " + username + ". You have been sent an email with your details. To get going simply deposit a minimum of £ 5.00 and start betting.";
                    }
                    else
                    {
                        balanceCurrXpath = "//div[@class='samount' and contains(text(), 'Balance: $')]/span[@id='headerBalance' and contains(text(), '0.0')]";
                        regMsg = "Thanks " + firstname + ", your account has now been set up. Your customer ID is " + username + ". You have been sent an email with your details. To get going simply deposit a minimum of $ 5.00 and start betting.";
                    }
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.successfulRgMsg), "Successful Registration message was not displayed, failed to register a new customer '" + username + "'");
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.registrationTitle), "Registration page title was not found in the header after registration");
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.closebutton), "Done button was not displayed after registartion");
                    Assert.IsTrue(browser.IsVisible(balanceCurrXpath), "Balance header was not displayed after registartion");
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.depositFunds), "Deposit funds button was not found after registartion");


                    xPath = "//div[@id='mainContent']/p[contains(text(), '" + regMsg + "')]";
                    Assert.IsTrue(browser.IsElementPresent(xPath), "Registrain messages was not displayed to the user");

                    Console.WriteLine("Customer '" + username + "' registered successfully");
                }
                return username;
            }
            catch (Exception ex)
            {
                CaptureScreenshot(browser, "RegisterCustomer");
                Console.WriteLine("Function 'RegisterCustomer' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }



        ///<summary>
        /// This method enters all details of a customer
        /// <example>EnterRegisterDetails(browser) </example>
        public string EnterRegisterDetails(ISelenium browser, string promocode, string country, string accountCurrency, string DOByear, string firstname, string lastname, string username)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                Random rnd = new Random();
                int rndNumber = rnd.Next(10000);
                string[] pCode = new string[] { "HA2 9SR", "HA2 9SG", "HA2 9SE", "HA2 9SN", "HA2 9SW", "HA2 9SX", "HA2 8SS", "HA2 8SE", "HA2 8SX", "HA2 8SN", "HA2 8SA" };

                string title = "Mr";
                string gender = "male";
                string DOBmonth = "January";
                string DOBday = rnd.Next(20, 30).ToString();

                string houseno = rndNumber.ToString();
                string postcode = pCode[rnd.Next(0, 10)];;
                string address1 = "Ladbrokes Ltd, Imperial Drive";
                string address2 = "Harrow";
                string city = "Middx";
                string email = firstname + lastname + "@gmail.com";
                string teleCode = "+44";
                string telnumber = "1234567890";
                string mobnumber = "1234512345";

                string password = "12345678";
                string confirmPassword = "12345678";
                string securityQuestion = "Favourite Colour";
                string securityAnswer = "Blue";

                WaitForLoadingIcon_MobileLobby(browser, FrameGlobals.IconLoadTimeout);
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.registrationTitle), "Registration Page is not displayed");

                //Enter data in all the fields
                browser.Type(MobileLobbyControls.promocode, promocode);
                browser.Select(MobileLobbyControls.title, title);
                browser.Type(MobileLobbyControls.firstname, firstname);
                browser.Type(MobileLobbyControls.lastname, lastname);
                //gender
                if (gender.ToLower().Trim() == "male")
                {
                    browser.Click(MobileLobbyControls.genderMale);
                }
                else
                {
                    browser.Click(MobileLobbyControls.genderFemale);
                }

                browser.Select(MobileLobbyControls.DOBday, DOBday);
                browser.Select(MobileLobbyControls.DOBmonth, DOBmonth);
                browser.Select(MobileLobbyControls.DOByear, DOByear);
                if ((DateTime.Now.Year - int.Parse(DOByear)) < 18)
                {
                    Assert.IsTrue(browser.IsVisible(MobileLobbyControls.registrationTitle), "Registration Page is not displayed");
                    string xPath = "//div[@class='monthformError parentFormundefined formError']/div[@class='formErrorContent' and contains(text(), 'You are under 18')]";
                    Assert.IsTrue(browser.IsElementPresent(xPath), "'You are under 18' error message was not displayed");
                }
                
                browser.Select(MobileLobbyControls.country, country);
                browser.Type(MobileLobbyControls.housename, houseno);
                browser.Type(MobileLobbyControls.postcode, postcode);
                if (country.ToLower() == "united kingdom")
                {
                    clickObject_MobileLobby(browser, MobileLobbyControls.findaddress);
                }
                browser.Type(MobileLobbyControls.address1, address1);
                browser.Type(MobileLobbyControls.address2, address2);
                browser.Type(MobileLobbyControls.city, city);
                browser.Type(MobileLobbyControls.email, email);

                browser.Type(MobileLobbyControls.telintcode, teleCode);
                browser.Type(MobileLobbyControls.telnumber, telnumber);
                browser.Type(MobileLobbyControls.mobintcode, teleCode);
                browser.Type(MobileLobbyControls.mobnumber, mobnumber);
                browser.Select(MobileLobbyControls.accountCurrency, accountCurrency);

                browser.Type(MobileLobbyControls.username, username);
                browser.Type(MobileLobbyControls.password, password);
                browser.Type(MobileLobbyControls.confirmPassword, confirmPassword);
                browser.Select(MobileLobbyControls.securityQuestion, securityQuestion);
                browser.Type(MobileLobbyControls.securityAnswer, securityAnswer);
                MLcommonObj.SelectCheckbox(browser, MobileLobbyControls.contactMe, "on");
                MLcommonObj.SelectCheckbox(browser, MobileLobbyControls.aggreement, "on");
                Thread.Sleep(1000);

                return username;
            }
            catch (Exception ex)
            {
                CaptureScreenshot(browser, "EnterRegisterDetails");
                Console.WriteLine("Function 'EnterRegisterDetails' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
                return null;
            }
        }



        ///<summary>
        /// This method clicks on any desired object(button, link)
        /// <example>clickObject_MobileLobby(browser, xPath)</example>        
        public void clickObject_MobileLobby(ISelenium browserObj, string strLocator)
        {
            try
            {
                Assert.IsTrue(browserObj.IsVisible(strLocator), strLocator + " element is not present");
                browserObj.Focus(strLocator);
                browserObj.Click(strLocator);
                WaitForLoadingIcon_MobileLobby(browserObj, Convert.ToInt32(FrameGlobals.IconLoadTimeout));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'clickObject' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }


        //This method verifies the UI on deposit page
        public void VerifyDepositPage(ISelenium browser)
        {
            try
            {
                clickObject_MobileLobby(browser, MobileLobbyControls.depositFunds);
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.depositTitle), "Deposit page not found");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.closebutton), "Done button was not displayed after registartion");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.balanceHeader), "Balance header was not displayed after registartion");

                // verify UI of deposit page
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.cardTypeImages), "Card images was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.cardHolderName), "Card holder's name field was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.cardNo), "Card number field was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.expiryMonth), "Card expiry month listbox was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.cvvNo), "Card expiry year listbox was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.cardPassword), "Card password field was not displayed in the deposit page");

                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.amout1), "Amount field was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.amout2), "pens field was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.depositText), "Deposit static text was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.currencySymbol), "Currency symbol was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.dailyLimit), "Daily deposit limit radio was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.weeklyLimit), "Weekly deposit limit radio was not displayed in the deposit page");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.depositLimitAmount), "Deposit amout listbox was not displayed in the deposit page");

                Console.WriteLine("UI of Deposit page was verified successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'VerifyDepositPage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method creates a customer with the same details of a self exclusion customer
        /// <example>RegisterCustomer_selfExclusion(portalbrowser, adminbrowser) </example>
        public void RegisterCustomer_selfExclusion(ISelenium browser, ISelenium adminBrowser)
        {
            try
            {
                string regMsg, gender, xPath;
                var admincommonObj = new AdminSuite.Common();
                Random rnd = new Random();
                int rndNumber = rnd.Next(10000);

                //get details of customer in OB
                adminBrowser.WindowFocus();
                string username = "AutoUser" + rndNumber;
                admincommonObj.SelectMainFrame(adminBrowser);
                string firstname = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), 'First Name:')]/following-sibling::td");
                string lastname = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), 'Last Name:')]/following-sibling::td");
                string title = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), 'Title:')]/following-sibling::td");
                if (title.ToLower() == "mr")
                {
                    gender = "male";
                }
                else
                {
                    gender = "female";
                }

                string dob = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), 'Date of Birth:')]/following-sibling::td");
                string[] arr = dob.Split('-');
                string DOByear = arr[0];
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string DOBmonth = mfi.GetMonthName(int.Parse(arr[1])).ToString();
                string DOBday = arr[2];
                
                string houseno = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), 'Address (1)')]/following-sibling::td");
                string postcode = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), 'Postcode:')]/following-sibling::td");

                string address1 = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), '(2)')]/following-sibling::td") + adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), '(3)')]/following-sibling::td");
                string address2 = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), '(3)')]/following-sibling::td");
                string city = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), 'City:')]/following-sibling::td");
                string email = adminBrowser.GetText("//tr/td[@class='caption' and contains(text(), 'Email:')]/following-sibling::td");
                string teleCode = "+44";
                string telnumber = "1234567890";
                string mobnumber = "1234512345";

                string password = "12345678";
                string confirmPassword = "12345678";
                string securityQuestion = "Favourite Colour";
                string securityAnswer = "Blue";

                string accountCurrency = "UK Pound Sterling";
                string country = "United Kingdom";

                browser.WindowFocus();
                WaitForLoadingIcon_MobileLobby(browser, FrameGlobals.IconLoadTimeout);
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.registrationTitle), "Registration Page is not displayed");

                //Enter data in all the fields
                browser.Type(MobileLobbyControls.promocode, "");
                browser.Select(MobileLobbyControls.title, title);
                browser.Type(MobileLobbyControls.firstname, firstname);
                browser.Type(MobileLobbyControls.lastname, lastname);
                //gender
                if (gender.ToLower().Trim() == "male")
                {
                    browser.Click(MobileLobbyControls.genderMale);
                }
                else
                {
                    browser.Click(MobileLobbyControls.genderFemale);
                }

                browser.Select(MobileLobbyControls.DOBday, DOBday);
                browser.Select(MobileLobbyControls.DOBmonth, DOBmonth);
                browser.Select(MobileLobbyControls.DOByear, DOByear);
                browser.Select(MobileLobbyControls.country, country);

                browser.Type(MobileLobbyControls.housename, houseno);
                browser.Type(MobileLobbyControls.postcode, postcode);
                browser.Type(MobileLobbyControls.address1, address1);
                browser.Type(MobileLobbyControls.address2, address2);
                browser.Type(MobileLobbyControls.city, city);
                browser.Type(MobileLobbyControls.email, email);

                browser.Type(MobileLobbyControls.telintcode, teleCode);
                browser.Type(MobileLobbyControls.telnumber, telnumber);
                browser.Type(MobileLobbyControls.mobintcode, teleCode);
                browser.Type(MobileLobbyControls.mobnumber, mobnumber);
                browser.Select(MobileLobbyControls.accountCurrency, accountCurrency);

                browser.Type(MobileLobbyControls.username, username);
                browser.Type(MobileLobbyControls.password, password);
                browser.Type(MobileLobbyControls.confirmPassword, confirmPassword);
                browser.Select(MobileLobbyControls.securityQuestion, securityQuestion);
                browser.Type(MobileLobbyControls.securityAnswer, securityAnswer);
                MLcommonObj.SelectCheckbox(browser, MobileLobbyControls.contactMe, "on");
                MLcommonObj.SelectCheckbox(browser, MobileLobbyControls.aggreement, "on");
                Thread.Sleep(1000);

                //Validate registration
                clickObject_MobileLobby(browser, MobileLobbyControls.registerNow);
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.registrationTitle), "Registration page title was not found in the header after registration");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.failureRgMsg), "Registration failure message was not displayed");

                regMsg = "We are sorry but your country of residence is currently prohibited from using the Ladbrokes service.";
                xPath = "//ul[@class='error_align']/li[contains(text()[2], '" + regMsg + "')]";
                Assert.IsTrue(browser.IsElementPresent(xPath), "Registration failure message was not displayed to the user");
                Assert.IsTrue(browser.IsVisible(MobileLobbyControls.contactMessage), "Customer contact message was not displayed on failing to create a  customer from banned country");
                Console.WriteLine("Customer was not registered as his details provided matched a self excluded customer");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(browser, "EnterRegisterDetails");
                Console.WriteLine("Function 'EnterRegisterDetails' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }


    }//end class
}//end namespace