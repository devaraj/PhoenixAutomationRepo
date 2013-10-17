using System;
using System.Globalization;
using Selenium;
using Framework;
using MbUnit.Framework;
using System.Resources;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Collections;
using System.Xml;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Framework.Common;



namespace AdminSuite
{
    public class Common : BaseTest
    {
        readonly Framework.Common.Common _frameworkCommon = new Framework.Common.Common();


        //Initializing the Customers resource file path in resFilePath variable
        public static DirectoryInfo CurrentDirPath = new DirectoryInfo(Environment.CurrentDirectory);
        public static string ResFilePath = CurrentDirPath.FullName + "\\Resources\\Customers.resx"; //CurrentDirPath.Parent.FullName.ToString(CultureInfo.InvariantCulture) + "\\AdminSuite\\Resources\\Customers.resx";

        //public Common()
        //{
        //    DirectoryInfo CurrentDirPath = new DirectoryInfo(Environment.CurrentDirectory);
        //    ResFilePath = CurrentDirPath.FullName + "\\Resources\\Customers.resx";
        //    if (!File.Exists(ResFilePath))
        //    {
        //        ResFilePath = CurrentDirPath.Parent.FullName.ToString(CultureInfo.InvariantCulture) + "\\AdminSuite\\Resources\\Customers.resx";
        //        AddTestCase("Customers.resx", "Taken from other than the Output directory");
        //    }
        //    else
        //    {
        //        AddTestCase("Customers.resx", " Taken from the Output directory");
        //    }
        //}

        /// <summary>
        ///  Add a new customer
        /// </summary>
        ///  Author: Yogesh
        ///  Created Date:21-Dec-2011
        /// <param name="myBrowser">Selenium browser</param>
        /// <param name="strUserName">Customer Name to be created</param>
        /// <param name="strPassword">Password for the user</param>
        /// <returns>Customer Created</returns>
        /// Modified By: Pradeep
        /// Modified code to return Username
        public string CreateCustomer(ISelenium myBrowser, string strUserName, string strPassword)
        {
            Random rnd = new Random();
            string strSignificantDate;
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
            strSignificantDate = day + month + year;

            string[] fName = new string[] { "Dylan", "Ethan", "George", "Hary", "Jacob", "John", "Jessica", "Amelia", "Grace", "Emily", "Ruby" };
            string[] lName = new string[] { "Brown", "Jones", "Miller", "Roberts", "Taylor", "Wilson" };
            string[] pCode = new string[] { "HA2 9SR", "HA2 9SG", "HA2 9SE", "HA2 9SN", "HA2 9SW", "HA2 9SX", "HA2 8SS", "HA2 8SE", "HA2 8SX", "HA2 8SN", "HA2 8SA" };

            string firstName = "Auto" + fName[rnd.Next(0, 10)];
            string lastName = "Auto" + lName[rnd.Next(0, 5)];
            string postCode = pCode[rnd.Next(0, 10)];
            string houseNumTxt = rnd.Next(10000).ToString();

            string DOBday = rnd.Next(20, 30).ToString();
            string DOBmonth = rnd.Next(1, 12).ToString();
            if (DOBmonth.Length < 2)
            {
                DOBmonth = "0" + DOBmonth;
            }
            string DOByear = rnd.Next(1950, 1980).ToString();
            string DOB = DOByear + "-" + DOBmonth + "-" + DOBday;

            try
            {
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, myBrowser);
                SelectMainFrame(myBrowser);
                Assert.IsTrue(myBrowser.IsElementPresent(AdminSuite.CustomerCreation.CustomersPage.AddCustomerBtn), "Add Customer button doesn't exist in Customer Search Home Page");

                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.AddCustomerBtn);
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                // Entering all the field to create UserName
                myBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.AccDepLstBx, "Deposit");
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.EntrUsrNmeTxtBx, strUserName);
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.EntrPwdTxtBx, strPassword);
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.EntrPwdAgnTxtBx, strPassword);
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.SgnfiDateTxtBx, strSignificantDate);

                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.Chlg1TxtBx, "Challenge1");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.Resp1TxtBx, "Response1");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.Chlg2TxtBx, "Challenge2");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.Resp2TxtBx, "Response2");
                myBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.RegChTxtBx, "Internet");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.FNameTxtBx, firstName);
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.LNameTxtBx, lastName);

                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.HouseNumTxtBx, houseNumTxt);
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.Addr2TxtBx, "Ladbrokes Ltd, Imperial Drive");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.Addr3TxtBx, "Harrow");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.Addr4TxtBx, "");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.CityTxtBx, "Middx");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.PostCodeTxtBx, postCode);
                myBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.CntryTxtBx, "United Kingdom");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.TelDialCdeTxtBx, "44");
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.TelPhTxtBx, "1234567890");
                System.Threading.Thread.Sleep(100);

                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.EmailTxtBx, firstName + lastName + "@gmail.com");
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.GenderRadioButton);
                myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.DobTxtBox, DOB);
                // Clicking on AddCustomer button
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.AddCustomerBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(myBrowser);
                System.Threading.Thread.Sleep(10000);

                // Writing to Resource file             
                Console.WriteLine("User created is {0}", strUserName);
                if (myBrowser.IsTextPresent(strUserName))
                {
                    return strUserName;
                }
                else
                {
                    return "Fail";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return "Fail";
            }
        }

        /// <summary>
        /// To Search Customer by CustomerName
        /// </summary>
        ///  Author: Pradeep
        /// <param name="strCustomerName"> </param>
        /// <param name="myBrowser"> </param>
        /// Ex: Adit_Test_SelfExcluded
        /// <returns>None</returns>
        /// Created Date: 21-Dec-2011
        /// Modified Date: 21-Dec-2011
        /// Modification Comments:
        public void SearchCustomer(string strCustomerName, ISelenium myBrowser)
        {
            try
            {
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, myBrowser);

                // Navigating to RHN
                SelectMainFrame(myBrowser);
                _frameworkCommon.WaitUntilElementPresent(myBrowser, AdminSuite.CustomerCreation.CustomersPage.CusSearUsernameTxtBx, "60");
                if (myBrowser.IsElementPresent(AdminSuite.CustomerCreation.CustomersPage.CusSearUsernameTxtBx))
                {
                    myBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.CusSearUsernameTxtBx, strCustomerName);
                }
                else
                {
                    myBrowser.Type("name=Username", strCustomerName);
                }
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.FindCustomerUnBtn);
                Thread.Sleep(10000);
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Assert.IsTrue(myBrowser.IsTextPresent(strCustomerName), "Customer information is not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
        /// <summary>
        /// To click on Left hand navigation Link
        /// </summary>
        ///  Author: Pradeep
        /// <param name="strStringLocator">ID or Xpath of the Link to be clicked</param>
        /// Ex:
        /// <returns>None</returns>
        /// Created Date: 21-Dec-2011
        /// Modified Date: 21-Dec-2011
        /// Modification Comments:
        public void LHNavigation(string strStringLocator, ISelenium myBrowser)
        {
            try
            {
                SelectTopBarFrame(myBrowser);
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Assert.IsTrue(myBrowser.IsElementPresent(strStringLocator), "Expected Link doesn't exist in Admin Home Page LHN");
                myBrowser.Click(strStringLocator);
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }


        ///<summary>
        /// To Select Main Frame
        /// Author: Yogesh
        /// Ex:SelectMainFrame(myBrowser)
        /// <returns>None</returns>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 22-Dec-2011
        /// Modification Comments:

        public void SelectMainFrame(ISelenium myBrowser)
        {
            try
            {
                myBrowser.SelectFrame("relative=top");
                myBrowser.SelectFrame("MainArea");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }


        ///<summary>
        /// To Select Top Bar Frame
        /// Author: Yogesh
        /// Ex:SelectMainFrame(myBrowser)
        /// <returns>Selection of Top Bar frame in Admin home page</returns>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 22-Dec-2011
        /// Modification Comments:

        public void SelectTopBarFrame(ISelenium myBrowser)
        {
            try
            {
                myBrowser.SelectFrame("relative=top");
                myBrowser.SelectFrame("TopBar");
            }


            //throw new NotImplementedException();
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// To Add additional fields to the Customers Resource file
        /// </summary>
        /// Authour: Pradeep
        /// <param name="strTypeOfUser">Type of the user</param>
        /// <param name="UserName">User Created</param>
        /// <param name="Password">Password</param>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 22-Dec-2011
        /// Modification Comments:
        public void AddToCustomerResouceFile(string strTypeOfUser, string UserName, string Password)
        {
            try
            {
                Stream stream = new FileStream(ResFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                var doc = new XmlDocument();
                doc.Load(stream);
                XmlNode node = doc.CreateNode(XmlNodeType.Element, "data", null);
                XmlAttribute name;
                name = node.OwnerDocument.CreateAttribute("name");
                name.Value = strTypeOfUser;
                XmlAttribute xmlSpace;
                xmlSpace = node.OwnerDocument.CreateAttribute("xml:space");
                xmlSpace.Value = "preserve";
                node.Attributes.Append(name);
                node.Attributes.Append(xmlSpace);
                XmlNode value = doc.CreateElement("value");
                value.InnerText = UserName + ";" + Password;
                node.AppendChild(value);
                XmlNodeList nodeList = doc.GetElementsByTagName("root");
                nodeList[0].AppendChild(node);
                stream.Dispose();
                doc.Save(ResFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        }
        /// <summary>
        /// To Upadte the Customer Resource file
        /// </summary>
        /// Authour: Pradeep
        /// <param name="typeOfUser">Type of the user</param>
        /// <param name="userName">User Created</param>
        /// <param name="password">Password</param>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 22-Dec-2011
        /// Modification Comments:
        public bool UpdateCustomerResouceFile(string typeOfUser, string userName, string password)
        {
            bool result = false;
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(ResFilePath);
                XmlNodeList nlist = xmlDoc.SelectNodes("//root/data");
                for (int i = 0; i <= xmlDoc.SelectNodes("//root/data").Count - 1; i++)
                {
                    string[] itemArray = nlist[i].OuterXml.Split('"');
                    if (itemArray[1].Equals(typeOfUser))
                    {
                        xmlDoc.SelectNodes("//root/data/value").Item(i).InnerText = userName + ";" + password;
                        xmlDoc.Save(ResFilePath);
                        result = true;
                        break;
                    }
                }
                return result;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }

        }

        /// <summary>
        /// To Get the user credentials from Customer Resource file
        /// </summary>
        /// Authour: Pradeep
        /// <param name="custType">Type of the customer</param>
        /// <returns>returns an array with UserName and Password</returns>
        /// Created Date: 22-Dec-2011
        /// Modified Date: 22-Dec-2011
        /// Modification Comments
        public static string[] GetCustomerCredentials(string custType)
        {
            try
            {
                Stream stream = new System.IO.FileStream(ResFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                var resFileData = new ResXResourceReader(stream);
                IDictionaryEnumerator resFileDataDict = resFileData.GetEnumerator();
                var customerCreds = new string[2];
                while (resFileDataDict.MoveNext())
                {
                    if (custType.ToString(CultureInfo.InvariantCulture) == resFileDataDict.Key.ToString())
                    {
                        string temp = resFileDataDict.Value.ToString();
                        customerCreds = temp.Split(';');
                        break;
                    }
                }

                resFileData.Close();
                stream.Dispose();
                return customerCreds;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return null;

            }
        }

        /// <summary>
        /// Login to Openbet admin
        /// </summary>
        /// Authour: pradeep
        /// Created Date: 02-Feb-2012
        public ISelenium LogOnToAdmin()
        {
            IWebDriver ffDriver = new FirefoxDriver();
            ISelenium myBrowser = new WebDriverBackedSelenium(ffDriver, FrameGlobals.AdminBase);
            myBrowser.Start();
            myBrowser.Open(FrameGlobals.AdminBase);
            ffDriver.Manage().Window.Maximize();
            myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            // PreProd has some issues
            myBrowser.Refresh();
            myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

            myBrowser.Type(AdminSuite.CommonControls.AdminHomePage.UsrNmeTxtBx, FrameGlobals.AdminUserName);
            myBrowser.Type(AdminSuite.CommonControls.AdminHomePage.PwdTxtBx, FrameGlobals.AdminPassWord);
            myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            myBrowser.Click(AdminSuite.CommonControls.AdminHomePage.LoginBtn);
            myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            Thread.Sleep(2000);

            return myBrowser;
        }

        /// <summary>
        /// To update the Selection 
        /// </summary>
        /// <param name="browser">Selenium browser Instance</param>
        /// <param name="selectionId">ID of the selection</param>
        /// <param name="price">Price tobe updated</param>
        /// <param name="status"> Status tobe updated</param>
        /// Ex: Pass Argument as "Active"
        public void UpdateSelection(ISelenium browser, string selectionId, string price, string status, string multipleKeyVal)
        {
            UpdateSelection(browser, selectionId, price, status, multipleKeyVal, string.Empty);
        }

        /// <summary>
        /// To update the Selection 
        /// </summary>
        /// <param name="browser">Selenium browser Instance</param>
        /// <param name="selectionId">ID of the selection</param>
        /// <param name="price">Price tobe updated</param>
        /// <param name="status"> Status tobe updated</param>
        /// Ex: Pass Argument as "Active"
        public void UpdateSelection(ISelenium browser, string selectionId, string price, string status, string multipleKeyVal, string displayed)
        {
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionId);
            browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
            browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            if (String.IsNullOrWhiteSpace(status) == false)
            {
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.SelectionStatusLstBx, "label=" + status + "");
            }
            if (String.IsNullOrWhiteSpace(price) == false)
            {
                browser.Type(AdminSuite.CommonControls.EventDetailsPage.PriceTxtBx, price);
            }
            // Adding multiplekeyvalue for the selection
            if (multipleKeyVal != "")
            {
                browser.Type(AdminSuite.CommonControls.EventDetailsPage.multipleKeyTxtBx, multipleKeyVal);
            }

            if (displayed != string.Empty)
            {
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.SelectionDispStatusListBx, "label=" + displayed + "");
            }
            // Updating the event details page
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.SelectionUpdateBtn);
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
        }

        /// <summary>
        /// Getting event start Time
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public string GetEventStartTimeByEventID(ISelenium browser, string eventID)
        {
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, eventID);
            browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyLevelEvent);
            browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

            //string startTimeXpath = "//input[@name='EvStartTime']";
            string startTime = GetEventStartDateTime(browser);
            //if (browser.IsElementPresent(startTimeXpath))
            //{
            //    //startTime = browser.GetText(startTimeXpath);
            //    startTime = browser.GetValue(startTimeXpath);
            //}
            return startTime;
        }

        public string GetEventStartDateTime(ISelenium browser)
        {
            string startTimeXpath = "//input[@name='EvStartTime']";
            string startTime = string.Empty;
            if (browser.IsElementPresent(startTimeXpath))
            {
                //startTime = browser.GetText(startTimeXpath);
                startTime = browser.GetValue(startTimeXpath);
            }
            return startTime;
        }

        public string GetEventStartDateTimeBySelectionID(ISelenium browser, string selectionID)
        {

            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
            browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionID);
            browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, "label=Even outcome");// AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
            browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
            _frameworkCommon.WaitUntilAllElementsLoad(browser);

            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton);//click on back button present in the selection page

            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton);//click on back button present in the selection page
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

            string startTime = GetEventStartDateTime(browser);
            return startTime;

            //2013-05-07 22:22:03
        }

        public void UpdateEventStartDateTimeBySelectionID(ISelenium browser, string selectionID, string startDateTime)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionID);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, "label=Even outcome");// AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton);//click on back button present in the selection page
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton);//click on back button present in the selection page
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

                string startTimeXpath = "//input[@name='EvStartTime']";

                browser.Type(startTimeXpath, startDateTime);

                //if (browser.IsElementPresent(startTimeXpath))
                //{
                //    //startTime = browser.GetText(startTimeXpath);
                //    startTime = browser.GetValue(startTimeXpath);
                //}

                //Updating the Event
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn))
                {
                    browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    Assert.IsTrue(_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                //return null;
            }
        }

        public void UpdateEventStartDateTimeByEventID(ISelenium browser, string eventID, string startDateTime)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, eventID);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyLevelEvent);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

                //string startTime = GetEventStartDateTime(browser);

                string startTimeXpath = "//input[@name='EvStartTime']";

                browser.Type(startTimeXpath, startDateTime);

                //Updating the Event
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn))
                {
                    browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    Assert.IsTrue(_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                //return null;
            }
        }


        public void UpdateHorseRaceSelectionResultSettlement(ISelenium browser, string selectionID, string resultValue)
        {
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionID);
            browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
            browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

            //browser.SelectFrame("MainArea");

            if (String.IsNullOrWhiteSpace(resultValue) == false)
            {
                browser.Select("name=OcResult", "label=" + resultValue + "");
            }

            // Updating the event details page
            browser.Click("//th/input[@value='Set Selection Results']");
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

            //th/input[@value='Set Selection Results']
        }

        /// <summary> Updates the PayoutFraction
        /// Author: Yogesh
        /// Date Created: FEb 29 2012
        /// </summary>
        /// <param name="browser">Selenium browser Instance</param>
        /// <param name="selectionId">Selection Id of the selection</param>
        /// <param name="updatePayoutFractionNumerator">Payout fraction numerator</param>
        /// <param name="updatePayoutFractionDenominator">Payout fraction denominator</param>
        /// Ex: UpdatePayourFraction(browser, "12345678", "1", "2", "3");
        /// Returns: Nothing
        public void UpdatePayoutFraction(ISelenium browser, string selectionId, string updatePayoutFractionNumerator, string updatePayoutFractionDenominator)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionId);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton); //click on back button
                string payoutFractionNumerator = browser.GetText("//tr[td[contains(text(), 'Each-Way')]]/td/input[@name='MktEWFacNum']");
                string payoutFractionDenominator = browser.GetText("//tr[td[contains(text(), 'Each-Way')]]/td/input[@name='MktEWFacDen']");
                if (String.IsNullOrWhiteSpace(payoutFractionNumerator) && String.IsNullOrWhiteSpace(payoutFractionDenominator))
                {
                    browser.Type("//tr[td[contains(text(), 'Each-Way')]]/td/input[@name='MktEWFacNum']", updatePayoutFractionNumerator);
                    browser.Type("//tr[td[contains(text(), 'Each-Way')]]/td/input[@name='MktEWFacDen']", updatePayoutFractionDenominator);
                }
                // Updating the market details page
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary> Update the Handicap Value of a selection
        /// Author : Yogesh M
        /// </summary>
        /// Date Created: Feb 03 2012
        /// eg: updatehandicapValue = UpdateHandicapValue(browser, "1234555", "2");
        /// Note: You need to specify the list number present in the Handicap list box & not the value in the list box
        /// <param name="browser">Selenium Browser Instance</param>
        /// <param name="selectionId">Selection Id of the selection</param>
        /// <param name="handicapListNumber">The list number in the handicap list box(1 to 81)</param>
        /// <returns> Updated handicap value</returns>
        public string UpdateHandicapValue(ISelenium browser, string selectionId, int handicapListNumber)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                Thread.Sleep(2000);
                browser.WaitForPageToLoad("60000");
                browser.SelectFrame("relative=top");
                browser.SelectFrame("MainArea");
                browser.WaitForPageToLoad("10000");
                //SelectMainFrame(browser);
                browser.Type("//input[@name='openbet_id']", selectionId);
                // browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Select("//select[@name='hierarchy_level']", AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Focus(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                browser.Focus(AdminSuite.CommonControls.EventDetailsPage.BackButton); //click on back button present in the selection page
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton); //click on back button present in the selection page
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                //update the handicap value in the market page
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                Thread.Sleep(5000);
                _frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.HandicapValueListBox, "20000");
                int labels = driver.FindElements(By.XPath(AdminSuite.CommonControls.EventDetailsPage.HandicaValueLabels)).Count;
                // verify if the user has entered the handicap list number present in the handicap list box in market page
                Assert.IsTrue(handicapListNumber <= labels, "Please select a handicap list number <= " + labels + "");
                string updatedHandicapValue = driver.FindElement(By.XPath("//select[@name='MktHcapValue']/option[" + handicapListNumber + "]")).Text;
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.HandicapValueListBox, updatedHandicapValue);   // Handicap list drop down in the market page
                // Updating the event details page
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                Console.WriteLine("Handicapped value is updated to : " + updatedHandicapValue + "");
                return updatedHandicapValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// Author : Roopa
        /// Date : 22-1-2013
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="selectionId"></param>
        /// <returns></returns>
        public string GetHandicapValue(ISelenium browser, string selectionId)
        {
            string handicapValue = null;
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                Thread.Sleep(2000);
                //Selecting TopFrame
                browser.WaitForPageToLoad("60000");
                browser.SelectFrame("relative=top");
                browser.SelectFrame("MainArea");
                browser.WaitForPageToLoad("10000");

                // SelectMainFrame(browser);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionId);
                browser.Type("//input[@name='openbet_id']", selectionId);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Focus(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                browser.Focus(AdminSuite.CommonControls.EventDetailsPage.BackButton); //click on back button present in the selection page
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton); //click on back button present in the selection page
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                Thread.Sleep(3000);
                //get the handicap value in the market page
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.currentHandicapValue))
                {
                    // handicapValue = browser.GetSelectedLabel(AdminSuite.CommonControls.EventDetailsPage.currentHandicapValue);
                    handicapValue = browser.GetSelectedLabel(AdminSuite.CommonControls.EventDetailsPage.HandicapValueListBox);
                    //browser.Gets
                }
                else
                {
                    return null;
                }
                return handicapValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary> Update the Market Index Value of a selection dynamically, also we can suspend the market
        /// Author : Yogesh M
        /// Date Created: Feb 05 2011 
        /// Date Modfied: March 06 2011
        /// Modification Comments: Added the status of market in the method
        /// Note: Market Index value is <= number of selections the market has
        /// eg: updatedMarketIndexValue = UpdateMarketIndex(browser, selectionId, 2, Suspended);
        ///                               UpdateMarketIndex(browser, selectionId, 0, Active); //label=Suspended or label=Active
        /// <param name="browser">Selenium Browser instance</param>
        /// </summary>
        /// <param name="browser"> </param>
        /// <param name="selectionId">Selection Id of the selection</param>
        /// <param name="updatedMarketIndexValue">User entered value of updated market index</param>
        /// <param name="status"> </param>
        /// <returns>Updated Market Index value</returns>
        public string UpdateMarketIndex(ISelenium browser, string selectionId, int updatedMarketIndexValue, string status)
        {
            try
            {

                string updatedMarektIndex = "";
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionId);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton);//click on back button present in the selection page
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                if (updatedMarketIndexValue != 0)
                {
                    //update the marketIndex value in the market page
                    updatedMarektIndex = Convert.ToString(updatedMarketIndexValue, CultureInfo.InvariantCulture);
                    browser.Type(AdminSuite.CommonControls.EventDetailsPage.BirIndexTextBox, updatedMarektIndex);
                    Console.WriteLine("Market index is updated to : " + updatedMarektIndex + "");
                    //return updatedMarektIndex;
                }
                if (String.IsNullOrWhiteSpace(status) == false)
                {
                    browser.Select("name=MktStatus", "label=" + status + "");
                }
                // Updating the event details page
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                return updatedMarektIndex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Update each way in Open bet.
        /// </summary>
        /// <param name="browser">Browser instance</param>
        /// <param name="selectionId">Selection id</param>
        /// <param name="eachWayPlaces">Each way place</param>
        /// <param name="eachWayTopValue">Top value</param>
        /// <param name="eachWayBottomValue">bottom value</param>
        /// <returns></returns>
        public string UpdateEachWay(ISelenium browser, string selectionId, string eachWayPlaces, string eachWayTopValue, string eachWayBottomValue)
        {
            try
            {
                string updatedMarektIndex = "";
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionId);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton);//click on back button present in the selection page
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

                if (string.Empty != eachWayPlaces && "0" != eachWayPlaces.Trim())
                {
                    browser.Type(AdminSuite.CommonControls.EventDetailsPage.EachWayPlacesTxtBx, eachWayPlaces);
                    Console.WriteLine("Each way Places is  updated to : " + eachWayPlaces + "");
                }

                if (string.Empty != eachWayTopValue && "0" != eachWayTopValue.Trim())
                {
                    browser.Type(AdminSuite.CommonControls.EventDetailsPage.EachWayTopTxtBx, eachWayTopValue);
                    Console.WriteLine("Each way top is  updated to : " + eachWayTopValue + "");
                }

                if (string.Empty != eachWayBottomValue && "0" != eachWayBottomValue.Trim())
                {
                    browser.Type(AdminSuite.CommonControls.EventDetailsPage.EachWayBottomTxtBx, eachWayBottomValue);
                    Console.WriteLine("Each way Bottom is  updated to : " + eachWayBottomValue + "");
                }

                // Updating the event details page
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                return updatedMarektIndex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void UpdateEachWayByEventID(ISelenium browser, string eventId, string status)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, eventId);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyLevelEvent);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

                //browser.Click("label=|Race Winner|");

                if (browser.IsElementPresent(string.Format("link=|{0}|", "Race Winner")))
                {
                    browser.Click(string.Format("link=|{0}|", "Race Winner"));
                    _frameworkCommon.WaitUntilAllElementsLoad(browser);
                    browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

                    browser.Select("name=MktEWAvail", "label=" + status);

                    if (status == "Yes")
                    {
                        browser.Type(AdminSuite.CommonControls.EventDetailsPage.EachWayPlacesTxtBx, "1");
                        browser.Type(AdminSuite.CommonControls.EventDetailsPage.EachWayTopTxtBx, "1");
                        browser.Type(AdminSuite.CommonControls.EventDetailsPage.EachWayBottomTxtBx, "1");
                    }

                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                    //update Market
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton))
                    {
                        browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                        Assert.IsTrue(_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Market Updation is not Successfull");
                    }

                }
                else
                {
                    Console.WriteLine("Market is not available");
                }
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// To Update Customer Flag Information
        /// </summary>
        ///  Author: Anand
        /// <param name="flagName"> </param>
        /// <param name="flagVal"> </param>
        /// <param name="myBrowser"> </param>
        /// Ex: Adit_Test_SelfExcluded
        /// <returns>None</returns>
        /// Created Date: 12-April-2012
        public bool UpdateCustomerFlag(string flagName, string flagVal, ISelenium myBrowser)
        {
            try
            {
                // Navigating to MainFrame
                SelectMainFrame(myBrowser);
                IWebDriver driver = ((WebDriverBackedSelenium)myBrowser).UnderlyingWebDriver;
                Assert.IsTrue(myBrowser.IsElementPresent(AdminSuite.CustomerCreation.CustomersPage.ViewCustFlag), "Customer Flag Information table isnot found");
                myBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.ViewCustFlag);
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                //Updating the customer flags
                Assert.IsTrue(driver.FindElement(By.XPath("//tr//td/b[text()='" + flagName + "']//ancestor::tr/td/Select")).Displayed, "Unable to navigate to Customer Flags page");
                myBrowser.Select("//tr//td/b[text()='" + flagName + "']//ancestor::tr/td/Select", flagVal);
                Assert.IsTrue(driver.FindElement(By.XPath("//th[@class='buttons']/input[@value='Update Customer Flags']")).Displayed, "Update Customer Flags button is not displayed in Customer Flags page");
                myBrowser.Click("//th[@class='buttons']/input[@value='Update Customer Flags']");
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                return true;
            }
            catch (Exception ex)
            {
                BaseTest.CaptureScreenshot(myBrowser, "");
                Console.WriteLine(ex.StackTrace);
                BaseTest.Fail(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To logout from Admin
        /// </summary>
        ///  Author: Anand
        /// <param name="myBrowser"> </param>
        /// Created Date: 12-April-2012
        public void AdminLogout(ISelenium myBrowser)
        {
            try
            {
                // Navigating to LHN
                SelectTopBarFrame(myBrowser);
                IWebDriver driver = ((WebDriverBackedSelenium)myBrowser).UnderlyingWebDriver;
                //Assert.IsTrue(myBrowser.IsElementPresent("//a[contains(text(),'Logout')]"),"logout link"

                if (myBrowser.IsElementPresent("//a[contains(text(),'Logout')]"))
                {
                    myBrowser.Click("//a[contains(text(),'Logout')]");
                    myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }
                else
                {
                    ConsoleColor clr = new ConsoleColor();
                    clr = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("!!!Warning: Logout link not present in LHN of Admin");
                    Console.ForegroundColor = clr;
                }

            }
            catch (Exception ex)
            {
                BaseTest.CaptureScreenshot(myBrowser, "");
                Console.WriteLine(ex.StackTrace);
                BaseTest.Fail(ex.Message);
            }
        }


        /// <summary>
        /// To suspend/ Activate the Events
        /// </summary>
        /// <param name="browser">Selenium browser instance</param>
        /// <param name="catName">Category Name</param>
        /// <param name="eventClsName">Event ClassName</param>
        /// <param name="eventTypeName">Event TypeName</param>
        /// <param name="eventSubType">Event SubType</param>
        /// <param name="eventName">Event Name</param>
        /// <param name="eventStatus">Event Status</param>
        public void UpdateEvents(ISelenium browser, string catName, string eventClsName, string eventTypeName, string eventSubType, string eventName, string eventStatus)
        {
            string errorMessage = "";
            bool eventUpdationStatus = false;
            bool finalEventStatus = true;
            Framework.Common.Common frameworkcommon = new Framework.Common.Common();
            AdminSuite.Common com = new AdminSuite.Common();
            Framework.Common.Common Fcommon = new Framework.Common.Common();



            TimeSpan ts = new TimeSpan(0, 1, 0);
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            System.Threading.Thread.Sleep(10000);
            SelectMainFrame(browser);

            frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "60");
            catName = catName.Replace("|", "").Trim();
            if (catName != "")
            {
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx))
                {
                    Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, catName), "Category Name does not present in DropdownList");
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "label=" + catName);
                }
            }
            eventClsName = eventClsName.Replace("|", "").Trim();

            if (eventClsName != "")
            {
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx))
                {
                    Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, eventClsName), "EventClass Name does not present");
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, "label=" + eventClsName);
                }
            }
            if (eventTypeName != "")
            {
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx))
                {
                    Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, eventTypeName), "EventClass Name does not present");
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, "label=" + eventTypeName);
                }
            }
            if (eventSubType != "")
            {
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.subEventTypeLstBx))
                {
                    Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.subEventTypeLstBx, eventSubType), "EventSubType Name does not present");
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.subEventTypeLstBx, "label=" + eventSubType);
                }
            }
            Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "--"), "Date range dropdown missing");

            // Selecting Daterange 
            if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx))
            {
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "label=--");
            }
            //Clicking on Seach button
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.eventSearchBtn);
            _frameworkCommon.WaitUntilAllElementsLoad(browser);
            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            //Thread.Sleep(2000);
            // Wait for Element to present

            if (frameworkcommon.WaitUntilElementPresent(browser, "link=" + eventName + "", "60") == true)
            {
                browser.Click("link=" + eventName);
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                if (eventStatus == "Taginfo" || eventStatus == "Taginfoundo")
                {
                    browser.Click("//a[contains(string(),'Race Winner')]");
                    Thread.Sleep(10000);
                    browser.Click("//tr[@class='active']//a[contains(string(),'Horse1')]");
                    Thread.Sleep(10000);
                    if (eventStatus == "Taginfoundo")
                        browser.Select("name=OcFlag", "label=Named runner");
                    else
                        browser.Select("name=OcFlag", "label=Unnamed favourite");

                    browser.Click("//input[@value='Modify Selection']");
                }
                else
                {
                    //Wait Untill the EventDetails Page loads
                    if (frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "60") == true)
                    {
                        if (eventStatus == "Suspend")
                        {
                            browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Suspended");
                            eventUpdationStatus = false;
                            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                        }
                        else
                        {
                            browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Active");
                        }

                        //Updating the Event
                        if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn))
                        {
                            if (eventUpdationStatus == false)
                            {
                                browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                                Assert.IsTrue(frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                            }


                        }
                    }
                }
            }

            else
            {
                finalEventStatus = false;
                errorMessage = errorMessage + eventName;
                errorMessage = errorMessage + Environment.NewLine;
            }





            //Finally checking whether all the Events are Suspended or not
            if (finalEventStatus == false)
            {
                Console.WriteLine("Following Events are not Suspended, Please verify Manually" + Environment.NewLine + errorMessage);
                Framework.BaseTest.Fail("Event suspending Process is Failed");
            }

        }
        /// <summary>
        /// Unlock the User which is locked
        /// </summary>
        ///  Author: Anand
        /// <param name="browser">browser Instance</param>
        /// <param name="userName">User Name</param>
        public void UnLockTheLockedUser(ISelenium browser, string userName)
        {
            decimal numberOfAciveRow;
            string amend = "//input[@value='Amend Status Flags']";
            // Enter Customer Name and Search
            SearchCustomer(userName, browser);
            // Click on Amend Status Flag
            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            SelectMainFrame(browser);
            if (browser.IsElementPresent(amend))
                browser.Click(amend);
            System.Threading.Thread.Sleep(5000);
            // Get Number of Active rows where user should be unlocked
            numberOfAciveRow = browser.GetXpathCount("//tbody/tr[@class='active']");
            if (numberOfAciveRow > 0)
            {
                for (int i = 0; i < numberOfAciveRow; i++)
                {
                    browser.Click("link=[clear]");
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    System.Threading.Thread.Sleep(5000);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }
                Console.WriteLine("AdminSuite:Common:UnLockTheLockedUser-Pass: User " + userName + " Unlocked ");
            }
        }

        /// <summary>
        /// releases any SelfExcluded User
        /// </summary>
        ///  Author: Anand
        /// <param name="browser">Browser</param>
        /// <param name="userName">User Name</param>
        public void ReleaseSelfExcludedUser(ISelenium browser, string userName)
        {
            try
            {
                decimal numberOfSelfExcludedLink;
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                // Enter Customer Name and Search
                SearchCustomer(userName, browser);
                // Getting number of SelfExcluded Link
                numberOfSelfExcludedLink = browser.GetXpathCount(AdminSuite.CustomerCreation.CustomersPage.NoOfSelfExcludedCustomer);
                if (numberOfSelfExcludedLink > 0)
                {
                    for (int i = 0; i < numberOfSelfExcludedLink; i++)
                    {
                        browser.Click(AdminSuite.CustomerCreation.CustomersPage.NoOfSelfExcludedCustomer);
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                        System.Threading.Thread.Sleep(10000);
                        if (!browser.IsTextPresent("Application Error"))
                        {
                            if (browser.IsElementPresent("//input[@type='submit' and @value='Confirm']"))
                            {
                                browser.Click("//input[@type='submit' and @value='Confirm']");
                                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                                System.Threading.Thread.Sleep(10000);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("AdminSuite:ReleaseSelfExcludedUser-Fail- Customer is not SelfExcluded");
                }
                Assert.IsTrue(browser.IsTextPresent("Self exclusion successfully removed"), "Failed to clear SelfExclusion for customer");
                Console.WriteLine("AdminSuite:ReleaseSelfExcludedUser-Pass- Cleared the SelfExcluded");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Releases any Company Excluded Customer
        /// </summary>
        ///   Author: Anand
        /// <param name="customerName">Customer Name</param>
        /// <param name="browser">Browser Instance</param>
        public void ReleaseCompanyExcludedUser(string customerName, ISelenium browser)
        {
            decimal numberOfCompanyExcludedLink;
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            // Enter Customer Name and Search
            SearchCustomer(customerName, browser);
            // Getting number of SelfExcluded Link
            numberOfCompanyExcludedLink = browser.GetXpathCount(AdminSuite.CustomerCreation.CustomersPage.NoOfCompanyExludedCustomer);
            if (numberOfCompanyExcludedLink > 0)
            {
                for (int i = 0; i < numberOfCompanyExcludedLink; i++)
                {
                    browser.Click(AdminSuite.CustomerCreation.CustomersPage.NoOfCompanyExludedCustomer);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    System.Threading.Thread.Sleep(10000);
                    if (!browser.IsTextPresent("Application Error"))
                    {
                        if (browser.IsElementPresent("//input[@type='submit' and @value='Confirm']"))
                        {
                            browser.Click("//input[@type='submit' and @value='Confirm']");
                            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                            System.Threading.Thread.Sleep(10000);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("AdminSuite:ReleaseCompanyExcludedUser-Fail- Customer not Company Excluded");
            }
            //Assert.IsTrue(browser.IsTextPresent("Company exclusion successfully removed"), "Failed to clear CompanyExclusion for customer");
            Console.WriteLine("AdminSuite:ReleaseCompanyExcludedUser-Pass- Cleared the CompanyExcluded");
        }


        /// <summary>
        ///  Add desired Customer to Banned User Group
        /// </summary>
        ///  Author: Anand
        /// <param name="browser">Browser Instance</param>
        /// <param name="userName">User Name</param>
        public bool UpdateBannedCountryCustomer(ISelenium browser, string userName, string countryName)
        {
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            // Enter Customer Name and Search
            SearchCustomer(userName, browser);
            //
            if (browser.IsElementPresent(AdminSuite.CustomerCreation.CustomersPage.UpdateRegistrationButton))
            {
                browser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdateRegistrationButton);
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                //const string strBannedCountryName = ;
                browser.Select(AdminSuite.CustomerCreation.CustomersPage.CntryTxtBx, countryName);
                browser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdateCustomerButton);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                if (browser.IsTextPresent(countryName))
                {
                    //UpdateCustomerStatus(browser, userName, "Active", "-- unset --", "-- unset --");
                    Console.WriteLine("Change of country to " + countryName + " is successfull");
                    return true;
                }
                else
                {
                    Console.WriteLine("Change of country to " + countryName + " was unsuccessfull");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("AdminSuite:Common:Fail- Update Registration Button is not Found");
                return false;
            }
        }


        /// <summary>
        ///  Active the status of a customer
        /// </summary>
        ///  Author: Kiran
        /// <param name="browser">Browser Instance</param>
        /// <param name="userName">User Name</param>
        public void UpdateCustomerStatus(ISelenium browser, string userName, string status, string statusGroup, string statusReason)
        {
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            // Enter Customer Name and Search
            SearchCustomer(userName, browser);
            //
            if (browser.IsElementPresent(AdminSuite.CustomerCreation.CustomersPage.UpdateCustomerButton))
            {
                browser.Select(AdminSuite.CustomerCreation.CustomersPage.CustomerStatusLstBx, status);
                browser.Select(AdminSuite.CustomerCreation.CustomersPage.ReasonForSuspendingUserLstBx, statusGroup);
                Thread.Sleep(2000);
                browser.Select(AdminSuite.CustomerCreation.CustomersPage.StatusReasonLstBx, statusReason);

                browser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdateCustomerButton);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
            }
        }

        /// <summary>
        /// Add desired Customer to SelfExcluded User Group
        /// </summary>
        ///  Author: Anand
        /// <param name="browser">Selenium Browser</param>
        /// <param name="customerName">Customer Name</param>
        public bool SelfExcludedCustomer(ISelenium browser, string customerName)
        {

            try
            {

                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                // Enter Customer Name and Search
                SearchCustomer(customerName, browser);

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
                browser.Click(AdminSuite.CustomerCreation.CustomersPage.CustomerExcluPeriodLstBx);
                browser.Type(AdminSuite.CustomerCreation.CustomersPage.CustomerExcluDateTxtBx, strExclusionDate);
                // myBrowser.Type("name=exclusion_date", "2011-12-21");
                browser.Click(AdminSuite.CustomerCreation.CustomersPage.CustomerSelfExlusionBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                System.Threading.Thread.Sleep(10000);
                if (browser.IsTextPresent("New Self Exclusion Added"))
                {
                    Console.WriteLine("Self Exclustion is successfull for this customer");
                    return true;
                }
                else
                {
                    Console.WriteLine("Self Exclustion is unsuccessfull for this customer");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        /// <summary>
        ///  Add desired Customer to SelfExcluded User Group
        /// </summary>
        ///  Author: Anand
        /// <param name="browser">Browser Instance</param>
        /// <param name="customerName">Customer Name</param>
        public bool CompanyExcludedCustomer(ISelenium browser, string customerName)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                // Enter Customer Name and Search
                SearchCustomer(customerName, browser);            //
                browser.Click(AdminSuite.CustomerCreation.CustomersPage.CompanyExcludedCustomerBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                System.Threading.Thread.Sleep(10000);
                if (browser.IsTextPresent("New Company Exclusion Added"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To click on Left hand navigation Link
        /// </summary>
        ///  Author: Anand
        /// <param name="linkName">Link Name</param>
        /// Ex:
        /// <returns>True/False</returns>
        /// Created Date: 07-June-2012
        public bool AdminLHNClick(string linkName, ISelenium myBrowser)
        {
            try
            {
                SelectTopBarFrame(myBrowser);
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                IWebDriver driver = ((WebDriverBackedSelenium)myBrowser).UnderlyingWebDriver;
                Assert.IsTrue(driver.FindElement(By.LinkText(linkName)).Displayed, "The link name: " + linkName + " is not present in Admin LHN");
                driver.FindElement(By.LinkText(linkName)).Click();
                myBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Thread.Sleep(2000);
                SelectMainFrame(myBrowser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Operate on any object inside the table in Admin
        /// </summary>
        /// Author: Anand
        /// <param name="adminBrowser">browser object</param>
        /// <param name="tableHeading">Table Heading</param>
        /// <param name="searchText">row string to be searched</param>
        /// <param name="objName">Object Type you wanted to operate on</param>
        /// <param name="objValue">Any value you would want to enter or select</param>
        /// <param name="index">column number</param>
        /// <returns> true/false</returns>
        /// ex: AdminTableObjects(adminBrowser, "Reward Adhoc token", "Username", "Text Box", custName, 1)
        /// Created Date: 07-June-2012
        public bool AdminTableObjects(ISelenium adminBrowser, string tableHeading, string searchText, string objName, string objValue, int index)
        {
            try
            {
                bool retVal = false;

                //Handling the link or button in the table
                if (objName.Contains("Link") || objName.Contains("Button"))
                {
                    retVal = AdminTableButtonLink(adminBrowser, tableHeading, searchText, objName, objValue, 0);
                    return retVal;
                }
                //Creating driver object
                IWebDriver driver = ((WebDriverBackedSelenium)adminBrowser).UnderlyingWebDriver;
                //finding all the rows in the desired table
                ReadOnlyCollection<IWebElement> tableRows = driver.FindElements(By.XPath("//table//th[contains(text(),'" + tableHeading + "')]//ancestor::table/tbody/tr"));

                //looping through a particular row
                for (int rowNum = 0; rowNum < tableRows.Count; rowNum++)
                {
                    if (tableRows[rowNum].Text.IndexOf(searchText) == 0)
                    {
                        ReadOnlyCollection<IWebElement> tdS = tableRows[rowNum].FindElements(By.TagName("td"));
                        switch (objName)
                        {
                            case "Text Box":
                                ReadOnlyCollection<IWebElement> txtBox = tdS[index].FindElements(By.TagName("input"));
                                txtBox[0].SendKeys(objValue);
                                retVal = true;
                                break;
                            case "Select Box":
                                ReadOnlyCollection<IWebElement> listBox = tdS[index].FindElements(By.TagName("select"));
                                var selectElement = new SelectElement(listBox[0]);
                                selectElement.SelectByText(objValue);
                                retVal = true;
                                break;
                        }
                        Thread.Sleep(2000);
                    }
                    //if the required functionality is done exit the action
                    if (retVal)
                    {
                        return true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Clicking on the link or button in the table
        /// </summary>
        /// Author: Anand
        /// <param name="adminBrowser">browser object</param>
        /// <param name="tableHeading">Table Heading</param>
        /// <param name="searchText">row string to be searched</param>
        /// <param name="objName">Object Type you wanted to operate on</param>
        /// <param name="objValue">Any value you would want to enter or select</param>
        /// <param name="index">column number</param>
        /// <returns> true/false</returns>
        /// Created Date: 07-June-2012
        public bool AdminTableButtonLink(ISelenium adminBrowser, string tableHeading, string searchText, string objName, string objValue, int index)
        {
            try
            {
                if (objName.Contains("Link"))
                {
                    //Click on the link without a search string
                    if (string.IsNullOrEmpty(searchText))
                    {
                        adminBrowser.Click("//table//th[contains(string(),'" + tableHeading + "')]//ancestor::table/tbody//a[contains(text(),'" + objValue + "']");
                    }
                    //Click on the link with a search string
                    else
                    {
                        adminBrowser.Click("//table//th[contains(string(),'" + tableHeading + "')]//ancestor::table//td[contains(text(),'" + searchText + "')]/following-sibling::td/a");
                    }
                    _frameworkCommon.WaitUntilAllElementsLoad(adminBrowser);
                    adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }
                //Click on the button
                else
                {
                    adminBrowser.Click("//table//th[contains(string(),'" + tableHeading + "')]//ancestor::table/tbody//input[@value='" + objValue + "']");
                    _frameworkCommon.WaitUntilAllElementsLoad(adminBrowser);
                    adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Adding a Reward Adhoc token to customer in Admin.
        /// </summary>
        /// Date Created:
        /// Author: Anand C
        /// User should be login to Admin application
        /// <param name="adminBrowser">Browser Instance</param>
        /// <param name="custName">Customer name for whom the token be added</param>
        /// <param name="tokenVal">token value to be added</param>
        /// Created Date: 07-June-2012

        public bool AdminRewardAdhoctoken(ISelenium adminBrowser, string custName, string tokenVal)
        {
            try
            {
                int dt = Convert.ToInt32(DateTime.Now.Day.ToString()) + 1;
                string relativeDate = dt.ToString() + " 12:12";
                //Click on Reward Adhoc token link in Admin LHN
                if (AdminLHNClick("Reward Adhoc token", adminBrowser))
                {
                    Console.WriteLine("Successfully clicked on the Admin LHN link Reward Adhoc token");
                }
                else
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on the Admin LHN link");
                    return false;
                }
                //Entering value in Username Text box in Reward Adhoc token page

                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "Username", "Text Box", custName, 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Username Text box in Reward Adhoc token page");
                    return false;
                }
                //clink on the Search button in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "", "Button", "Search", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on the Search button in Reward Adhoc token page");
                    return false;
                }
                //Entering value in GBP text box in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "GBP", "Text Box", tokenVal, 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to to locate GBP text box in Reward Adhoc token page");
                    return false;
                }
                //Entering value in Relative Expiry text box in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "Relative Expiry", "Text Box", relativeDate, 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to locate Relative Expiry text box in Reward Adhoc token page");
                    return false;
                }
                //click on the Reward token button in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "", "Button", "Reward token", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to to click on Reward token buttonReward Adhoc token page");
                    return false;
                }
                if (!adminBrowser.IsTextPresent("Token Rewarded"))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Success message is not displayed on creating the Token");
                    return false;
                }
                Console.WriteLine("Successfully created Adhoc Token to customer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }

        public bool AdminRewardAdhoctoken(ISelenium adminBrowser, string custName, string tokenVal, string redemptionVal)
        {
            try
            {
                int dt = Convert.ToInt32(DateTime.Now.Day.ToString()) + 1;
                string relativeDate = dt.ToString() + " 12:12";
                //Click on Reward Adhoc token link in Admin LHN
                if (AdminLHNClick("Reward Adhoc token", adminBrowser))
                {
                    Console.WriteLine("Successfully clicked on the Admin LHN link Reward Adhoc token");
                }
                else
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on the Admin LHN link");
                    return false;
                }
                //Entering value in Username Text box in Reward Adhoc token page

                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "Username", "Text Box", custName, 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Username Text box in Reward Adhoc token page");
                    return false;
                }
                //click on the Search button in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "", "Button", "Search", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on the Search button in Reward Adhoc token page");
                    return false;
                }
                //Entering value in GBP text box in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "GBP", "Text Box", tokenVal, 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to to locate GBP text box in Reward Adhoc token page");
                    return false;
                }
                // Selecting Redemtion Value in Reward Adhoc token page
                adminBrowser.Select("//select[@name='RValID']", "label=" + redemptionVal);

                //Entering value in Relative Expiry text box in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "Relative Expiry", "Text Box", relativeDate, 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to to locate Relative Expiry text box in Reward Adhoc token page");
                    return false;
                }
                //click on the Reward token button in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Reward Adhoc token", "", "Button", "Reward token", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on the Reward token button in Reward Adhoc token page");
                    return false;
                }
                if (!adminBrowser.IsTextPresent("Token Rewarded"))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Success message is not displayed on creating the Token");
                    return false;
                }
                Console.WriteLine("Successfully created Adhoc Token to customer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }

        public bool AdminSearchCustomer(ISelenium adminBrowser, string custName)
        {
            try
            {
                ///Click on Customers link in Admin LHN
                if (AdminLHNClick("Customers", adminBrowser))
                {
                    Console.WriteLine("Successfully clicked on the Admin LHN link Customers");
                    adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }
                else
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on the Admin LHN link");
                    return false;
                }
                //Entering value in Username Text box in Reward Adhoc token page

                if (!AdminTableObjects(adminBrowser, "Customer Search Criteria", "Username", "Text Box", custName, 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Username Text box in Customer Search page");
                    return false;
                }
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                //click on the Find Customer button
                if (!AdminTableObjects(adminBrowser, "Customer Search Criteria", "", "Button", "Find Customer", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable click on Find Customer button in Customer Search page");
                    return false;
                }
                _frameworkCommon.WaitUntilAllElementsLoad(adminBrowser);
                _frameworkCommon.WaitUntilElementPresent(adminBrowser, "//table//th/font[contains(text(),'Account information')]", FrameGlobals.PageLoadTimeOut);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                //verifying the customer name in page
                if (!adminBrowser.IsTextPresent(custName))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to verify the customer name in the page");
                    return false;
                }
                Console.WriteLine("Successfully searched the customer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }

        /// <summary>
        /// Restricting the customer for placing bets
        /// </summary>
        /// Author : Hemalatha
        /// Date Created : 11-07-2012
        /// Pre-conditions : User should logged into the admin
        /// <param name="adminBrowser">browser instance</param>
        /// <param name="custName">Customer Name</param>
        /// <param name="reasonDesc">Reason for restricting the customer</param>
        /// <returns></returns>
        public bool AddCustFlag(ISelenium adminBrowser, string custName, string reasonDesc, string flagName)
        {
            try
            {
                if (AdminSearchCustomer(adminBrowser, custName) == true)
                {
                    //Click on the Amend Status Flag Button
                    if (!AdminTableObjects(adminBrowser, "Customer attributes", "", "Button", "Amend Status Flags", 1))
                    {
                        Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                        Console.WriteLine("Unable to click on the Amend Status Flags button in Customer detail page");
                        return false;
                    }

                    //select Customer Status Flag
                    if (!AdminTableObjects(adminBrowser, "Add Customer Status Flag", "Customer Status Flag", "Select Box", flagName, 1))
                    {
                        Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                        Console.WriteLine("Unable to click on the Customer Status Flags button in Customer detail page");
                        return false;
                    }

                    //Entering reason for adding the flag
                    string reason = "//table[contains(string(),'Add Customer Status Flag')]//tr[contains(string(),'Reason Description')]/td/textarea[@name='reason']";
                    adminBrowser.Highlight(reason);
                    adminBrowser.Type(reason, reasonDesc);

                    //Clicking the Add Status Flag button
                    string statusFlagButton = "//input[@value='Add Status Flag']";
                    adminBrowser.Click(statusFlagButton);
                }
                Console.WriteLine("Successfully restricted the customer for bets");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }

        /// <summary>       
        /// This method gets the customer's last name
        /// </summary>
        /// <Author>Kiran</Author>
        /// <Date>10 July 2012</Date>
        /// <param name="browser">Browser Instance</param>
        /// <param name="custName">name of the customer</param>
        /// <example>GetCustLastName(browser, Livetestgbp)</example> 

        public string GetCustLastName(ISelenium browser, string custName)
        {
            string lName = "";
            try
            {

                if (AdminSearchCustomer(browser, custName) == true)
                {

                    lName = browser.GetText("//td/table/tbody/tr/td[@class='caption' and contains(text(), 'First Name')]/following-sibling::td");

                }
                return lName;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }



        /// <summary>
        ///  Suspend a customer
        /// </summary>
        ///  Author: Kiran
        /// <param name="browser">Browser Instance</param>
        /// <param name="customerName">Customer Name</param>
        public bool UpdateSuspCust(ISelenium browser, string customerName)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                // Enter Customer Name and Search
                SearchCustomer(customerName, browser);
                browser.Select("name=Status", "label=Suspended");
                browser.Select("name=StatusReasonType", "label=Test Accounts");
                browser.Select("name=StatusReason", "label=Test Account: No Longer Required");
                browser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdateCustomerButton);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                System.Threading.Thread.Sleep(10000);
                if (browser.IsTextPresent("Current Customer Status: Suspended"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Makes a suspended cust active
        /// </summary>
        ///   Author: Kiran
        /// <param name="customerName">Customer Name</param>
        /// <param name="browser">Browser Instance</param>
        public void ReleaseSuspCust(string customerName, ISelenium browser)
        {
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.CustomersLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            // Enter Customer Name and Search
            SearchCustomer(customerName, browser);
            browser.Select("name=Status", "label=Active");
            browser.Select("name=StatusReasonType", "label=-- unset --");
            browser.Select("name=StatusReason", "label=-- unset --");
            browser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdateCustomerButton);
            _frameworkCommon.WaitUntilAllElementsLoad(browser);
            System.Threading.Thread.Sleep(10000);

            if (browser.IsTextPresent("Current Customer Status: Active"))
            {
                Console.WriteLine("AdminSuite:ReleaseSelfExcludedUser-Pass- Customer made Active");
            }
            else
            {
                Console.WriteLine("AdminSuite:ReleaseSelfExcludedUser-Fail- Customer remains Suspended");
            }

        }//end Test method

        /// <summary>
        /// Adding a Reward Adhoc token to customer in Admin.
        /// </summary>
        /// Date Created:
        /// Author: Anand C
        /// User should be login to Admin application
        /// <param name="adminBrowser">Browser Instance</param>
        /// <param name="custName">Customer name for whom the token be added</param>
        /// <param name="tokenVal">token value to be added</param>

        public bool AdminDeleteFBlist(ISelenium adminBrowser, string custName)
        {
            try
            {
                IWebDriver driver = ((WebDriverBackedSelenium)adminBrowser).UnderlyingWebDriver;
                //Verifying freebet link in customer detail page
                adminBrowser.Highlight("//table//th/font[contains(text(),'Account information')]//ancestor::table//td[contains(text(),'Free bet tokens (open):')]/following-sibling::td/a");
                if (adminBrowser.IsElementPresent("//table//th/font[contains(text(),'Account information')]//ancestor::table//td[contains(text(),'Free bet tokens (open):')]/following-sibling::td/a"))
                {
                    adminBrowser.Click("//table//th/font[contains(text(),'Account information')]//ancestor::table//td[contains(text(),'Free bet tokens (open):')]/following-sibling::td/a");
                    adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    ReadOnlyCollection<IWebElement> chkBoxs = driver.FindElements(By.XPath("//table//input[@type='checkbox']"));

                    //check all the check box
                    for (int chkBoxCount = 0; chkBoxCount < chkBoxs.Count; chkBoxCount++)
                    {
                        chkBoxs[chkBoxCount].Click();
                    }

                    //click on the Delete Token button
                    if (!AdminTableObjects(adminBrowser, "Free Token List", "", "Button", "Delete Token", 1))
                    {
                        Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                        Console.WriteLine("Unable click on Find Customer button in Customer Search page");
                        return false;
                    }
                    Thread.Sleep(3000);
                    if (adminBrowser.IsElementPresent("//table//th[contains(string(),'Account information')]//ancestor::table//td[contains(text(),'" + "Free bet tokens (open):" + "')]/following-sibling::td/a"))
                    {
                        return false;
                    }
                }
                Console.WriteLine("Successfully deleted the FreeBets for customer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }


        /// <summary>
        /// To update the Selection 
        /// </summary>
        /// <param name="browser">Selenium browser Instance</param>
        /// <param name="selectionId">ID of the selection</param>
        /// <param name="status"> Display Status to be updated</param>

        public void UpdateSelectionDisplayStatus(ISelenium browser, string selectionId, string displayStatus)
        {
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionId);
            browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
            browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            if (String.IsNullOrWhiteSpace(displayStatus) == false)
            {
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.SelectionDispStatusListBx, "label=" + displayStatus + "");
            }
            else
            {
                Fail("unable to update the Displayed status in admin as Displayed list box is not present in admin");
            }
            // Updating the event details page
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.SelectionUpdateBtn);
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
        }

        /// <summary>
        /// To Update Event Display Status
        /// </summary>
        /// <param name="browser">Selenium browser instance</param>
        /// <param name="catName">Category Name</param>
        /// <param name="eventClsName">Event ClassName</param>
        /// <param name="eventTypeName">Event TypeName</param>
        /// <param name="eventSubType">Event SubType</param>
        /// <param name="eventName">Event Name</param>
        /// <param name="eventStatus">Event Display Status</param>
        public void UpdateMarketDisplayStatus(ISelenium browser, string catName, string eventClsName, string eventTypeName, string eventSubType, string eventName, string mktName, string eventStatus)
        {
            string errorMessage = "";
            bool eventUpdationStatus = false;
            bool finalEventStatus = true;
            Framework.Common.Common frameworkcommon = new Framework.Common.Common();
            AdminSuite.Common com = new AdminSuite.Common();
            Framework.Common.Common Fcommon = new Framework.Common.Common();

            TimeSpan ts = new TimeSpan(0, 1, 0);
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            System.Threading.Thread.Sleep(10000);

            frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "60");
            catName = catName.Replace("|", "").Trim();
            if (catName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, catName), "Category Name does not present in DropdownList");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "label=" + catName);
            }
            eventClsName = eventClsName.Replace("|", "").Trim();

            if (eventClsName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, eventClsName), "EventClass Name does not present");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, "label=" + eventClsName);
            }
            if (eventTypeName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, eventTypeName), "EventClass Name does not present");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, "label=" + eventTypeName);
            }
            Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "--"), "Date range dropdown missing");

            // Selecting Daterange 
            browser.Select(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "label=--");

            //Clicking on Seach button
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.eventSearchBtn);
            _frameworkCommon.WaitUntilAllElementsLoad(browser);
            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

            // Wait for Element to present
            if (frameworkcommon.WaitUntilElementPresent(browser, "link=" + eventName + "", "60") == true)
            {
                browser.Click("link=" + eventName);
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Thread.Sleep(5000);
                //Wait Untill the EventDetails Page loads
                if (frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "60") == true)
                {

                    if (frameworkcommon.WaitUntilElementPresent(browser, "link=" + "|" + mktName + "|" + "", "60") == true)
                    {

                        //browser.Click("link=" + mktName);
                        browser.Click("link=" + "|" + mktName + "|");
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                        if (eventStatus == "No")
                        {
                            browser.Select(AdminSuite.CommonControls.EventDetailsPage.mktDisplayListBox, "label=No");
                            eventUpdationStatus = false;
                            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                        }
                        else
                        {
                            browser.Select(AdminSuite.CommonControls.EventDetailsPage.mktDisplayListBox, "label=Yes");
                        }

                        //Updating the Event
                        if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton))
                        {
                            if (eventUpdationStatus == false)
                            {
                                browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                                Assert.IsTrue(frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "markrt display status Updation is not Successfull");
                                browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                            }

                        }
                    }
                }
            }
            else
            {
                finalEventStatus = false;
                errorMessage = errorMessage + eventName;
                errorMessage = errorMessage + Environment.NewLine;
            }

            //Finally checking whether all the Events are Suspended or not
            if (finalEventStatus == false)
            {
                Framework.BaseTest.Fail("market display status updation Process is Failed");
            }

        }

        /// <summary>
        /// To Update event display status
        /// </summary>
        /// <param name="browser">Selenium browser instance</param>
        /// <param name="catName">Category Name</param>
        /// <param name="eventClsName">Event ClassName</param>
        /// <param name="eventTypeName">Event TypeName</param>
        /// <param name="eventSubType">Event SubType</param>
        /// <param name="eventName">Event Name</param>
        /// <param name="eventStatus">Event Status</param>
        public void UpdateEventsDisplaySts(ISelenium browser, string catName, string eventClsName, string eventTypeName, string eventSubType, string eventName, string eventDisplayStatus)
        {
            string errorMessage = "";
            bool eventUpdationStatus = false;
            bool finalEventStatus = true;
            Framework.Common.Common frameworkcommon = new Framework.Common.Common();
            AdminSuite.Common com = new AdminSuite.Common();
            Framework.Common.Common Fcommon = new Framework.Common.Common();



            TimeSpan ts = new TimeSpan(0, 1, 0);
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            System.Threading.Thread.Sleep(10000);


            catName = catName.Replace("|", "").Trim();
            if (catName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, catName), "Category Name does not present in DropdownList");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "label=" + catName);
            }
            eventClsName = eventClsName.Replace("|", "").Trim();

            if (eventClsName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, eventClsName), "EventClass Name does not present");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, "label=" + eventClsName);
            }
            if (eventTypeName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, eventTypeName), "EventClass Name does not present");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, "label=" + eventTypeName);
            }
            Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "--"), "Date range dropdown missing");

            // Selecting Daterange 
            browser.Select(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "label=--");

            //Clicking on Seach button
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.eventSearchBtn);
            _frameworkCommon.WaitUntilAllElementsLoad(browser);
            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

            // Wait for Element to present
            if (frameworkcommon.WaitUntilElementPresent(browser, "link=" + eventName + "", "60") == true)
            {
                browser.Click("link=" + eventName);
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                //Wait Untill the EventDetails Page loads
                if (frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "60") == true)
                {
                    if (eventDisplayStatus == "No")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=No");
                        eventUpdationStatus = false;
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    }
                    else
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=Active");
                    }

                    //Updating the Event
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn))
                    {
                        if (eventUpdationStatus == false)
                        {
                            browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                            Assert.IsTrue(frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                        }


                    }
                }
            }
            else
            {
                finalEventStatus = false;
                errorMessage = errorMessage + eventName;
                errorMessage = errorMessage + Environment.NewLine;
            }
            //Finally checking whether all the Events are Suspended or not
            if (finalEventStatus == false)
            {
                Framework.BaseTest.Fail("Settinf=g event display status process is failed");
            }

        }

        /// <summary>
        /// Update the event by market id in open bet.
        /// </summary>
        /// <param name="browser">Browser instance</param>
        /// <param name="marketId">market id</param>
        /// <param name="eventStatus">status</param>
        /// <param name="eventDisplayed">displayed</param>
        public void UpdateEventsByMarketID(ISelenium browser, string marketId, string eventStatus, string eventDisplayed)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, marketId);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, "label=Event market");// AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton);//click on back button present in the selection page
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

                if (string.Empty != eventStatus)
                {
                    if (eventStatus == "Suspend")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Suspended");
                    }
                    else if (eventStatus == "Active")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Active");
                    }
                    else
                    {
                        throw new AutomationException("Event status is invalid.");
                    }
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }

                if (string.Empty != eventDisplayed)
                {
                    if (eventDisplayed == "Yes")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=Yes");
                    }
                    else if (eventDisplayed == "No")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=No");
                    }
                    else
                    {
                        throw new AutomationException("Event displayed is invalid.");
                    }
                }

                //Updating the Event
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn))
                {
                    browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    Assert.IsTrue(_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                //return null;
            }
        }

        public void UpdateEventsByEventID(ISelenium browser, string eventID, string eventStatus, string eventDisplayed)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, eventID);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyLevelEvent);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

                if (string.Empty != eventStatus)
                {
                    if (eventStatus == "Suspend")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Suspended");
                    }
                    else if (eventStatus == "Active")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Active");
                    }
                    else
                    {
                        throw new AutomationException("Event status is invalid.");
                    }
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }

                if (string.Empty != eventDisplayed)
                {
                    if (eventDisplayed == "Yes")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=Yes");
                    }
                    else if (eventDisplayed == "No")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=No");
                    }
                    else
                    {
                        throw new AutomationException("Event displayed is invalid.");
                    }
                }

                //Updating the Event
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn))
                {
                    browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    Assert.IsTrue(_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                //return null;
            }
        }

        /// <summary>
        /// Update market by Market id in Open bet
        /// </summary>
        /// <param name="browser">Browser instance</param>
        /// <param name="marketId">Market id</param>
        /// <param name="marketStatus">Market status</param>
        /// <param name="marketDisplayed">Display</param>
        public void UpdateMarketByMarketID(ISelenium browser, string marketId, string marketStatus, string marketDisplayed)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                SelectMainFrame(browser);
                browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
                browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, marketId);
                browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, "label=Event market");// AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                //browser.Click(AdminSuite.CommonControls.EventDetailsPage.BackButton);//click on back button present in the selection page
                //browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);

                if (string.Empty != marketStatus)
                {
                    if (marketStatus == "Suspend")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.marketStatusListBox, "label=Suspended");
                    }
                    else if (marketStatus == "Active")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.marketStatusListBox, "label=Active");
                    }
                    else
                    {
                        throw new AutomationException("Market status is invalid.");
                    }
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }

                if (string.Empty != marketDisplayed)
                {
                    if (marketDisplayed == "Yes")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.mktDisplayListBox, "label=Yes");
                    }
                    else if (marketDisplayed == "No")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.mktDisplayListBox, "label=No");
                    }
                    else
                    {
                        throw new AutomationException("Market displayed is invalid.");
                    }
                }

                //update Market
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton))
                {
                    browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    Assert.IsTrue(_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Market Updation is not Successfull");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                //return null;
            }
        }

        /// <summary>
        /// To Update event display status y and event status n
        /// </summary>
        /// <param name="browser">Selenium browser instance</param>
        /// <param name="catName">Category Name</param>
        /// <param name="eventClsName">Event ClassName</param>
        /// <param name="eventTypeName">Event TypeName</param>
        /// <param name="eventSubType">Event SubType</param>
        /// <param name="eventName">Event Name</param>
        /// <param name="eventStatus">Event Status</param>
        public void UpdateEventsDisplayStsYEventStsN(ISelenium browser, string catName, string eventClsName, string eventTypeName, string eventSubType, string eventName, string eventDisplayStatus, string eventStatus)
        {
            string errorMessage = "";
            bool eventUpdationStatus = false;
            bool finalEventStatus = true;
            Framework.Common.Common frameworkcommon = new Framework.Common.Common();
            AdminSuite.Common com = new AdminSuite.Common();
            Framework.Common.Common Fcommon = new Framework.Common.Common();

            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            System.Threading.Thread.Sleep(10000);


            catName = catName.Replace("|", "").Trim();
            if (catName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, catName), "Category Name does not present in DropdownList");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "label=" + catName);
            }
            eventClsName = eventClsName.Replace("|", "").Trim();

            if (eventClsName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, eventClsName), "EventClass Name does not present");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, "label=" + eventClsName);
            }
            if (eventTypeName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, eventTypeName), "EventClass Name does not present");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, "label=" + eventTypeName);
            }
            Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "--"), "Date range dropdown missing");

            // Selecting Daterange 
            browser.Select(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "label=--");

            //Clicking on Seach button
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.eventSearchBtn);
            _frameworkCommon.WaitUntilAllElementsLoad(browser);
            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

            // Wait for Element to present
            if (frameworkcommon.WaitUntilElementPresent(browser, "link=" + eventName + "", "60") == true)
            {
                browser.Click("link=" + eventName);
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                //Wait Untill the EventDetails Page loads
                if (frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "60") == true)
                {
                    if (eventDisplayStatus == "No")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=No");
                        eventUpdationStatus = false;
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    }
                    else
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=Yes");
                    }
                    //Wait Untill the EventDetails Page loads

                    if (eventStatus == "Suspend")
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Suspended");
                        eventUpdationStatus = false;
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    }
                    else
                    {
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Active");
                    }

                    //Updating the Event
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn))
                    {
                        if (eventUpdationStatus == false)
                        {
                            browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                            Assert.IsTrue(frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                        }


                    }
                }
            }
            else
            {
                finalEventStatus = false;
                errorMessage = errorMessage + eventName;
                errorMessage = errorMessage + Environment.NewLine;
            }
            //Finally checking whether all the Events are Suspended or not
            if (finalEventStatus == false)
            {
                Framework.BaseTest.Fail("Settinf=g event display status process is failed");
            }

        }


        /// <summary>
        /// To Update Markt  Status
        /// </summary>
        /// <param name="browser">Selenium browser instance</param>
        /// <param name="catName">Category Name</param>
        /// <param name="eventClsName">Event ClassName</param>
        /// <param name="eventTypeName">Event TypeName</param>
        /// <param name="eventSubType">Event SubType</param>
        /// <param name="eventName">Event Name</param>
        /// <param name="eventStatus">Event Display Status</param>
        public void UpdateMarketStatus(ISelenium browser, string catName, string eventClsName, string eventTypeName, string eventSubType, string eventName, string mktName, string marketStatus)
        {
            string errorMessage = "";
            bool eventUpdationStatus = false;
            bool finalEventStatus = true;
            Framework.Common.Common frameworkcommon = new Framework.Common.Common();
            AdminSuite.Common com = new AdminSuite.Common();
            Framework.Common.Common Fcommon = new Framework.Common.Common();



            TimeSpan ts = new TimeSpan(0, 1, 0);
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            System.Threading.Thread.Sleep(10000);

            frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "60");
            catName = catName.Replace("|", "").Trim();
            if (catName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, catName), "Category Name does not present in DropdownList");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "label=" + catName);
            }
            eventClsName = eventClsName.Replace("|", "").Trim();

            if (eventClsName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, eventClsName), "EventClass Name does not present");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, "label=" + eventClsName);
            }
            if (eventTypeName != "")
            {
                Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, eventTypeName), "EventClass Name does not present");
                browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, "label=" + eventTypeName);
            }
            Assert.IsTrue(frameworkcommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "--"), "Date range dropdown missing");

            // Selecting Daterange 
            browser.Select(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "label=--");

            //Clicking on Seach button
            browser.Click(AdminSuite.CommonControls.EventDetailsPage.eventSearchBtn);
            _frameworkCommon.WaitUntilAllElementsLoad(browser);
            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

            // Wait for Element to present
            if (frameworkcommon.WaitUntilElementPresent(browser, "link=" + eventName + "", "60") == true)
            {
                browser.Click("link=" + eventName);
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Thread.Sleep(10000);
                //Wait Untill the EventDetails Page loads
                if (frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "60") == true)
                {

                    if (frameworkcommon.WaitUntilElementPresent(browser, "link=" + "|" + mktName + "|" + "", "60") == true)
                    {
                        //browser.Click("link=" + mktName);
                        browser.Click("link=" + "|" + mktName + "|");
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                        if (marketStatus == "Suspend")
                        {
                            browser.Select(AdminSuite.CommonControls.EventDetailsPage.marketStatusListBox, "label=Suspended");
                            eventUpdationStatus = false;
                            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                        }
                        else
                        {
                            browser.Select(AdminSuite.CommonControls.EventDetailsPage.marketStatusListBox, "label=Active");
                        }

                        //Updating the Event
                        if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton))
                        {
                            if (eventUpdationStatus == false)
                            {
                                browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                                Assert.IsTrue(frameworkcommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "markrt display status Updation is not Successfull");
                                browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                            }
                        }
                    }
                }
            }
            else
            {
                finalEventStatus = false;
                errorMessage = errorMessage + eventName;
                errorMessage = errorMessage + Environment.NewLine;
            }

            //Finally checking whether all the Events are Suspended or not
            if (finalEventStatus == false)
            {
                Framework.BaseTest.Fail("market display status updation Process is Failed");
            }

        }

        /// <summary>
        /// Goto Event details page by Event id
        /// </summary>
        /// <param name="browser">Browser instance</param>
        /// <param name="testData">TestDAta</param>
        /// <param name="eventId">Event id</param>
        public void GogoEventPageByEventId(ISelenium browser, TestData testData, string eventId)
        {
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
            //Selecting TopFrame
            SelectMainFrame(browser);
            System.Threading.Thread.Sleep(10000);

            browser.WaitForFrameToLoad(AdminSuite.CommonControls.AdminHomePage.EventNameLink, FrameGlobals.PageLoadTimeOut);
            browser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, eventId);
            browser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, "label=Event");// AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
            browser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
            //_frameworkCommon.WaitUntilAllElementsLoad(browser);
            browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            Thread.Sleep(3000);
        }

        /// <summary>
        /// Verifying event details data in openbet
        /// </summary>
        /// <param name="browser">Browser instance</param>
        /// <param name="testData">Test data</param>
        public void VerifyEventData(ISelenium browser, TestData testData)
        {
            //IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

            string eventStatus = string.Empty;
            string eventDisplay = string.Empty;
            bool updateEventFlg = false;
            //Wait Untill the EventDetails Page loads
            if (_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "60") == true)
            {

                eventStatus = browser.GetValue(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox);

                eventDisplay = browser.GetValue(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx);

                if ("a" != eventStatus.ToLower())
                {
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventStatusListBox, "label=Active");
                    Console.WriteLine("Updated Event status to Active");
                    updateEventFlg = true;
                }

                if ("y" != eventDisplay.ToLower())
                {
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventDisplayStatusLstBx, "label=Yes");
                    Console.WriteLine("Updated Event display to Yes");
                    updateEventFlg = true;
                }

                if (updateEventFlg)
                {
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn))
                    {
                        browser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                        Assert.IsTrue(_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                    }
                    else
                    {
                        Console.WriteLine("Unable to update the event in Openbet");
                    }
                }
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            }
        }

        /// <summary>
        /// Goto market page from the event page
        /// </summary>
        /// <param name="browser">browser instance</param>
        /// <param name="testData">test data</param>
        public void GotoMarketPageFromEventPage(ISelenium browser, TestData testData)
        {
            if (browser.IsElementPresent(string.Format("link=|{0}|", testData.MarketName)))
            {
                browser.Click(string.Format("link=|{0}|", testData.MarketName));
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            }
            else
            {
                Console.WriteLine("Market is not available");
            }
            Thread.Sleep(3000);
        }

        /// <summary>
        /// Goto selection page from the market page
        /// </summary>
        /// <param name="browser">browser instance</param>
        /// <param name="testData">Test data</param>
        public void GotoSelectionPageFromMarketPage(ISelenium browser, TestData testData)
        {
            if (browser.IsElementPresent(string.Format("link={0}", testData.SelectionName)))
            {
                browser.Click(string.Format("link={0}", testData.SelectionName));
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            }
            else
            {
                Console.WriteLine("Market is not available");
            }
            Thread.Sleep(3000);
            browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
        }

        /// <summary>
        /// Verifying market data in openbet
        /// </summary>
        /// <param name="browser">Browser instance</param>
        /// <param name="testData">Test data</param>
        public void VerifyMarketData(ISelenium browser, TestData testData)
        {
            string marketStatus = string.Empty;
            string marketDisplay = string.Empty;
            bool updateMarketFlg = false;

            if (_frameworkCommon.WaitUntilElementPresent(browser, "name=MktName", "60") == true)
            {

                marketStatus = browser.GetValue(AdminSuite.CommonControls.EventDetailsPage.marketStatusListBox);

                marketDisplay = browser.GetValue(AdminSuite.CommonControls.EventDetailsPage.mktDisplayListBox);

                if ("a" != marketStatus.ToLower())
                {
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.marketStatusListBox, "label=Active");
                    Console.WriteLine("Updated Market status to Active");
                    updateMarketFlg = true;
                }

                if ("y" != marketDisplay.ToLower())
                {
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.mktDisplayListBox, "label=Yes");
                    Console.WriteLine("Updated market display to Yes");
                    updateMarketFlg = true;
                }

                if (updateMarketFlg)
                {
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton))
                    {
                        browser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifyMarketButton);
                        browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                        Assert.IsTrue(_frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, "120"), "Event Updation is not Successfull");
                    }
                    else
                    {
                        Console.WriteLine("Unable to update the event in Openbet");
                    }
                }
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            }
        }

        /// <summary>
        /// Verifying selection data
        /// </summary>
        /// <param name="browser">Browser instance</param>
        /// <param name="testData">TestData</param>
        public void VerifySelectionData(ISelenium browser, TestData testData)
        {
            string selectionStatus = string.Empty;
            string selectionDisplay = string.Empty;
            string selectionPrice = string.Empty;
            bool updateSelectionFlg = false;

            if (_frameworkCommon.WaitUntilElementPresent(browser, "name=OcDesc", "60") == true)
            {
                selectionStatus = browser.GetValue(AdminSuite.CommonControls.EventDetailsPage.SelectionStatusLstBx);

                selectionDisplay = browser.GetValue(AdminSuite.CommonControls.EventDetailsPage.SelectionDispStatusListBx);

                selectionPrice = browser.GetValue(AdminSuite.CommonControls.EventDetailsPage.PriceTxtBx);

                if ("a" != selectionStatus.ToLower())
                {
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.SelectionStatusLstBx, "label=Active");
                    Console.WriteLine("Updated selection status to Active");
                    updateSelectionFlg = true;
                }

                if ("y" != selectionDisplay.ToLower())
                {
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.SelectionDispStatusListBx, "label=Yes");
                    Console.WriteLine("Updated selection display to Yes");
                    updateSelectionFlg = true;
                }

                if (testData.Odds != selectionPrice)
                {
                    browser.Type(AdminSuite.CommonControls.EventDetailsPage.PriceTxtBx, testData.Odds);
                    Console.WriteLine("Updated selection price from test data");
                    updateSelectionFlg = true;
                }

                if (updateSelectionFlg)
                {
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.SelectionUpdateBtn))
                    {
                        browser.Click(AdminSuite.CommonControls.EventDetailsPage.SelectionUpdateBtn);
                        browser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                    }
                    else
                    {
                        Console.WriteLine("Unable to update the event in Openbet");
                    }

                    Thread.Sleep(3000);
                }
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            }
        }

        /// <summary>
        /// Verify and update the event/market/selection details in openbet
        /// </summary>
        /// <param name="browser">Browser instance</param>
        /// <param name="testData">Test data</param>
        /// <param name="eventId">Event id</param>
        public void VerifyAndUpdateEventDetailsInOpenBet(ISelenium browser, TestData testData, string eventId)
        {
            try
            {
                GogoEventPageByEventId(browser, testData, eventId);
                VerifyEventData(browser, testData);
                Thread.Sleep(5000);

                GogoEventPageByEventId(browser, testData, eventId);
                GotoMarketPageFromEventPage(browser, testData);
                VerifyMarketData(browser, testData);
                Thread.Sleep(5000);

                GogoEventPageByEventId(browser, testData, eventId);
                GotoMarketPageFromEventPage(browser, testData);
                GotoSelectionPageFromMarketPage(browser, testData);
                VerifySelectionData(browser, testData);
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Issues with checking the openbet.  Message: " + ex.Message);
            }
        }

        /// <summary>
        /// Adding a Reward Adhoc token to customer in Admin.
        /// </summary>
        /// Date Created:
        /// Author: Anand C
        /// User should be login to Admin application
        /// <param name="adminBrowser">Browser Instance</param>
        /// <param name="custName">Customer name for whom the token be added</param>
        /// <param name="tokenVal">token value to be added</param>
        public bool CardSearch(ISelenium adminBrowser, long cardNum, string status, string CustomerName)
        {
            try
            {
                IWebDriver adminDriver = ((WebDriverBackedSelenium)adminBrowser).UnderlyingWebDriver;
                //Click on Reward Adhoc token link in Admin LHN
                if (AdminLHNClick("Pay Methods", adminBrowser))
                {
                    Console.WriteLine("Successfully clicked on the Admin LHN link Customers");
                    adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }
                else
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on the Admin LHN link");
                    return false;
                }

                //Entering value in Username Text box in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Card Search", "CardNo", "Text Box", Convert.ToString(cardNum), 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Card Search Text box in Card Search page");
                    return false;
                }
                //adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                //click on the Find Customer button
                if (!AdminTableObjects(adminBrowser, "Card Search", "", "Button", "Find Methods", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable click on Find Customer button in Customer Search page");
                    return false;
                }
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                ReadOnlyCollection<IWebElement> tableRows = adminDriver.FindElements(By.XPath("//table//tr[td[contains(.,'Active')]]"));

                //looping through a particular row
                for (int rowNum = 0; rowNum < tableRows.Count; rowNum++)
                {
                    if (tableRows[rowNum].Text.IndexOf("Active") > 0 && tableRows[rowNum].Text.IndexOf(CustomerName) < 0)
                    {
                        IWebElement custName = tableRows[rowNum].FindElement(By.TagName("a"));
                        custName.Click();
                        Thread.Sleep(3000);
                        adminBrowser.WaitForPageToLoad("5000");
                        if (!UpdatePayMtd(adminBrowser, cardNum.ToString(), status, CustomerName))
                        {
                            Console.WriteLine("Unable to remove card for the customer");
                            return false;
                        }
                        Console.WriteLine("Successfully searched the customer");
                        return true;
                    }
                }
                Console.WriteLine("Successfully searched the customer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }


        /// <summary>
        /// Adding a Reward Adhoc token to customer in Admin.
        /// </summary>
        /// Date Created:
        /// Author: Anand C
        /// User should be login to Admin application
        /// <param name="adminBrowser">Browser Instance</param>
        /// <param name="custName">Customer name for whom the token be added</param>
        /// <param name="tokenVal">token value to be added</param>

        public bool UpdatePayMtd(ISelenium adminBrowser, string cardNum, string status, string CustomerName)
        {
            try
            {
                //Click on Debit/Credit link
                /*if (!AdminTableObjects(adminBrowser, "Customer Payment Methods", cardNum.Substring(cardNum.Length - 4, 4), "Link", "", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser);
                    Console.WriteLine("Unable to put value in Card Search Text box in Card Search page");
                    return false;
                }*/
                Assert.IsTrue(adminBrowser.IsElementPresent("//table//th[contains(text(),'Customer Payment Methods')]//ancestor::table/tbody/tr[td[contains(.,'" + cardNum.Substring(cardNum.Length - 4, 4) + "')]]//a"), cardNum + " card is not available for this customer: " + CustomerName);
                adminBrowser.Click("//table//th[contains(text(),'Customer Payment Methods')]//ancestor::table/tbody/tr[td[contains(.,'" + cardNum.Substring(cardNum.Length - 4, 4) + "')]]//a");
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                //
                Assert.IsTrue(adminBrowser.IsElementPresent("//select[@name='status']"), "Authorisation status list box is not visible");
                Assert.IsTrue(adminBrowser.IsElementPresent("//select[@name='auth_dep']"), "Authorisation deposit list box is not visible");
                Assert.IsTrue(adminBrowser.IsElementPresent("//select[@name='auth_wtd']"), "Authorisation withdrawal list box is not visible");
                adminBrowser.Select("//select[@name='status']", status);
                adminBrowser.Select("//select[@name='auth_dep']", "Yes");
                adminBrowser.Select("//select[@name='status_dep']", "Active");
                adminBrowser.Select("//select[@name='auth_wtd']", "Yes");
                adminBrowser.Select("//select[@name='status_wtd']", "Active");

                //click on the Update Payment Method button
                if (!AdminTableObjects(adminBrowser, "Authorisation", "", "Button", "Update Payment Method", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable click on  Payment Method button in Customer Payment page");
                    return false;
                }
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Thread.Sleep(5000);
                Console.WriteLine("Successfully searched the customer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }

        /// <summary>
        /// Adding a Reward Adhoc token to customer in Admin.
        /// </summary>
        /// Date Created:
        /// Author: Anand C
        /// User should be login to Admin application
        /// <param name="adminBrowser">Browser Instance</param>
        /// <param name="custName">Customer name for whom the token be added</param>
        /// <param name="tokenVal">token value to be added</param>

        public bool addDebitCreditCard(ISelenium adminBrowser, string cardNum, string status, string CustomerName)
        {
            try
            {
                if (adminBrowser.IsElementPresent("//table//th[contains(text(),'Customer Payment Methods')]//ancestor::table/tbody/tr[td[contains(.,'" + cardNum.Substring(cardNum.Length - 4, 4) + "')]]//a"))
                {
                    if (!UpdatePayMtd(adminBrowser, cardNum, "Active", CustomerName))
                    {
                        Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                        Console.WriteLine("Unable update the payment method");
                        return false;
                    }
                }
                else
                {
                    if (!addDebitCard(adminBrowser, cardNum, "", status, CustomerName))
                    {
                        Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                        Console.WriteLine("Unable to Add Payment method");
                        return false;
                    }
                }
                Console.WriteLine("Successfully Added Payment method");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }

        /// <summary>
        /// Adding a Reward Adhoc token to customer in Admin.
        /// </summary>
        /// Date Created:
        /// Author: Anand C
        /// User should be login to Admin application
        /// <param name="adminBrowser">Browser Instance</param>
        /// <param name="custName">Customer name for whom the token be added</param>
        /// <param name="tokenVal">token value to be added</param>

        public bool addDebitCard(ISelenium adminBrowser, string cardNum, string issueNum, string status, string CustomerName)
        {
            try
            {
                //Click on Add new payment method button
                if (!AdminTableObjects(adminBrowser, "Customer Payment Methods", "", "Button", "Add new payment method", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on Customer Payment Methods button in Add new payment method page");
                    return false;
                }
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                //Select Debit/Credit in select box
                if (!AdminTableObjects(adminBrowser, "Add New Payment Method", "Method", "Select Box", "Debit/Credit Card", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to select value in Add New Payment Method in Add New Payment Method page");
                    return false;
                }

                //Click on Debit/Credit link
                if (!AdminTableObjects(adminBrowser, "Add New Payment Method", "", "Button", "Add Method", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable click on Add Method button in New Payment Method page");
                    return false;
                }
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                //Entering value in Username Text box in Reward Adhoc token page
                if (!AdminTableObjects(adminBrowser, "Add payment method", "Card Number", "Text Box", Convert.ToString(cardNum), 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Card Number Text box in Add New Payment Method page");
                    return false;
                }

                //Entering value in Issue Number Text box in Add New Payment Method page
                if (!AdminTableObjects(adminBrowser, "Add payment method", "Start Date", "Text Box", "11/11", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Start Date Text box in Add New Payment Method page");
                    return false;
                }

                //Entering value in Issue Number Text box in Add New Payment Method page
                if (!AdminTableObjects(adminBrowser, "Add payment method", "Expiry Date", "Text Box", "10/14", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Expiry Date Text box in Add New Payment Method page");
                    return false;
                }

                //Entering value in Issue Number Text box in Add New Payment Method page
                if (!AdminTableObjects(adminBrowser, "Add payment method", "Issue Number", "Text Box", issueNum, 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Issue Number Text box in Add New Payment Method page");
                    return false;
                }

                //Entering value in Issue Number Text box in Add New Payment Method page
                if (!AdminTableObjects(adminBrowser, "Add payment method", "Cardholder Name", "Text Box", "Test", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Cardholder Name Text box in Add New Payment Method page");
                    return false;
                }

                //Entering value in Issue Number Text box in Add New Payment Method page
                if (!AdminTableObjects(adminBrowser, "Add payment method", "Card Name", "Text Box", "Test", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Card Name Text box in Add New Payment Method page");
                    return false;
                }


                //Click on Add Payment Method button
                if (!AdminTableObjects(adminBrowser, "Add payment method", "", "Button", "Add Payment Method", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable click on Add Payment Method button in New Payment Method page");
                    return false;
                }

                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Thread.Sleep(5000);
                Console.WriteLine("Successfully added payment method for customer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }

        /// <summary>
        /// Adding a Reward Adhoc token to customer in Admin.
        /// </summary>
        /// Date Created:
        /// Author: Anand C
        /// User should be login to Admin application
        /// <param name="adminBrowser">Browser Instance</param>
        /// <param name="custName">Customer name for whom the token be added</param>
        /// <param name="tokenVal">token value to be added</param>

        public bool AgeVerification(ISelenium adminBrowser, string cardNum, string issueNum, string CustomerName)
        {
            try
            {
                Framework.Common.Common FrameWKCommon = new Framework.Common.Common();
                //Click on Customer Verification button
                if (!AdminTableObjects(adminBrowser, "Customer attributes", "", "Button", "Customer Verification", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to click on Customer Verification button in Customer attributes table");
                    return false;
                }


                Assert.IsTrue(adminBrowser.IsElementPresent(AdminSuite.CommonControls.AdminHomePage.custVerifyCountry), "Unable to find country select box in customer verify page");
                Assert.IsTrue(adminBrowser.IsElementPresent("//input[@type='radio' and @value = 'A']"), "Unable to find country select box in customer verify page");

                adminBrowser.Click("//input[@type='radio' and @value = 'A']");

                //Entering value in OVS Score: Text box in Customer verification page
                if (!AdminTableObjects(adminBrowser, "Manually Verify Customer", "OVS Score", "Text Box", "100", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in OVS Score Text box in Manually Verify Customer page");
                    return false;
                }

                //Entering value in Reference: Text box in Customer verification page
                if (!AdminTableObjects(adminBrowser, "Manually Verify Customer", "Reference", "Text Box", "100", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Reference Text box in Manually Verify Customer page");
                    return false;
                }

                //Entering value in Notes Text box in Customer verification page
                if (!AdminTableObjects(adminBrowser, "Manually Verify Customer", "Notes", "Text Box", "Notes", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Notes Text box in Manually Verify Customer page");
                    return false;
                }

                Assert.IsTrue(adminBrowser.IsElementPresent("//input[@type='radio' and @value = 'A']"), "Unable to find country select box in customer verify page");
                adminBrowser.Click("//input[@type='radio' and @value = 'A']");

                //Entering value in Driving Licence No Text box in Customer verification page
                if (!AdminTableObjects(adminBrowser, "Manually Verify Customer", "Driving Licence No", "Text Box", "Notes", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable to put value in Driving Licence No Text box in Manually Verify Customer page");
                    return false;
                }

                //Click on Add Payment Method button
                if (!AdminTableObjects(adminBrowser, "Add payment method", "", "Button", "Add Payment Method", 1))
                {
                    Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                    Console.WriteLine("Unable click on Add Payment Method button in New Payment Method page");
                    return false;
                }


                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Thread.Sleep(5000);
                Console.WriteLine("Successfully added payment method for customer");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Framework.BaseTest.CaptureScreenshot(adminBrowser, "");
                return false;
            }
        }
        /// <summary>
        /// Update Faceted flag
        /// </summary>
        /// Authour: Revathy 
        /// Created Date: 28-Mar-2012

        public void UpdateFacetedflag(string facet1, string facet2, string Eventclass)
        {
            var admincommonObj = new AdminSuite.Common();
            ISelenium adminBrowser = admincommonObj.LogOnToAdmin();
            IWebDriver driver = ((WebDriverBackedSelenium)adminBrowser).UnderlyingWebDriver;
            LHNavigation("//div[@id='adminMenuDiv']//a[@class='menu_item' and text()='Event Classes']", adminBrowser);
            SelectMainFrame(adminBrowser);
            adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            driver.FindElement(By.XPath("//a[text()='|Horse Racing|']")).Click();
            adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            adminBrowser.Click("//a[text()='|" + Eventclass + "|']");
            adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            if (!adminBrowser.IsChecked(facet1))
                adminBrowser.Click(facet1);
            if (adminBrowser.IsChecked(facet2))
                adminBrowser.Click(facet2);
            //click on Modify Event Type button
            adminBrowser.Click("//th[@class='buttons']/input[@value='Modify Event Type']");
        }

        /// <Summary>
        /// Show all Horse Racing Events that starts "Tomorrow"
        /// Author: Aswathy 
        /// Created Date: Apr-19-2013
        /// </Summary>
        public void ShowTomoHorseRacingEvents(ISelenium adminBrowser)
        {
            Framework.Common.Common frameworkcommon = new Framework.Common.Common();
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, adminBrowser);
            //Selecting TopFrame
            SelectMainFrame(adminBrowser);
            System.Threading.Thread.Sleep(10000);
            adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "label=RACING");
            adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, "label=Horse Racing");
            // adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, "label=" + eventTypeName);
            // adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.subEventTypeLstBx, "label=" + eventSubType);
            adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "label=Tomorrow");
            adminBrowser.Select("name=status", "label=Active");
            adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.showEventBtn);
            adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
        }

        /// <Summary>
        /// Update Horse Racing event start date to tomorrow's date
        /// Author: Aswathy 
        /// Created Date: Apr-19-2013
        /// </Summary>


        /*
        public void UpdateEventStartDate(ISelenium adminBrowser, string eventID, DateTime date)
        {
            string x = date.ToString("yyyy-MM-dd hh:mm:ss"); //Eg:2013-04-29 15:00:00
            //Clicking on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, adminBrowser);
            //Selecting TopFrame
            SelectMainFrame(adminBrowser);
            adminBrowser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, eventID);
            adminBrowser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyLevelEvent);
            adminBrowser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
            adminBrowser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            adminBrowser.Type("//input[@id='start_time']", date.ToString());
            adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
            adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
        }
        */

        /// <Summary>
        /// Update Horse Racing event start date to tomorrow's date
        /// Author: Aswathy 
        /// Created Date: Apr-26-2013
        /// </Summary>
        /// 
        public void UpdateOdds(ISelenium adminBrowser, string eventID, string newOddVal)
        {
            // Click on Event Link in LHN
            LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, adminBrowser);
            SelectMainFrame(adminBrowser);
            adminBrowser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, eventID);
            adminBrowser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
            adminBrowser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
            adminBrowser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
            // Updating the price
            adminBrowser.Type(AdminSuite.CommonControls.EventDetailsPage.PriceTxtBx, newOddVal);
            adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.ModifySelectionButton);
            adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
        }

        /// <Summary>
        /// Create Horse Racing event of Place Betting Type
        /// Author: Aswathy 
        /// Created Date: Apr-26-2013
        /// </Summary>
        /// 
        public string CreateEvent(ISelenium adminBrowser, string eventCategory, string eventClass, string eventType, string eventSubType, string eventName)
        {

            try
            {
                adminBrowser = LogOnToAdmin();
                // Click on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, adminBrowser);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                SelectMainFrame(adminBrowser);
                if (eventCategory != "")
                {
                    adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "label=" + eventCategory);
                }
                if (eventClass != "")
                {
                    adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, "label=" + eventClass);
                }
                if (eventType != "")
                {
                    adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, "label=" + eventType);
                }
                if (eventSubType != "")
                {
                    adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.subEventTypeLstBx, "label=" + eventSubType);
                }
                adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.addEventBtn);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                DateTime date = DateTime.Now.AddHours(24);
                int appendVal = DateTime.Now.Minute;
                string eventStartDate = date.ToString("yyyy-MM-dd hh:mm:ss");
                adminBrowser.Type(AdminSuite.CommonControls.EventDetailsPage.eventDescriptionTextBox, eventName + appendVal + " ");
                adminBrowser.Type(AdminSuite.CommonControls.EventDetailsPage.StartTimetxtBox, eventStartDate);
                adminBrowser.Highlight(AdminSuite.CommonControls.EventDetailsPage.addEventBtn);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.addEventMarketBtn);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.mktDisplayStatusLstBox, "label=Yes");
                adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.marketEWAvailLstBox, "label=Yes");
                adminBrowser.Type("//input[@name='MktEWPlaces']", "2");
                adminBrowser.Type("//input[@name='MktEWFacNum']", "1");
                adminBrowser.Type("//input[@name='MktEWFacNum']", "4");
                adminBrowser.Select("//select[@name='MktPLAvail']", "label=Yes");
                adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.addMarketBtn);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                int i;
                string[] selectionName = { "S1", "S2", "S3" };
                string[] price = { "1.25", "1.50", "1.75" };
                for (i = 0; i < 3; i++)
                {
                    adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.addMarketSelectionBtn);
                    adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    adminBrowser.Type(AdminSuite.CommonControls.EventDetailsPage.selectionDescTxtBox, selectionName[i]);
                    adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.SelectionDispStatusListBx, "label=Yes");
                    adminBrowser.Select(AdminSuite.CommonControls.EventDetailsPage.SelectionStatusLstBx, "label=Active");
                    adminBrowser.Type(AdminSuite.CommonControls.EventDetailsPage.PriceTxtBx, price[i]);
                    adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.addSelectionBtn);
                    adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                }
                return (eventName + appendVal);
            }
            catch (Exception ex)
            {
                CaptureScreenshot(adminBrowser, "");
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Update a Selection result
        /// Author: Aswathy 
        /// Created Date: Jun-10-2013
        /// </summary>

        public void SetSelectionResult(ISelenium adminBrowser, string selectionID, string resultStatus)
        {
            try
            {
                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, adminBrowser);
                //Selecting TopFrame
                SelectMainFrame(adminBrowser);
                adminBrowser.Type(AdminSuite.CommonControls.AdminHomePage.OpenBetIdTextBox, selectionID);
                adminBrowser.Select(AdminSuite.CommonControls.AdminHomePage.OpenBetHierarchyLevelDrpLst, AdminSuite.CommonControls.AdminHomePage.OpenBetHeierarchyName);
                adminBrowser.Click(AdminSuite.CommonControls.AdminHomePage.EventFindBtn);
                adminBrowser.WaitForPageToLoad(Framework.FrameGlobals.PageLoadTimeOut);
                adminBrowser.Select(AdminSuite.CommonControls.AdminHomePage.ResultStatus, "label=" + resultStatus + "");
                adminBrowser.Click("//input[@type='button' and @value='Set Selection Results']");
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
            }
            catch (Exception ex)
            {
                CaptureScreenshot(adminBrowser, "");
                Console.WriteLine(ex);
            }
        }


        /*
        /// <Summary>
        /// Update Horse Racing event start date 
        /// Author: Revathy 
        /// Created Date: Apr-30-2013
        /// </Summary>
        public string UpdateEventStartDateTime(ISelenium adminBrowserObj, string eventid, string date)
        {
            string EventStartTime = GetEventStartTimeByEventID(adminBrowserObj, eventid);
            SelectMainFrame(adminBrowserObj);
            adminBrowserObj.Type("//input[@name='EvStartTime']", date);
            adminBrowserObj.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
            return EventStartTime;
        }
         */

        /// <Summary>
        /// Update Horse Racing event Early price status 
        /// Author: Revathy 
        /// Created Date: Apr-30-2013
        /// </Summary>
        public void SetEventLPStatus(ISelenium adminBrowserObj, string eventid, string status)
        {
            string EventStartTime = GetEventStartTimeByEventID(adminBrowserObj, eventid);
            adminBrowserObj.Click("//a[contains(string(),'Race Winner')]");
            Thread.Sleep(5000);
            SelectMainFrame(adminBrowserObj);
            adminBrowserObj.Select("//select[@name='MktLPAvail']", status);
            adminBrowserObj.Click("//input[@value='Modify Market']");

        }



        /// <summary>
        /// Change the customer Password
        /// </summary>
        ///  Author: Kiran
        /// <param name="browser">browser Instance</param>
        /// <param name="userName">User Name</param>
        /// <param name="userName">passwordUser</param>
        public void ChangeCustomerPassword(ISelenium adminBrowser, string userName, string password)
        {
            try
            {
                // Enter Customer Name and Search
                SearchCustomer(userName, adminBrowser);
                SelectMainFrame(adminBrowser);
                adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.UpdCustPwdField1, password);
                adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.UpdCustPwdField2, password);
                adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.UpdatePassword);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Assert.IsTrue(adminBrowser.IsTextPresent("Successfully changed password"), "Failed to update password");
            }
            catch (Exception ex)
            {
                CaptureScreenshot(adminBrowser, "");
                Fail(ex.Message);
            }
        }




        /// <summary>
        /// Updates a perticular segment value
        /// </summary>
        ///  Author: Kiran
        /// <param name="browser">browser Instance</param>
        /// <param name="userName">User Name</param>
        /// <param name="segmentType">HVC</param>
        /// <param name="segmentVal">Yes</param>
        public bool UpdateSegments(ISelenium adminBrowser, string userName, string segmentType, string segmentVal)
        {
            try
            {
                // Enter Customer Name and Search
                SearchCustomer(userName, adminBrowser);
                SelectMainFrame(adminBrowser);
                adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.updateSegment);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                string xPath = "//tr/td[contains(text(), '" + segmentType + "')]/following-sibling::td/select";
                Assert.IsTrue(adminBrowser.IsElementPresent(xPath), "Element not found " + xPath);
                adminBrowser.Select(xPath, segmentVal);
                Thread.Sleep(1000);
                adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.updateSegmentandURL);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                xPath = "//tr/td[@class='caption' and contains(text(), '" + segmentType + "')]/following-sibling::td[contains(text(), '" + segmentVal + "')]";
                if (segmentVal.ToLower().Contains("unset") || segmentVal.ToLower().Contains("no"))
                {
                    if (adminBrowser.IsAlertPresent())
                    {
                        adminBrowser.ChooseOkOnNextConfirmation();
                    }
                    xPath = "//tr/td[@class='caption' and contains(text(), '" + segmentType + "')]/following-sibling::td/i[contains(text(), '" + segmentVal + "')]";
                }
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.backBtn);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Assert.IsTrue(adminBrowser.IsElementPresent(xPath), "Element not found " + xPath);
                return true;
            }
            catch (Exception ex)
            {
                CaptureScreenshot(adminBrowser, "");
                Fail(ex.Message);
                return false;
            }
        }



        /// <summary>
        /// Stores age evrification details for a customer
        /// </summary>
        ///  Author: Kiran
        /// <param name="browser">browser Instance</param>
        /// <param name="userName">User Name</param>
        /// <param name="segmentType">HVC</param>
        /// <param name="segmentVal">Yes</param>
        public bool StoreAgeVerification(ISelenium adminBrowser, string userName, string ageVerStatus, int ovsScore, string ovsRef, string notes, string driverLinNo)
        {
            try
            {
                // Enter Customer Name and Search
                SearchCustomer(userName, adminBrowser);
                SelectMainFrame(adminBrowser);
                adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.custVerificationBtn);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                Assert.IsTrue(adminBrowser.IsElementPresent(AdminSuite.CustomerCreation.CustomersPage.ageVerfStatusPassRdo), "Customer Verification page not loaded");
                if (ageVerStatus.ToLower() == "pass")
                {
                    adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.ageVerfStatusPassRdo);
                }
                else
                {
                    adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.ageVerfStatusFailRdo);
                }

                adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.ovsReferenceTxt, ovsRef);
                adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.notesTxt, notes);

                adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.driversRdo);
                adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.driverNoTxt, driverLinNo);
                adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.strVerificationBtn);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                Assert.IsTrue(adminBrowser.IsTextPresent("Verification check stored successfully for customer"), "Failed to store verification");
                return true;
            }
            catch (Exception ex)
            {
                CaptureScreenshot(adminBrowser, "");
                Console.WriteLine(ex);
                return false;
            }
        }





        /// <summary>
        /// Search for an Event
        /// </summary>
        /// <param name="browser">Selenium browser instance</param>
        /// <param name="catName">Category Name</param>
        /// <param name="eventClsName">Event ClassName</param>
        /// <param name="eventTypeName">Event TypeName</param>
        /// <param name="eventSubType">Event SubType</param>
        /// <param name="eventName">Event Name</param>
        /// <param name="eventStatus">Event Status</param>
        public bool SearchEvent(ISelenium browser, string catName, string eventClsName, string eventTypeName, string eventSubType, string eventName)
        {
            try
            {
                bool eventFound = false;

                TimeSpan ts = new TimeSpan(0, 1, 0);
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

                //Clicking on Event Link in LHN
                LHNavigation(AdminSuite.CommonControls.AdminHomePage.EventNameLink, browser);
                //Selecting TopFrame
                System.Threading.Thread.Sleep(10000);
                SelectMainFrame(browser);

                _frameworkCommon.WaitUntilElementPresent(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "60");
                catName = catName.Replace("|", "").Trim();
                if (catName != "")
                {
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx))
                    {
                        Assert.IsTrue(_frameworkCommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, catName), "Category Name does not present in DropdownList");
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.categoryNameLstBx, "label=" + catName);
                    }
                }
                eventClsName = eventClsName.Replace("|", "").Trim();

                if (eventClsName != "")
                {
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx))
                    {
                        Assert.IsTrue(_frameworkCommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, eventClsName), "EventClass Name does not present");
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.classNameLstBx, "label=" + eventClsName);
                    }
                }
                if (eventTypeName != "")
                {
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx))
                    {
                        Assert.IsTrue(_frameworkCommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, eventTypeName), "EventClass Name does not present");
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.eventTypeLstBx, "label=" + eventTypeName);
                    }
                }
                if (eventSubType != "")
                {
                    if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.subEventTypeLstBx))
                    {
                        Assert.IsTrue(_frameworkCommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.subEventTypeLstBx, eventSubType), "EventSubType Name does not present");
                        browser.Select(AdminSuite.CommonControls.EventDetailsPage.subEventTypeLstBx, "label=" + eventSubType);
                    }
                }
                Assert.IsTrue(_frameworkCommon.CheckItemPresentInDropDownList(browser, AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "--"), "Date range dropdown missing");

                // Selecting Daterange 
                if (browser.IsElementPresent(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx))
                {
                    browser.Select(AdminSuite.CommonControls.EventDetailsPage.dateRangeLstBx, "label=--");
                }
                //Clicking on Seach button
                browser.Click(AdminSuite.CommonControls.EventDetailsPage.eventSearchBtn);
                _frameworkCommon.WaitUntilAllElementsLoad(browser);
                browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                //Thread.Sleep(2000);
                // Wait for Element to present

                if (_frameworkCommon.WaitUntilElementPresent(browser, "link=" + eventName + "", "60") == true)
                {
                    browser.Click("link=" + eventName);
                    browser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                    eventFound = true;
                }
                return eventFound;
            }

            catch (Exception ex)
            {
                CaptureScreenshot(browser, "");
                Fail(ex.Message);
                return false;
            }
        }




        /// <Summary>
        /// Update start date of an Event
        /// Author: Kiran 
        /// </Summary>
        public bool UpdateEventStartDate(ISelenium adminBrowser, string catName, string eventClsName, string eventTypeName, string eventSubType, string eventName, DateTime date)
        {
            try
            {
                string x = date.ToString("yyyy-MM-dd hh:mm:ss"); //Eg:2013-04-29 15:00:00
                bool bStatus = false;

                bStatus = SearchEvent(adminBrowser, catName, eventClsName, eventTypeName, eventSubType, eventName);

                //Selecting TopFrame
                SelectMainFrame(adminBrowser);
                adminBrowser.Type("//input[@id='start_time']", date.ToString());
                adminBrowser.Click(AdminSuite.CommonControls.EventDetailsPage.updateEventBtn);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                return bStatus;
            }
            catch (Exception ex)
            {
                CaptureScreenshot(adminBrowser, "");
                Fail(ex.Message);
                return false;
            }
        }



        /// <Summary>
        /// Do Manual Adjustment
        /// Author: Kiran 
        /// </Summary>
        public bool PerformManualAdjustment(ISelenium adminBrowser, string customerName, string typeGroup, string amount, string withdrawal, string type, string description, string dateFrom, string dateTo, string dateRange)
        {
            try
            {
                bool bStatus = false;
                SearchCustomer(customerName, adminBrowser);
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);

                //Selecting TopFrame
                SelectMainFrame(adminBrowser);
                adminBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.typeGrouplstBox, typeGroup);
                adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.amountFld, amount);
                adminBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.withdrawableLst, withdrawal);
                adminBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.typeLstBox, type);
                adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.descriptionFld, description);
                if (!string.IsNullOrEmpty(dateFrom))
                {
                    adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.dateFromFld, Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd hh:mm:ss"));
                }
                if (!string.IsNullOrEmpty(dateTo))
                {
                    adminBrowser.Type(AdminSuite.CustomerCreation.CustomersPage.dateToFld, Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd hh:mm:ss"));
                }
                adminBrowser.Select(AdminSuite.CustomerCreation.CustomersPage.dateRangeLst, dateRange);
                adminBrowser.Click(AdminSuite.CustomerCreation.CustomersPage.ManAdjBtn);

                adminBrowser.ChooseOkOnNextConfirmation();
                adminBrowser.WaitForPageToLoad(FrameGlobals.PageLoadTimeOut);
                Assert.IsTrue(adminBrowser.IsTextPresent("Manual Adjustment Successful"), "Failed to do Manual Adjustment");
                bStatus = true;
                return bStatus;
            }
            catch (Exception ex)
            {
                CaptureScreenshot(adminBrowser, "");
                Fail(ex.Message);
                return false;
            }
        }



    }// End of method
}// ENd of class
