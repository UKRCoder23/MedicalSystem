using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MedicalSystem.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly AppDbContext _context;
    public ProfileController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        if (patient != null)
        {
            return View("Patient", patient); 
        }

        var doctor = await _context.Doctors
            .Include(d => d.Specializations)
            .FirstOrDefaultAsync(d => d.Email == email);
        if (doctor != null)
        {
            ViewBag.AllSpecializations = await _context.Specializations.ToListAsync();
            return View("Doctor", doctor); 
        }

        return RedirectToAction("Login", "Auth");
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateDoctorProfile(string biography, int cabinet, string phone, List<int> selectedSpecializationIds, IFormFile? photoFile)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var doctor = await _context.Doctors
            .Include(d => d.Specializations)
            .FirstOrDefaultAsync(d => d.Email == email);

        if (doctor != null)
        {
            doctor.Biography = biography;
            doctor.Cabinet = cabinet;
            doctor.PhoneNumber = phone;

            if (photoFile != null && photoFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await photoFile.CopyToAsync(ms);
                    doctor.Photo = ms.ToArray();
                }
            }

            doctor.Specializations.Clear();
            if (selectedSpecializationIds != null && selectedSpecializationIds.Any())
            {
                var specs = await _context.Specializations
                    .Where(s => selectedSpecializationIds.Contains(s.Id))
                    .ToListAsync();
                foreach (var s in specs) doctor.Specializations.Add(s);
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Профіль лікаря успішно оновлено!";
        }
        return RedirectToAction("Index");
    }

    
    [HttpPost]
    public async Task<IActionResult> UpdatePatientProfile(string phone, DateTime? birthDate)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);

        if (patient != null)
        {
            patient.PhoneNumber = phone;
            patient.BirthDate = birthDate;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Дані оновлено!";
        }
        return RedirectToAction("Index");
    }
}