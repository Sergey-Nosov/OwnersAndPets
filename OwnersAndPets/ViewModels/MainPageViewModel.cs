using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwnersAndPets.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            Title = "All users";
        }

        public IEnumerable<PetsGroupingViewModel> Groups { get; set; }
    }
}