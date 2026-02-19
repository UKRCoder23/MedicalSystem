using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.Models;
using MedicalSystem.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult RegisterPatient() => View();

        [HttpPost]
        public async Task<IActionResult> RegisterPatient(RegisterPatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Email = model.Email,
                    HashPassword = model.HashPassword, 
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone, 
                    Gender = model.Gender
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Auth");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RegisterDoctor()
        {
            ViewBag.Specializations = await _context.Specializations.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(RegisterDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    Email = model.Email,
                    HashPassword = model.HashPassword, 
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone, 
                    Gender = model.Gender,
                    IsApproved = false,
                    Specializations = await _context.Specializations
                        .Where(s => model.SelectedSpecializationIds.Contains(s.Id))
                        .ToListAsync()
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                return View("RegistrationPending");
            }
            ViewBag.Specializations = await _context.Specializations.ToListAsync();
            return View(model);
        }
    }
}