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
            var contrase�a = PasswordEntry.Text;

            // Recuperar datos almacenados
            var usuarioGuardado = Preferences.Get("Usuario", string.Empty);
            var contrase�aGuardada = Preferences.Get("Contrase�a", string.Empty);

            // Verificar si los datos coinciden
            if (usuario == usuarioGuardado && contrase�a == contrase�aGuardada)
            {
                await DisplayAlert("Inicio de Sesi�n", "Inicio de sesi�n exitoso", "OK");
                // Navegar a otra p�gina, por ejemplo, la p�gina principal
                await Navigation.PushAsync(new PerfilCliente());
            }
            else
            {
                ErrorMessage.Text = "Nombre de usuario o contrase�a incorrectos.";
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
        //    var storedPassword = await SecureStorage.GetAsync("Contrase�a");

        //    // Verificar si los datos coinciden
        //    return username == storedUsername && password == storedPassword;
        //}
    }
}
