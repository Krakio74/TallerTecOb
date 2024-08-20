
/* Unmerged change from project 'ObligatorioTTec (net8.0-windows10.0.19041.0)'
Before:
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
After:
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
*/

/* Unmerged change from project 'ObligatorioTTec (net8.0-android)'
Before:
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
After:
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
*/

/* Unmerged change from project 'ObligatorioTTec (net8.0-maccatalyst)'
Before:
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
After:
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
*/
using Newtonsoft.Json;

namespace ObligatorioTTec.Models
{

    public partial class Serie
    {
        [JsonProperty("adult")]
        public bool? Adult { get; set; }

        [JsonProperty("backdrop_path")]
        public string? BackdropPath { get; set; }

        [JsonProperty("created_by")]
        public CreatedBy[]? CreatedBy { get; set; }

        [JsonProperty("episode_run_time")]
        public int[]? EpisodeRunTime { get; set; }

        [JsonProperty("first_air_date")]
        public DateTimeOffset? FirstAirDate { get; set; }

        [JsonProperty("genres")]
        public Genre[]? Genres { get; set; }

        [JsonProperty("homepage")]
        public Uri? Homepage { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("in_production")]
        public bool? InProduction { get; set; }

        [JsonProperty("languages")]
        public string[]? Languages { get; set; }

        [JsonProperty("last_air_date")]
        public DateTimeOffset? LastAirDate { get; set; }

        [JsonProperty("last_episode_to_air")]
        public LastEpisodeToAir? LastEpisodeToAir { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("next_episode_to_air")]
        public object? NextEpisodeToAir { get; set; }

        [JsonProperty("networks")]
        public Network[]? Networks { get; set; }

        [JsonProperty("number_of_episodes")]
        public long? NumberOfEpisodes { get; set; }

        [JsonProperty("number_of_seasons")]
        public long? NumberOfSeasons { get; set; }

        [JsonProperty("origin_country")]
        public string[]? OriginCountry { get; set; }

        [JsonProperty("original_language")]
        public string? OriginalLanguage { get; set; }

        [JsonProperty("original_name")]
        public string? OriginalName { get; set; }

        [JsonProperty("overview")]
        public string? Overview { get; set; }

        [JsonProperty("popularity")]
        public double? Popularity { get; set; }

        [JsonProperty("poster_path")]
        public string? PosterPath { get; set; }

        [JsonProperty("production_companies")]
        public Network[]? ProductionCompanies { get; set; }

        [JsonProperty("production_countries")]
        public ProductionCountry[]? ProductionCountries { get; set; }

        [JsonProperty("seasons")]
        public Season[]? Seasons { get; set; }

        [JsonProperty("spoken_languages")]
        public SpokenLanguage[]? SpokenLanguages { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("tagline")]
        public string? Tagline { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("vote_average")]
        public double? VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public long? VoteCount { get; set; }

        public string? FullPosterPath => PosterPath != null ? $"https://image.tmdb.org/t/p/w500{PosterPath}" : null;
        public string? FullBackdropPath => BackdropPath != null ? $"https://image.tmdb.org/t/p/w500{BackdropPath}" : null;
    }

    public partial class CreatedBy
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("credit_id")]
        public string CreditId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        [JsonProperty("gender")]
        public long Gender { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
    }

    public partial class LastEpisodeToAir
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("vote_average")]
        public long VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public long VoteCount { get; set; }

        [JsonProperty("air_date")]
        public DateTimeOffset? AirDate { get; set; }

        [JsonProperty("episode_number")]
        public long EpisodeNumber { get; set; }

        [JsonProperty("episode_type")]
        public string EpisodeType { get; set; }

        [JsonProperty("production_code")]
        public string ProductionCode { get; set; }

        [JsonProperty("runtime")]
        public long Runtime { get; set; }

        [JsonProperty("season_number")]
        public long SeasonNumber { get; set; }

        [JsonProperty("show_id")]
        public long ShowId { get; set; }

        [JsonProperty("still_path")]
        public string StillPath { get; set; }
    }

    public partial class Network
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

    public partial class Season
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("air_date")]
        public DateTimeOffset? AirDate { get; set; }

        [JsonProperty("episodes")]
        public Episode[]? Episodes { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("overview")]
        public string? Overview { get; set; }

        [JsonProperty("id")]
        public long WelcomeId { get; set; }

        [JsonProperty("poster_path")]
        public string? PosterPath { get; set; }

        [JsonProperty("season_number")]
        public long SeasonNumber { get; set; }

        [JsonProperty("vote_average")]
        public long? VoteAverage { get; set; }
        public string FullPosterPath => $"https://image.tmdb.org/t/p/w500{PosterPath}";
    }

    public partial class Episode
    {
        [JsonProperty("air_date")]
        public DateTimeOffset? AirDate { get; set; }

        [JsonProperty("crew")]
        public Crew[]? Crew { get; set; }

        [JsonProperty("episode_number")]
        public long EpisodeNumber { get; set; }

        [JsonProperty("guest_stars")]
        public Crew[]? GuestStars { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("overview")]
        public string? Overview { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("production_code")]
        public string? ProductionCode { get; set; }

        [JsonProperty("runtime")]
        public long? Runtime { get; set; }

        [JsonProperty("season_number")]
        public long? SeasonNumber { get; set; }

        [JsonProperty("still_path")]
        public string? StillPath { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public long VoteCount { get; set; }


        public string FullPosterPath => $"https://image.tmdb.org/t/p/w500{StillPath}";
    }

    public partial class Crew
    {
        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public string? Department { get; set; }

        [JsonProperty("job", NullValueHandling = NullValueHandling.Ignore)]
        public string? Job { get; set; }

        [JsonProperty("credit_id")]
        public string? CreditId { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("gender")]
        public long Gender { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("known_for_department")]
        public string? KnownForDepartment { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("original_name")]
        public string? OriginalName { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("profile_path")]
        public string? ProfilePath { get; set; }

        [JsonProperty("character", NullValueHandling = NullValueHandling.Ignore)]
        public string? Character { get; set; }

        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public long? Order { get; set; }
    }
}

