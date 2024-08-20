using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class SerieEpisodeDetailsPage : ContentPage
{
    private readonly MovieService _movieService;
    private long _serie;
    private long _season;
    private long _episode;
    public SerieEpisodeDetailsPage(long serie, long season, long episode)
    {
        InitializeComponent();
        _movieService = new MovieService();
        _serie = serie;
        _season = season;
        _episode = episode;
        LoadEpisodeDetails();
    }

    private async void LoadEpisodeDetails()
    {
        try
        {
            Episode ep = await _movieService.GetEpisodeDetails(_serie, _season, _episode);
            BindingContext = ep;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

}

