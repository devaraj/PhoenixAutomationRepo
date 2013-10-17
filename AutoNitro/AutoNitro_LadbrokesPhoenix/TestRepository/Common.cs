using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

using Selenium;
using OpenQA.Selenium;
using Framework;
using TestRepository.ControlsRepository;


namespace TestRepository
{
    public class Common : BaseTest
    {
        Framework.Common.Common CFframeworkCommonObj = new Framework.Common.Common();

        /// <summary>
        /// This method will wait for loader icon to complete loading
        /// </summary>
        /// <param name="browserObj">Browser Instance</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <example>WaitForLoadingIcon(browser,60)</example>      
        public void WaitForLoadingIcon(ISelenium browserObj, int timeout)
        {
            IWebDriver driver = ((WebDriverBackedSelenium)browserObj).UnderlyingWebDriver;
            //CFframeworkCommonObj.PageSync(browserObj);
            DateTime now;
            DateTime delay1 = DateTime.Now.AddSeconds(timeout);
            while (browserObj.IsVisible(LoginLogoutControls.loadingIcon1))
            {
                now = DateTime.Now;
                if (now < delay1)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }


            DateTime delay2 = DateTime.Now.AddSeconds(timeout);
            while (browserObj.IsVisible(LoginLogoutControls.loadingIcon2))
            {
                now = DateTime.Now;
                if (now < delay2)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }


            DateTime delay3 = DateTime.Now.AddSeconds(timeout);
            while (browserObj.IsVisible(LoginLogoutControls.loadingIcon3))
            {
                now = DateTime.Now;
                if (now < delay3)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            CFframeworkCommonObj.PageSync(browserObj);
            TimeSpan ts = new TimeSpan(0, 0, 0);
            driver.Manage().Timeouts().ImplicitlyWait(ts);
            if (browserObj.IsElementPresent(LoginLogoutControls.InfoPageCloseBtn))
            {
                browserObj.Click(LoginLogoutControls.InfoPageCloseBtn);
            }
        }



        ///<summary>
        /// This method selects the links from side bar and validates of navigated to appropriate page
        /// <example>OddTypeSwitch(browser, "Football", "Football") </example>
        public void SelectLinksFromSideBar(ISelenium browserObj, string linkToSelect, string title)
        {
            IWebDriver driver = ((WebDriverBackedSelenium)browserObj).UnderlyingWebDriver;
            try
            {
                selectMenuButton(browserObj);
                string xPath = "//div[@id='menu-container']//span[@class='ml10']";
                clickObjectInColl(browserObj, xPath, linkToSelect);
                //clickObject(browserObj, "//div[@id='menu-container']//span[@class='ml10' and contains(text(), '" + linkToSelect + "')]");                                                                            

                Assert.IsTrue(driver.FindElement(By.XPath("//span[@class='t7 page-title' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + title + "')]")).Displayed, "Failed to navigate to '" + linkToSelect + "' page.");
                Assert.IsTrue(browserObj.IsElementPresent(LoginLogoutControls.LadbrokesHomeLink), "Ladbrokes Home link is not present on the header");
                Assert.IsTrue(browserObj.IsElementPresent(BetslipControls.betslipButton), "Betslip button is not present on the header");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'SelectLinksFromSideBar' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method navigates to the home page
        /// <example>NavigateToHomePage(browser, SideMenu/Header) </example>
        public void NavigateToHomePage(ISelenium browserObj, string location)
        {
            try
            {
                if (location.ToLower().Contains("side"))
                {
                    selectMenuButton(browserObj);
                    clickObject(browserObj, LoginLogoutControls.homeLinkOnSideBar);
                }
                else
                {
                    if (browserObj.IsVisible(LoginLogoutControls.LadbrokesHomeLink))
                    {
                        clickObject(browserObj, LoginLogoutControls.LadbrokesHomeLink);
                        Assert.IsTrue(browserObj.IsElementPresent(LoginLogoutControls.carousel), "Failed to navigate to Home page from Ladbrokes Home link");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'NavigateToHomePage' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method selects Menu button and check if the side bar has appeared
        /// <example>selectMenuButton(browser)</example>        
        public void selectMenuButton(ISelenium browserObj)
        {
            try
            {
                CFframeworkCommonObj.WaitUntilElementPresent(browserObj, LoginLogoutControls.menuIcon, FrameGlobals.PageLoadTimeOut);
                Assert.IsTrue(browserObj.IsElementPresent(LoginLogoutControls.menuIcon), "Menu button is not present in the page");

                browserObj.Click(LoginLogoutControls.menuIcon);
                CFframeworkCommonObj.WaitUntilElementPresent(browserObj, LoginLogoutControls.sidebar, "10000");
                CFframeworkCommonObj.PageSync(browserObj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'selectMenuButton' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method checks for the existance of an element(button, link..)
        /// <example>CheckElementPresent(browser, xPath)</example>        
        public void CheckElementPresent(ISelenium browserObj, string strLocator)
        {
            try
            {
                WaitForLoadingIcon(browserObj, Convert.ToInt32(FrameGlobals.IconLoadTimeout));
                Assert.IsTrue(browserObj.IsVisible(strLocator), strLocator + " element is not present");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'CheckElementPresent' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method click on any desired object(button, link)
        /// <example>clickObject(browser, xPath)</example>        
        public void clickObject(ISelenium browserObj, string strLocator)
        {
            try
            {
                Assert.IsTrue(browserObj.IsVisible(strLocator), strLocator + " element is not present");
                browserObj.Focus(strLocator);
                browserObj.Click(strLocator);
                WaitForLoadingIcon(browserObj, Convert.ToInt32(FrameGlobals.IconLoadTimeout));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'clickObject' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method selects an object in a collection(button, link)
        /// <example>clickObjectInColl(browser, xPath, elementName)</example>       
        public void clickObjectInColl(ISelenium browserObj, string strLocator, string elementName)
        {
            IWebDriver driver = ((WebDriverBackedSelenium)browserObj).UnderlyingWebDriver;
            try
            {
                Thread.Sleep(1000);
                ReadOnlyCollection<IWebElement> element = driver.FindElements(By.XPath(strLocator));
                for (int i = 0; i < element.Count; i++)
                {
                    if (element[i].Text.ToLower().Trim() == elementName.ToLower().Trim())
                    {
                        element[i].Click();
                        break;
                    }
                    else
                    {
                        if (i == element.Count - 1)
                        {
                            Fail(elementName + " object was not found");
                        }
                    }
                }
                WaitForLoadingIcon(browserObj, Convert.ToInt32(FrameGlobals.IconLoadTimeout + 5000));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'clickObject' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method enters text in field
        /// <example>EnterField(browser, xPath)</example>       
        public void EnterField(ISelenium browserObj, string strLocator, string text)
        {
            try
            {
                Assert.IsTrue(browserObj.IsVisible(strLocator), strLocator + " element is not present");
                browserObj.Focus(strLocator);
                browserObj.Type(strLocator, "");
                browserObj.Type(strLocator, text);
                CFframeworkCommonObj.PageSync(browserObj);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'EnterField' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method selects avalue from list/combo box
        /// <example>SelectValueFromListbox(browser, xPath, value)</example>       
        public void SelectValueFromListbox(ISelenium browserObj, string strLocator, string value)
        {
            try
            {
                CFframeworkCommonObj.PageSync(browserObj);
                Assert.IsTrue(browserObj.IsVisible(strLocator), strLocator + " element is not present");
                string[] itemArray = browserObj.GetSelectOptions(strLocator);
                for (int i = 0; i < itemArray.Length; i++)
                {
                    if (itemArray[i].ToLower().Trim() == value.ToLower().Trim())
                    {
                        browserObj.Select(strLocator, value);
                        CFframeworkCommonObj.PageSync(browserObj);
                        break;
                    }
                    else
                    {
                        if (i == itemArray.Length - 1)
                        {
                            Console.WriteLine(value + " not found in the list box");
                            Fail(value + " not found in the list box");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'SelectValueFromListbox' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        ///<summary>
        /// This method check/unchecks the check box
        /// <example>SelectCheckbox(browser, xPath, On/Off)</example>       
        public void SelectCheckbox(ISelenium browserObj, string strLocator, string value)
        {
            try
            {
                Assert.IsTrue(browserObj.IsVisible(strLocator), strLocator + " element is not present");
                if (value.ToLower().Trim() == "on")
                {
                    if (!browserObj.IsChecked(strLocator))
                    {
                        browserObj.Check(strLocator);
                        CFframeworkCommonObj.PageSync(browserObj);
                        if (!(browserObj.IsChecked(strLocator)))
                        {
                            Console.WriteLine("Failed to check the checkbox - " + strLocator);
                            Fail("Failed to check the checkbox - " + strLocator);
                        }
                    }
                }
                else
                {
                    browserObj.Uncheck(strLocator);
                    CFframeworkCommonObj.PageSync(browserObj);
                    if (browserObj.IsChecked(strLocator))
                    {
                        Console.WriteLine("Failed to Uncheck the checkbox - " + strLocator);
                        Fail("Failed to check the Uncheckbox - " + strLocator);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'SelectCheckbox' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// To switch control from one browser from another browser
        /// </summary>
        /// <param name="browserObj"></param>
        /// <param name="pageTitle"></param>
        public void SwitchWindow(ISelenium browserObj, string pageTitle)
        {
            try
            {
                Thread.Sleep(2000);
                string[] windowTitles = browserObj.GetAllWindowTitles();
                for (int i = 0; i < windowTitles.Length; i++)
                {
                    string strTemp = windowTitles[i].ToString();
                    if (strTemp.Contains(pageTitle))
                    {
                        // browser.WaitForPopUp(windowTitles[i], "30000");
                        browserObj.SelectWindow(windowTitles[i]);
                        Thread.Sleep(2000);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Function 'SwitchWindow' - Failed");
                Console.WriteLine(ex.Message);
                Fail(ex.Message);
            }
        }



        /// <summary>
        /// This function will remove "|" symbols from test data values
        /// This is because in portal "|" will be excluded in the display
        /// </summary>
        /// <param name="stringName">string value</param>
        public void RemovePipeSymbolsFromString(ref string stringName)
        {
            stringName = stringName.Replace("|", "").Trim();
        }



        public void KillAdminObject()
        {
            string processtoKill = null;
            var process = new Process[1];
            processtoKill = "firefox";

            //For closing other instances of selected browser type
            process = Process.GetProcessesByName(processtoKill);
            foreach (Process p in process)
            {
                p.Kill();
            }
        }

        /// <summary>
        /// Method to resize the window
        /// </summary>
        /// <param name="width">Width of window</param>
        /// <param name="height">Height o</param>
        public void ReSizeWindow(ISelenium browser,int width, int height)
        {
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
            driver.Manage().Window.Size = new System.Drawing.Size(width, height);
        }


        /// <summary>
        /// Method to get the selection ID from betslip
        /// </summary>
        public string GetSelectionIDFromBetslip(ISelenium browser, string selectionName, string eventName)
        {
            string SelectionId;
            string[] itemArr;
            IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;

            SelectionId = driver.FindElement(By.XPath("//div[div[@class='slip-item']]//input[@name]")).GetAttribute("name");
            
            itemArr = SelectionId.Split('-');
            SelectionId = itemArr[itemArr.Length - 1];
            return SelectionId;
        }

 

    }//end class
}//end namespace
