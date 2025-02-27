﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

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
            //var villaNumbers = _db.VillaNumbers.ToList();
            var villaNumbers = _db.VillaNumbers.Include(v => v.Villa).ToList();

            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(v => new SelectListItem
            //{
            //    Text = v.Name,
            //    Value = v.Id.ToString()
            //});

            ////ViewData["VillaList"] = list;
            //ViewBag.VillaList = list;

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            //// The following line removes villa from validation
            //ModelState.Remove("Villa");

            bool roomNumberExists = _db.VillaNumbers.Any(v => v.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();

                TempData["success"] = "Villa Number Created Successfully";

                return RedirectToAction(nameof(Index));
            }

            // Error message if room number already exists
            if (roomNumberExists)
            {
                TempData["error"] = "Villa Number Already Exists";
            }

            // Need to repopulate the dropdown
            obj.VillaList = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(obj);

        }


        public IActionResult Update(int villaNumberId)
        {
            //var VillaList = _db.Villas.FirstOrDefault(v => v.Price > 50 && v.Occupancy > 0);

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(v => v.Villa_Number == villaNumberId)
            };

            //var villa = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {

            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();

                TempData["success"] = "Villa Number has been updated Successfully";

                return RedirectToAction(nameof(Index));
            }

            // Need to repopulate the dropdown
            villaNumberVM.VillaList = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(villaNumberVM);

        }


        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(v => v.Villa_Number == villaNumberId)
            };

            //var villa = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _db.VillaNumbers
                .FirstOrDefault(v => v.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();

                TempData["success"] = "Villa Number has been Deleted Successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Villa Number Could not be Deleted";

            return View();

        }
    }
}
