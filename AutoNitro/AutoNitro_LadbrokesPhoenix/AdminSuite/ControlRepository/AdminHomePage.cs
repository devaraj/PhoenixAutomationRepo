namespace AdminSuite.CommonControls
{
    /// <summary>
    /// Left hand navigation Controls in Admin home page
    /// </summary>
    ///  Author: Yogesh
    /// <param name=></param>
    /// Ex: 
    /// <returns>None</returns>
    /// Created Date: 22-Dec-2011
    /// Modified Date: 22-Dec-2011
    /// Modification Comments
    class AdminHomePage
    {
        ///<summary>
        ///Controls for Admin Login page
        ///</summary>
        #region Admin Login Page
        public const string UsrNmeTxtBx = "id=username";
        public const string PwdTxtBx = "id=password";
        public const string LoginBtn = "//input[@type='submit' and @value='Login']";
        #endregion

        /// <summary>
        /// Controls for link element @ LHN->Queries-->Customers
        /// </summary>
        #region Queries in LHN
        //Control for link element @ LHN->Queries-->Customers
        // public const string CustomersLink = "//li[a[text()='Pools bet']]/following-sibling::li[a[text()='Customers']]";
        public const string CustomersLink = "link=Customers";
        #endregion

        /// <summary>
        /// All the links in the Left Hand Frame of Admin Home Page
        /// </summary>
        #region Admin Home Page> Left Hand Bar Links
        public const string EventNameLink = "link=Events";
        public const string EventClassesLink = "//li[@class='menu_item']/a[@class='menu_item' and text()='Event Classes']";
        public const string EventClassesName = "//a[text()='|Football|']";
        #endregion

        /// <summary>
        /// Events Search Page
        /// Go to Admin Home Page and Click on Events Link in the left Hand frame
        /// </summary>
        #region Admin HomePage>Events Page
        public const string OpenBetIdTextBox = "name=openbet_id";
        public const string OpenBetHierarchyLevelDrpLst = "name=hierarchy_level";
        public const string OpenBetHeierarchyName = "label=Even outcome";
        public const string EventFindBtn = "//input[@value='Find']";
        public const string OpenBetHeierarchyLevelEvent = "label=Event";
        public const string ResultStatus = "//select[@name='OcResult']";
        #endregion

        #region Customer Home Page
        public const string custVerifyCountry = "//select[@name='country']";
        #endregion

    }
}
