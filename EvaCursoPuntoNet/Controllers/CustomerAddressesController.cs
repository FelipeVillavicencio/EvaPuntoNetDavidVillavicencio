using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvaCursoPuntoNet.Models;
using Microsoft.AspNetCore.Authorization;

namespace EvaCursoPuntoNet.Controllers
{
    [Authorize]
    public class CustomerAddressesController : Controller
    {
        private readonly AdventureWorksLt2019Context _context;

        public CustomerAddressesController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // GET: CustomerAddresses
        public async Task<IActionResult> Index()
        {
            var adventureWorksLt2019Context = _context.CustomerAddresses.Include(c => c.Address).Include(c => c.Customer);
            return View(await adventureWorksLt2019Context.ToListAsync());
        }

        // GET: CustomerAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerAddresses == null)
            {
                return NotFound();
            }

            var customerAddress = await _context.CustomerAddresses
                .Include(c => c.Address)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customerAddress == null)
            {
                return NotFound();
            }

            return View(customerAddress);
        }

        // GET: CustomerAddresses/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();
        }

        // POST: CustomerAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,AddressId,AddressType,Rowguid,ModifiedDate")] CustomerAddress customerAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", customerAddress.AddressId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", customerAddress.CustomerId);
            return View(customerAddress);
        }

        // GET: CustomerAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerAddresses == null)
            {
                return NotFound();
            }

            var customerAddress = await _context.CustomerAddresses.FindAsync(id);
            if (customerAddress == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", customerAddress.AddressId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", customerAddress.CustomerId);
            return View(customerAddress);
        }

        // POST: CustomerAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,AddressId,AddressType,Rowguid,ModifiedDate")] CustomerAddress customerAddress)
        {
            if (id != customerAddress.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerAddressExists(customerAddress.CustomerId))
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
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", customerAddress.AddressId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", customerAddress.CustomerId);
            return View(customerAddress);
        }

        // GET: CustomerAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerAddresses == null)
            {
                return NotFound();
            }

            var customerAddress = await _context.CustomerAddresses
                .Include(c => c.Address)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customerAddress == null)
            {
                return NotFound();
            }

            return View(customerAddress);
        }

        // POST: CustomerAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerAddresses == null)
            {
                return Problem("Entity set 'AdventureWorksLt2019Context.CustomerAddresses'  is null.");
            }
            var customerAddress = await _context.CustomerAddresses.FindAsync(id);
            if (customerAddress != null)
            {
                _context.CustomerAddresses.Remove(customerAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerAddressExists(int id)
        {
          return (_context.CustomerAddresses?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
