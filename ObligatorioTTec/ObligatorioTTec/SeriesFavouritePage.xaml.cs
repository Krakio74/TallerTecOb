using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;
public partial class SeriesFavouritePage : ContentPage
{
    private readonly MovieService _movieService;
    private List<Serie> favoritesSeries;
    private readonly string favoritesSeriesFilePath = Path.Combine(FileSystem.AppDataDirectory, "favoritesSeries.json");

    public SeriesFavouritePage()
    {
        InitializeComponent();
        _movieService = new MovieService();
        LoadFavorites();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadFavorites();
    }
    private void LoadFavorites()
    {
        if (File.Exists(favoritesSeriesFilePath))
        {
            var favoritesSeriesString = File.ReadAllText(favoritesSeriesFilePath);
            favoritesSeries = JsonConvert.DeserializeObject<List<Serie>>(favoritesSeriesString);
        }
        else
        {
            favoritesSeries = new List<Serie>();
        }
        FavoriteSeriesCollectionView.ItemsSource = favoritesSeries;
    }

    private async void OnSerieSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedSerie = e.CurrentSelection[0] as Movie;
            if (selectedSerie != null)
            {
                await Navigation.PushAsync(new SerieDetailsPage(selectedSerie.Id));
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }

    private async void OnSerieTapped(object sender, EventArgs e)
    {
        var image = (Image)sender;
        var serie = (Serie)image.BindingContext;
        if (serie != null)
        {
            await Navigation.PushAsync(new SerieDetailsPage(serie.Id));
        }
    }
}