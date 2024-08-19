using System.Text.Json;

namespace ObligatorioTTec
{
    public partial class RegistrarCliente : ContentPage
    {
        private string _photoPath;
        private List<Persona> _personas;

        private bool isEditing = false;

        public RegistrarCliente()
        {
            InitializeComponent();
            _personas = LoadPersonas();
        }

        private void BtnEditar_Clicked(object sender, EventArgs e)
        {
            if (isEditing)
            {
                // Guardar los cambios
                var nombre = NombreEntry.Text;
                var apellido = ApellidoEntry.Text;
                var sexo = SexoPicker.SelectedItem?.ToString();
                var correo = CorreoEntry.Text;
                var usuario = UsuarioEntry.Text;
                var contraseña = ContraseñaEntry.Text;

                // Aquí puedes guardar los datos donde necesites, por ejemplo en preferencias
                Preferences.Set("Nombre", nombre);
                Preferences.Set("Apellido", apellido);
                Preferences.Set("Sexo", sexo);
                Preferences.Set("Correo", correo);
                Preferences.Set("Usuario", usuario);
                Preferences.Set("Contraseña", contraseña);

                // Cambiar el estado de los controles de entrada
                SetEntriesEnabled(false);

                // Cambiar el texto del botón
                BtnEditar.Text = "Editar";
                isEditing = false;
            }
            else
            {
                // Habilitar los campos para edición
                SetEntriesEnabled(true);

                // Cambiar el texto del botón
                BtnEditar.Text = "Guardar";
                isEditing = true;
            }
        }

        private void SetEntriesEnabled(bool isEnabled)
        {
            NombreEntry.IsEnabled = isEnabled;
            ApellidoEntry.IsEnabled = isEnabled;
            SexoPicker.IsEnabled = isEnabled;
            CorreoEntry.IsEnabled = isEnabled;
            UsuarioEntry.IsEnabled = isEnabled;
            ContraseñaEntry.IsEnabled = isEnabled;
        }

        private async void OnTakePhotoClicked(object sender, EventArgs e)
        {
            var photo = await TakePhotoAsync();
            if (photo != null)
            {
                _photoPath = photo.FullPath;
                ProfileImage.Source = ImageSource.FromFile(_photoPath);
            }
        }

        private async Task<FileResult> TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                    return null;

                var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                {
                    await stream.CopyToAsync(newStream);
                }

                return new FileResult(newFile);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
                return null;
            }
        }

        public async void OnRegisterClicked(object sender, EventArgs e)
        {
            var nombre = NombreEntry.Text;
            var apellido = ApellidoEntry.Text;
            var sexo = SexoPicker.SelectedItem.ToString();
            var correo = CorreoEntry.Text;
            var usuario = UsuarioEntry.Text;
            var contraseña = ContraseñaEntry.Text;

            var nuevaPersona = new Persona
            {
                Nombre = nombre,
                Apellido = apellido,
                Sexo = sexo,
                Correo = correo,
                Usuario = usuario,
                Contraseña = contraseña
            };

            _personas.Add(nuevaPersona);
            SavePersonas(_personas);

            await DisplayAlert("Registro", "Usuario registrado exitosamente", "OK");

            await Navigation.PushAsync(new MainPage());
        }

        private void SavePersonas(List<Persona> personas)
        {
            var personasJson = JsonSerializer.Serialize(personas);
            Preferences.Set("Personas", personasJson);
        }

        private List<Persona> LoadPersonas()
        {
            var personasJson = Preferences.Get("Personas", string.Empty);
            if (string.IsNullOrEmpty(personasJson))
                return new List<Persona>();

            return JsonSerializer.Deserialize<List<Persona>>(personasJson);
        }

        public void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Catalogo
            {
                BindingContext = new Persona ()
            });
        }

        private void BtnVolver_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Catalogo
            {
                BindingContext = new Persona()
            });
        }
    }
}

