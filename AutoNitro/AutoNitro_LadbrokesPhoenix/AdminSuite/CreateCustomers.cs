using System;
using System.Globalization;
using MbUnit.Framework;
using Selenium;


namespace AdminSuite.CustomerCreation
{
    public class CreateCustomers
    {
        readonly Framework.Common.Common _frameworkCommon = new Framework.Common.Common();
        private const string StrPasswrd = "12345678";
        bool _isUpdateCustomersResourceFile = false;
        readonly Common _common = new Common();
        AdminBase _browser = new AdminBase();

        /// <summary>
        /// To create a Self Excluded customer
        /// </summary>
        ///  Author: Pradeep
        /// <param name="myBrowser">Selenium Browser</param>
        /// <returns>None</returns>
        /// Created Date: 21-Dec-2011
        /// Modified By: 
        /// Modified Date: 
        /// Modification Comments:
        public void SelfExcludedCustomer(ISelenium myBrowser)
        {
            _isUpdateCustomersResourceFile = false;
            string slfExclCust = "ATEcommATSlfExcl";
            string guidslf = Guid.NewGuid().ToString().Substring(0, 4);
            slfExclCust = slfExclCust + guidslf;

            string strUserCreated = _common.CreateCustomer(myBrowser, slfExclCust, StrPasswrd);
            if (strUserCreated != "Fail")
            {
                string day = DateTime.Today.Day.ToString(CultureInfo.InvariantCulture);
                string month = DateTime.Today.Month.ToString(CultureInfo.InvariantCulture);
                string year = DateTime.Today.Year.ToString(CultureInfo.InvariantCulture);

                // Getting DDMMYYYY format
                if (day.Length == 1)
                {
                    day = "0" + day;
                }
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                // Getting YYYY-MM-DD format
                string strExclusionDate = year + "-" + month + "-" + day;


                _common.SearchCustomer(strUserCreated, myBrowser);
                // Clciking on SelfExcluded user
                // myBrowser.Click("//input[@name='exclusion_period' and @value='2Y']");
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.CustomerExcluPeriodLstBx);
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.CustomerExcluDateTxtBx, strExclusionDate);
                // myBrowser.Type("name=exclusion_date", "2011-12-21");
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.CustomerSelfExlusionBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(myBrowser);
                System.Threading.Thread.Sleep(10000);
                Assert.IsTrue(myBrowser.IsTextPresent("New Self Exclusion Added"), "Self Exclustion is unsuccessfull for this customer");
                _isUpdateCustomersResourceFile = _common.UpdateCustomerResouceFile("SelfExcludedCustomer", strUserCreated, StrPasswrd);
                if (_isUpdateCustomersResourceFile == false)
                {
                    Framework.BaseTest.Fail("Customers Resource File updation failed.");
                }
            }
            else
            {
                Framework.BaseTest.Fail("User creation Failed");
            }
        }

        ///<summary>
        /// To add a customer belonging to a banned country
        /// Author: Yogesh
        /// Ex:Customer_CompanyExclusion(myBrowser)
        /// <returns>None</returns>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 
        public void CustomerFrmBannedCntry(ISelenium myBrowser)
        {
            _isUpdateCustomersResourceFile = false;
            string banCustomer = "ATEcommATBnCust";
            string guidBnCust = Guid.NewGuid().ToString().Substring(0, 4);
            banCustomer = banCustomer + guidBnCust;

            string strUser = _common.CreateCustomer(myBrowser, banCustomer, StrPasswrd);
            if (strUser != "Fail")
            {
                //strUser = common.SelectMainFrame(myBrowser);
                _common.SelectMainFrame(myBrowser);
                Assert.IsTrue(myBrowser.IsElementPresent(AdminSuite.CustomerCreation.CustomersPage.UpdateRegistrationButton), "Update Registration button is not found");
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdateRegistrationButton);

                const string strBannedCountryName = "China";
                myBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.CntryTxtBx, strBannedCountryName);
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdateCustomerButton);
                _frameworkCommon.WaitUntilAllElementsLoad(myBrowser);
                Assert.IsTrue(myBrowser.IsTextPresent(strBannedCountryName), "Change of country to a Banned Country is not successfull");
                _isUpdateCustomersResourceFile = _common.UpdateCustomerResouceFile("BannedCountryCustomer", strUser, StrPasswrd);
                if (_isUpdateCustomersResourceFile == false)
                {
                    Framework.BaseTest.Fail("Customers Resource File updation failed.");
                }
            }
            else
            {
                Framework.BaseTest.Fail("User creation Failed");
            }
        }

        ///<summary>
        /// To exclude a customer from a company
        /// Author: Yogesh
        /// Ex:Customer_CompanyExclusion(myBrowser)
        /// <returns>None</returns>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 22-Dec-2011
        /// Modification Comments:
        public void CustomerCompanyExclusion(ISelenium myBrowser)
        {
            _isUpdateCustomersResourceFile = false;
            string cmpnyExclu = "ATEcommATCmpnyExcl";
            string guidBnCust = Guid.NewGuid().ToString().Substring(0, 3);
            cmpnyExclu = cmpnyExclu + guidBnCust;
            string strUser = _common.CreateCustomer(myBrowser, cmpnyExclu, StrPasswrd);
            if (strUser != "Fail")
            {
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.CompanyExcludedCustomerBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(myBrowser);
                System.Threading.Thread.Sleep(10000);
                Assert.IsTrue(myBrowser.IsTextPresent("New Company Exclusion Added"), "Company Exclusion is unsuccessfull");
                _isUpdateCustomersResourceFile = _common.UpdateCustomerResouceFile("CompanyExcludedCustomer", strUser, StrPasswrd);
                if (_isUpdateCustomersResourceFile == false)
                {
                    Framework.BaseTest.Fail("Customers Resource File updation failed.");
                }
            }
            else
            {
                Framework.BaseTest.Fail("User creation Failed");
            }
        }

        ///<summary>
        /// To suspend a customer
        /// Author: Yogesh
        /// Ex:SuspCustomer(myBrowser)
        /// <returns>None</returns>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 26-Dec-2011
        /// Modified By: Yogesh
        /// Modificatin Comments: Changed the value of GetValue condition
        public void SuspCustomer(ISelenium myBrowser)
        {
            string strUser;
            _isUpdateCustomersResourceFile = false;
            string suspCust = "SuspCust";
            string guidSuspCust = Guid.NewGuid().ToString().Substring(0, 2);
            suspCust = suspCust + guidSuspCust;


            strUser = _common.CreateCustomer(myBrowser, suspCust, StrPasswrd);
            if (strUser != "Fail")
            {
                _common.SelectMainFrame(myBrowser);

                if (myBrowser.GetValue(AdminSuite.CustomerCreation.CustomersPage.CustomerStatusLstBx) == "A")
                {
                    myBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.CustomerStatusLstBx, "Suspended");
                    myBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.ReasonForSuspendingUserLstBx, "Other");
                    myBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.StatusReasonLstBx, "CTS");
                    myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.ReasonForSuspendingUserTxtBx, "used for eCommerce Automation");
                    myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdateCustomerButton);
                    _frameworkCommon.WaitUntilAllElementsLoad(myBrowser);
                    System.Threading.Thread.Sleep(10000);

                    if (myBrowser.GetValue(AdminSuite.CustomerCreation.CustomersPage.CustomerStatusLstBx) != "S")
                    {
                        Framework.BaseTest.Fail("Suspension of Customer: Unsuccessfull");
                    }
                    else
                    {
                        _isUpdateCustomersResourceFile = _common.UpdateCustomerResouceFile("SuspendedCustomer", strUser, StrPasswrd);
                    }
                    if (_isUpdateCustomersResourceFile == false)
                    {
                        Framework.BaseTest.Fail("Customers Resource File updation failed.");
                    }
                }
            }
            else
            {
                Framework.BaseTest.Fail("User creation Failed");
            }
        }
    }
}