using System;

namespace People.Data.ViewModel
{
    public class PetsViewModels
    {
        public PetsViewModel PetsViewModelAgl { get; set; }
        public PetsViewModel PetsViewModelApi { get; set; }

        public Uri AglJsonUrl { get; set; }
        public Uri PeopleApiUrl { get; set; }
    }
}
