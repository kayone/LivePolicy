using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using FluentAssertions;
using Kayone.TestFoundation;
using Moq;
using NUnit.Framework;

namespace LivePolicy.Tests
{
    [TestFixture]
    public class PolicyServiceFixture : TestCore<PolicyService>
    {

        private PolicyInfo fakePolicy;
        private PolicyService _policyService;

        [SetUp]
        public void Setup()
        {
            fakePolicy = new PolicyInfo();

            _policyService = Mocker.Resolve<PolicyService>();
        }

        protected override PolicyService Subject
        {
            get
            {
                return _policyService;
            }

        }

        private void GiveUnavilableSource()
        {
            Mocker.GetMock<IPolicySource>()
                .Setup(c => c.FetchPolicy()).Throws<WebException>();
        }

        private void GiveUnavilableStorage()
        {
            Mocker.GetMock<IPolicyStorage>()
                .Setup(c => c.Write(It.IsAny<PolicyInfo>())).Throws<FileNotFoundException>();

            Mocker.GetMock<IPolicyStorage>()
                .Setup(c => c.Read()).Throws<FileNotFoundException>();
        }

        private void GiveSourcePolicy(PolicyInfo policyInfo)
        {
            Mocker.GetMock<IPolicySource>()
                  .Setup(c => c.FetchPolicy()).Returns(policyInfo);
        }

        private void GiveStoredPolicy(PolicyInfo policyInfo)
        {
            Mocker.GetMock<IPolicyStorage>()
                  .Setup(c => c.Read()).Returns(policyInfo);
        }

        [Test]
        public void load_should_readpolicy_from_source_and_store_to_storage()
        {
            GiveSourcePolicy(fakePolicy);

            Subject.Load().Should().BeTrue();
            Subject.Current.Should().Equal(fakePolicy);

            Mocker.GetMock<IPolicyStorage>().Verify(c => c.Write(fakePolicy), Times.Once());
            Mocker.GetMock<IPolicySource>().Verify(c => c.FetchPolicy(), Times.Once());
        }

        [Test]
        public void if_remote_load_failes_stored_policy_should_be_used()
        {
            GiveUnavilableSource();
            GiveStoredPolicy(fakePolicy);

            Subject.Load().Should().BeTrue();



            Mocker.GetMock<IPolicyStorage>().Verify(c => c.Write(It.IsAny<PolicyInfo>()), Times.Never());
            Subject.Current.Should().Equal(fakePolicy);
        }

        [Test]
        public void if_storage_is_not_avilable_load_should_pass()
        {
            GiveUnavilableStorage();

            Subject.Current = fakePolicy;
            Subject.Load().Should().BeFalse();
            
            Subject.Current.Should().Equal(fakePolicy);
        }


    }
}
