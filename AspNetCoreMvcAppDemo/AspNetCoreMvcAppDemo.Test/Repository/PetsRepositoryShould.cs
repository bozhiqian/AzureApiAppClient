using System;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Infrastructure;
using Xunit;

namespace AspNetCoreMvcAppDemo.Test.Repository
{
    public class PetsRepositoryShould
    {
        [Fact]
        public async Task ThrowExceptionWhenApiUrlInvalid()
        {
            var sut = new PetsRepository();

            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetPetOwnerAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetPetOwnerAsync(null, "people.json"));
            await Assert.ThrowsAsync<UriFormatException>(() => sut.GetPetOwnerAsync(string.Empty, "people.json"));
        }
    }
}
