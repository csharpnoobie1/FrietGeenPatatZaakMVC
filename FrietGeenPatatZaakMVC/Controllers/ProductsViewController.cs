using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrietGeenPatatZaakMVC.Models;
using System.Threading.Tasks;
using ClassLibrary1.Models;
using FrietGeenPatatZaakMVC.View_Model;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FrietGeenPatatZaakMVC.Controllers.API;

namespace FrietGeenPatatZaakMVC.Controllers
{
    public class ProductsViewController : Controller
    {

        private readonly ProductsAPIController _productsApiController;

        public ProductsViewController(ProductsAPIController productsApiController)
        {
            _productsApiController = productsApiController;
        }

        public async Task<IActionResult> Index()
        {
            // Roep de API aan om producten op te halen
            var result = await _productsApiController.GetProducts();

            // Controleer of de response een geldig resultaat bevat
            if (result.Result is OkObjectResult okResult)
            {
                // Probeer de data om te zetten naar een lijst van Product
                var productsData = okResult.Value as List<Product>;
                if (productsData != null)
                {
                    // Zet de Product objecten om naar ProductViewModel met een object initializer
                    List<ProductViewModel> products = productsData.Select(item => new ProductViewModel(item.ProductId, item.Name, item.Price, item.Status, item.CategoryId, item.Category, item.OrderDetails)).ToList();

                    // Geef de producten door aan de view
                    return View(products);
                }
            }

            // Als er geen geldige producten zijn, geef een foutpagina terug
            return View("Error");
        }




        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Roep de API aan om de productdetails op te halen
            var productResponse = await _productsApiController.GetProductById(id.Value);

            // Controleer of de API-aanroep succesvol was
            if (productResponse is OkObjectResult okResult)
            {
                // Haal het Product-object uit het OkObjectResult
                var product = okResult.Value as Product;

                // Controleer of het product bestaat
                if (product != null)
                {
                    // Maak een ProductViewModel van het opgehaalde Product-object
                    var viewModel = new ProductViewModel(product);

                    // Geef de view terug met het ProductViewModel
                    return View(viewModel);
                }
            }

            // Als het product niet gevonden is, geef een foutpagina terug
            return View("Error");
        }


        public async Task<IActionResult> Create()
        {
            var categoriesResponse = await _productsApiController.GetCategories();
            if (categoriesResponse is OkObjectResult okResult)
            {
                var categories = okResult.Value as List<Category>;
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,Status,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Stuur de gegevens naar de API om het product op te slaan
                var result = await _productsApiController.CreateProduct(product);

                // Controleer of het resultaat een succesvolle 201 Created response is
                if (result is CreatedAtActionResult || result is ObjectResult objectResult && objectResult.StatusCode == 201)
                {
                    // Als het product succesvol is aangemaakt, ga terug naar de Index
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Als er een fout is, geef een foutpagina weer
                    return View("Error");
                }
            }

            // Als het model ongeldig is, haal de categorieën opnieuw op en toon het formulier opnieuw
            var categoriesResponse = await _productsApiController.GetCategories();
            if (categoriesResponse is OkObjectResult okResult)
            {
                var categories = okResult.Value as List<Category>;
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name");
            }

            return View(product); // Geef het formulier opnieuw weer met de foutmeldingen
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,Status,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Roep de API aan om het product bij te werken
                    var result = await _productsApiController.PutProduct(id, product);

                    if (result is NoContentResult)
                    {
                        // Als de update geslaagd is, ga terug naar de Index
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Als de update niet geslaagd is, geef een foutpagina weer
                        return View("Error");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Controleer of het product nog steeds bestaat via de API
                    var exists = await _productsApiController.ProductExists(id);
                    if (exists is OkObjectResult okResult && (bool)okResult.Value)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Als het model niet geldig is, geef het formulier opnieuw weer met de categorieën
            var categoriesResponse = await _productsApiController.GetCategories();
            if (categoriesResponse is OkObjectResult okCategoriesResult)
            {
                var categories = okCategoriesResult.Value as List<Category>;
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name", product.CategoryId);
            }

            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var productResponse = await _productsApiController.GetProductById(id.Value);
            if (productResponse is OkObjectResult okProductResult)
            {
                var product = okProductResult.Value as Product;
                if (product == null) return NotFound();

                var categoriesResponse = await _productsApiController.GetCategories();
                if (categoriesResponse is OkObjectResult okCategoriesResult)
                {
                    var categories = okCategoriesResult.Value as List<Category>;
                    ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name", product.CategoryId);
                }

                return View(product);
            }

            return NotFound();
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var productResponse = await _productsApiController.GetProductById(id.Value);
            if (productResponse is OkObjectResult okProductResult)
            {
                var product = okProductResult.Value as Product;
                if (product == null) return NotFound();

                return View(product); // De view die bevestigt of je het product wilt verwijderen
            }

            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productsApiController.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task<bool> ProductExists(int id)
        {
            var existsResponse = await _productsApiController.ProductExists(id); // Zorg ervoor dat je await gebruikt

            if (existsResponse is OkObjectResult okResult)
            {
                return (bool)okResult.Value;
            }

            return false;
        }


    }

}
