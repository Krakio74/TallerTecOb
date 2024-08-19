using ObligatorioTTec.Models;

namespace ObligatorioTTec;

public partial class UserPanel : ContentPage
{
    private readonly ApiService _apiService;
    public UserPanel()
	{
		InitializeComponent();
        _apiService = new ApiService();


    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var items = await _apiService.GetItemsAsync();
        if (items != null)
        {
            // Bind items to your UI, for example to a ListView
            ItemsListView.ItemsSource = items;
        }
        else
        {
            await DisplayAlert("Error", "Unable to retrieve data from the API.", "OK");
        }
    }
}