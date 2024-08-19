using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class MovieDetailsPage : ContentPage
{
    private readonly MovieService _movieService;
    private List<Movie> favorites;
    private readonly string favoritesFilePath = Path.Combine(FileSystem.AppDataDirectory, "favorites.json");

    public MovieDetailsPage(long id)
    {
        InitializeComponent();
        _movieService = new MovieService();

        LoadFavorites();

        LoadMovieDetails(id);
        LoadMovieProviders(id);
        LoadActors(id);
        LoadDirectors(id);
        LoadSimilarMovies(id);
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
    }

    private void SaveFavorites()
    {
        var favoritosString = JsonConvert.SerializeObject(favorites);
        File.WriteAllText(favoritesFilePath, favoritosString);
    }

    private void ToggleFavorite(object sender, EventArgs e)
    {
        var movie = (Movie)BindingContext;

        var favoriteExist = favorites.FirstOrDefault(m => m.Id == movie.Id);
        if (favoriteExist != null)
        {
            favorites.Remove(favoriteExist);
            ((Button)sender).ImageSource = "favorite.png";
        }
        else
        {
            favorites.Add(movie);
            ((Button)sender).ImageSource = "favorite_filled.png";
        }
        UpdateButtonState();
        SaveFavorites();
    }

    private void UpdateButtonState()
    {
        var movie = (Movie)BindingContext;
        if (favorites.Any(m => m.Id == movie.Id))
        {
            favoriteButton.ImageSource = "favorite_filled.png";
        }
        else
        {
            favoriteButton.ImageSource = "favorite.png";
        }
    }

    private async void LoadMovieDetails(long id)
    {
        try
        {
            Movie pelicula = await _movieService.GetMovieDetails(id);
            BindingContext = pelicula;
            UpdateButtonState(); 
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void LoadMovieProviders(long id)
    {
        try
        {
            var providers = await _movieService.GetProviders(id, "movie");
            ProvidersCollectionView.ItemsSource = providers;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void LoadSimilarMovies(long id)
    {
        try
        {
            var similarMovies = await _movieService.GetSimilarMovies(id);
            SimilarMoviesCollectionView.ItemsSource = similarMovies;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void LoadDirectors(long id)
    {
        try
        {
            var directores = await _movieService.GetDirectors(id, "movie");
            DirectorsCollectionView.ItemsSource = directores;
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
            var actores = await _movieService.GetActors(id, "movie");
            ActorsCollectionView.ItemsSource = actores;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnVerTrailerClicked(object sender, EventArgs e)
    {
        try
        {
            var pelicula = (Movie)BindingContext;
            var trailer = await _movieService.GetTrailer(pelicula.Id,"movie");

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