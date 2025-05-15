using albionSCRAPERV2.ViewModels;
using albionSCRAPERV2.ViewModels.Converters;
using albionSCRAPERV2.Views;

namespace albionSCRAPERV2;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        var vm = new MainViewModel();
        vm.OnOpenSearchingItemsView = () => OpenSearchingItemsView();

        BindingContext = vm;
    }

    private async void OpenSearchingItemsView()
    {
        var viewModel = new SearchingItemsViewModel();
        var page = new SearchingItemsView
        {
            BindingContext = viewModel
        };
        await Navigation.PushAsync(page);
    }

    private void dir()
    {
        string path = "/Users/adrianstanisz/RiderProjects/albionSCRAPER/albionSCRAPER/Data/items.json";

        if (File.Exists(path))
        {
            Console.WriteLine("plik istnieje");
        }
    }
}