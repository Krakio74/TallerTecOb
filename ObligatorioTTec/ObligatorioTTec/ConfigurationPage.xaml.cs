namespace ObligatorioTTec;

public partial class ConfigurationPage : ContentPage
{
    private List<LanguageItem> _languages;
    private List<ThemeItem> _themes;

    public ConfigurationPage()
    {
        InitializeComponent();
        LoadLanguages();
        LoadThemes();
    }

    private void LoadLanguages()
    {
        _languages = new List<LanguageItem>
            {
                new LanguageItem { LanguageCode = "en-US", FlagImage = "en_us.png", LanguageName = "English" },
                new LanguageItem { LanguageCode = "es-ES", FlagImage = "es_es.png", LanguageName = "Español" },
                new LanguageItem { LanguageCode = "fr-FR", FlagImage = "fr_fr.png", LanguageName = "Français" },
                new LanguageItem { LanguageCode = "de-DE", FlagImage = "de_de.png", LanguageName = "Deutsch" },
                new LanguageItem { LanguageCode = "it-IT", FlagImage = "it_it.png", LanguageName = "Italiano" },
                new LanguageItem { LanguageCode = "pt-PT", FlagImage = "pt_pt.png", LanguageName = "Português" },
                new LanguageItem { LanguageCode = "zh-CN", FlagImage = "zh_cn.png", LanguageName = "简体中文" },
                new LanguageItem { LanguageCode = "ja-JP", FlagImage = "ja_jp.png", LanguageName = "日本語" },
                new LanguageItem { LanguageCode = "ko-KR", FlagImage = "ko_kr.png", LanguageName = "한국어" },
                new LanguageItem { LanguageCode = "ru-RU", FlagImage = "ru_ru.png", LanguageName = "Русский" }
            };

        LanguageListView.ItemsSource = _languages;

        var selectedLanguageCode = Preferences.Get("SelectedLanguage", null);
        if (!string.IsNullOrEmpty(selectedLanguageCode))
        {
            var selectedLanguage = _languages.FirstOrDefault(l => l.LanguageCode == selectedLanguageCode);
            if (selectedLanguage != null)
            {
                LanguageListView.SelectedItem = selectedLanguage;
            }
        }
    }

    private void LoadThemes()
    {
        _themes = new List<ThemeItem>
            {
                new ThemeItem { ThemeName = "Light", ThemeImage = "dotnet_bot.png" },
                new ThemeItem { ThemeName = "Dark", ThemeImage = "dotnet_bot.png" },
                new ThemeItem { ThemeName = "Blue", ThemeImage = "dotnet_bot.png" }
            };

        ThemeCollectionView.ItemsSource = _themes;

        var selectedTheme = Preferences.Get("SelectedTheme", "Dark");
        var selectedThemeItem = _themes.FirstOrDefault(t => t.ThemeName == selectedTheme);
        if (selectedThemeItem != null)
        {
            ThemeCollectionView.SelectedItem = selectedThemeItem;
        }
    }
    private void OnThemeTapped(object sender, EventArgs e)
    {
        var image = sender as Image;
        var selectedTheme = image?.BindingContext as ThemeItem;
        if (selectedTheme != null)
        {
            Preferences.Set("SelectedTheme", selectedTheme.ThemeName);
            ApplyTheme(selectedTheme.ThemeName);
            ThemeCollectionView.SelectedItem = selectedTheme;
        }
    }
    private void OnLanguageSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is LanguageItem selectedLanguage)
        {
            Preferences.Set("SelectedLanguage", selectedLanguage.LanguageCode);
        }
    }

    private void OnThemeSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ThemeItem selectedTheme)
        {
            Preferences.Set("SelectedTheme", selectedTheme.ThemeName);
            ApplyTheme(selectedTheme.ThemeName);
        }
    }

    private void ApplyTheme(string themeName)
    {
        switch (themeName)
        {
            case "Light":
                App.Current.UserAppTheme = AppTheme.Light;
                break;
            case "Dark":
                App.Current.UserAppTheme = AppTheme.Dark;
                break;
            case "Blue":

                break;
            default:
                App.Current.UserAppTheme = AppTheme.Dark;
                break;
        }


    }

}

public class LanguageItem
{
    public string LanguageCode { get; set; }
    public string FlagImage { get; set; }
    public string LanguageName { get; set; }
}

public class ThemeItem
{
    public string ThemeName { get; set; }
    public string ThemeImage { get; set; }
}

