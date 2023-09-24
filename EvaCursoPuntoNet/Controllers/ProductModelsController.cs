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
    public class ProductModelsController : Controller
    {
        private readonly AdventureWorksLt2019Context _context;

        public ProductModelsController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // GET: ProductModels
        public async Task<IActionResult> Index()
        {
              return _context.ProductModels != null ? 
                          View(await _context.ProductModels.ToListAsync()) :
                          Problem("Entity set 'AdventureWorksLt2019Context.ProductModels'  is null.");
        }

        // GET: ProductModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductModels == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModels
                .FirstOrDefaultAsync(m => m.ProductModelId == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // GET: ProductModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductModelId,Name,CatalogDescription,Rowguid,ModifiedDate")] ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }

        // GET: ProductModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductModels == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModels.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            return View(productModel);
        }

        // POST: ProductModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductModelId,Name,CatalogDescription,Rowguid,ModifiedDate")] ProductModel productModel)
        {
            if (id != productModel.ProductModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.ProductModelId))
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
            return View(productModel);
        }

        // GET: ProductModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductModels == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModels
                .FirstOrDefaultAsync(m => m.ProductModelId == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: ProductModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductModels == null)
            {
                return Problem("Entity set 'AdventureWorksLt2019Context.ProductModels'  is null.");
            }
            var productModel = await _context.ProductModels.FindAsync(id);
            if (productModel != null)
            {
                _context.ProductModels.Remove(productModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductModelExists(int id)
        {
          return (_context.ProductModels?.Any(e => e.ProductModelId == id)).GetValueOrDefault();
        }
    }
}
