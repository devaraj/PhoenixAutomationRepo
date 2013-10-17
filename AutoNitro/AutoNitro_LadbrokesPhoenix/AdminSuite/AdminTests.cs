using System.Threading;
using AdminSuite.CustomerCreation;
using Framework;
using MbUnit.Framework;

namespace AdminSuite
{
    [TestFixture(ApartmentState = ApartmentState.STA, TimeOut = FrameGlobals.TestCaseTimeout)]
    public class AdminTests : AdminBase
    {
        private readonly CreateCustomers _createNewCustomer = new CreateCustomers();

        [Test]
        public void CreateSelfExcludedCustomer()
        {
            _createNewCustomer.SelfExcludedCustomer(MyBrowser);
        }

        [Test]
        public void CreateCustomerBannedForSomeCountry()
        {
            _createNewCustomer.CustomerFrmBannedCntry(MyBrowser);
        }

        [Test]
        public void CreateCompanyExcludedCustomer()
        {
            _createNewCustomer.CustomerCompanyExclusion(MyBrowser);
        }

        [Test]
        public void CreateSuspendedCustomer()
        {
            _createNewCustomer.SuspCustomer(MyBrowser);
        }
    }
}