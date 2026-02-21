using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MedicalSystem.Models;
using MedicalSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MedicalSystem.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchName, int? specializationId)
    {
        ViewBag.Specializations = await _context.Specializations.ToListAsync();

        if (string.IsNullOrEmpty(searchName) && !specializationId.HasValue)
        {
            return View(new List<Doctor>());
        }

        var query = _context.Doctors
            .Include(d => d.Specializations)
            .Where(d => d.IsApproved)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchName))
        {
            query = query.Where(d => d.LastName.Contains(searchName) || d.FirstName.Contains(searchName));
        }

        if (specializationId.HasValue)
        {
            query = query.Where(d => d.Specializations.Any(s => s.Id == specializationId));
        }

        var doctors = await query.ToListAsync();
        return View(doctors);
    }

    public async Task<IActionResult> IndexDoctor()
    {
       var email = User.FindFirstValue(ClaimTypes.Email);
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);

        if (doctor == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        return View("IndexDoctor", doctor); 
    }

    public async Task<IActionResult> DoctorDetails(int id)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Specializations)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doctor == null) return NotFound();

        return View(doctor);
    }

    public async Task<IActionResult> DoctorSchedules(int id)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Schedules) 
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doctor == null) return NotFound();

        return View(doctor);
    }
}
