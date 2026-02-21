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
        public async Task<IActionResult> AdminPanel()
        {
            var doctors = await _context.Doctors.Include(d => d.Schedules).ToListAsync();
            return View(doctors);
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
            return RedirectToAction(nameof(AdminPanel));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null) {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminPanel));
        }
        [HttpPost]
        public async Task<IActionResult> SetSchedule(int doctorId, List<DayOfWeek> selectedDays, TimeSpan start, TimeSpan end)
        {
            var oldSchedules = _context.Schedules.Where(s => s.DoctorId == doctorId);
            _context.Schedules.RemoveRange(oldSchedules);

            foreach (var day in selectedDays)
            {
                _context.Schedules.Add(new Schedule
                {
                    DoctorId = doctorId,
                    Day = day,
                    StartTime = start,
                    EndTime = end
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
    }
}