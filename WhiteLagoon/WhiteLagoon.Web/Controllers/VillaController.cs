using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();

            return View(villas);
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

                return RedirectToAction("Index");
            }

            return View();

        }
    }
}
