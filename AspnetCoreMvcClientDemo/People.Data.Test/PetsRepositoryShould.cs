using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using People.Data.Services;
using Xunit;

namespace People.Data.Test
{
    public class PetsRepositoryShould
    {
        private readonly string _petOwnersJson;

        public PetsRepositoryShould()
        {
            using (StreamReader r = new StreamReader("people.json"))
            {
                _petOwnersJson = r.ReadToEnd();
            }

        }

        [Fact]
        public async Task ValidatePetsFromOwnersOk()
        {
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_petOwnersJson)
            });

            var client = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = new Uri("http://agl-developer-test.azurewebsites.net/")
            };

            var sut = new PetsRepository(client);
            var petOwnersViewModel = await sut.GetPetOwnerAsync();

            Assert.True(petOwnersViewModel.Exception == null);
            Assert.True(petOwnersViewModel.StatusCode == HttpStatusCode.OK);
            Assert.True(petOwnersViewModel.PetOwners != null && petOwnersViewModel.PetOwners.Count == 6);
        }

        [Fact]
        public async Task ValidatePetsFromOwnersOkButNoData()
        {
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            });

            var client = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = new Uri("http://agl-developer-test.azurewebsites.net/")
            };

            var sut = new PetsRepository(client);
            var petOwnersViewModel = await sut.GetPetOwnerAsync();

            Assert.True(petOwnersViewModel.Exception == null);
            Assert.True(petOwnersViewModel.StatusCode == HttpStatusCode.OK);
            Assert.True(petOwnersViewModel.PetOwners == null);
        }

        [Fact]
        public async Task ValidatePetsFromOwnersNotFound()
        {
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(_petOwnersJson)
            });

            var client = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = new Uri("http://agl-developer-test.azurewebsites.net/")
            };

            var sut = new PetsRepository(client);
            var petOwnersViewModel = await sut.GetPetOwnerAsync();

            Assert.True(petOwnersViewModel.Exception == null);
            Assert.True(petOwnersViewModel.StatusCode == HttpStatusCode.NotFound);
            Assert.True(petOwnersViewModel.PetOwners != null && petOwnersViewModel.PetOwners.Count == 0);
        }

        [Fact]
        public async Task ValidatePetsFromOwnersInternalServerError()
        {
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(string.Empty)
            });

            var client = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = new Uri("http://agl-developer-test.azurewebsites.net/")
            };

            var sut = new PetsRepository(client);
            var petOwnersViewModel = await sut.GetPetOwnerAsync();

            Assert.True(petOwnersViewModel.Exception == null);
            Assert.True(petOwnersViewModel.StatusCode == HttpStatusCode.InternalServerError);
            Assert.True(petOwnersViewModel.PetOwners != null && petOwnersViewModel.PetOwners.Count == 0);
        }
        

        [Fact]
        public async Task NoExceptionAndReturnOk()
        {

            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://agl-developer-test.azurewebsites.net/")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var sut = new PetsRepository(client) {RequestUri = "people.json"};
            var petOwnersViewModel = await sut.GetPetOwnerAsync();

            Assert.True(petOwnersViewModel.Exception == null);
            Assert.True(petOwnersViewModel.StatusCode == HttpStatusCode.OK);
            Assert.True(petOwnersViewModel.PetOwners != null);
        }

        [Fact]
        public async Task ThrowExceptionWhenApiUrlInvalid()
        {

            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://agl-developer-test.azurewebsites.net/")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var sut = new PetsRepository(client);
            //sut.RequestUri = "people.json";
            var petOwnersViewModel = await sut.GetPetOwnerAsync();

            Assert.True(petOwnersViewModel.Exception == null);
            Assert.True(petOwnersViewModel.StatusCode != HttpStatusCode.OK);

        }
    }
}