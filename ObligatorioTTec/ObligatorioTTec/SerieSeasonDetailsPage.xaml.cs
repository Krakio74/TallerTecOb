using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using ObligatorioTTec.Models;
using System.Security.Cryptography;


namespace ObligatorioTTec;

public partial class SerieSeasonDetailsPage : ContentPage
{
    private readonly MovieService _movieService;
    private long _serie;
    private long _season;
    public SerieSeasonDetailsPage(long serie, long season)
    {
        InitializeComponent();
        _movieService = new MovieService();
        _serie = serie;
        _season = season;
        LoadSerieDetails();
        LoadSeasonDetails();
    }
    private async void LoadSerieDetails()
    {
        try
        {
            Serie serie = await _movieService.GetSerieDetails(_serie);
            if (BindingContext is SerieSeasonDetailsViewModel viewModel)
            {
                viewModel.Serie = serie;
            }
            else
            {
                BindingContext = new SerieSeasonDetailsViewModel { Serie = serie };
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private async void LoadSeasonDetails()
    {
        try
        {
            Season season = await _movieService.GetSeasonDetails(_serie, _season);
            if (BindingContext is SerieSeasonDetailsViewModel viewModel)
            {
                viewModel.Season = season;
            }
            else
            {
                BindingContext = new SerieSeasonDetailsViewModel { Season = season };
            }

            PosterImage.Source = season.FullPosterPath;
            SeasonNumberLabel.Text = $"Season {season.SeasonNumber}";
            SeasonOverviewLabel.Text = season.Overview;
            EpisodesCollectionView.ItemsSource = season.Episodes;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnEpisodeSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedEpisode = e.CurrentSelection[0] as Episode;
            if (selectedEpisode != null)
            {
                await Navigation.PushAsync(new SerieEpisodeDetailsPage(_serie, _season, selectedEpisode.EpisodeNumber));
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }

    private async void OnEpisodeTapped(object sender, EventArgs e)
    {
        var image = (Image)sender;
        var episode = (Episode)image.BindingContext;
        if (episode != null)
        {
            await Navigation.PushAsync(new SerieEpisodeDetailsPage(_serie, _season, episode.EpisodeNumber));
        }
    }
}

public class SerieSeasonDetailsViewModel
{
    public Serie Serie { get; set; }
    public Season Season { get; set; }
    public string FullName => $"{Serie?.Name} - {Season?.Name}";

}