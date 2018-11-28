using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using People.Data.Model;
using People.Data.ViewModel;

namespace People.Data.Services
{
    public class PetsRepository : IPetsRepository
    {
        public PetsRepository()
        {

        }

        // Used to call AGL json web service http://agl-developer-test.azurewebsites.net/people.json
        public async Task<List<PetOwner>> GetPetOwnerAsync(string baseUrl, string requestUri)
        {
            var json = await Request(baseUrl, requestUri);
            if(!string.IsNullOrEmpty(json))
            {
                var petOwners = JsonConvert.DeserializeObject<List<PetOwner>>(json);
                return petOwners;
            }
            return null;
        }

        // Used to call People WebAPI http://localhost:51888/api/Pets?includePerson=true
        public async Task<PetsViewModel> GetPetsAsync(string baseUrl, string requestUri)
        {
            var json = await Request(baseUrl, requestUri);
            if(!string.IsNullOrEmpty(json))
            {
                var pets = JsonConvert.DeserializeObject<PetsViewModel>(json);
                return pets;
            }
            return null;
        }

        public async Task<string> Request(string baseUrl, string requestUri)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method
                HttpResponseMessage response = await client.GetAsync(requestUri);
                if(response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    return json;
                }
                return null;
            }
        }
    }
}
