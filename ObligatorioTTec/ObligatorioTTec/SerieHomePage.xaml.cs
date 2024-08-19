using Newtonsoft.Json;
using ObligatorioTTec.Models;
using ObligatorioTTec.Views;
namespace ObligatorioTTec;

public partial class SerieHomePage : ContentPage
{
    private readonly MovieService _movieService;
    public SerieHomePage()
	{
        InitializeComponent();
        _movieService = new MovieService();
        LoadSeries();
    }

    private async void LoadSeries()
    {
        try
        {
            // Action & Adventure (Acción y Aventura)
            var actionAdventureResponseContent = await _movieService.GetListByGenre(10759, "tv");
            var actionAdventureSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(actionAdventureResponseContent);
            ActionAdventureSeriesCollectionView.ItemsSource = actionAdventureSeries.Results;

            // Animation (Animación)
            var animationResponseContent = await _movieService.GetListByGenre(16, "tv");
            var animationSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(animationResponseContent);
            AnimationSeriesCollectionView.ItemsSource = animationSeries.Results;

            // Comedy (Comedia)
            var comedyResponseContent = await _movieService.GetListByGenre(35, "tv");
            var comedySeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(comedyResponseContent);
            ComedySeriesCollectionView.ItemsSource = comedySeries.Results;

            // Crime (Crimen)
            var crimeResponseContent = await _movieService.GetListByGenre(80, "tv");
            var crimeSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(crimeResponseContent);
            CrimeSeriesCollectionView.ItemsSource = crimeSeries.Results;

            // Documentary (Documental)
            var documentaryResponseContent = await _movieService.GetListByGenre(99, "tv");
            var documentarySeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(documentaryResponseContent);
            DocumentarySeriesCollectionView.ItemsSource = documentarySeries.Results;

            // Drama
            var dramaResponseContent = await _movieService.GetListByGenre(18, "tv");
            var dramaSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(dramaResponseContent);
            DramaSeriesCollectionView.ItemsSource = dramaSeries.Results;

            // Family (Familia)
            var familyResponseContent = await _movieService.GetListByGenre(10751, "tv");
            var familySeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(familyResponseContent);
            FamilySeriesCollectionView.ItemsSource = familySeries.Results;

            // Kids (Infantil)
            var kidsResponseContent = await _movieService.GetListByGenre(10762, "tv");
            var kidsSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(kidsResponseContent);
            KidsSeriesCollectionView.ItemsSource = kidsSeries.Results;

            // Mystery (Misterio)
            var mysteryResponseContent = await _movieService.GetListByGenre(9648, "tv");
            var mysterySeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(mysteryResponseContent);
            MysterySeriesCollectionView.ItemsSource = mysterySeries.Results;

            // News (Noticias)
            var newsResponseContent = await _movieService.GetListByGenre(10763, "tv");
            var newsSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(newsResponseContent);
            NewsSeriesCollectionView.ItemsSource = newsSeries.Results;

            //// Reality (Reality)
            var realityResponseContent = await _movieService.GetListByGenre(10764, "tv");
            var realitySeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(realityResponseContent);
            RealitySeriesCollectionView.ItemsSource = realitySeries.Results;

            // Sci-Fi & Fantasy (Ciencia Ficción y Fantasía)
            var scifiResponseContent = await _movieService.GetListByGenre(10765, "tv");
            var scifiSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(scifiResponseContent);
            SciFiFantasySeriesCollectionView.ItemsSource = scifiSeries.Results;

            // Soap (Telenovela)
            var soapResponseContent = await _movieService.GetListByGenre(10766, "tv");
            var soapSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(soapResponseContent);
            SoapSeriesCollectionView.ItemsSource = soapSeries.Results;

            // Talk (Talk Show)
            var talkResponseContent = await _movieService.GetListByGenre(10767, "tv");
            var talkSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(talkResponseContent);
            TalkSeriesCollectionView.ItemsSource = talkSeries.Results;

            // War & Politics (Guerra y Política)
            var warPoliticsResponseContent = await _movieService.GetListByGenre(10768, "tv");
            var warPoliticsSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(warPoliticsResponseContent);
            WarPoliticsSeriesCollectionView.ItemsSource = warPoliticsSeries.Results;

            // Western
            var westernResponseContent = await _movieService.GetListByGenre(37, "tv");
            var westernSeries = JsonConvert.DeserializeObject<TmdbSeriesResponse>(westernResponseContent);
            WesternSeriesCollectionView.ItemsSource = westernSeries.Results;


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
                await Navigation.PushAsync(new SerieByGenrePage(categoryNumber));
            }
        }
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
