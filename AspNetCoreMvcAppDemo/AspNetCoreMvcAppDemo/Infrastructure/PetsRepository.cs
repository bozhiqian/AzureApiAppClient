using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AspNetCoreMvcAppDemo.Models;
using Newtonsoft.Json;

namespace AspNetCoreMvcAppDemo.Infrastructure
{
    public class PetsRepository : IPetsRepository
    {
        public PetsRepository()
        {
            
        }
        public async Task<List<PetOwner>> GetPetOwnerAsync(string baseUrl, string requestUri)
        {
            try
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
                        var petOwners = JsonConvert.DeserializeObject<List<PetOwner>>(json);

                        return petOwners;
                    }
                }
            }
            catch(Exception ex)
            {
                // loging...

                // Rethrow.
                throw ex;
            }
            return new List<PetOwner>();
        }
    }
}
