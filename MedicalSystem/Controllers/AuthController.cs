using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.Models;
using MedicalSystem.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registration() => View();

        [HttpGet]
        public IActionResult RegisterPatient() 
        {
            return View(new RegisterPatientViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPatient(RegisterPatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Email = model.Email,
                    HashPassword = model.Password, 
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone, 
                    UserGender = model.Gender
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
            return View(new RegisterDoctorViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(RegisterDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    Email = model.Email,
                    HashPassword = model.Password, 
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone, 
                    UserGender = model.Gender,
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