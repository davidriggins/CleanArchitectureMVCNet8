using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        //private readonly IVillaRepository _villaRepo;
        //public VillaController(IVillaRepository villaRepo)
        //{
        //    _villaRepo = villaRepo;
        //}

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

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
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;

                    //string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    //string uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.Image.FileName;
                    //string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //obj.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    //obj.ImageUrl = uniqueFileName;
                }
                else {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }


                ////_db.Villas.Add(obj);
                ////_db.SaveChanges();
                //_villaRepo.Add(obj);
                //_villaRepo.Save();
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();

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

                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImageFile = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImageFile))
                        {
                            System.IO.File.Delete(oldImageFile);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;

                }


                ////_db.Villas.Update(obj);
                ////_db.SaveChanges();
                //_villaRepo.Update(obj);
                //_villaRepo.Save();
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();

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
                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImageFile = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImageFile))
                    {
                        System.IO.File.Delete(oldImageFile);
                    }
                }

                ////_db.Villas.Remove(objFromDb);
                ////_db.SaveChanges();
                //_villaRepo.Remove(objFromDb);
                //_villaRepo.Save();
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();

                TempData["success"] = "Villa Deleted Successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Villa Could not be Deleted";

            return View();

        }
    }
}
