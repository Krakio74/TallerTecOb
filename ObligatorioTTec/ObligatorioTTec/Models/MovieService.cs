using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ObligatorioTTec.Models
{
    public class MovieService
    {
        private readonly RestClient _client;
        private const string ApiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI1ZGExYzU3YzY1NGVkZGU0Yjk0MjdkYjJmZGY4NmIxOSIsIm5iZiI6MTcyMjA0MzE2Ni4wODM2OCwic3ViIjoiNjZhNDJiNWI2NDk2ODRhN2I2MzY0YmJmIiwic2NvcGVzIjpbImFwaV9yZWFkIl0sInZlcnNpb24iOjF9.EBgjkO267hfYJwgEhh9LHSAqkhcpw3voVUaQnH8kq5g";

        public MovieService()
        {
            var options = new RestClientOptions("https://api.themoviedb.org")
            {
                ThrowOnAnyError = true,
                Timeout = TimeSpan.FromSeconds(30)
            };
            _client = new RestClient(options);
        }
        private string GetLanguageParameter()
        {
            return Preferences.Get("SelectedLanguage", "en-US");
        }

        public async Task<Movie> GetMovieDetails(long id)
        {
            var request = new RestRequest($"/3/movie/{id}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Movie>(response.Content);
            }
            else
            {
                throw new Exception($"Error retrieving data: {response.ErrorMessage}");
            }
        }

        public async Task<List<Movie>> GetSimilarMovies(long id)
        {
            var request = new RestRequest($"/3/movie/{id}/similar", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var similarMoviesResponse = JsonConvert.DeserializeObject<TmdbMoviesResponse>(response.Content);
                return similarMoviesResponse.Results;
            }
            else
            {
                throw new Exception($"Error retrieving similar movies: {response.ErrorMessage}");
            }
        }

        public async Task<List<Actor>> GetActors(long id,string type)
        {
            var request = new RestRequest($"/3/{type}/{id}/credits", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var creditsResponse = JsonConvert.DeserializeObject<CreditsResponse>(response.Content);
                return creditsResponse.Cast
                    .Where(a => a.KnownForDepartment == "Acting")
                    .ToList();
            }
            else
            {
                throw new Exception($"Error retrieving data Actores: {response.ErrorMessage}");
            }
        }
        public async Task<List<CrewMember>> GetDirectors(long id,string type)
        {
            var request = new RestRequest($"/3/{type}/{id}/credits", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var creditsResponse = JsonConvert.DeserializeObject<CreditsResponse>(response.Content);
                if (creditsResponse != null)
                {
                    foreach (var crewMember in creditsResponse.Crew)
                    {
                        Console.WriteLine($"Name: {crewMember.Name}, Profile Path: {crewMember.ProfilePath}");
                    }

                    return creditsResponse.Crew
                        .Where(c => c.Job == "Director")
                        .ToList();
                }
                else
                {
                    throw new Exception("Error deserializando la respuesta de créditos.");
                }
            }
            else
            {
                throw new Exception($"Error retrieving data Directores: {response.ErrorMessage}");
            }
        }
        public async Task<string> Search(string query, string type)
        {
            var request = new RestRequest($"/3/search/{type}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("query", query);
            request.AddParameter("language", GetLanguageParameter());
            request.AddParameter("page", 1);

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new Exception($"Failed load search: {response.ErrorMessage}");
            }
        }
        public async Task<string> GetMoviesByActorId(long actorId,string type)
        {
            var request = new RestRequest($"/3/person/{actorId}/{type}_credits", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new Exception($"Failed load actor id: {response.ErrorMessage}");
            }
        }
        public async Task<string> SearchPeople(string query)
        {
            var request = new RestRequest("/3/search/person", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("query", query);
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new Exception($"Failed load people: {response.ErrorMessage}");
            }
        }

        public async Task<Serie> GetSerieDetails(long serie)
        {
            var request = new RestRequest($"/3/tv/{serie}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Serie>(response.Content);
            }
            else
            {
                throw new Exception($"Failed load series: {response.ErrorMessage}");
            }
        }
        public async Task<Season> GetSeasonDetails(long serie, long season)
        {
            var request = new RestRequest($"/3/tv/{serie}/season/{season}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Season>(response.Content);
            }
            else
            {
                throw new Exception($"Failed load seasons: {response.ErrorMessage}");
            }
        }
        public async Task<Episode> GetEpisodeDetails(long serie, long season,long episode)
        {
            var request = new RestRequest($"/3/tv/{serie}/season/{season}/episode/{episode}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Episode>(response.Content);
            }
            else
            {
                throw new Exception($"Failed load episodes: {response.ErrorMessage}");
            }
        }

        public async Task<string> GetListByGenre(int genreId,string type)
        {
            var request = new RestRequest($"/3/discover/{type}?with_genres={genreId}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new Exception($"Failed to load List by genre: {response.ErrorMessage}");
            }
        }

        public async Task<string> GetList(string type,string category)
        {
            var request = new RestRequest($"/3/{type}/{category}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new Exception($"Error retrieving data LISTAS: {response.ErrorMessage}");
            }
        }

        public async Task<MovieVideo> GetTrailer(long id,string type)
        {
            var request = new RestRequest($"/3/{type}/{id}/videos", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");
            request.AddParameter("language", GetLanguageParameter());

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var videoResponse = JsonConvert.DeserializeObject<MovieVideosResponse>(response.Content);
                var mainTrailer = videoResponse.Results
                    .Where(video => video.Type == "Trailer" && video.Site == "YouTube")
                    .FirstOrDefault();

                return mainTrailer;
            }
            else
            {
                throw new Exception($"Failed load trailer: {response.ErrorMessage}");
            }
        }

        public class MovieVideosResponse
        {
            [JsonProperty("results")]
            public List<MovieVideo> Results { get; set; }
        }

        public class MovieVideo
        {
            [JsonProperty("key")]
            public string Key { get; set; }
            [JsonProperty("site")]
            public string Site { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public async Task<List<WatchProvider>> GetProviders(long id,string type)
        {
            var request = new RestRequest($"/3/{type}/{id}/watch/providers", Method.Get);
            request.AddHeader("Authorization", $"Bearer {ApiKey}");

            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var providersResponse = JsonConvert.DeserializeObject<WatchProvidersResponse>(response.Content);
                if (providersResponse != null && providersResponse.Results.ContainsKey("US"))
                {
                    return providersResponse.Results["US"].Flatrate;
                }
                return new List<WatchProvider>();
            }
            else
            {
                throw new Exception($"Error retrieving watch providers: {response.ErrorMessage}");
            }
        }

        public class WatchProvider
        {
            [JsonProperty("provider_name")]
            public string ProviderName { get; set; }

            [JsonProperty("logo_path")]
            public string LogoPath { get; set; }
            public string FullLogoPath => $"https://image.tmdb.org/t/p/w500{LogoPath}";
        }

        public class WatchProvidersResponse
        {
            [JsonProperty("results")]
            public Dictionary<string, WatchProviderResult> Results { get; set; }
        }

        public class WatchProviderResult
        {
            [JsonProperty("flatrate")]
            public List<WatchProvider> Flatrate { get; set; }
        }

    }

    public partial class Genre
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class ProductionCompany
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("logo_path")]
        public string LogoPath { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("origin_country")]
        public string OriginCountry { get; set; }
    }

    public partial class ProductionCountry
    {
        [JsonProperty("iso_3166_1")]
        public string Iso3166_1 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class SpokenLanguage
    {
        [JsonProperty("english_name")]
        public string EnglishName { get; set; }

        [JsonProperty("iso_639_1")]
        public string Iso639_1 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Actor
    {
        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("gender")]
        public long Gender { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("known_for_department")]
        public string KnownForDepartment { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }

        [JsonProperty("cast_id")]
        public long CastId { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("credit_id")]
        public string CreditId { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }
        public string FullProfilePath => $"https://image.tmdb.org/t/p/w500{ProfilePath}";
    }

    public class CreditsResponse
    {
        [JsonProperty("cast")]
        public List<Actor> Cast { get; set; }

        [JsonProperty("crew")]
        public List<CrewMember> Crew { get; set; }
    }
    public class Director
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Department { get; set; }
    }

    public class CrewMember
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
        public string FullProfilePath => $"https://image.tmdb.org/t/p/w500{ProfilePath}";

    }
    public class TmdbMoviesResponse
    {
        public List<Movie> Results { get; set; }
    }

    public class TmdbSeriesResponse
    {
        public List<Serie> Results { get; set; }
    }
    public class TmdbPeopleResponse
    {
        [JsonProperty("results")]
        public List<Actor> Results { get; set; }
    }

    public class TmdbActorMoviesResponse
    {
        [JsonProperty("cast")]
        public List<Movie> Cast { get; set; }
    }
    public class TmdbActorSeriesResponse
    {
        [JsonProperty("cast")]
        public List<Serie> Cast { get; set; }
    }
}

