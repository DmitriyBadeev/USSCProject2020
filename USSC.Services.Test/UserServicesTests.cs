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
    }
}