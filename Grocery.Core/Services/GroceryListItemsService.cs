using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            var groceries = _groceriesRepository.GetAll();
            var stats = new Dictionary<int, int>();
            foreach (var grocerie in groceries)
            {
                if (stats.ContainsKey(grocerie.ProductId))
                {
                    stats[grocerie.ProductId] += grocerie.Amount;
                }
                else
                {
                    stats.Add(grocerie.ProductId, grocerie.Amount);
                }
            }
            var statsOrder = stats.OrderByDescending(kv => kv.Value);
            List<BestSellingProducts> result = new List<BestSellingProducts>();
            int loop = 5;
            if (statsOrder.Count() < 5)
            {
                loop = statsOrder.Count();
            }
            for (int i = 0; i < loop; i++)
            {
                var item = statsOrder.ElementAt(i);
                var product = _productRepository.Get(item.Key);
                if (product != null)
                {
                    result.Add(new BestSellingProducts(product.Id, product.Name, product.Stock, item.Value, i+1));
                }
            }
            return result;
            //throw new NotImplementedException();
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
