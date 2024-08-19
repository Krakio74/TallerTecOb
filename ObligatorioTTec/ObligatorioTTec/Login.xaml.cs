namespace ObligatorioTTec
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var usuario = UsernameEntry.Text;
            var contraseña = PasswordEntry.Text;

            // Recuperar datos almacenados
            var usuarioGuardado = Preferences.Get("Usuario", string.Empty);
            var contraseñaGuardada = Preferences.Get("Contraseña", string.Empty);

            // Verificar si los datos coinciden
            if (usuario == usuarioGuardado && contraseña == contraseñaGuardada)
            {
                await DisplayAlert("Inicio de Sesión", "Inicio de sesión exitoso", "OK");
                // Navegar a otra página, por ejemplo, la página principal
                await Navigation.PushAsync(new PerfilCliente());
            }
            else
            {
                ErrorMessage.Text = "Nombre de usuario o contraseña incorrectos.";
                ErrorMessage.IsVisible = true;
            }
        }

        private async void BtnVolver_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        //public async Task<bool> IsLoginValidAsync(string username, string password)
        //{
        //    // Recuperar los datos almacenados
        //    var storedUsername = Preferences.Get("Usuario", string.Empty);
        //    var storedPassword = await SecureStorage.GetAsync("Contraseña");

        //    // Verificar si los datos coinciden
        //    return username == storedUsername && password == storedPassword;
        //}
    }
}
