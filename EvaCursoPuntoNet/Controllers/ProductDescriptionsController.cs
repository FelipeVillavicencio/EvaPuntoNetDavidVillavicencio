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
    public class ProductDescriptionsController : Controller
    {
        private readonly AdventureWorksLt2019Context _context;

        public ProductDescriptionsController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // GET: ProductDescriptions
        public async Task<IActionResult> Index()
        {
              return _context.ProductDescriptions != null ? 
                          View(await _context.ProductDescriptions.ToListAsync()) :
                          Problem("Entity set 'AdventureWorksLt2019Context.ProductDescriptions'  is null.");
        }

        // GET: ProductDescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductDescriptions == null)
            {
                return NotFound();
            }

            var productDescription = await _context.ProductDescriptions
                .FirstOrDefaultAsync(m => m.ProductDescriptionId == id);
            if (productDescription == null)
            {
                return NotFound();
            }

            return View(productDescription);
        }

        // GET: ProductDescriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductDescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductDescriptionId,Description,Rowguid,ModifiedDate")] ProductDescription productDescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productDescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productDescription);
        }

        // GET: ProductDescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductDescriptions == null)
            {
                return NotFound();
            }

            var productDescription = await _context.ProductDescriptions.FindAsync(id);
            if (productDescription == null)
            {
                return NotFound();
            }
            return View(productDescription);
        }

        // POST: ProductDescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductDescriptionId,Description,Rowguid,ModifiedDate")] ProductDescription productDescription)
        {
            if (id != productDescription.ProductDescriptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productDescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductDescriptionExists(productDescription.ProductDescriptionId))
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
            return View(productDescription);
        }

        // GET: ProductDescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductDescriptions == null)
            {
                return NotFound();
            }

            var productDescription = await _context.ProductDescriptions
                .FirstOrDefaultAsync(m => m.ProductDescriptionId == id);
            if (productDescription == null)
            {
                return NotFound();
            }

            return View(productDescription);
        }

        // POST: ProductDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductDescriptions == null)
            {
                return Problem("Entity set 'AdventureWorksLt2019Context.ProductDescriptions'  is null.");
            }
            var productDescription = await _context.ProductDescriptions.FindAsync(id);
            if (productDescription != null)
            {
                _context.ProductDescriptions.Remove(productDescription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductDescriptionExists(int id)
        {
          return (_context.ProductDescriptions?.Any(e => e.ProductDescriptionId == id)).GetValueOrDefault();
        }
    }
}
