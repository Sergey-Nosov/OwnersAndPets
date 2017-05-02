using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwnersAndPets.Models
{
    public class OwnerPet
    {
        public int? OwnerId { get; set; }
        public int? PetId { get; set; }
    }
}