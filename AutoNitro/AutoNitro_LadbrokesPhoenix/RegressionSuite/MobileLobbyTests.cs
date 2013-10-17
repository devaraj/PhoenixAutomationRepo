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
    public class MobileLobbyTests : BaseTest
    {
        TestRepository.LoginLogout.LoginLogoutFunctions MLloginLogoutObj = new TestRepository.LoginLogout.LoginLogoutFunctions();
        TestRepository.MobileLobby.MobileLobbyFunctions MLmobilelobbyObj = new TestRepository.MobileLobby.MobileLobbyFunctions();
        TestRepository.Common MLcommonObj = new TestRepository.Common();
        Framework.Common.Common MLframeworkCommonObj = new Framework.Common.Common();
        AdminSuite.Common admincommonObj = new AdminSuite.Common();


        [Test]
        public void ValidateRegistration_UKCustomer()
        {
            Console.WriteLine("***** Executing Test Case --- 'ValidateRegistration_UKCustomer', To validate that UK customer can be registered *****");                
            try
            {
                MLcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                MLmobilelobbyObj.NavigateToRegistrationPage(MyBrowser);
                MLmobilelobbyObj.RegisterCustomer(MyBrowser, "", "United Kingdom", "UK Pound Sterling", "1975");

                //Check if the user can access deposit page on registration
                MLmobilelobbyObj.VerifyDepositPage(MyBrowser);
                Console.WriteLine("TestCase 'ValidateRegistration_UKCustomer' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateRegistration_UKCustomer");
                Console.WriteLine("TestCase 'ValidateRegistration_UKCustomer' - FAIL");
                Fail(ex.Message);
            }
        }



        [Test]
        public void ValidateRegistration_NoNUKCustomer()
        {
            Console.WriteLine("***** Executing Test Case --- 'ValidateRegistration_NoNUKCustomer', To validate that NoN UK customer can be registered *****");                
            try
            {
                MLcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                MLmobilelobbyObj.NavigateToRegistrationPage(MyBrowser);
                MLmobilelobbyObj.RegisterCustomer(MyBrowser, "", "Canada", "Canadian Dollars", "1975");

                // click if the user is logged in after the is registered
                MLcommonObj.clickObject(MyBrowser, MobileLobbyControls.closebutton);
                string xPath = "//div[@class='balance' and contains(text(), 'Balance:')]/span[@id='headerBalance' and contains(text(), '$0.00')]";
                Assert.IsTrue(MyBrowser.IsVisible(xPath), "User is not logged in on Registration(Balance element not found)");
                MLcommonObj.SelectLinksFromSideBar(MyBrowser, "Football", "Football");
                Assert.IsTrue(MyBrowser.IsVisible(xPath), "Balance not displayed on navigating to Football page");
                Console.WriteLine("User remains logged in on registration");
                Console.WriteLine("TestCase 'ValidateRegistration_NoNUKCustomer' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateRegistration_NoNUKCustomer");
                Console.WriteLine("TestCase 'ValidateRegistration_NoNUKCustomer' - FAIL");
                Fail(ex.Message);
            }
        }



        [Test]
        public void ValidateRegistration_BannedCountry()
        {
            Console.WriteLine("***** Executing Test Case --- 'ValidateRegistration_BannedCountry', To validate customers in not allowed to register from a Banned country *****");        
            try
            {
                MLcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                MLmobilelobbyObj.NavigateToRegistrationPage(MyBrowser);
                MLmobilelobbyObj.RegisterCustomer(MyBrowser, "", "United States", "United States Dollars", "1975");
                Console.WriteLine("TestCase 'ValidateRegistration_BannedCountry' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateRegistration_BannedCountry");
                Console.WriteLine("TestCase 'ValidateRegistration_BannedCountry' - FAIL");
                Fail(ex.Message);
            }
        }




        [Test]
        public void ValidateRegistration_BelowAge18()
        {
            Console.WriteLine("***** Executing Test Case --- 'ValidateRegistration_BelowAge18', To validate customers of below age 18 is not allowed to register *****");
            try
            {                
                MLcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                MLmobilelobbyObj.NavigateToRegistrationPage(MyBrowser);
                MLmobilelobbyObj.RegisterCustomer(MyBrowser, "", "United Kingdom", "UK Pound Sterling", "2010");
                Console.WriteLine("TestCase 'ValidateRegistration_BannedCountry' - PASS");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateRegistration_BelowAge18");
                Console.WriteLine("TestCase 'ValidateRegistration_BelowAge18' - FAIL");
                Fail(ex.Message);
            }
        }




        [Test]
        public void ValidateRegistration_SelfExclusion()
        {            
            ISelenium adminBrowser = null;
            DataTable dt = XlsReader.LoadExcelData(FrameGlobals.TestDataPath, "Users");
            string password, username;
            bool bStatus;
            username = dt.Rows[7]["UserName"].ToString();
            password = dt.Rows[7]["Password"].ToString();
            Console.WriteLine("***** Executing Test Case --- 'ValidateRegistration_SelfExclusion', To validate account suspension of a user registering with the same details as an existing self-excluded user account *****");            
            try
            {
                //self Excl the customer in OB
                adminBrowser = admincommonObj.LogOnToAdmin();
                adminBrowser.WindowFocus();
                bStatus = admincommonObj.SelfExcludedCustomer(adminBrowser, username);
                if (bStatus == true)
                {
                    Console.WriteLine("Customer Self excluded in OB");
                    MLcommonObj.WaitForLoadingIcon(MyBrowser, FrameGlobals.IconLoadTimeout);
                    MLmobilelobbyObj.NavigateToRegistrationPage(MyBrowser);
                    MLmobilelobbyObj.RegisterCustomer_selfExclusion(MyBrowser, adminBrowser);
                }
                else
                {
                    Fail("Failed to Self Exclude the customer '" + username + "'");
                }
                Console.WriteLine("TestCase 'VerifyLoginErrorMessage_SelfExclusion' - Pass");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(MyBrowser, "ValidateRegistration_SelfExclusion");
                Console.WriteLine("TestCase 'ValidateRegistration_SelfExclusion' - FAIL");
                Fail(ex.Message);
            }
            finally
            {
                //Release Self Exclussion
                admincommonObj.ReleaseSelfExcludedUser(adminBrowser, username);
                admincommonObj.UpdateCustomerStatus(adminBrowser, username, "Active", "-- unset --", "-- unset --");
                MLcommonObj.KillAdminObject();
            }
        }



    }//end class
}//end namespace

