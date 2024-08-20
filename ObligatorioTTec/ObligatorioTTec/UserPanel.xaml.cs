using ObligatorioTTec.Models;
using ObligatorioTTec.Views;

namespace ObligatorioTTec;

public partial class UserPanel : ContentPage
{
    private readonly ApiService _apiService;

    Entry entrCon;
    Entry entrCorreo;
    bool newLogin;
    public UserPanel()
    {
        InitializeComponent();
        _apiService = new ApiService();
        // Navigation.PushAsync(new Login());

    }
    private async void refresh()
    {
        var page = Navigation.NavigationStack.LastOrDefault();


        // Load new page
        await Navigation.PushAsync(new UserPanel());

        // Remove old page
        Navigation.RemovePage(page);
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
        if (LoggedUsers.Count > 0 && newLogin == false)
        {

            foreach (var User in LoggedUsers)
            {
                var userGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
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

                }, 0, 0);

                userGrid.Add(new Label
                {
                    Text = User.Nombre,
                    VerticalOptions = LayoutOptions.Center
                }, 1, 0);
                userGrid.Add(new Label
                {
                    Text = "Correo:",
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center
                }, 0, 1);
                userGrid.Add(new Label
                {
                    Text = User.Correo,
                    VerticalOptions = LayoutOptions.Center
                }, 1, 1);
                userGrid.Add(new Image
                {
                    Source = await getPhoto(User.Correo),
                    MaximumWidthRequest = 100,
                    MaximumHeightRequest = 100,
                });
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
            var btnReg = new Button
            {
                Text = "Registrarse",
                Padding = new Thickness(10),
                Margin = new Thickness(0, 10),



            };
            btnReg.Clicked += async (sender, args) => Navigation.PushAsync(new Register());
            var botonLogin = new Button
            {
                Text = "Login",
                Padding = new Thickness(10),
                Margin = new Thickness(0, 10),


            };
            botonLogin.Clicked += async (sender, args) => NuevoLogin();
            content.Add(btnReg);
            content.Add(botonLogin);
            Content = new StackLayout
            {
                Children = { content },
                Padding = new Thickness(20)
            };

        }

    }
    public void NuevoLogin()
    {
        var content = new StackLayout();
        content.Add(new Label
        {
            Text = "Ingrese su correo y contraseña",
            HorizontalOptions = LayoutOptions.Center,
        });
        entrCorreo = new Entry
        {
            Placeholder = "Correo",
            Keyboard = Keyboard.Email,
            BindingContext = CorreoUs,
            Margin = new Thickness(0, 10),

        };

        content.Add(entrCorreo);
        entrCon = new Entry
        {
            Placeholder = "Contraseña",
            BindingContext = PasswordUs,
            IsPassword = true,
            Margin = new Thickness(0, 10)
        };

        content.Add(entrCon);
        var enviar = new Button
        {
            Text = "Ingresar",
            Margin = new Thickness(0, 10)
        };

        enviar.Clicked += async (sender, args) => Confirm_Login2();
        content.Add(enviar);
        Content = new StackLayout
        {
            Children = { content },
            Padding = new Thickness(20)
        };
    }
    private void seleccionUsuario(String Correo)
    {
        CurrentUser.usuario = App.CineDB.GetUsuario(Correo).Result;
        Content = new StackLayout();
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

        }, 0, 0);
        userGrid.Add(new Label
        {
            Text = CurrentUser.usuario.Nombre,
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center,

        }, 0, 1);
        userGrid.Add(new Label
        {
            Text = "Apellido:",
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center,

        }, 1, 0);
        userGrid.Add(new Label
        {
            Text = CurrentUser.usuario.Apellido,
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center,

        }, 1, 1);
        userGrid.Add(new Label
        {
            Text = "Edad:",
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center,

        }, 2, 0);
        userGrid.Add(new Label
        {
            Text = CurrentUser.usuario.Edad.ToString(),
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center,

        }, 2, 1);
        userGrid.Add(new Label
        {
            Text = "Correo:",
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center,

        }, 3, 0);
        userGrid.Add(new Label
        {
            Text = CurrentUser.usuario.Correo,
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center,

        }, 3, 1);
        var botonRegistro = new Button
        {
            Text = "Registrarse",


        };
        botonRegistro.Clicked += async (sender, args) => Navigation.PushAsync(new Register());
        var botonLogin = new Button
        {
            Text = "Login",


        };
        botonLogin.Clicked += async (sender, args) => NuevoLogin();
        Content = new StackLayout
        {
            Children = { userGrid },
            Padding = new Thickness(20)
        };
    }
    private async void Pos_Gps(object sender, EventArgs e)
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location != null)
        {
            await DisplayAlert("Loc", location.Latitude + " " + location.Longitude, "Chau");
        }
    }
    private void Confirm_Login2()
    {
        string email = entrCorreo.Text;
        string password = entrCon.Text;
        _apiService.VerifyLogin(email, password);
        var resp = _apiService.VerifyLogin(email, password);
        OnAppearing();

    }
    private void Confirm_Login(object sender, EventArgs e)
    {
        string email = CorreoUs.Text;
        string password = PasswordUs.Text;
        _apiService.VerifyLogin(email, password);
        var resp = _apiService.VerifyLogin(email, password);
        //    if (true == true)
        //    {
        //        if (DeviceInfo.Current.Platform == DevicePlatform.Android || DeviceInfo.Current.Platform == DevicePlatform.iOS)
        //        {
        //            //CrossFingerprint.SetCurrentActivityResolver(() => this);
        //        }

        //        await App.CineDB.SetUsuario(CurrentUser.usuario);
        //        return true;
        //    }
        //    else
        //    {
        //        await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
        //        return false;
        //    }
    }
    private void Register(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Register());
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string email = CorreoUs.Text;
        string password = PasswordUs.Text;
        var response = await _apiService.VerifyLogin(email, password);
        if (response)
        {

            DisplayAlert("Inicio", "Ingreso correctamente", "Continuar");
        }
    }
    private async Task<string> getPhoto(string correo)
    {
        try
        {
            if(CurrentUser.usuario != null)
            {
                var appDataDirectory = FileSystem.AppDataDirectory;
                var imagesDirectory = Path.Combine(appDataDirectory, "Images");
                var files = Directory.GetFiles(imagesDirectory);
                var nombre = correo + ".jpg";
                var dev = Directory.GetFiles(imagesDirectory, nombre, SearchOption.TopDirectoryOnly).FirstOrDefault();
                return dev;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}