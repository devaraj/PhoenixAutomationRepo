using System;
using MbUnit.Framework;
using Selenium;
using System.Threading;
using Framework;
using System.IO;
namespace TestDataCreation
{
    [TestFixture(ApartmentState = ApartmentState.STA, TimeOut = FrameGlobals.TestCaseTimeout)]
    public class CreateTestData : TestDataBase
    {
        string _testDataFilePath = "";
        readonly DirectoryInfo _currentDirPath = new DirectoryInfo(Environment.CurrentDirectory);

        readonly TestData _testDataObj = new TestData();

        [Test]
        public void FillUploadAndMapTestData()
        {
            ISelenium browser = MyBrowser;

            _testDataObj.FillCsvFilesBeforeUpload();
            _testDataFilePath = _currentDirPath.Parent.FullName;
            _testDataObj.UploadCsvFilesToAdmin(_testDataFilePath + "\\TestData\\UploadFiles\\EVENTS.CSV", ref browser);
            _testDataObj.UploadCsvFilesToAdmin(_testDataFilePath + "\\TestData\\UploadFiles\\market.CSV", ref browser);
            _testDataObj.UploadCsvFilesToAdmin(_testDataFilePath + "\\TestData\\UploadFiles\\SELECTIONS.CSV", ref browser);
            _testDataObj.MapTestDataXls();

            _testDataObj.CreateCustomerData(ref browser);
        }

    }
}
