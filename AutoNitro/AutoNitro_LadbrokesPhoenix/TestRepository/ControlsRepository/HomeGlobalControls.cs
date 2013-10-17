using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestRepository.ControlsRepository
{
    public class HomeGlobalControls
    {
        public const string bogbanner = "//div[@class='bog-banner']";
        public const string plus18 = "//a[@class='bxc responsible eighteen']";
        public const string govGibraltar = "//a[@class='bxc responsible gib']";
        public const string copyRightTxt = "//div[@class='bxc mb10 mt10' and contains(text(), '© Copyright © 2013 Ladbrokes')]";
        public const string ladbrokesAddress = "//div[@class='bxc mb10 mt10' and contains(text(), 'Ladbrokes International plc & Ladbrokes Sportsbook LP, Suites 6-8, 5th Floor, Europort, Gibraltar are licensed (RGL Nos. 010, 012 & 044) by the Government of Gibraltar and regulated by the Gibraltar Gambling Commissioner')]";
    }
}
