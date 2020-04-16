using System;
using System.Globalization;
using NUnit.Framework;
using USSC.Services.UserServices;

namespace USSC.Services.Test
{
    public class UserServicesTests
    {
        [Test]
        public void HashPasswords__ShouldEquals()
        {
            var hash1 = Helpers.ComputeHash("123");
            var hash2 = Helpers.ComputeHash("123");
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void HashPasswords__CapitalsLetters()
        {
            var hash1 = Helpers.ComputeHash("asW");
            var hash2 = Helpers.ComputeHash("asW");
            var hash3 = Helpers.ComputeHash("asw");

            Assert.AreEqual(hash1, hash2);
            Assert.AreNotEqual(hash2, hash3);
        }

        [Test]
        public void DataTest()
        {
            var dateString = "20200414";
            var format = "yyyyMMdd";
            var culture = CultureInfo.InvariantCulture;

            var dataTime = DateTime.ParseExact(dateString, format, culture);

            var str = dataTime.ToString("ddd dd MMM yyyy h:mm tt zzz");
        }
    }
}