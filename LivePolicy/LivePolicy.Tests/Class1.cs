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


    [TestFixture]
    public class PolicyInfoFixture
    {

        private PolicyInfo _fakePolicy;

        [SetUp]
        public void Setup()
        {
            _fakePolicy = new PolicyInfo();
        }

        [Test]
        public void should_be_able_to_read_and_write_strings()
        {
            _fakePolicy.Add("Key", "Value");
            _fakePolicy["Key"].Should().Be("Value");
        }

        [Test]
        public void should_be_able_to_read_and_write_ints()
        {
            _fakePolicy.Add("Key", 1);
            _fakePolicy.GetInt("Key").Should().Be(1);
        }

        [Test]
        public void should_be_able_to_read_and_write_dates()
        {
            var time = new DateTime(2001, 1, 3, 5, 6, 9);

            _fakePolicy.Add("Key", time);
            _fakePolicy.GetDate("Key").Should().Be(time);
        }

        [Test]
        public void should_be_able_to_read_and_write_booleans()
        {
            _fakePolicy.Add("Key", true);
            _fakePolicy.GetBoolean("Key").Should().Be(true);

            _fakePolicy.Add("Key2", false);
            _fakePolicy.GetBoolean("Key2").Should().Be(false);
        }
    }
}
