using MedTrack.Api.Services;
using Xunit;

namespace MedTrack.Api.Tests.Services
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _hasher = new PasswordHasher();

        [Fact]
        public void Verify_ReturnsTrue_ForCorrectPassword()
        {
            var hash = _hasher.Hash("correct horse battery");

            Assert.True(_hasher.Verify("correct horse battery", hash));
        }

        [Fact]
        public void Verify_ReturnsFalse_ForWrongPassword()
        {
            var hash = _hasher.Hash("correct horse battery");

            Assert.False(_hasher.Verify("correct horse stapler", hash));
        }

        [Fact]
        public void Hash_UsesRandomSalt()
        {
            Assert.NotEqual(_hasher.Hash("same-password"), _hasher.Hash("same-password"));
        }

        [Fact]
        public void Verify_ReturnsFalse_ForMalformedHash()
        {
            Assert.False(_hasher.Verify("whatever", "not-a-hash"));
        }
    }
}
