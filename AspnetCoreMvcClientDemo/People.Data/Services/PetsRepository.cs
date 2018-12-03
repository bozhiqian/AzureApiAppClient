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
        private readonly HttpClient _httpClient;
        public PetsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string RequestUri { get; set; }
        public string BaseAddress => _httpClient.BaseAddress.ToString(); 


        // Used to call AGL json web service http://agl-developer-test.azurewebsites.net/people.json
        public async Task<PetOwnersViewModel> GetPetOwnerAsync()
        {
            var petOwnersViewModel = new PetOwnersViewModel();

            //GET Method
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(RequestUri);
                petOwnersViewModel.StatusCode = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var petOwners = JsonConvert.DeserializeObject<List<PetOwner>>(json);

                    petOwnersViewModel.PetOwners = petOwners;
                }
                else
                {
                    petOwnersViewModel.PetOwners = new List<PetOwner>();
                }

            }
            catch (HttpRequestException ex)
            {
                petOwnersViewModel.Exception = ex;
            }
            catch (Exception ex)
            {
                petOwnersViewModel.Exception = ex;
            }

            return petOwnersViewModel;
        }

        
    }
}
