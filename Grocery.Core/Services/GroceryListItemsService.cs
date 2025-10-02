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
            // Get all grocery list items from the repository
            var groceries = _groceriesRepository.GetAll();
            // dictionary to store productId and total amount sold
            var stats = new Dictionary<int, int>();

            foreach (var grocerie in groceries)
            {
                // if productId already in dictionary, add amount to total else add new entry with amount
                if (stats.ContainsKey(grocerie.ProductId))
                {
                    stats[grocerie.ProductId] += grocerie.Amount;
                }
                else
                {
                    stats.Add(grocerie.ProductId, grocerie.Amount);
                }
            }
            // order dictionary by total amount sold descending
            var statsOrder = stats.OrderByDescending(kv => kv.Value);
            // list to store and return best selling products
            List<BestSellingProducts> result = new List<BestSellingProducts>();
            // default loop is 5 it will be less if there are less than 5 products sold
            int loop = 5;
            // check if there are less than 5 products sold and adjust loop accordingly
            if (statsOrder.Count() < 5)
            {
                loop = statsOrder.Count();
            }
            // loop through the top 5 (or les) best selling products and get product details from product repository and add to result list
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
