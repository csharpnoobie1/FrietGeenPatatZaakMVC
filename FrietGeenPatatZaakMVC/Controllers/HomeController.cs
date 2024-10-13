using FrietGeenPatatZaakMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ClassLibrary1.Models;
using FrietGeenPatatZaakMVC.Controllers.API;
using FrietGeenPatatZaakMVC.View_Model;


namespace FrietGeenPatatZaakMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductsAPIController _productsApiController;

        public HomeController(ProductsAPIController productsApiController)
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

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
