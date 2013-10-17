using System;
using MbUnit.Framework;
using Selenium;
using Framework;
using System.Data;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace TestDataCreation
{
    public class TestData : TestDataBase
    {
        #region Variables declaration
        string _testDataFilePath = "";
        readonly DirectoryInfo _currentDirPath = new DirectoryInfo(Environment.CurrentDirectory);
        private static Workbook _workBook;
        private static Sheets _workSheets;
        private static Worksheet _workSheet;
        private static Application _app;
        string _param1 = "", _param2 = "", _param3 = "", _param4 = "", _param5 = "", _param6 = "", _param7 = "";
        string eventAppendDateTime;
        #endregion

        /// <summary>
        /// Fills the events,market,selections csv file templates to make them ready to upload to open bet admin.
        /// This uses Fields.xls which contains Events hierarchy definitions and also uses mapping.xls to fill the specified files.
        /// </summary>
        public void FillCsvFilesBeforeUpload()
        {
            if (_currentDirPath.Parent != null) _testDataFilePath = _currentDirPath.Parent.FullName;
            System.Globalization.DateTimeFormatInfo format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
            var oneDay = new TimeSpan(1, 0, 0, 0);
            var ts = new TimeSpan(0, -1, 0, 0);
            var endTs = new TimeSpan(2, 0, 0, 0);
            var futureTs = new TimeSpan(15, 13, 50, 0);
            var tomoTs = new TimeSpan(1, 13, 45, 0);

            string todayDateTime = DateTime.Today.Add(oneDay).Add(ts).ToString(format.UniversalSortableDateTimePattern);
            string liveDateTime = DateTime.Now.ToString(format.UniversalSortableDateTimePattern);
            string endDateTime = DateTime.Now.Add(endTs).ToString(format.UniversalSortableDateTimePattern);
            string tomoDateTime = DateTime.Today.Add(tomoTs).ToString(format.UniversalSortableDateTimePattern);
            string futureDateTime = DateTime.Today.Add(oneDay).Add(futureTs).ToString(format.UniversalSortableDateTimePattern);
            todayDateTime = todayDateTime.TrimEnd('Z');
            endDateTime = endDateTime.TrimEnd('Z');
            liveDateTime = liveDateTime.TrimEnd('Z');
            futureDateTime = futureDateTime.TrimEnd('Z');
            tomoDateTime = tomoDateTime.TrimEnd('Z');

            eventAppendDateTime = DateTime.Now.Add(ts).Day.ToString(CultureInfo.CurrentCulture) + DateTime.Now.Add(ts).Month.ToString(CultureInfo.CurrentCulture) + "_" + DateTime.Now.Add(ts).Hour.ToString(CultureInfo.CurrentCulture) + DateTime.Now.Add(ts).Minute.ToString(CultureInfo.CurrentCulture);

            System.Data.DataTable mappingDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\FillUploadFiles\\Mapping.xls", "CSV");
            System.Data.DataTable fieldsDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\FillUploadFiles\\Fields.xls", "UpdateEvent");
            System.Data.DataTable eventsDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\FillUploadFiles\\EVENTS.xls", "EVENTS");
            System.Data.DataTable marketsDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\FillUploadFiles\\market.xls", "market");
            System.Data.DataTable selectionsDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\FillUploadFiles\\SELECTIONS.xls", "SELECTIONS");
            
            for (int i = 0; i < mappingDataTable.Rows.Count; i++)
            {
                int sourceRow = Convert.ToInt32(mappingDataTable.Rows[i]["SourceRow"]);
                _param1 = fieldsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param1"].ToString()].ToString();
                _param2 = fieldsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param2"].ToString()].ToString();
                _param3 = fieldsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param3"].ToString()].ToString();
                _param4 = fieldsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param4"].ToString()].ToString();
                //_param6 = fieldsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param6"].ToString()].ToString();
                int destinationRow = Convert.ToInt32(mappingDataTable.Rows[i]["DestRow"]);
                string destinationFile = mappingDataTable.Rows[i]["Destination"].ToString();
                string eventNameValue = "";
                switch (destinationFile.ToUpper().Trim())
                {
                    case "EVENTS.CSV":
                        eventsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param1"].ToString()] = _param1.Trim();
                        eventsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param2"].ToString()] = _param2.Trim();
                        eventsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param3"].ToString()] = _param3.Trim();
                        eventsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param4"].ToString()] = _param4.Trim();
                        //eventsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param6"].ToString()] = _param6.Trim();
                        eventNameValue = eventsDataTable.Rows[destinationRow - 2]["NAME"].ToString();
                        eventsDataTable.Rows[destinationRow - 2]["NAME"] = (eventNameValue + eventAppendDateTime).Trim();
                        if (eventsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Live"))
                        {
                            eventsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = liveDateTime.Trim();
                            eventsDataTable.Rows[destinationRow - 2]["SUSPEND AT"] = endDateTime.Trim();
                        }
                        else if (eventsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Today"))
                            eventsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = todayDateTime.Trim();
                        else if (eventsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Tomorrow"))
                            eventsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = tomoDateTime.Trim();
                        else
                            eventsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = futureDateTime.Trim();

                        break;
                    case "MARKET.CSV":
                        marketsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param1"].ToString()] = _param1.Trim();
                        marketsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param2"].ToString()] = _param2.Trim();
                        marketsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param3"].ToString()] = _param3.Trim();
                        marketsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param4"].ToString()] = _param4.Trim();
                        //marketsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param6"].ToString()] = _param6.Trim();
                        eventNameValue = marketsDataTable.Rows[destinationRow - 2]["NAME"].ToString();
                        marketsDataTable.Rows[destinationRow - 2]["NAME"] = (eventNameValue + eventAppendDateTime).Trim();
                        if (marketsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Live"))
                        {
                            marketsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = liveDateTime.Trim();
                        }
                        else if (marketsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Today"))
                            marketsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = todayDateTime.Trim();
                        else if (marketsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Tomorrow"))
                            marketsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = tomoDateTime.Trim();
                        else
                            marketsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = futureDateTime.Trim();
                        break;
                    case "SELECTIONS.CSV":
                        selectionsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param1"].ToString()] = _param1.Trim();
                        selectionsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param2"].ToString()] = _param2.Trim();
                        selectionsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param3"].ToString()] = _param3.Trim();
                        selectionsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param4"].ToString()] = _param4.Trim();
                        //selectionsDataTable.Rows[destinationRow - 2][mappingDataTable.Rows[i]["param6"].ToString()] = _param6.Trim();
                        eventNameValue = selectionsDataTable.Rows[destinationRow - 2]["NAME"].ToString();
                        selectionsDataTable.Rows[destinationRow - 2]["NAME"] = (eventNameValue + eventAppendDateTime).Trim();
                        if (selectionsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Live"))
                        {
                            selectionsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = liveDateTime.Trim();
                        }
                        else if (selectionsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Today"))
                            selectionsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = todayDateTime.Trim();
                        else if (selectionsDataTable.Rows[destinationRow - 2]["EVENT DATE"].ToString().Contains("Tomorrow"))
                            selectionsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = tomoDateTime.Trim();
                        else
                            selectionsDataTable.Rows[destinationRow - 2]["DATE/TIME"] = futureDateTime.Trim();
                        break;
                    default:
                        break;
                }

            }

            CreateCsvFile(eventsDataTable, _testDataFilePath + "\\TestData\\UploadFiles\\EVENTS.CSV");
            CreateCsvFile(marketsDataTable, _testDataFilePath + "\\TestData\\UploadFiles\\market.CSV");
            CreateCsvFile(selectionsDataTable, _testDataFilePath + "\\TestData\\UploadFiles\\SELECTIONS.CSV");

            //Creating same set of above csv files as excel files for coding convinience in MapTestDataXls() method
            Excel_FromDataTable(eventsDataTable, _testDataFilePath + "\\TestData\\UploadFiles\\EVENTS.xls", "EVENTS");
            Excel_FromDataTable(marketsDataTable, _testDataFilePath + "\\TestData\\UploadFiles\\market.xls", "market");
            Excel_FromDataTable(selectionsDataTable, _testDataFilePath + "\\TestData\\UploadFiles\\SELECTIONS.xls", "SELECTIONS");

        }

        /// <summary>
        /// Uploads the specified csv files to create Events/Markets/Selections
        /// </summary>
        /// <param name="filepath">Exact file name along with full path</param>
        /// <param name="browser">Browser instance</param>
        public void UploadCsvFilesToAdmin(string filepath, ref ISelenium browser)
        {
            try
            {

                browser.SelectFrame("relative=top");
                browser.SelectFrame("TopBar");
                //browser.Click("//li[a[text()='Events']]/following-sibling::li[a[text()='Event Upload']]");
                IWebDriver driver = ((WebDriverBackedSelenium)browser).UnderlyingWebDriver;
                driver.FindElement(By.LinkText("Event Upload")).Click();
                browser.WaitForPageToLoad("60000");
                browser.SelectFrame("relative=top");
                browser.SelectFrame("MainArea");
                browser.WaitForPageToLoad("10000");
                string[] filename = filepath.Split('\\');
                Console.WriteLine(filename[filename.Length - 1] + " : Is the file name");

                switch (filename[filename.Length - 1])
                {
                    case "EVENTS.CSV":

                        //events,markets,selections,results,fbscores
                        browser.Select("name=filetype", "Events");
                        browser.Type("name=filename", filepath);

                        browser.Focus("//table/tbody/tr/th[@class='buttons' and @colspan='2']/input[@type='submit' and @value='Upload File' and @name='DoIt']");
                        browser.Click("//table/tbody/tr/th[@class='buttons' and @colspan='2']/input[@type='submit' and @value='Upload File' and @name='DoIt']");
                        browser.WaitForPageToLoad("10000");
                        Assert.IsTrue(browser.IsElementPresent("//td/a[(text()='EVENTS.CSV')]"), "");
                        browser.Click("//td/a[contains(text(),'EVENTS.CSV')]");
                        browser.WaitForPageToLoad("10000");
                        browser.Click("name=SetResult");
                        browser.WaitForPageToLoad("10000");

                        // to handle javascript pop up
                        if (browser.IsAlertPresent())
                            browser.ChooseOkOnNextConfirmation();
                        Thread.Sleep(2000);
                        //string fileUploadmsg = browser.GetText("//tr[@class=infoyes/tb/b");
                        //Console.WriteLine(fileUploadmsg + "is the message displayed after File upload, Expected message is 'File uploaded OK'");
                        browser.Click("//input[@value='Delete File']");
                        browser.WaitForPageToLoad("10000");
                        break;

                    case "market.CSV":

                        browser.Select("name=filetype", "Markets");//events,markets,selections,results,fbscores
                        browser.Type("name=filename", filepath);

                        browser.Focus("//table/tbody/tr/th[@class='buttons' and @colspan='2']/input[@type='submit' and @value='Upload File' and @name='DoIt']");
                        browser.Click("//table/tbody/tr/th[@class='buttons' and @colspan='2']/input[@type='submit' and @value='Upload File' and @name='DoIt']");//upload file
                        browser.WaitForPageToLoad("10000");
                        Assert.IsTrue(browser.IsElementPresent("//td/a[(text()='market.CSV')]"), "");
                        browser.Click("//td/a[contains(text(),'market.CSV')]");
                        browser.WaitForPageToLoad("10000");
                        browser.Click("name=SetResult");
                        browser.WaitForPageToLoad("10000");

                        /// to handle javascript pop up
                        if (browser.IsAlertPresent())
                            browser.ChooseOkOnNextConfirmation();

                        Thread.Sleep(2000);
                        //string fileUplodmsg = browser.GetText("//tr[@class=infoyes/tb/b");
                        //Console.WriteLine(fileUplodmsg + "is the message displayed after File upload, Expected message is 'File uploaded OK'");
                        browser.Click("//input[@value='Delete File']");
                        browser.WaitForPageToLoad("10000");
                        break;

                    case "SELECTIONS.CSV":

                        browser.Select("name=filetype", "Selections");//events,markets,selections,results,fbscores
                        browser.Type("name=filename", filepath);

                        browser.Focus("//table/tbody/tr/th[@class='buttons' and @colspan='2']/input[@type='submit' and @value='Upload File' and @name='DoIt']");
                        browser.Click("//table/tbody/tr/th[@class='buttons' and @colspan='2']/input[@type='submit' and @value='Upload File' and @name='DoIt']");//upload file
                        browser.WaitForPageToLoad("10000");
                        Assert.IsTrue(browser.IsElementPresent("//td/a[(text()='SELECTIONS.CSV')]"), "");
                        browser.Click("//td/a[contains(text(),'SELECTIONS.CSV')]");
                        browser.WaitForPageToLoad("10000");
                        browser.Click("name=SetResult");
                        browser.WaitForPageToLoad("10000");

                        Thread.Sleep(2000);
                        /// to handle javascript pop up
                        if (browser.IsAlertPresent())
                            browser.ChooseOkOnNextConfirmation();

                        Thread.Sleep(2000);
                        //string fileUplodmessage = browser.GetText("//tr[@class=infoyes/tb/b");
                        //Console.WriteLine(fileUplodmessage + "is the message displayed after File upload, Expected message is 'File uploaded OK'");
                        browser.Click("//input[@value='Delete File']");
                        browser.WaitForPageToLoad("10000");
                        break;

                    default:
                        break;

                }
            }
            catch (Exception Ex)
            {
                Framework.BaseTest.CaptureScreenshot(browser, "");
                Console.WriteLine(Ex.StackTrace);
                Framework.BaseTest.Fail(Ex.Message);
            }


        }

        /// <summary>
        /// Fills the TestData.xls file based from Events,market and selections files based on Mapping.xls
        /// </summary>
        public void MapTestDataXls()
        {
            try
            {
                _testDataFilePath = _currentDirPath.Parent.FullName;
                System.Data.DataTable mappingDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\FillUploadFiles\\Mapping.xls", "Events");
                System.Data.DataTable eventsDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\UploadFiles\\EVENTS.xls", "EVENTS");
                System.Data.DataTable marketsDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\UploadFiles\\market.xls", "market");
                System.Data.DataTable selectionsDataTable = XlsReader.LoadExcelData(_testDataFilePath + "\\TestData\\UploadFiles\\SELECTIONS.xls", "SELECTIONS");
                _app = new Application();
                var testDataObj = new TestData();
                _app.DisplayAlerts = false;
                _workBook = _app.Workbooks.Open(_testDataFilePath + "\\TestData\\TestData.xls", 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                _workSheets = _workBook.Worksheets;
                //GUITestData,BetSlipTestData,GUITestData,Others
                for (int i = 0; i < mappingDataTable.Rows.Count; i++)
                {

                    string sourceFile = mappingDataTable.Rows[i]["Source"].ToString();
                    string sourceSheet = mappingDataTable.Rows[i]["SourceSheet"].ToString();
                    int sourceRow = Convert.ToInt32(mappingDataTable.Rows[i]["SourceRow"]);

                    string destinationFile = mappingDataTable.Rows[i]["Destination"].ToString();
                    string destinationSheet = mappingDataTable.Rows[i]["DestSheet"].ToString();
                    int destinationRow = Convert.ToInt32(mappingDataTable.Rows[i]["DestRow"]);
                    _workSheet = (Microsoft.Office.Interop.Excel.Worksheet)_workSheets.get_Item(destinationSheet);
                    Microsoft.Office.Interop.Excel.Range range = _workSheet.UsedRange;
                    int colCount = range.Columns.Count;
                    int rowCount = range.Rows.Count;
                    int colIndexParam1 = 1;
                    int colIndexParam2 = 1;
                    int colIndexParam3 = 1;
                    int colIndexParam4 = 1;
                    int colIndexParam5 = 1;
                    int colIndexParam6 = 1;
                    int colIndexParam7 = 1;

                    switch (sourceFile.ToUpper().Trim())
                    {

                        case "EVENTS.CSV":

                            if (mappingDataTable.Rows[i]["param1"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param1 = eventsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param1"].ToString()].ToString();
                                colIndexParam1 = GetColumnIndex(mappingDataTable.Rows[i]["param1"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam1] = _param1;
                            }
                            if (mappingDataTable.Rows[i]["param2"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param2 = eventsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param2"].ToString()].ToString();
                                colIndexParam2 = GetColumnIndex(mappingDataTable.Rows[i]["param2"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam2] = _param2;
                            }
                            if (mappingDataTable.Rows[i]["param3"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param3 = eventsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param3"].ToString()].ToString();
                                colIndexParam3 = GetColumnIndex(mappingDataTable.Rows[i]["param3"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam3] = _param3;
                            }
                            if (mappingDataTable.Rows[i]["param4"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param4 = eventsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param4"].ToString()].ToString();
                                colIndexParam4 = GetColumnIndex(mappingDataTable.Rows[i]["param4"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam4] = _param4;
                            }
                            if (mappingDataTable.Rows[i]["param5"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param5 = eventsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param5"].ToString()].ToString();
                                colIndexParam5 = GetColumnIndex(mappingDataTable.Rows[i]["param5"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam5] = _param5;
                            }
                            if (mappingDataTable.Rows[i]["param6"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param6 = eventsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param6"].ToString()].ToString();
                                colIndexParam6 = GetColumnIndex(mappingDataTable.Rows[i]["param6"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam6] = _param6;
                            }
                            break;

                        case "MARKET.CSV":

                            if (mappingDataTable.Rows[i]["param1"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param1 = marketsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param1"].ToString()].ToString();
                                colIndexParam1 = GetColumnIndex(mappingDataTable.Rows[i]["param1"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam1] = _param1;
                            }
                            if (mappingDataTable.Rows[i]["param2"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param2 = marketsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param2"].ToString()].ToString();
                                colIndexParam2 = GetColumnIndex(mappingDataTable.Rows[i]["param2"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam2] = _param2;
                            }
                            if (mappingDataTable.Rows[i]["param3"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param3 = marketsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param3"].ToString()].ToString();
                                colIndexParam3 = GetColumnIndex(mappingDataTable.Rows[i]["param3"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam3] = _param3;
                            }
                            if (mappingDataTable.Rows[i]["param4"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param4 = marketsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param4"].ToString()].ToString();
                                colIndexParam4 = GetColumnIndex(mappingDataTable.Rows[i]["param4"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam4] = _param4;
                            }
                            if (mappingDataTable.Rows[i]["param5"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param5 = marketsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param5"].ToString()].ToString();
                                colIndexParam5 = GetColumnIndex(mappingDataTable.Rows[i]["param5"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam5] = _param5;
                            }
                            if (mappingDataTable.Rows[i]["param7"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param7 = marketsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param7"].ToString()].ToString();
                                colIndexParam7 = GetColumnIndex(mappingDataTable.Rows[i]["param7"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam7] = _param7;
                            }
                            break;

                        case "SELECTIONS.CSV":

                            if (mappingDataTable.Rows[i]["param1"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param1 = selectionsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param1"].ToString()].ToString();
                                colIndexParam1 = GetColumnIndex(mappingDataTable.Rows[i]["param1"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam1] = _param1;
                            }
                            if (mappingDataTable.Rows[i]["param2"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param2 = selectionsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param2"].ToString()].ToString();
                                colIndexParam2 = GetColumnIndex(mappingDataTable.Rows[i]["param2"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam2] = _param2;
                            }
                            if (mappingDataTable.Rows[i]["param3"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param3 = selectionsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param3"].ToString()].ToString();
                                colIndexParam3 = GetColumnIndex(mappingDataTable.Rows[i]["param3"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam3] = _param3;
                            }
                            if (mappingDataTable.Rows[i]["param4"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param4 = selectionsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param4"].ToString()].ToString();
                                colIndexParam4 = GetColumnIndex(mappingDataTable.Rows[i]["param4"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam4] = _param4;
                            }
                            if (mappingDataTable.Rows[i]["param5"].ToString().ToUpper().Trim() != "NULL")
                            {
                                _param5 = selectionsDataTable.Rows[sourceRow - 2][mappingDataTable.Rows[i]["param5"].ToString()].ToString();
                                colIndexParam5 = GetColumnIndex(mappingDataTable.Rows[i]["param5"].ToString(), ref _workSheet, ref colCount);
                                _workSheet.Cells[destinationRow, colIndexParam5] = _param5;
                            }
                            break;

                        default:

                            break;
                    }

                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.StackTrace);
                Framework.BaseTest.Fail(Ex.Message);
            }
            finally
            {
                _workBook.Save();
                _workBook.Close();
                _workSheet = null;
                _workBook = null;
                _app.Quit();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

        }

        /// <summary>
        /// Gets the column index based on column name
        /// This is a helper method for MapTestDataXls()
        /// </summary>
        /// <param name="columnHeaderName">Column Name</param>
        /// <param name="workSheet">Worksheet name</param>
        /// <param name="colCount">total used range of columns count</param>
        /// <returns>Column index from the desired sheet</returns>
        public int GetColumnIndex(string columnHeaderName, ref Worksheet workSheet, ref int colCount)
        {
            int colIndex = 1;
            for (int forLoopCount = 1; forLoopCount <= colCount; forLoopCount++)
            {
                if (workSheet.Cells[1, forLoopCount].Value == columnHeaderName)
                {
                    colIndex = forLoopCount;
                    return colIndex;
                }
            }
            return colIndex;

        }

        /// <summary>
        /// Converts a data table to a csv file
        /// </summary>
        /// <param name="dt">Data table to be converted</param>
        /// <param name="strFilePath">File path along with file name</param>
        static private void CreateCsvFile(System.Data.DataTable dt, string strFilePath)
        {
            // Create the CSV file to which grid data will be exported.
            var sw = new StreamWriter(strFilePath, false);
            int iColCount = dt.Columns.Count;

            // First we will write the headers.

            //DataTable dt = m_dsProducts.Tables[0];
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();

        }

        /// <summary>
        /// Converts a data table to an excel file
        /// </summary>
        /// <param name="dt">Data table to be converted</param>
        /// <param name="fileName">File path along with file name</param>
        /// <param name="sheetName">sheet name to be specified</param>
        private static void Excel_FromDataTable(System.Data.DataTable dt, string fileName, string sheetName)
        {
            _app = new Application();
            Workbook workbook = _app.Workbooks.Add(true);
            // Adding column headings with existing column headers
            int iCol = 0;
            foreach (DataColumn c in dt.Columns)
            {
                iCol++;
                _app.Cells[1, iCol] = c.ColumnName;
            }
            // for each row of data
            int iRow = 0;
            foreach (DataRow r in dt.Rows)
            {
                iRow++;
                // add each row's cell data...
                iCol = 0;
                foreach (DataColumn c in dt.Columns)
                {
                    iCol++;
                    _app.Cells[iRow + 1, iCol] = r[c.ColumnName];
                }
            }
            _workSheet = (Worksheet)workbook.Worksheets.get_Item(1);
            _workSheet.Name = sheetName;
            // Global missing reference for objects we are not defining...
            object missing = System.Reflection.Missing.Value;
            //Delete the file if already exists
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            //Saving the work book as .xls
            workbook.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, missing, missing, false, false, XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            _app.Quit();
        }


        public void CreateCustomerData(ref ISelenium browser)
        {
            try
            {
                var admincommonObj = new AdminSuite.Common();
                string user, password = "12345678";

                _testDataFilePath = _currentDirPath.Parent.FullName;
                _app = new Application();
                //var testDataObj = new TestData();
                _app.DisplayAlerts = false;
                _workBook = _app.Workbooks.Open(_testDataFilePath + "\\TestData\\TestData.xls", 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                _workSheets = _workBook.Worksheets;
                var _userSheet = (Microsoft.Office.Interop.Excel.Worksheet)_workSheets.get_Item("Users");
                Random rnd = new Random();
                int rndNumber;

                for (int i = 0; i < 10; i++)
                {
                    rndNumber = rnd.Next(10000);
                    user = "AutoUser" + rndNumber;
                    admincommonObj.CreateCustomer(browser, user, password);
                    _userSheet.Cells[i + 2, "B"] = user;
                    _userSheet.Cells[i +2, "C"] = password;
                    admincommonObj.PerformManualAdjustment(browser, user, "Test Accounts", "100", "Yes", "Test Accounts", "", "", "", "Today");
                    _workBook.Save();
                }
            }
            catch (Exception Ex)
            {
                Framework.BaseTest.CaptureScreenshot(browser, "");
                Console.WriteLine(Ex.StackTrace);
                Framework.BaseTest.Fail(Ex.Message);
            }
            finally 
            {
                _workBook.Save();
                _workBook.Close();
                _workSheet = null;
                _workBook = null;
                _app.Quit();
            }
        }


    }//End of Method
}//End of Class
