using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;
public partial class MovieHomePage : ContentPage
{
    private readonly MovieService _movieService;
    public MovieHomePage()
    {
        InitializeComponent();
        _movieService = new MovieService();
        LoadMovies();
    }



    private async void LoadMovies()
    {
        try
        {
            // Acción
            var actionResponseContent = await _movieService.GetListByGenre(28, "movie");
            var actionMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(actionResponseContent);
            ActionMoviesCollectionView.ItemsSource = actionMovies.Results;

            // Aventura
            var adventureResponseContent = await _movieService.GetListByGenre(12, "movie");
            var adventureMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(adventureResponseContent);
            AdventureMoviesCollectionView.ItemsSource = adventureMovies.Results;

            // Animación
            var animationResponseContent = await _movieService.GetListByGenre(16, "movie");
            var animationMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(animationResponseContent);
            AnimationMoviesCollectionView.ItemsSource = animationMovies.Results;

            // Comedia
            var comedyResponseContent = await _movieService.GetListByGenre(35, "movie");
            var comedyMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(comedyResponseContent);
            ComedyMoviesCollectionView.ItemsSource = comedyMovies.Results;

            // Crimen
            var crimeResponseContent = await _movieService.GetListByGenre(80, "movie");
            var crimeMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(crimeResponseContent);
            CrimeMoviesCollectionView.ItemsSource = crimeMovies.Results;

            // Documental
            var documentaryResponseContent = await _movieService.GetListByGenre(99, "movie");
            var documentaryMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(documentaryResponseContent);
            DocumentaryMoviesCollectionView.ItemsSource = documentaryMovies.Results;

            // Drama
            var dramaResponseContent = await _movieService.GetListByGenre(18, "movie");
            var dramaMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(dramaResponseContent);
            DramaMoviesCollectionView.ItemsSource = dramaMovies.Results;

            // Familia
            var familyResponseContent = await _movieService.GetListByGenre(10751, "movie");
            var familyMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(familyResponseContent);
            FamilyMoviesCollectionView.ItemsSource = familyMovies.Results;

            // Fantasía
            var fantasyResponseContent = await _movieService.GetListByGenre(14, "movie");
            var fantasyMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(fantasyResponseContent);
            FantasyMoviesCollectionView.ItemsSource = fantasyMovies.Results;

            // Historia
            var historyResponseContent = await _movieService.GetListByGenre(36, "movie");
            var historyMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(historyResponseContent);
            HistoryMoviesCollectionView.ItemsSource = historyMovies.Results;

            // Horror
            var horrorResponseContent = await _movieService.GetListByGenre(27, "movie");
            var horrorMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(horrorResponseContent);
            HorrorMoviesCollectionView.ItemsSource = horrorMovies.Results;

            // Música
            var musicResponseContent = await _movieService.GetListByGenre(10402, "movie");
            var musicMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(musicResponseContent);
            MusicMoviesCollectionView.ItemsSource = musicMovies.Results;

            // Misterio
            var mysteryResponseContent = await _movieService.GetListByGenre(9648, "movie");
            var mysteryMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(mysteryResponseContent);
            MysteryMoviesCollectionView.ItemsSource = mysteryMovies.Results;

            // Romance
            var romanceResponseContent = await _movieService.GetListByGenre(10749, "movie");
            var romanceMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(romanceResponseContent);
            RomanceMoviesCollectionView.ItemsSource = romanceMovies.Results;

            // Ciencia ficción
            var scienceFictionResponseContent = await _movieService.GetListByGenre(878, "movie");
            var scienceFictionMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(scienceFictionResponseContent);
            ScienceFictionMoviesCollectionView.ItemsSource = scienceFictionMovies.Results;

            // Película de TV
            var tvMovieResponseContent = await _movieService.GetListByGenre(10770, "movie");
            var tvMovieMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(tvMovieResponseContent);
            TvMovieMoviesCollectionView.ItemsSource = tvMovieMovies.Results;

            // Suspenso
            var thrillerResponseContent = await _movieService.GetListByGenre(53, "movie");
            var thrillerMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(thrillerResponseContent);
            ThrillerMoviesCollectionView.ItemsSource = thrillerMovies.Results;

            // Bélica
            var warResponseContent = await _movieService.GetListByGenre(10752, "movie");
            var warMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(warResponseContent);
            WarMoviesCollectionView.ItemsSource = warMovies.Results;

            // Western
            var westernResponseContent = await _movieService.GetListByGenre(37, "movie");
            var westernMovies = JsonConvert.DeserializeObject<TmdbMoviesResponse>(westernResponseContent);
            WesternMoviesCollectionView.ItemsSource = westernMovies.Results;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnGenreTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame)
        {
            if (int.TryParse(frame.AutomationId, out int categoryNumber))
            {
                await Navigation.PushAsync(new MovieByGenrePage(categoryNumber));
            }
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


