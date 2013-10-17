namespace TestDataCreation
{
    class ControlsRepository
    {
        ///<summary>
        ///Controls for Admin Login page
        ///</summary>
        #region
        public const string UsrNmeTxtBx = "id=username";
        public const string PwdTxtBx = "id=password";
        public const string LoginBtn = "//input[@type='submit' and @value='Login']";
        #endregion
        /// <summary>
        /// Controls for link element @ LHN->Queries-->Customers
        /// </summary>
        #region Queries in LHN
        //Control for link element @ LHN->Queries-->Customers
        public const string CustomersLink = "//li[a[text()='Pools bet']]/following-sibling::li[a[text()='Customers']]";
        #endregion
    }
}
