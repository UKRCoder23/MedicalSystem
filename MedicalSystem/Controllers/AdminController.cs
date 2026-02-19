using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.Models;
using MedicalSystem.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> AdminApproveList()
        {
            var pendingDoctors = await _context.Doctors
                .Where(d => !d.IsApproved)
                .ToListAsync();
            return View(pendingDoctors);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                doctor.IsApproved = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminApproveList));
        }
    }
    
}