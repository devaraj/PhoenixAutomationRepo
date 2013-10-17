using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestRepository.ControlsRepository
{
    public class LoginLogoutControls                        
    {
        public const string loadingIcon1 = "//div[@class='bxcvl splash-container']"; 
        public const string loadingIcon2 = "//div[@class='bxc spinner']";    
        public const string loadingIcon3 = "//*[@id='loading-div']/div";        
        public const string sidebar = "id=sidebar";
        public const string menuIcon = "//*[@id='menu-button']";

        public const string loginOrRegisterLink = "//div[@class='bxc login-box' and contains(text(),'Login / Register')]";
        public const string loginUsernameTextBox =  "id=username";           
        public const string loginPasswordTextBox = "//input[@name='login_password' and @class='input']";
        public const string loginSubmitButton = "id=login-submit-button";    
        public const string lostLoginButton = "//a[@class='bxc ml2 mt10 sec-button small' and contains(text(), 'Lost Login?')]";
        public const string logoutLink = "//span[@class='ml10' and text()='Logout']";
        public const string accountLink = "//span[@class='bxc bxf account-text rel t2 ml5 bxf' and text()='Account']";

        public const string loginBanner = "//span[@class='t7 page-title' and contains(text(), 'Login')]";
        public const string newToLadbrokesBanner = "//div[@class='bxc ffdbc fs25px ttu lsm1' and contains(text(), 'New to ladbrokes?')]";
        public const string registerButton = "//a[@class='bxc bxf sec-button' and contains(text(), 'Register')]";
        public const string homeLinkOnSideBar = "//span[@class='bxc sidebar-icon home']";
        public const string LadbrokesHomeLink = "//span[@class='bxc logo']";
        public const string balance = "id=headerBalance";
        public const string carousel = "id=carousel";
        public const string promotionalBanner = "//div[@id='carousel']/div[1]/a[1]/img";

        public const string contactUsBanner = "//span[@class='t7 page-title' and contains(text(), 'Contact Us')]";        
        public const string UKContacts = "//div[text()='UK:']/following-sibling :: a[1][contains(text(), '0')]";
        public const string NonUKContacts = "//div[text()='Rest of the world:']/following-sibling :: a[contains(text(), '+44')]";
        public const string emailContacts = "//span[text()='Email us at:']/following-sibling :: a[contains(text(),'@ladbrokes.com')]";
        

        public const string deposit = "//span[@class='ml10' and text()='Deposit']";
        public const string withdraw = "//span[@class='ml10' and text()='Withdraw']";
        public const string histroy = "//span[@class='ml10' and text()='History']";
        public const string viewBalances = "//span[@class='ml10' and text()='View Balances']";
        public const string redeemFreeBets = "//span[@class='ml10' and text()='Redeem Free Bets']";
        public const string depositLimits = "//span[@class='ml10' and text()='Deposit Limit']";
        public const string Transfer = "//span[@class='ml10' and text()='Transfer']";                
        public const string moreFromLadbrokes = "//span[@class='bxcl ffdc ttu lh12 fs15px' and contains(text(), 'Ladbrokes')]/following-sibling::span[@class='bxcl fs12px' and contains(text(), 'More from Ladbrokes')]";
        public const string quickLinks = "//span[@class='ml20' and contains(text(), 'Quick Links')]";

        public const string oddsInSideMenu = "//div[@class='bxcl bxf ml10' and contains(text(), 'Odds')]";
               
        public const string promoTitleBanner = "//span[@class='t7 page-title' and text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'promo')]]";

        public const string loginErrorpanel = "//div[@class='mt10 ml35 mr35 p14b t-red']";
        public const string alertContainer = "//div[@class='alert-box3-main-container w100p']//div[@id='alert3-one' and contains(text(), 'Information')]";
        public const string CloseButtonInAlertContainer = "//div[@class='alert3-button-inner ars' and contains(text(), 'Cancel')]";
        public const string ContinueButtonInAlertContainer = "//a[@class='bxc bxf button continue-button' and contains(text(), 'Continue')]";
        public const string fractionalOdd = "id=oddsTypeFractional_index";
        public const string decimalOdd = "id=oddsTypeDecimal_index";
        public const string InfoPageCloseBtn = "//div[@class='bxc bxf tac sec-button intro-page-button-close']";
    }
}

