using System.Collections.Generic;

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