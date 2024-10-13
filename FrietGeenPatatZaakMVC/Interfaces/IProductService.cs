using FrietGeenPatatZaakMVC.View_Model;

namespace FrietGeenPatatZaakMVC.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetProductsAsync();
    }
}
