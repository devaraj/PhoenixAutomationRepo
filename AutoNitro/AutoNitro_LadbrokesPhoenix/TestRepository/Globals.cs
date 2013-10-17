using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using AdminSuite;
using Framework;


namespace TestRepository
{

    public static class Globals
    {

        public enum CustomerTypes
        {
            BannedCountryCustomer,
            CompanyExcludedCustomer,
            DefaultBetaCustomer,
            DefaultCustomer,
            SelfExcludedCustomer,
            SuspendedCustomer,
            LockAttemptCustomer,
            HVCCustomer
        }
        public enum searchQueryType
        {
            EventClass,
            EventType,
            EventSubType,
            Market,
            EventName,
            Others
        }

        public enum BetSlipTerms
        {
            TricastBetType,
            ForecastBetType
        }
        /// <summary>
        /// Retrieves customer credentials from Admin resource file
        /// </summary>
        /// <param name="user">The user you wish to retrieve</param>
        /// <returns>The MySpaceUser you specified</returns>
        public static CustomerInformation GetCustomerCredentials(CustomerTypes customerType)
        {
            var cusInfo = new CustomerInformation();
            string[] custCreds = AdminSuite.Common.GetCustomerCredentials(Convert.ToString(customerType));
            cusInfo.MCustomerName = custCreds[0];
            cusInfo.MPassword = custCreds[1];
            return cusInfo;
        }

        public static class MultiBetType
        {
            public const string multipleSingleBetString = "Singles";
            public const string doubleBetString = "Double";
            public const string trebleBetString = "Treble";
            public const string trixieBetString = "Trixie";
            public const string patentBetString = "Patent";
            public const string yankeeBetString = "Yankee";
            public const string heinzeBetString = "Heinz";
            public const string canadianBetString = "Canadian";
            public const string lucky15BetString = "Lucky 15";
            public const string lucky31BetString = "Lucky 31";
            public const string lucky64BetString = "Lucky 64";
            public const string lucky63BetString = "Lucky 63";
            public const string accumulator4BetString = "Accumulator (4)";
            public const string accumulator5BetString = "Accumulator (5)";
            public const string accumulator6BetString = "Accumulator (6)";
            public const string accumulator7BetString = "Accumulator (7)";
            public const string accumulator8BetString = "Accumulator (8)";
            public const string superHeinzBetString = "Super Heinz";
        }
    
        /// <summary>
        /// Init of Test repository Globals
        /// </summary>
        private static void Init()
        {
            FrameGlobals.Init();
            //  Configuration dllConfig = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().EscapedCodeBase.Replace("file:///", "").Replace("/", "\\").Replace("%20", " "));
        }
        public const string homePageUrl = "//tst-mobile.ladbrokes.com/lobby/games";

        #region Home page elements URLs


        #endregion
    }
    public class CustomerInformation
    {

        public string MCustomerName = "";

        public string CustomerName
        {
            get { return MCustomerName; }
            set { value = MCustomerName; }
        }

        public string MPassword = "";
        public string Password
        {
            get { return MPassword; }
            set { value = MPassword; }
        }
      
    }
}


