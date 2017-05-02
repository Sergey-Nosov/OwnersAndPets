using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using OwnersAndPets.Data;
using OwnersAndPets.Models;
using OwnersAndPets.ViewModels;
using OwnersAndPets.RequestObjects;

namespace OwnersAndPets.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(SortingData sort = null)
        {
            using (Context context = Context.Create())
            {
                var havePets = context.Owners
                    .Join(context.OwnerPets, owner => owner.Id, ownerpet => ownerpet.OwnerId,
                        ((o, op) => new {op.OwnerId, op.PetId, o.Name}))
                    .GroupBy(entity => entity.OwnerId).Select(g => new PetsGroupingViewModel
                    {
                        OwnerId = g.FirstOrDefault().OwnerId,
                        Count = g.Count(),
                        OwnerName = g.FirstOrDefault().Name
                    });
                var dontHavePets = from o in context.Owners
                    join op in context.OwnerPets on o.Id equals op.OwnerId into resInner
                    from subpet in resInner.DefaultIfEmpty()
                    where !resInner.Any()
                    select new PetsGroupingViewModel {OwnerId = o.Id, Count = 0, OwnerName = o.Name};
                var data = havePets.Concat(dontHavePets).OrderBy(x => x.OwnerId);
                IQueryable<PetsGroupingViewModel> result = data.ToList().AsQueryable();
                if (sort != null && !string.IsNullOrEmpty(sort.sortDirection) && !string.IsNullOrEmpty(sort.fieldName))
                {
                    result = result.OrderBy($"{sort.fieldName} {sort.sortDirection}");
                }
                ViewBag.SortOwners = sort;
                return View(new MainPageViewModel {Groups = result.ToList()});
            }
        }

        [HttpGet]
        public ActionResult OwnerPets(PetsRequest petsRequest = null)
        {
            using (Context context = Context.Create())
            {
                var ownerId = petsRequest != null ? petsRequest.OwnerId : 1;
                var ownerEntity = context.Owners.FirstOrDefault(owner => owner.Id == ownerId);
                if (ownerEntity == null)
                {
                    throw new DataException();
                }
                var ownerName = ownerEntity.Name;
                var pets = (from op in context.OwnerPets
                        join p in context.Pets
                        on op.PetId equals p.Id
                        where op.OwnerId == ownerId
                        select new PetViewModel { Name = p.Name, Id = p.Id });//.ToList();
                //IQueryable<PetViewModel> result = pets;//.ToList().AsQueryable();
                if (petsRequest != null && !string.IsNullOrEmpty(petsRequest.sortDirection) && !string.IsNullOrEmpty(petsRequest.fieldName))
                {
                    pets = pets.OrderBy($"{petsRequest.fieldName} {petsRequest.sortDirection}");
                }
                ViewBag.SortPets = petsRequest;
                //return View(new MainPageViewModel { Groups = result.ToList() });
                return View(new ChildPageViewModel(ownerName) { Pets = pets.ToList(), OwnerId = ownerId});
            }
        }

        [HttpPost]
        public ActionResult AddOwnerRecord(string ownerName)
        {
            using (Context context = Context.Create())
            {
                context.Owners.Add(new Owner { Name = ownerName });
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
       
        public ActionResult AddPetRecord(AddPetData petData)
        {
            using (Context context = Context.Create())
            {
                var bdContextTransaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {  
                    var petToAdd = new Pet { Name = petData.PetName };
                    context.Pets.Add(petToAdd);
                    context.SaveChanges();
                    var ownerPet = new OwnerPet { OwnerId = petData.OwnerId, PetId = petToAdd.Id };
                    context.OwnerPets.Add(ownerPet);
                    context.SaveChanges();
                    bdContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    bdContextTransaction.Rollback();
                    throw;
                }              
                return RedirectToAction("OwnerPets", "Home", new { ownerId  = petData.OwnerId });
            }
        }

        // action parameters should start from lower case character.
        public ActionResult DeleteOwnerRecord(int OwnerId)
        {
            using (Context context = Context.Create())
            {
                var ownerToDelete = context.Owners.FirstOrDefault(owner => owner.Id == OwnerId);
                if (ownerToDelete != null)
                {
                    context.Owners.Remove(ownerToDelete);
                    context.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DeletePetRecord(DeletePetData data)
        {
            using (Context context = Context.Create())
            {
                var petToDelete = context.Pets.FirstOrDefault(pet => pet.Id == data.PetId);
                if (petToDelete != null)
                {
                    context.Pets.Remove(petToDelete);
                    context.SaveChanges();
                }
                return RedirectToAction("OwnerPets", "Home", new
                {
                    ownerId = data.OwnerId
                });
            }
        }
    }
}