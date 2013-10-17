namespace AdminSuite.CommonControls
{
    class EventDetailsPage
    {
        #region Selection Details page
        public const string SelectionStatusLstBx = "name=OcStatus";
        public const string SelectionUpdateBtn = "css=input[type=\"button\"]";
        public const string PriceTxtBx = "name=OcLP";
        public const string BackButton = "//input[@type='button' and @value='Back']";
        public const string multipleKeyTxtBx = "name=OcMultKey";
        public const string SelectionDispStatusListBx = "name=OcDisplayed";
        public const string addSelectionBtn = "//input[@value='Add Selection']";
        #endregion

        #region Market Details page
        public const string ModifyMarketButton = "css=input[value='Modify Market']";
        public const string HandicapValueListBox = "css=select[name='MktHcapValue']";
        public const string HandicaValueLabels = "//select[@name='MktHcapValue']/option";
        public const string BirIndexTextBox = "css=input[name='MktBirIndex']"; // BIR index field value in the market page
        public const string mktDisplayListBox = "name=MktDisplayed";
        public const string marketStatusListBox = "name=MktStatus";


        #region Each way details
        public const string EachWayPlacesTxtBx = "name=MktEWPlaces";
        public const string EachWayTopTxtBx = "name=MktEWFacNum";
        public const string EachWayBottomTxtBx = "name=MktEWFacDen";
        #endregion

        #endregion

        #region Event Details Page Controls
        // Event Display Status ListBox present in Event Details page
        public const string eventDisplayStatusLstBx = "name=EvDisplayed";
        // Event Status ListBox present in Event Details page
        public const string eventStatusListBox = "name=EvStatus";
        // Event Description present in Event Details page
        public const string eventDescriptionTextBox = "name=EvDesc";
        // Update button in Event Details page
        public const string updateEventBtn = "css=th.buttons > input[type=\"button\"]";
        public const string showEventBtn = "//form[@name='evform']//th[@class='buttons']//input[@type='button' and @value='Show Events']";
        public const string addEventBtn = "//th[@class='buttons']//input[@type='button' and @value='Add Event']";
        #endregion

        #region Search Page Controls
        //Events Link present in LHN
        public const string eventLink = "link=Events";
        //Category Name Listbox present Events Search Page
        public const string categoryNameLstBx = "name=Category";
        //Event Class Name Listbox present Events Search Page
        public const string classNameLstBx = "name=ClassId";
        //Event Event Type Listbox present Events Search Page
        public const string eventTypeLstBx = "name=TypeId";
        //Event Event SubType Listbox present Events Search Page
        public const string subEventTypeLstBx = "name=SubTypeId";
        //DateRange Listbox present Events Search Page
        public const string dateRangeLstBx = "name=date_range";
        // Event Search button present Events Search Page
        public const string eventSearchBtn = "css=input[type=\"button\"]";

        #endregion

        public const string currentHandicapValue = "//select[@name='MktHcapValue']/option[@selected='']";
        public const string ModifySelectionButton = "//input[@type='button'][@value='Modify Selection']";

        #region Create Event Page Controls
        public const string addMarketBtn = "//input[@type='button' and @value='Add Market']";
        public const string StartTimetxtBox = "//input[@id='start_time']";
        public const string addEventMarketBtn = "//input[@type='button' and @value='Add Event Market']";
        public const string mktDisplayStatusLstBox = "//select[@name='MktDisplayed']";
        public const string marketEWAvailLstBox = "//select[@name='MktEWAvail']";
        public const string addMarketSelectionBtn = "//th[@class='buttons']//input[@type='button' and @value='Add Market Selection']";
        public const string selectionDescTxtBox = "//input[@type='text' and @name='OcDesc']";
        #endregion

    }
}
