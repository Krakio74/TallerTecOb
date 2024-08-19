using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class SerieByGenrePage : ContentPage
{
    private readonly MovieService _movieService;
    public string genre { get; set; }

    private readonly Dictionary<int, string> _genreDictionary = new Dictionary<int, string>
{
    { 10759, "Action & Adventure" },
    { 16, "Animation" },
    { 35, "Comedy" },
    { 80, "Crime" },
    { 99, "Documentary" },
    { 18, "Drama" },
    { 10751, "Family" },
    { 10762, "Kids" },
    { 9648, "Mystery" },
    { 10763, "News" },
    { 10764, "Reality" },
    { 10765, "Sci-Fi & Fantasy" },
    { 10766, "Soap" },
    { 10767, "Talk" },
    { 10768, "War & Politics" },
    { 37, "Western" }
};
    public SerieByGenrePage(int genreId)
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
        LoadSeriesByGenre(genreId);

    }
    private async void LoadSeriesByGenre(int genreId)
    {
        try
        {
            var responseContent = await _movieService.GetListByGenre(genreId, "tv");
            var serieResponse = JsonConvert.DeserializeObject<TmdbSeriesResponse>(responseContent);
            SeriesCollectionView.ItemsSource = serieResponse.Results;
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
