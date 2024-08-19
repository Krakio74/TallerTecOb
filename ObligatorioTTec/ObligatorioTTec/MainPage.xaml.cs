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

    }

}
