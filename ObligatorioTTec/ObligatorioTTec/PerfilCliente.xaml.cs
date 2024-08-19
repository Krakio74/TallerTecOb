namespace ObligatorioTTec
{
    public partial class PerfilCliente : ContentPage
    {
        public PerfilCliente()
        {
            InitializeComponent();
            LoadUserProfile();
        }

        private async void LoadUserProfile()
        {
            // Recuperar datos almacenados
            var username = Preferences.Get("Usuario", string.Empty);
            var password = await SecureStorage.GetAsync("Contrase�a");
            var nombre = Preferences.Get("Nombre", string.Empty);
            var apellido = Preferences.Get("Apellido", string.Empty);
            var sexo = Preferences.Get("Sexo", string.Empty);
            var correo = Preferences.Get("Correo", string.Empty);

            // Mostrar datos en la interfaz de usuario
            UsernameLabel.Text = $"Nombre de Usuario: {username}";
            PasswordLabel.Text = $"Contrase�a: {password}"; // Mostrar contrase�a en texto plano para demostraci�n
            NombreLabel.Text = $"Nombre: {nombre}";
            ApellidoLabel.Text = $"Apellido: {apellido}";
            SexoLabel.Text = $"Sexo: {sexo}";
            CorreoLabel.Text = $"Correo: {correo}";

            // Cargar la imagen de perfil
            var photoPath = Preferences.Get("ProfilePhotoPath", string.Empty);
            if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
            {
                ProfileImage.Source = ImageSource.FromFile(photoPath);
            }
        }

        private async void BtnVolver_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void BtnEditar_Clicked(object sender, EventArgs e)
        {

        }
    }
}
