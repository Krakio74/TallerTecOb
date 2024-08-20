using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class MovieByGenrePage : ContentPage
{
    private readonly MovieService _movieService;
    public string genre { get; set; }

    private readonly Dictionary<int, string> _genreDictionary = new Dictionary<int, string>
    {
        { 28, "Action" },
        { 12, "Adventure" },
        { 16, "Animation" },
        { 35, "Comedy" },
        { 80, "Crime" },
        { 99, "Documentary" },
        { 18, "Drama" },
        { 10751, "Family" },
        { 14, "Fantasy" },
        { 36, "History" },
        { 27, "Horror" },
        { 10402, "Music" },
        { 9648, "Mystery" },
        { 10749, "Romance" },
        { 878, "Science Fiction" },
        { 10770, "TV Movie" },
        { 53, "Thriller" },
        { 10752, "War" },
        { 37, "Western" }
    };
    public MovieByGenrePage(int genreId)
    {
        InitializeComponent();

        if (_genreDictionary.TryGetValue(genreId, out string genreName))
        {
            Title = genreName;
        }
        else
        {
            Title = "Unknown Genre";
        }
        genre = genre;
        _movieService = new MovieService();
        LoadMoviesByGenre(genreId);

    }
    private async void LoadMoviesByGenre(int genreId)
    {
        try
        {
            var responseContent = await _movieService.GetListByGenre(genreId, "movie");
            var movieResponse = JsonConvert.DeserializeObject<TmdbMoviesResponse>(responseContent);
            MoviesCollectionView.ItemsSource = movieResponse.Results;
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
