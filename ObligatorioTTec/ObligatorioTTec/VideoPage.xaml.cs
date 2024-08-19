using Newtonsoft.Json;
using ObligatorioTTec.Models;
namespace ObligatorioTTec;

public partial class VideoPage : ContentPage
{
    private readonly MovieService _movieService;
    public VideoPage(string videoKey)
    {
        InitializeComponent();
        TrailerWebView.Source = $"https://www.youtube.com/embed/{videoKey}";
    }
}