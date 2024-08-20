using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class SerieSearchPage : ContentPage
{
    private readonly MovieService _movieService;

    public SerieSearchPage()
    {
        InitializeComponent();
        _movieService = new MovieService();
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchText = SearchBar.Text?.Trim();

        try
        {
            var searchResults = new List<Serie>();
            var seriesResponseContent = await _movieService.Search(searchText, "tv");
            var serieResponse = JsonConvert.DeserializeObject<TmdbSeriesResponse>(seriesResponseContent);

            if (serieResponse.Results.Any())
            {
                searchResults.AddRange(serieResponse.Results);
            }

            var peopleResponseContent = await _movieService.SearchPeople(searchText);
            var peopleResponse = JsonConvert.DeserializeObject<TmdbPeopleResponse>(peopleResponseContent);

            if (peopleResponse.Results.Any())
            {
                var actorId = peopleResponse.Results.First().Id;
                var actorSeriesResponseContent = await _movieService.GetMoviesByActorId(actorId, "tv");
                var actorSerieResponse = JsonConvert.DeserializeObject<TmdbActorSeriesResponse>(actorSeriesResponseContent);
                searchResults.AddRange(actorSerieResponse.Cast);
            }

            if (searchResults.Any())
            {
                SeriesCollectionView.ItemsSource = searchResults;
            }
            else
            {
                await DisplayAlert("No Results", "No series found with the given name or actor.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private async void OnSerieSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedSerie = e.CurrentSelection[0] as Serie;
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
