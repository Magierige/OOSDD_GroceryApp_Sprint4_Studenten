
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            var groceryListItems = this.FetchGroceryListItems(productId ?? 0);
            List<BoughtProducts> results = this.ListResults(groceryListItems).ToList();
            return results;
        }
        private IReadOnlyList<GroceryListItem> FetchGroceryListItems(int productId)
        {
            var groceryListItems = _groceryListItemsRepository.GetAll();
            for (int i = 0; i < groceryListItems.Count; i++)
            {
                if (groceryListItems[i].ProductId != productId)
                {
                    groceryListItems.RemoveAt(i);
                }
            }
            return groceryListItems;
        }
        private IReadOnlyList<BoughtProducts> ListResults(IReadOnlyList<GroceryListItem> groceryListItems)
        {
            List<BoughtProducts> results = new List<BoughtProducts>();
            foreach (var item in groceryListItems)
            {
                GroceryList groceryList = _groceryListRepository.Get(item.GroceryListId);
                Client client = _clientRepository.Get(groceryList.ClientId);
                Product product = _productRepository.Get(item.ProductId);
                results.Add(new BoughtProducts(client, groceryList, product));
            }
            return results;
        }
    }
}
