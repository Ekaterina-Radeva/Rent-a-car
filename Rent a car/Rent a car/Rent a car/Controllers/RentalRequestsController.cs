using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rent_a_car.Data;
using Rent_a_car.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Rent_a_car.Controllers
{
    [Authorize]
    public class RentalRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public RentalRequestsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RentalRequests
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); // Get current user ID

            IQueryable<RentalRequest> requests = _context.RentalRequest
                .Include(r => r.Car)
                .Include(r => r.User);

            if (!User.IsInRole("Admin"))
            {
                requests = requests.Where(r => r.UserId == userId && r.IsApproved);
            }

            return View(await requests.ToListAsync());

        }

        // GET: RentalRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequest
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalRequest == null)
            {
                return NotFound();
            }

            return View(rentalRequest);
        }

        // GET: RentalRequests/Create
        public IActionResult Create(int carId)
        {
            var car = _context.Car.Find(carId);
            if (car == null) return NotFound();

            var model = new RentalRequest { CarId = carId };
            return View(model);
        }

        // POST: RentalRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CarId,StartDate,EndDate")] RentalRequest rentalRequest)
        {

            if (ModelState.IsValid)
            {
                // Check for overlapping reservations
                bool isCarAvailable = !_context.RentalRequest
                    .Where(r => r.CarId == rentalRequest.CarId && r.IsApproved)
                    .Any(r => (rentalRequest.StartDate <= r.EndDate) && (rentalRequest.EndDate >= r.StartDate));

                if (!isCarAvailable)
                {
                    ModelState.AddModelError(string.Empty, "This car is already reserved for the selected dates.");
                    return View(rentalRequest);
                }

                var userId = _userManager.GetUserId(User); // Gets the logged-in user's ID
                rentalRequest.UserId = userId;

                _context.Add(rentalRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "id", "id", rentalRequest.CarId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rentalRequest.UserId);
            return View(rentalRequest);

        }

        // GET: RentalRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequest.FindAsync(id);
            if (rentalRequest == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "id", "id", rentalRequest.CarId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rentalRequest.UserId);
            return View(rentalRequest);
        }

        // POST: RentalRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CarId,StartDate,EndDate")] RentalRequest rentalRequest)
        {
            if (id != rentalRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalRequestExists(rentalRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Car, "id", "id", rentalRequest.CarId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rentalRequest.UserId);
            return View(rentalRequest);
        }

        // GET: RentalRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequest
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalRequest == null)
            {
                return NotFound();
            }

            return View(rentalRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.RentalRequest.FindAsync(id);
            if (request == null)
                return NotFound();

            request.IsApproved = true;
            _context.Update(request);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: RentalRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalRequest = await _context.RentalRequest.FindAsync(id);
            _context.RentalRequest.Remove(rentalRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalRequestExists(int id)
        {
            return _context.RentalRequest.Any(e => e.Id == id);
        }
    }
}
