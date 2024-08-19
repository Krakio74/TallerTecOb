using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class DetailsListPage : ContentPage
{
    private readonly MovieService _movieService;

    public DetailsListPage(string type,string category)
    {
        InitializeComponent();

        _movieService = new MovieService();

        if (type == "movie")
        {
            LoadMoviesList(type, category);
        }
        else 
        {
            LoadSeriesList(type, category);
        }


    }

    public async void LoadMoviesList(string type,string category)
    {
        try
        {
            var responseContent = await _movieService.GetList(type,category);
            var movieResponse = JsonConvert.DeserializeObject<TmdbMoviesResponse>(responseContent);
            MoviesCollectionView.ItemsSource = movieResponse.Results;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    public async void LoadSeriesList(string type, string category)
    {
        try
        {
            var responseContent = await _movieService.GetList(type, category);
            var serieResponse = JsonConvert.DeserializeObject<TmdbSeriesResponse>(responseContent);
            MoviesCollectionView.ItemsSource = serieResponse.Results;
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
}
