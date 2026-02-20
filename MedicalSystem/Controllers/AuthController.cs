using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.Models;
using MedicalSystem.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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

                TempData["SuccessMessage"] = "Дякуємо! Ваша заявка прийнята. Будь ласка, зачекайте на підтвердження адміністратора.";
            }
            ViewBag.Specializations = await _context.Specializations.ToListAsync();
            return RedirectToAction(nameof(RegisterDoctor));
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Email == model.Email && p.HashPassword == model.Password);

                if (patient != null)
                {
                    await Authenticate(patient.Email, "Patient");
                    return RedirectToAction("Index", "Home"); 
                }

                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.Email == model.Email && d.HashPassword == model.Password);

                if (doctor != null)
                {
                    if (!doctor.IsApproved)
                    {
                        ModelState.AddModelError("", "Ваш акаунт ще не підтверджено адміністратором.");
                        return View(model);
                    }
                    
                    await Authenticate(doctor.Email, "Doctor");
                    return RedirectToAction("IndexDoctor", "Home");
                }

                ModelState.AddModelError("", "Невірний email або пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync("Cookies",
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });
        }
    }
}