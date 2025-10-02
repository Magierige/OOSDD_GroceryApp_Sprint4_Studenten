using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        public Client CurrentClient { get; set; }
        private readonly IGroceryListService _groceryListService;
        private readonly IClientService _clientService;
        private readonly GlobalViewModel _global;
        public ICommand ShowBoughtProductsClick { get; }

        public GroceryListViewModel(IGroceryListService groceryListService, IClientService clientService, GlobalViewModel global) 
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            GroceryLists = new(_groceryListService.GetAll());
            _clientService = clientService;
            _global = global;
            CurrentClient = _global.Client;
            ShowBoughtProductsClick = new Command(ShowBoughtProducts);
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, paramater);
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            GroceryLists = new(_groceryListService.GetAll());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }

        public void ShowBoughtProducts()
        {
            if (CurrentClient.clientRole == Client.Role.Admin)
            {
                Shell.Current.GoToAsync(nameof(Views.BoughtProductsView));
            }
        }
    }
}
