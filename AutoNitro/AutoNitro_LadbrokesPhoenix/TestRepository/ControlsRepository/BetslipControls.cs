using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestRepository.ControlsRepository
{
    public class BetslipControls
    {

        public const string betslipBanner = "//span[@class='t7 page-title' and contains(text(), 'Betslip')]";
        public const string betslipButton = "//div[@class='ui betlsip-button']";
        public const string betslipCount = "id=betslip-value";
        public const string betslipBackArrow = "//div[@id='betslip']//span[@class='bxc arrow back-button']";
        public const string removeAllSels = "//span[@class='bxc remove-button' and contains(text(),'Remove all')]";
        public const string totalStake = "//div[contains(text(), 'Total stake')]/following-sibling::div[@id='display_stake_total']";
        public const string totalPotentialReturns = "//div[contains(text(), 'Total Potential Returns')]/following-sibling::div[@id='potential_return_total']";
        public const string placeBet = "//a[starts-with(@class, 'bxc bxf button mb50') and contains(text(),'PLACE BET')]";

        public const string betReceiptBanner = "//span[@class='t7 page-title' and contains(text(), 'Betslip Receipt')]";
        public const string backToHomeButton = "//a[@class='bxc bxf sec-button' and contains(text(), 'Back to homepage')]";
        public const string reUseSelection = "//a[@class='bxc bxf sec-button' and contains(text(), 'Back to homepage')]";
        public const string betSuccessfulMsg = "//div[@class='bxcl ffd ttu fs15px pa10 rel t2' and contains(text(), 'Bets placed successfully')]";
        public const string betInfoContainer = "//div[@class='popup-box']";
        public const string closeButtonInInfoContainer = "//div[starts-with(@class, 'alert3-button') and contains(text(), 'Close')]";

        public const string betslipErrorMessage = "//div[@class='bxc slip-error']";

        public const string multiplesBanner = "//span[@class='bxcl bxf']";
        public const string inplayCounter = "//p[@class='bxc man pan mt10 lh18 ffdc tac ttu c1 fs18px spinner-text' and contains(text(), 'Please wait while we place your In-Play bet')]";

        public const string insufficientFundErMsg = "//div[@id='alert3-one' and contains(text(), 'Insufficient Funds')]/following::div[@id='alert3-two' and contains(text(), 'You have Insufficient funds to place this bet. Please deposit funds or adjust your stake.')]";
        public const string depositFundsBtn = "//a[@id='deposit-button' and contains(text(), 'Deposit Funds')]";
        public const string backToBetslipBtn = "//div[@class='alert3-button-inner ars' and contains(text(), 'Back To Betslip')]";
        public const string selectionSuspendedMsg = "//div[@class='bxc slip-error' and contains(string(),'This selection is suspended')]";

        public const string negBalanceErrMsg = "//div[@class='alert3-header bxc']//div[@id='alert3-one' and contains(text(), 'Information')]/following::div[@id='alert3-two' and contains(text(), 'Please note your Sports wallet balance has slipped below zero. Press Continue to be taken to the Deposit page.')]";

    }

}
