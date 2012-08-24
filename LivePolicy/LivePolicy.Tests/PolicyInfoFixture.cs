using System;
using FluentAssertions;
using NUnit.Framework;

namespace LivePolicy.Tests
{
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
            _fakePolicy.GetString("Key").Should().Be("Value");
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
            var time = new DateTime(2001, 1, 3, 5, 6, 9,1);

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

        [Test]
        public void get_invalid_key()
        {
            _fakePolicy.GetInt("key").Should().Be(0);
            _fakePolicy.GetDate("key").Should().Be(DateTime.MinValue);
            _fakePolicy.GetBoolean("key").Should().Be(false);
            _fakePolicy.GetString("Key").Should().Be("");
        }

        [Test]
        public void can_automatically_convert_string_to_int()
        {
            _fakePolicy.Add("key","1234");
            _fakePolicy.GetInt("key").Should().Be(1234);
        }


        [Test]
        public void invalid_key_should_throw_argument_exceptions()
        {
            Assert.Throws<ArgumentException>(() => _fakePolicy.GetInt(""));
            Assert.Throws<ArgumentException>(() => _fakePolicy.GetDate(""));
            Assert.Throws<ArgumentException>(() => _fakePolicy.GetBoolean(""));
            Assert.Throws<ArgumentException>(() => _fakePolicy.GetString(""));

            Assert.Throws<ArgumentNullException>(() => _fakePolicy.GetInt(null));
            Assert.Throws<ArgumentNullException>(() => _fakePolicy.GetDate(null));
            Assert.Throws<ArgumentNullException>(() => _fakePolicy.GetBoolean(null));
            Assert.Throws<ArgumentNullException>(() => _fakePolicy.GetString(null));
        }
    }
}