using System.Collections.Generic;

namespace OwnersAndPets.ViewModels
{
    public class ChildPageViewModel : BaseViewModel
    {
        public ChildPageViewModel(string ownerName)
        {
            OwnerName = ownerName;
            Title = $"{OwnerName} pets";
        }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public IEnumerable<PetViewModel> Pets { get; set; }
    }
}