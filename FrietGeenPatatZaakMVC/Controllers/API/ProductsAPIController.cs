using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary1.Models;
using FrietGeenPatatZaakMVC.Models;
using FrietGeenPatatZaakMVC.Interfaces;

namespace FrietGeenPatatZaakMVC.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsAPIController : ControllerBase
    {
        private readonly FrietGeenPatatZaakContext _context;

        public ProductsAPIController(FrietGeenPatatZaakContext context)
        {
            _context = context;
        }

        // GET: api/NewProducts/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<Product>> GetDetails(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null) return NotFound();

            return product;
        }


        // PUT: api/NewProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            // Markeer het product als gewijzigd
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); // Probeer de wijzigingen op te slaan
            }
            catch (DbUpdateConcurrencyException)
            {
                // Gebruik een asynchrone versie van ProductExists
                if (!await ProductExistsAsync(id))
                {
                    return NotFound(); // Product bestaat niet
                }
                else
                {
                    throw; // Gooi de uitzondering opnieuw als het een andere fout is
                }
            }

            return NoContent(); // Succes, geen inhoud teruggeven
        }

        private async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.ProductId == id);
        }

        // POST: api/NewProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Retourneer een 201 Created response en de locatie van het nieuwe product
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        // DELETE: api/NewProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("exists/{id}")]
        public async Task<IActionResult> ProductExists(int id)
        {
            var exists = await Task.FromResult(_context.Products.Any(e => e.ProductId == id)); // Zorg ervoor dat het async is
            return Ok(exists);
        }


        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }


        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("products")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return Ok(products);
        }







    }
}
