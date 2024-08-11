namespace ObligatorioTTec.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}
    private async void Pos_Gps(object sender, EventArgs e)
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location != null)
        {
            await DisplayAlert("Loc", location.Latitude + " " + location.Longitude, "Chau");
        }
    }

    private void Confirm_Login(object sender, EventArgs e)
    {
        string email = CorreoUs.Text;
        string password = PasswordUs.Text;
        //var exist = App.Database.GetUsersAsync();//App.Database.VerifyLogin(email);
        //if (exist.Result)
        //{
        //    Navigation.PushAsync(new MainPage());
        //}
    }
    private void Register(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Register());
    }
}