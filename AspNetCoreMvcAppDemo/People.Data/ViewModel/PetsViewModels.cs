using System;
using System.Collections.Generic;
using System.Text;

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
