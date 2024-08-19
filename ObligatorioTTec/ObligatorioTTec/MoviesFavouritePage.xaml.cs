using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class MoviesFavouritePage : ContentPage
{
    private readonly MovieService _movieService;
    private List<Movie> favorites;
    private readonly string favoritesFilePath = Path.Combine(FileSystem.AppDataDirectory, "favorites.json");

    public MoviesFavouritePage()
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
        if (File.Exists(favoritesFilePath))
        {
            var favoritesString = File.ReadAllText(favoritesFilePath);
            favorites = JsonConvert.DeserializeObject<List<Movie>>(favoritesString);
        }
        else
        {
            favorites = new List<Movie>();
        }

        // Asignar la lista de favoritos al CollectionView
        FavoriteMoviesCollectionView.ItemsSource = favorites;
    }

    private async void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedMovie = e.CurrentSelection[0] as Movie;
            if (selectedMovie != null)
            {
                await Navigation.PushAsync(new MovieDetailsPage(selectedMovie.Id));
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }

    private async void OnMovieTapped(object sender, EventArgs e)
    {
        var image = (Image)sender;
        var movie = (Movie)image.BindingContext;
        if (movie != null)
        {
            await Navigation.PushAsync(new MovieDetailsPage(movie.Id));
        }
    }
}