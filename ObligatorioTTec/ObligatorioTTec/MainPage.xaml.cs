using ObligatorioTTec.Views;
namespace ObligatorioTTec
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            //var asd = App.CineDB.GetUsuarios().Result.Count;
            
        }

        private async void GoToLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }
        private void testing(object sender, EventArgs e)
        {
            DisplayAlert("AA", "AA", "AA");
            string Key = "-qIYsCOsyqw";
            var html = "<!DOCTYPE html>\r\n <html>\r\n <head>\r\n     <meta name='viewport' content='width=device-width, initial-scale=1.0'>\r\n </head>\r\n <body style='margin:0; padding:0;'>\r\n     <iframe width='100%' height='100%' src='https://www.youtube.com/embed/{key}' frameborder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe>\r\n </body>\r\n </html>\";";
            webView.Source = new HtmlWebViewSource
            {
                Html = html,
            };
        }
    }

}
