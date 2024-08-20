using ObligatorioTTec.Models;
namespace ObligatorioTTec.Views;

public partial class Register : ContentPage
{
    private readonly ApiService _apiService;
    public Register()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }
    private async void Confirm_Register(object sender, EventArgs e)
    {
        string nombre = NombreUs.Text;
        string apellido = ApellidoUs.Text;
        string correo = CorreoUs.Text;
        string password = PasswordUs.Text;
        int edad = int.Parse(EdadUs.Text);


        Usuario nuevoUsuario = new Usuario
        {
            Nombre = nombre,
            Apellido = apellido,
            Correo = correo,
            Pass = password,
            Edad = edad
        };

        var registerToDB = await _apiService.RegisterUser(nuevoUsuario);
        if (registerToDB)
        {
            await DisplayAlert("Registro de usuario", "El usuario se creo con exito, por favor inicie session", "Iniciar Session");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Error en la creacion de usuario", "Salir");
        }

        //if(await App.CineDB.SetUsuario(nuevoUsuario))
        //{
        //    Utilidades.TomarFoto(App.CineDB.GetUsuario(CorreoUs.Text).Result.ID);
        //    DisplayAlert("Registro", "Usuario creado con exito", "Iniciar Session");
        //    Navigation.PopAsync();
        //}
        //else{
        //    DisplayAlert("Registro", "El usuario ya existe", "Salir");
        //    Navigation.PopAsync();
        //}

        var usuarios = App.CineDB.GetUsuarios();
        //var usu = await _database.GetUsuarios();


    }
}