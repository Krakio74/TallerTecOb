using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class SerieDetailsPage : ContentPage
{
    private readonly MovieService _movieService;
    private long _serie;
    private List<Serie> favoritesSeries;
    private readonly string favoritesSeriesFilePath = Path.Combine(FileSystem.AppDataDirectory, "favoritesSeries.json");

    public SerieDetailsPage(long id)
    {
        InitializeComponent();
        _movieService = new MovieService();
        _serie = id;
        LoadFavorites();
        LoadSerieDetails(id);
        LoadProviders(id);
        LoadActors(id);
        LoadSeasons(id);
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
    }

    private void SaveFavorites()
    {
        var favoritesString = JsonConvert.SerializeObject(favoritesSeries);
        File.WriteAllText(favoritesSeriesFilePath, favoritesString);
    }

    private void ToggleFavorite(object sender, EventArgs e)
    {
        var serie = (Serie)BindingContext;

        var favoriteExist = favoritesSeries.FirstOrDefault(s => s.Id == serie.Id);
        if (favoriteExist != null)
        {
            favoritesSeries.Remove(favoriteExist);
            ((Button)sender).ImageSource = "favorite.png";
        }
        else
        {
            favoritesSeries.Add(serie);
            ((Button)sender).ImageSource = "favorite_filled.png";
        }
        UpdateButtonState();
        SaveFavorites();
    }

    private void UpdateButtonState()
    {
        var serie = (Serie)BindingContext;
        if (favoritesSeries.Any(m => m.Id == serie.Id))
        {
            favoriteButton.ImageSource = "favorite_filled.png";
        }
        else
        {
            favoriteButton.ImageSource = "favorite.png";
        }
    }
    private async void LoadSeasons(long id)
    {
        try
        {
            Serie serie = await _movieService.GetSerieDetails(id);
            SeasonsCollectionView.ItemsSource = serie.Seasons;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Failed to load seasons :", ex.Message, "OK");
        }
    }

    private async void LoadProviders(long id)
    {
        try
        {
            var providers = await _movieService.GetProviders(id, "tv");
            ProvidersCollectionView.ItemsSource = providers;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void LoadSerieDetails(long id)
    {
        try
        {
            Serie serie = await _movieService.GetSerieDetails(id);
            BindingContext = serie;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void LoadActors(long id)
    {
        try
        {
            List<Actor> actores = await _movieService.GetActors(id, "tv");
            ActorsCollectionView.ItemsSource = actores;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Failed actors load :", ex.Message, "OK");
        }
    }

    private async void OnVerTrailerClicked(object sender, EventArgs e)
    {
        try
        {
            var serie = (Serie)BindingContext;
            var serieId = serie.Id;
            var trailer = await _movieService.GetTrailer(serieId, "tv");

            if (trailer != null)
            {
                await Navigation.PushAsync(new VideoPage(trailer.Key));
            }
            else
            {
                await DisplayAlert("Error", "No se encontró el tráiler.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private async void OnSeasonSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedSeason = e.CurrentSelection[0] as Season;
            if (selectedSeason != null)
            {
                await Navigation.PushAsync(new SerieSeasonDetailsPage(_serie, selectedSeason.SeasonNumber));
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }

    private async void OnSeasonTapped(object sender, EventArgs e)
    {
        var image = (Image)sender;
        var season = (Season)image.BindingContext;
        if (season != null)
        {
            await Navigation.PushAsync(new SerieSeasonDetailsPage(_serie, season.SeasonNumber));
        }
    }
}