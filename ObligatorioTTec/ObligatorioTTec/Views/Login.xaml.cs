using ObligatorioTTec.Models;
namespace ObligatorioTTec.Views;

public partial class Login : ContentPage
{
    private readonly ApiService _apiService;
    public Login()
	{
		InitializeComponent();
        _apiService = new ApiService();
	}
    protected override async void OnAppearing()
    {
        var LoggedUsers = await App.CineDB.GetUsuarios();
        var content = new StackLayout();
        content.Add(new Label
        {
            Text = "Seleccione la cuenta que desea usar",
            HorizontalOptions = LayoutOptions.Center,
        });
        if (LoggedUsers.Count > 0)
        {
            
            foreach (var User in LoggedUsers)
            {
                var userGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        //new RowDefinition { Height = GridLength.Auto }
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    },
                    
                    //Padding = new Thickness(10),
                    //Margin = new Thickness(10),

                };
                userGrid.Add(new Label
                {
                    Text = "Nombre:",
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                    
                },0,0);
               
                userGrid.Add(new Label
                {
                    Text = User.Nombre,
                    VerticalOptions = LayoutOptions.Center
                },1,0);
                userGrid.Add(new Label
                {
                    Text = "Correo:",
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center
                },0,1);
                userGrid.Add(new Label
                {
                    Text = User.Correo,
                    VerticalOptions = LayoutOptions.Center
                },1,1);
                var frame = new Frame
                {
                    Content = userGrid,
                    BorderColor = Colors.Gray, // Set the border color
                    CornerRadius = 10,         // Set the corner radius
                    Margin = new Thickness(0, 10), // Margin around each Frame
                    HasShadow = true           // Add shadow (optional)
                };
                var clicked = new TapGestureRecognizer();
                clicked.Tapped += (s, e) => seleccionUsuario(User.Correo); // Pass the user object
                frame.GestureRecognizers.Add(clicked);
                content.Add(frame);
            }
            var botonRegistro = new Button
            {
                Text = "Registrarse",
                Style = (Style)Resources["BotonDefault"]

            };
            botonRegistro.Clicked += async (sender, args) => Navigation.PushAsync(new Register());
            Content = new StackLayout
            {
                Children = { content },
                Padding = new Thickness(20)
            };

        }

    }
    private void seleccionUsuario(String Correo)
    {
        DisplayAlert("aa", Correo, "ss");
    }
    private async void Pos_Gps(object sender, EventArgs e)
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location != null)
        {
            await DisplayAlert("Loc", location.Latitude + " " + location.Longitude, "Chau");
        }
    }

    private async void Confirm_Login(object sender, EventArgs e)
    {
        string email = CorreoUs.Text;
        string password = PasswordUs.Text;
        var resp = _apiService.VerifyLogin(email, password);
        if (resp.Result == true) {
            if(DeviceInfo.Current.Platform == DevicePlatform.Android || DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                //CrossFingerprint.SetCurrentActivityResolver(() => this);
            }
            
            await App.CineDB.SetUsuario(CurrentUser.usuario);
        }
    }
    private void Register(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Register());
    }
}