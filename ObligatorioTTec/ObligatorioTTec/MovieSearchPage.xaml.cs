using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;
public partial class MovieSearchPage : ContentPage
{
    private readonly MovieService _movieService;

    public MovieSearchPage()
    {
        InitializeComponent();
        _movieService = new MovieService();
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchText = SearchBar.Text?.Trim();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            await DisplayAlert("Error", "Search text cannot be empty.", "OK");
            return;
        }

        try
        {
            var searchResults = new List<Movie>();
            var moviesResponseContent = await _movieService.Search(searchText, "movie");
            var moviesResponse = JsonConvert.DeserializeObject<TmdbMoviesResponse>(moviesResponseContent);

            if (moviesResponse.Results.Any())
            {
                searchResults.AddRange(moviesResponse.Results);
            }

            var peopleResponseContent = await _movieService.SearchPeople(searchText);
            var peopleResponse = JsonConvert.DeserializeObject<TmdbPeopleResponse>(peopleResponseContent);

            if (peopleResponse.Results.Any())
            {
                var actorId = peopleResponse.Results.First().Id;
                var actorMoviesResponseContent = await _movieService.GetMoviesByActorId(actorId, "movie");
                var actorMoviesResponse = JsonConvert.DeserializeObject<TmdbActorMoviesResponse>(actorMoviesResponseContent);
                searchResults.AddRange(actorMoviesResponse.Cast);
            }

            if (searchResults.Any())
            {
                MoviesCollectionView.ItemsSource = searchResults;
            }
            else
            {
                await DisplayAlert("No Results", "No movies found with the given name or actor.", "OK");
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
