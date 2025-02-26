using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.ToList();

            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            // Custom Model Validation
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("", "Name and Description cannot be the same");
                //ModelState.AddModelError("Name", "Name and Description cannot be the same");
            }

            if (ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();

                TempData["success"] = "Villa Created Successfully";

                return RedirectToAction("Index");
            }

            return View();

        }


        public IActionResult Update(int villaId)
        {
            //var VillaList = _db.Villas.FirstOrDefault(v => v.Price > 50 && v.Occupancy > 0);

            var villa = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();

                TempData["success"] = "Villa Updated Successfully";

                return RedirectToAction("Index");
            }

            return View();

        }


        public IActionResult Delete(int villaId)
        {
            var villa = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(v => v.Id == obj.Id);

            if (objFromDb is not null)
            {
                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();

                TempData["success"] = "Villa Deleted Successfully";

                return RedirectToAction("Index");
            }

            TempData["error"] = "Villa Could not be Deleted";

            return View();

        }
    }
}
