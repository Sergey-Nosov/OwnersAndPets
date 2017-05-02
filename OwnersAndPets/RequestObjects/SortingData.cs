using System;

namespace OwnersAndPets.RequestObjects
{
    public class SortingData
    {
        public SortingData()
        {
            fieldName = String.Empty;
            sortDirection = String.Empty;
        }

        public string fieldName { get; set; }
        public string sortDirection { get; set; }

    }
}