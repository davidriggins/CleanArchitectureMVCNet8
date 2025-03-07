using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        //private readonly IVillaRepository _villaRepo;
        //public VillaController(IVillaRepository villaRepo)
        //{
        //    _villaRepo = villaRepo;
        //}

        private readonly IUnitOfWork _unitOfWork;
        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ////var villas = _db.Villas.ToList();
            //var villas = _villaRepo.GetAll();
            var villas = _unitOfWork.Villa.GetAll();

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
                ////_db.Villas.Add(obj);
                ////_db.SaveChanges();
                //_villaRepo.Add(obj);
                //_villaRepo.Save();
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Villa.Save();

                TempData["success"] = "Villa Created Successfully";

                return RedirectToAction(nameof(Index));
            }

            return View();

        }


        public IActionResult Update(int villaId)
        {
            //var VillaList = _db.Villas.FirstOrDefault(v => v.Price > 50 && v.Occupancy > 0);

            ////var villa = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            //Villa? obj = _villaRepo.Get(v => v.Id == villaId);
            Villa? obj = _unitOfWork.Villa.Get(v => v.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                ////_db.Villas.Update(obj);
                ////_db.SaveChanges();
                //_villaRepo.Update(obj);
                //_villaRepo.Save();
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Villa.Save();

                TempData["success"] = "Villa Updated Successfully";

                return RedirectToAction(nameof(Index));
            }

            return View();

        }


        public IActionResult Delete(int villaId)
        {
            ////var villa = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            //Villa? villa = _villaRepo.Get(v => v.Id == villaId);
            Villa? villa = _unitOfWork.Villa.Get(v => v.Id == villaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            ////Villa? objFromDb = _db.Villas.FirstOrDefault(v => v.Id == obj.Id);
            //Villa? objFromDb = _villaRepo.Get(v => v.Id == obj.Id);
            Villa? objFromDb = _unitOfWork.Villa.Get(v => v.Id == obj.Id);

            if (objFromDb is not null)
            {
                ////_db.Villas.Remove(objFromDb);
                ////_db.SaveChanges();
                //_villaRepo.Remove(objFromDb);
                //_villaRepo.Save();
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Villa.Save();

                TempData["success"] = "Villa Deleted Successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Villa Could not be Deleted";

            return View();

        }
    }
}
