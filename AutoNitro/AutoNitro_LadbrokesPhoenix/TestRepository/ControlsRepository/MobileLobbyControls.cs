using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestRepository.ControlsRepository
{
    public class MobileLobbyControls
    {

        public const string lbLoadingIcon = "//div[@id='cboxContent']";
        public const string registrationTitle = "//span[@class='title' and text() = 'Registration']";
        public const string closebutton = "//a[@class='sright']";
        public const string Logo = "//img[@alt='Ladbrokes']";
        public const string backbutton = "//a[@id='sleft']";

        public const string depositTitle = "//span[@class='title' and text() = 'Deposit']";
        public const string balanceHeader = "//div[@class='samount' and contains(text(), 'Balance: £')]/span[@id='headerBalance' and contains(text(), '0.0')]";
        public const string depositFunds = "//input[@class='btn_lg ' and @value='Deposit Funds']";
        public const string successfulRgMsg = "//div[@id='mainContent']/h1[contains(text(), 'Registration Successful')]";
        public const string failureRgMsg = "//div[@id='mainContent']/h1[contains(text(), 'Registration Failure')]";
        public const string contactMessage = "//div[@id='mainContent']/p[contains(text()[1], 'Please contact Customer Service on ') and contains(text()[2], ', or the international number ') and contains(text()[3], ', or email: ')]/a[contains(text(), '0800 032 1133')]/following-sibling::a[contains(text(), '+350 20043003')]/following-sibling::a[contains(text(), 'care@ladbrokescasino.com')]";


        public const string promocode = "id=promocode";
        public const string title = "id=title";
        public const string firstname = "id=firstname";
        public const string lastname = "id=lastname";
        public const string genderMale = "//input[@id='gender' and @value='M']";
        public const string genderFemale = "//input[@id='gender' and @value='F']";

        public const string DOBday = "id=day";
        public const string DOByear = "id=year";
        public const string DOBmonth = "id=month";

        public const string country = "id=country";
        public const string housename = "id=housename";
        public const string postcode = "id=postcode";
        public const string address1 = "id=address";
        public const string address2 = "id=address2";

        public const string city = "id=city";
        public const string email = "id=email";
        public const string findaddress = "id=findaddress";

        public const string telintcode = "id=telintcode";
        public const string telnumber = "id=telnumber";
        public const string mobintcode = "id=mobintcode";
        public const string mobnumber = "id=mobnumber";
        public const string accountCurrency = "id=accountcurrency";

        public const string username = "id=username";
        public const string password = "id=password";
        public const string confirmPassword = "id=confirmpassword";
        public const string securityQuestion = "id=maidenname";

        public const string securityAnswer = "id=maidennameans";
        public const string contactMe = "id=yescontact";
        public const string aggreement = "id=tandc";
        public const string registerNow = "id=createaccount";
        public const string licenseText = "//div[@class='licensetext' and contains(text(), 'Licensed and Regulated in Gibraltar')]";

        #region DepositPage
        public const string cardTypeImages = "//span[@class='p_title' and contains(text(),'Deposit Now')]/following-sibling::span[@class='creditCard laser']/following-sibling::span[@class='creditCard solo']/following-sibling::span[@class='creditCard switch']/following-sibling::span[@class='creditCard maestro']/following-sibling::span[@class='creditCard mastercard']/following-sibling::span[@class='creditCard delta']/following-sibling::span[@class='creditCard visa']";
        public const string cardHolderName = "//input[@id='cardHolderName']";
        public const string cardNo = "//input[@id='cardNo']";
        public const string expiryMonth = "//select[@id='Deposit_expiryMonth']";
        public const string expiryYear = "//select[@id='Deposit_expiryYear']";
        public const string cvvNo = "//input[@id='CvvNo']";
        public const string cardPassword = "//input[@id='password']";
        public const string amout1 = "//input[@id='amountEntered']";
        public const string amout2 = "//input[@id='amountEntered2']";

        public const string depositText = "//div[contains(text(), 'Deposit Limit')]";
        public const string currencySymbol = "//label[@class='depwrap' and contains(text(), '£')]/following-sibling::label[@class='depwrap' and contains(text(), '.')]";
        public const string dailyLimit = "//input[@value='Daily']";
        public const string weeklyLimit = "//input[@value='Weekly']";
        public const string depositLimitAmount = "//select[@name='Deposit[amount]']";
        #endregion
    }
}

